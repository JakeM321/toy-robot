namespace ToyRobot.Domain;

internal enum CommandResult
{
    Ok,
    OutOfBounds,
    InitialPlacementMissing
}

internal record Coordinates(int XPosition, int YPosition, Direction FDirection);

/// <summary>
/// Main container for robot movement logic. Defines a single tabletop and a single robot, requiring
/// the robot to be placed before it can be moved or rotated.
/// </summary>
internal interface ISession
{
    /// <summary>
    /// Places the robot on the table-top at the specified coordinates, if possible.
    /// </summary>
    /// <param name="coordinates">The location at which the session will attempt to place the robot.</param>
    /// <returns>Result indicating success or encountered problem.</returns>
    CommandResult Place(Coordinates coordinates);
    /// <summary>
    /// Attempts to move the robot 1 space forward in the direction it is currently facing.
    /// </summary>
    /// <returns>Result indicating success or encountered problem.</returns>
    CommandResult Move();
    /// <summary>
    /// Attempts to rotate the robot left, if it has been placed on the table-top.
    /// </summary>
    /// <returns>Result indicating success or encountered problem.</returns>
    CommandResult Left();
    /// <summary>
    /// Attempts to rotate the robot right, if it has been placed on the table-top.
    /// </summary>
    /// <returns>Result indicating success or encountered problem.</returns>
    CommandResult Right();
    /// <summary>
    /// Returns the robot's current position on the table-top.
    /// </summary>
    /// <returns>The current position of the robot, or NULL if it has not yet been placed.</returns>
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
        // Adding other failure reasons will require a more sophisticated return type from _robot.TryMove()
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