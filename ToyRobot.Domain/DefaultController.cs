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
    Coordinates Report();
}

internal class DefaultController
{
    
}