namespace ToyRobot.Domain;

internal static class Constants
{
    public static class DefaultTableTopSize
    {
        public const int X = 5;
        public const int Y = 5;
    }

    public static class Messages
    {
        public const string Ok = "";
        public const string CannotPlaceOutsideOfGrid = "Cannot place robot outside of grid boundary";
        public const string CannotMove = "Further movement would place robot outside of grid";
        public const string ParameterError = "Invalid {0} parameter for {1} command";
        public const string CannotMoveRobotInitialPlacementMissing = "Cannot move robot that has not yet been placed";
        public const string CannotReportInitialPlacementMissing = "Robot has not yet been placed: no position to report";
        public const string CommandNotRecognised = "Command [{0}] not recognised";
    }

    public static class Commands
    {
        public const string Place = "PLACE";
        public const string Move = "MOVE";
        public const string Left = "LEFT";
        public const string Right = "RIGHT";
        public const string Report = "REPORT";
    }

    public static class Parameters
    {
        public const string XPosition = "X";
        public const string YPosition = "Y";
        public const string FDirection = "F";
    }

    public static class Directions
    {
        public const string North = "NORTH";
        public const string East = "EAST";
        public const string South = "SOUTH";
        public const string West = "WEST";
    }
}