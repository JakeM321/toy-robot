namespace ToyRobot.Domain;

internal enum Direction
{
    North = 1,
    East = 2,
    South = 3,
    West = 4
}

internal interface IMovementCoordinator
{
    bool IsMoveLegal(int x, int y);
}

internal class Robot
{
    private IMovementCoordinator _movementCoordinator;
    private int _posX;
    private int _posY;
    private Direction _directionFacing;

    public Robot(IMovementCoordinator movementCoordinator, int posX, int posY, Direction directionFacing)
    {
        _movementCoordinator = movementCoordinator;
        _posX = posX;
        _posY = posY;
        _directionFacing = directionFacing;
    }

    public int PositionX => _posX;
    public int PositionY => _posY;
    public Direction DirectionFacing => _directionFacing;

    public bool TryMove()
    {
        var proposedXMove = _posX;
        var proposedYMove = _posY;

        switch (_directionFacing)
        {
            case Direction.North:
                proposedYMove++;
                break;
            case Direction.East:
                proposedXMove++;
                break;
            case Direction.South:
                proposedYMove--;
                break;
            case Direction.West:
                proposedXMove--;
                break;
        }

        if (!_movementCoordinator.IsMoveLegal(proposedXMove, proposedYMove))
            return false;

        _posX = proposedXMove;
        _posY = proposedYMove;
        return true;
    }
}