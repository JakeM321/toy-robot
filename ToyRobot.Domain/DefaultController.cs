namespace ToyRobot.Domain;

internal enum Result
{
    Ok,
    OutOfBounds,
    InitialPlacementMissing
}

internal record Coordinates(int XPosition, int YPosition, Direction FDirection);

internal interface IDefaultController
{
    Result Place(Coordinates coordinates);
    Result Move();
    void Left();
    void Right();
    Coordinates? Report();
}

internal class DefaultController : IDefaultController
{
    private TableTop _tableTop;
    private Robot? _robot;
    public DefaultController()
    {
        _tableTop = new TableTop(Constants.DefaultTableTopSize.X, Constants.DefaultTableTopSize.Y);
    }

    public Result Place(Coordinates coordinates)
    {
        if (!_tableTop.IsMoveLegal(coordinates.XPosition, coordinates.YPosition))
            return Result.OutOfBounds;
        _robot = _tableTop.PlaceRobot(coordinates.XPosition, coordinates.YPosition, coordinates.FDirection);
        return Result.Ok;
    }

    public Result Move()
    {
        if (_robot == null)
            return Result.InitialPlacementMissing;
        var success = _robot.TryMove();
        if (success)
            return Result.Ok;

        // NB: The only reason that the move attempt can fail at this stage is if it falls out of bounds.
        // Adding other failure reasons would require a more sophisticated return type from _robot.TryMove()
        return Result.OutOfBounds;
    }

    public void Left()
    {
        throw new NotImplementedException();
    }

    public void Right()
    {
        throw new NotImplementedException();
    }

    public Coordinates? Report()
    {
        if (_robot == null)
            return null;
        return new Coordinates(_robot.PositionX, _robot.PositionY, _robot.DirectionFacing);
    }
}