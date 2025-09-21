namespace ToyRobot.Domain;

internal enum ComandResult
{
    Ok,
    OutOfBounds,
    InitialPlacementMissing
}

internal record Coordinates(int XPosition, int YPosition, Direction FDirection);

internal interface ISession
{
    ComandResult Place(Coordinates coordinates);
    ComandResult Move();
    ComandResult Left();
    ComandResult Right();
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

    public ComandResult Place(Coordinates coordinates)
    {
        if (!_tableTop.IsMoveLegal(coordinates.XPosition, coordinates.YPosition))
            return ComandResult.OutOfBounds;
        _robot = _tableTop.PlaceRobot(coordinates.XPosition, coordinates.YPosition, coordinates.FDirection);
        return ComandResult.Ok;
    }

    public ComandResult Move()
    {
        if (_robot == null)
            return ComandResult.InitialPlacementMissing;
        var success = _robot.TryMove();
        if (success)
            return ComandResult.Ok;

        // NB: The only reason that the move attempt can fail at this stage is if it falls out of bounds.
        // Adding other failure reasons would require a more sophisticated return type from _robot.TryMove()
        return ComandResult.OutOfBounds;
    }

    public ComandResult Left()
    {
        if (_robot == null)
            return ComandResult.InitialPlacementMissing;
        _robot.TurnLeft();
        return ComandResult.Ok;
    }

    public ComandResult Right()
    {
        if (_robot == null)
            return ComandResult.InitialPlacementMissing;
        _robot.TurnRight();
        return ComandResult.Ok;
    }

    public Coordinates? Report()
    {
        if (_robot == null)
            return null;
        return new Coordinates(_robot.PositionX, _robot.PositionY, _robot.DirectionFacing);
    }
}