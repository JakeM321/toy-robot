namespace ToyRobot.Domain;

public class DefaultSession
{
    public string HandleCommand(string prompt)
    {
        var portions = prompt.Split(' ');
        var command = portions[0];
        if (command.ToUpper() == "PLACE")
        {
            var error = ValidatePlaceCommand(portions);
            if (error != null)
                return error;
        }

        return $"Command [{command}] not recognised";
    }

    private string? ValidatePlaceCommand(string[] portions)
    {
        var arguments = portions.Skip(1).SelectMany(p => p.Split(',')).ToArray();
        if (!ValidateInt(arguments, 1))
            return "Invalid X parameter for PLACE command";
        if (!ValidateInt(arguments, 2))
            return "Invalid Y parameter for PLACE command";
        if (!ValidateDirection(arguments, 3))
            return "Invalid F parameter for PLACE command";
        return null;
    }

    private static bool ValidateInt(string[] arguments, int position)
    {
        if (position > arguments.Length)
            return false;
        return Int32.TryParse(arguments[position - 1], out _);
    }

    private static bool ValidateDirection(string[] arguments, int position)
    {
        if (position > arguments.Length)
            return false;
        return new[] { "NORTH", "EAST", "SOUTH", "WEST" }.Contains(arguments[position - 1].ToUpper());
    }
}