namespace ToyRobot.Domain;

internal record Coordinates(int XPosition, int YPosition, Direction FDirection);

internal interface IDefaultController
{
    void Place(Coordinates coordinates);
}

internal class DefaultController
{
    
}