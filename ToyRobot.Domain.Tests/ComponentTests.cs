namespace ToyRobot.Domain.Tests
{
    public class ComponentTests
    {
        // Examples from the challenge worksheet
        [Fact]
        public void ExampleCaseOne()
        {
            Submit("PLACE 0,0,NORTH");
            Expect(Constants.Messages.Ok);

            Submit("MOVE");
            Expect(Constants.Messages.Ok);

            Submit("REPORT");
            Expect("0,1,NORTH");
        }

        [Fact]
        public void ExampleCaseTwo()
        {
            Submit("PLACE 0,0,NORTH");
            Expect(Constants.Messages.Ok);

            Submit("LEFT");
            Expect(Constants.Messages.Ok);

            Submit("REPORT");
            Expect("0,0,WEST");
        }

        [Fact]
        public void ExampleCaseThree()
        {
            Submit("PLACE 1,2,EAST");
            Expect(Constants.Messages.Ok);

            Submit("MOVE");
            Expect(Constants.Messages.Ok);

            Submit("MOVE");
            Expect(Constants.Messages.Ok);

            Submit("LEFT");
            Expect(Constants.Messages.Ok);

            Submit("MOVE");
            Expect(Constants.Messages.Ok);

            Submit("REPORT");
            Expect("3,3,NORTH");
        }

        // Demonstration to showcase a variety of commands and responses in a single debugging session
        [Fact]
        public void MainDemonstration()
        {
            Submit("MOVE");
            Expect("Cannot move robot that has not yet been placed");

            Submit("LEFT");
            Expect("Cannot move robot that has not yet been placed");

            Submit("PLACE 6,7,NORTH");
            Expect("Cannot place robot outside of grid boundary");

            Submit("PLACE 1,2,EAST");
            Submit("REPORT");
            Expect("1,2,EAST");

            Submit("MOVE");
            Submit("REPORT");
            Expect("2,2,EAST");

            Submit("MOVE");
            Submit("REPORT");
            Expect("3,2,EAST");

            Submit("MOVE");
            Submit("MOVE");
            Expect("Further movement would place robot outside of grid");

            Submit("REPORT");
            Expect("4,2,EAST");

            Submit("MOVE");
            Expect("Further movement would place robot outside of grid");

            Submit("LEFT");
            Submit("REPORT");
            Expect("4,2,NORTH");

            Submit("MOVE");
            Submit("REPORT");
            Expect("4,3,NORTH");

            Submit("MOVE");
            Submit("REPORT");
            Expect("4,4,NORTH");

            Submit("MOVE");
            Expect("Further movement would place robot outside of grid");

            Submit("LEFT");
            Submit("MOVE");
            Submit("MOVE");
            Submit("MOVE");
            Submit("REPORT");
            Expect("1,4,WEST");

            Submit("MOVE");
            Submit("REPORT");
            Expect("0,4,WEST");

            Submit("MOVE");
            Expect("Further movement would place robot outside of grid");

            Submit("PLACE 0,0,NORTH");
            Submit("MOVE");
            Submit("REPORT");
            Expect("0,1,NORTH");
        }

        #region Helpers
        private CommandLine _commandLine;
        private string _output;
        public ComponentTests()
        {
            _commandLine = CommandLine.Create();
        }
        private void Submit(string command)
        {
            _output = _commandLine.HandleCommand(command);
        }
        private void Expect(string output)
        {
            Assert.Equal(output, _output);
        }
        #endregion
    }
}
