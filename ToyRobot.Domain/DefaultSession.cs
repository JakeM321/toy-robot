namespace ToyRobot.Domain;

public class DefaultSession
{
    public string HandleCommand(string command)
    {
        var promptPortion = command.Split(" ")[0];
        return $"Command [{promptPortion}] not recognised";
    }
}