namespace ToyRobot.Domain;

internal class TableTop(int width, int height) : IMovementCoordinator
{
    public Robot PlaceRobot(int posX, int posY, Direction directionFacing)
    {
        return new Robot(this, posX, posY, directionFacing);
    }

    public bool IsMoveLegal(int x, int y)
    {
        var xPosMax = width - 1;
        var yPosMax = height - 1;

        return x <= xPosMax
            && x >= 0
            && y <= yPosMax
            && y >= 0;
    }
}