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
    private IMovementCoordinator _movementValidator;
    private int _posX;
    private int _posY;
    private Direction _directionFacing;

    public Robot(IMovementCoordinator movementValidator, int posX, int posY, Direction directionFacing)
    {
        _movementValidator = movementValidator;
        _posX = posX;
        _posY = posY;
        _directionFacing = directionFacing;
    }

    public void TryMove()
    {
        throw new NotImplementedException();
    }
}