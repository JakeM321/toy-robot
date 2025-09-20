namespace ToyRobot.Domain;

internal class TableTop(int width, int height) : IMovementCoordinator
{
    private int _width = width;
    private int _height = height;

    public Robot PlaceRobot(int posX, int posY, Direction directionFacing)
    {
        throw new NotImplementedException();
    }

    public bool IsMoveLegal(int x, int y)
    {
        var xPosMax = _width - 1;
        var yPosMax = _height - 1;

        return x <= xPosMax
            && x >= 0
            && y <= yPosMax
            && y >= 0;
    }
}