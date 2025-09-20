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
        if (command.ToUpper() == "PLACE")
        {
            (var coordinates, string error) = ValidatePlaceCommand(portions);
            if (coordinates == null)
                return error;
            _controller.Place(coordinates);
            return string.Empty;
        }

        return $"Command [{command}] not recognised";
    }

    private (Coordinates?, string) ValidatePlaceCommand(string[] portions)
    {
        var arguments = portions.Skip(1).SelectMany(p => p.Split(',')).ToArray();
        var xPos = ValidateInt(arguments, 1);
        var yPos = ValidateInt(arguments, 2);
        var fDir = ValidateDirection(arguments, 3);

        if (xPos == null)
            return (null, "Invalid X parameter for PLACE command");
        if (yPos == null)
            return (null, "Invalid Y parameter for PLACE command");
        if (fDir == null)
            return (null, "Invalid F parameter for PLACE command");

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
            case "NORTH":
                return Direction.North;
            case "EAST":
                return Direction.East;
            case "SOUTH":
                return Direction.South;
            case "WEST":
                return Direction.West;
            default:
                return null;
        }
    }
}