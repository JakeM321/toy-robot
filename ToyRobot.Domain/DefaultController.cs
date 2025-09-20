namespace ToyRobot.Domain;

internal record Coordinates(int XPosition, int YPosition, Direction FDirection);

internal enum Result
{
    Ok,
    OutOfBounds
}

internal interface IDefaultController
{
    Result Place(Coordinates coordinates);
    Result Move();
}

internal class DefaultController
{
    
}