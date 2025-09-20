namespace ToyRobot.Domain;

public class Session
{
    private const int TableWidth = 5;
    private const int TableHeight = 5;

    public void Example()
    {
        var tableTop = new TableTop(TableWidth, TableHeight);
        var robot = tableTop.PlaceRobot(0, 2, Direction.North);
        robot.TryMove();
        robot.TryMove();
        robot.TryMove();
    }
}