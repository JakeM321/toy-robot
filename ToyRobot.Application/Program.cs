using ToyRobot.Domain;

var session = CommandLine.Create();
while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    var output = session.HandleCommand(input);
    if (output != string.Empty)
        Console.WriteLine(output);
}