namespace ToyRobot.Domain;

public class DefaultSession
{
    private readonly IDefaultController _controller;

    internal DefaultSession(IDefaultController controller)
    {
        _controller = controller;
    }

    public string HandleCommand(string prompt)
    {
        var portions = prompt.Split(' ');
        var command = portions[0];
        if (command.ToUpper() == Constants.Commands.Place)
        {
            (var coordinates, string error) = ValidatePlaceCommand(portions);
            if (coordinates == null)
                return error;
            var result = _controller.Place(coordinates);
            if (result == Result.OutOfBounds)
                return Constants.Messages.CannotPlaceOutsideOfGrid;
            return Constants.Messages.Ok;
        }

        return string.Format(Constants.Messages.CommandNotRecognised, command);
    }

    private (Coordinates?, string) ValidatePlaceCommand(string[] portions)
    {
        var arguments = portions.Skip(1).SelectMany(p => p.Split(',')).ToArray();
        var xPos = ValidateInt(arguments, 1);
        var yPos = ValidateInt(arguments, 2);
        var fDir = ValidateDirection(arguments, 3);

        if (xPos == null)
            return (null, String.Format(Constants.Messages.ParameterError, Constants.Parameters.XPosition, Constants.Commands.Place));
        if (yPos == null)
            return (null, String.Format(Constants.Messages.ParameterError, Constants.Parameters.YPosition, Constants.Commands.Place));
        if (fDir == null)
            return (null, String.Format(Constants.Messages.ParameterError, Constants.Parameters.FDirection, Constants.Commands.Place));

        var coords = new Coordinates(xPos.Value, yPos.Value, fDir.Value);
        return (coords, string.Empty);
    }

    private static int? ValidateInt(string[] arguments, int position)
    {
        if (position > arguments.Length)
            return null;
        var success = Int32.TryParse(arguments[position - 1], out var parsed);
        if (!success)
            return null;
        return parsed;
    }

    private static Direction? ValidateDirection(string[] arguments, int position)
    {
        if (position > arguments.Length)
            return null;
        var direction = arguments[position - 1].ToUpper();
        switch (direction)
        {
            case Constants.Directions.North:
                return Direction.North;
            case Constants.Directions.East:
                return Direction.East;
            case Constants.Directions.South:
                return Direction.South;
            case Constants.Directions.West:
                return Direction.West;
            default:
                return null;
        }
    }
}