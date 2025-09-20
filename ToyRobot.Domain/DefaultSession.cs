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
        var command = portions[0].ToUpper();
        if (command == Constants.Commands.Place)
        {
            (var coordinates, string error) = ValidatePlaceCommand(portions);
            if (coordinates == null)
                return error;
            var result = _controller.Place(coordinates);
            if (result == Result.OutOfBounds)
                return Constants.Messages.CannotPlaceOutsideOfGrid;
            return Constants.Messages.Ok;
        }

        if (command == Constants.Commands.Move)
        {
            var result = _controller.Move();
            if (result == Result.OutOfBounds)
                return Constants.Messages.CannotMove;
            return Constants.Messages.Ok;
        }

        if (command == Constants.Commands.Left)
        {
            _controller.Left();
            return Constants.Messages.Ok;
        }

        if (command == Constants.Commands.Right)
        {
            _controller.Right();
            return Constants.Messages.Ok;
        }

        if (command == Constants.Commands.Report)
        {
            var result = _controller.Report();
            switch (result.FDirection)
            {
                case Direction.North:
                    return $"{result.XPosition},{result.YPosition},{Constants.Directions.North}";
                case Direction.East:
                    return $"{result.XPosition},{result.YPosition},{Constants.Directions.East}";
                case Direction.South:
                    return $"{result.XPosition},{result.YPosition},{Constants.Directions.South}";
                case Direction.West:
                    return $"{result.XPosition},{result.YPosition},{Constants.Directions.West}";
            }

            return Constants.Messages.Ok;
        }

        return string.Format(Constants.Messages.CommandNotRecognised, command);
    }

    private (Coordinates?, string) ValidatePlaceCommand(string[] portions)
    {
        var arguments = string.Join(string.Empty, portions.Skip(1))
            .Split(',')
            .Select(x => x.Trim())
            .ToArray();
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