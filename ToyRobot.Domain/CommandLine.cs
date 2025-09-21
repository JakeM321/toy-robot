namespace ToyRobot.Domain;

public class CommandLine
{
    private readonly ISession _session;

    internal CommandLine(ISession session)
    {
        _session = session;
    }

    public string HandleCommand(string prompt)
    {
        var portions = prompt.Split(' ');
        var command = portions[0].ToUpper();
        switch(command)
        {
            case Constants.Commands.Place:
                return HandlePlaceCommand(portions);
            case Constants.Commands.Move:
                return HandleMoveCommand();
            case Constants.Commands.Left:
                return HandleLeftCommand();
            case Constants.Commands.Right:
                return HandleRightCommand();
            case Constants.Commands.Report:
                return HandleReportCommand();
            default:
                return string.Format(Constants.Messages.CommandNotRecognised, command);
        }
    }

    #region Handlers
    private string HandlePlaceCommand(string[] portions)
    {
        (var coordinates, string error) = ValidatePlaceCommand(portions);
        if (coordinates == null)
            return error;
        var result = _session.Place(coordinates);
        if (result == CommandResult.OutOfBounds)
            return Constants.Messages.CannotPlaceOutsideOfGrid;
        return Constants.Messages.Ok;
    }

    private string HandleMoveCommand()
    {
        var result = _session.Move();
        if (result == CommandResult.OutOfBounds)
            return Constants.Messages.CannotMove;
        if (result == CommandResult.InitialPlacementMissing)
            return Constants.Messages.CannotMoveRobotInitialPlacementMissing;
        return Constants.Messages.Ok;
    }

    private string HandleLeftCommand()
    {
        var result = _session.Left();
        if (result == CommandResult.InitialPlacementMissing)
            return Constants.Messages.CannotMoveRobotInitialPlacementMissing;
        return Constants.Messages.Ok;
    }

    private string HandleRightCommand()
    {
        var result = _session.Right();
        if (result == CommandResult.InitialPlacementMissing)
            return Constants.Messages.CannotMoveRobotInitialPlacementMissing;
        return Constants.Messages.Ok;
    }

    private string HandleReportCommand()
    {
        var result = _session.Report();
        if (result == null)
            return Constants.Messages.CannotReportInitialPlacementMissing;
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
    #endregion

    #region Validation
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
            return (null, string.Format(Constants.Messages.ParameterError, Constants.Parameters.XPosition, Constants.Commands.Place));
        if (yPos == null)
            return (null, string.Format(Constants.Messages.ParameterError, Constants.Parameters.YPosition, Constants.Commands.Place));
        if (fDir == null)
            return (null, string.Format(Constants.Messages.ParameterError, Constants.Parameters.FDirection, Constants.Commands.Place));

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
    #endregion

    public static CommandLine Create()
    {
        var session = new Session();
        return new CommandLine(session);
    }
}