using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.BDDfy.Xunit;

namespace ToyRobot.Domain.Tests
{
    public class ComponentTests
    {
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
