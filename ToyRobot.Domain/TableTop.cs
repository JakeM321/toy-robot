namespace ToyRobot.Domain;

internal class TableTop(int width, int height) : IMovementValidator
{
    private int _width = width;
    private int _height = height;

    public Robot PlaceRobot(int posX, int posY, Direction directionFacing)
    {
        throw new NotImplementedException();
    }

    public bool IsMoveLegal(int x, int y)
    {
        throw new NotImplementedException();
    }
}