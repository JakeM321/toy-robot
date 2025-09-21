namespace ToyRobot.Domain;

internal enum CommandResult
{
    Ok,
    OutOfBounds,
    InitialPlacementMissing
}

internal record Coordinates(int XPosition, int YPosition, Direction FDirection);

internal interface ISession
{
    CommandResult Place(Coordinates coordinates);
    CommandResult Move();
    CommandResult Left();
    CommandResult Right();
    Coordinates? Report();
}

internal class Session : ISession
{
    private readonly TableTop _tableTop;
    private Robot? _robot;
    public Session()
    {
        _tableTop = new TableTop(Constants.DefaultTableTopSize.X, Constants.DefaultTableTopSize.Y);
    }

    public CommandResult Place(Coordinates coordinates)
    {
        if (!_tableTop.IsMoveLegal(coordinates.XPosition, coordinates.YPosition))
            return CommandResult.OutOfBounds;
        _robot = _tableTop.PlaceRobot(coordinates.XPosition, coordinates.YPosition, coordinates.FDirection);
        return CommandResult.Ok;
    }

    public CommandResult Move()
    {
        if (_robot == null)
            return CommandResult.InitialPlacementMissing;
        var success = _robot.TryMove();
        if (success)
            return CommandResult.Ok;

        // NB: The only reason that the move attempt can fail at this stage is if it falls out of bounds.
        // Adding other failure reasons would require a more sophisticated return type from _robot.TryMove()
        return CommandResult.OutOfBounds;
    }

    public CommandResult Left()
    {
        if (_robot == null)
            return CommandResult.InitialPlacementMissing;
        _robot.TurnLeft();
        return CommandResult.Ok;
    }

    public CommandResult Right()
    {
        if (_robot == null)
            return CommandResult.InitialPlacementMissing;
        _robot.TurnRight();
        return CommandResult.Ok;
    }

    public Coordinates? Report()
    {
        if (_robot == null)
            return null;
        return new Coordinates(_robot.PositionX, _robot.PositionY, _robot.DirectionFacing);
    }
}