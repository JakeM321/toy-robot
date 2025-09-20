using Moq;
using TestStack.BDDfy;
using TestStack.BDDfy.Xunit;

namespace ToyRobot.Domain.Tests;

public class DefaultSessionTests
{
    [BddfyTheory]
    [InlineData("ABC", "Command [ABC] not recognised")]
    [InlineData("MOVE123", "Command [MOVE123] not recognised")]
    [InlineData("MOVE123 C", "Command [MOVE123] not recognised")]
    [InlineData("PLACE456 55 23", "Command [PLACE456] not recognised")]
    public void ReturnsUnrecognisedCommandError(string command, string expectedResponse)
    {
        this.Given(s => s.AMockController())
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand(command), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.ResponseIsReturned(expectedResponse), "The following response is returned: \"{0}\"")
            .BDDfy();
    }

    // Only aiming for simple validation: returning the first and most immediate problem
    // instead of listing all of them.
    [BddfyTheory]
    // "Place" command remains valid in any casing; the missing X parameter is the
    // foremost problem (because there are no parameters)
    [InlineData("pLaCe", "Invalid X parameter for PLACE command")]
    [InlineData("place", "Invalid X parameter for PLACE command")]
    [InlineData("PLACE", "Invalid X parameter for PLACE command")]
    [InlineData("PLACE A,B,C", "Invalid X parameter for PLACE command")]
    [InlineData("PLACE 1,B,C", "Invalid Y parameter for PLACE command")]
    [InlineData("PLACE 1,2,C", "Invalid F parameter for PLACE command")]
    public void ReturnsErrorAtInvalidPlaceCommandParameters(string command, string expectedResponse)
    {
        this.Given(s => s.AMockController())
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand(command), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.ResponseIsReturned(expectedResponse), "The following response is returned: \"{0}\"")
            .BDDfy();
    }

    [BddfyTheory]
    [InlineData("PLACE 1,2,EAST", 1, 2, 2)]
    [InlineData("place 1,2,east", 1, 2, 2)]
    [InlineData("PLACE 2,4,NORTH", 2, 4, 1)]
    // Grid boundary (5x5) not validated here - the only aim is to map the input arguments correctly
    // and produce an error message if this is not possible
    [InlineData("PLACE 5,6,WEST", 5, 6, 4)]
    [InlineData("PLACE 5  ,  6 ,    WEST", 5, 6, 4)]
    public void ProcessesPlaceCommand(string command, int expectedXArgument, int expectedYArgument, int expectedFArgument)
    {
        this.Given(s => s.AMockController())
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand(command), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.TheControllerPlaceMethodIsCalledWith(expectedXArgument, expectedYArgument,
                (Direction)expectedFArgument))
            .BDDfy();
    }

    [BddfyFact]
    public void ReturnsMessageFromSuccessfulPlaceCommand()
    {
        this.Given(s => s.AMockControllerWithSuccessfulResult())
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand("PLACE 1,2,EAST"), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.ResponseIsReturned(Constants.Messages.Ok), "The following response is returned: \"{0}\"")
            .BDDfy();
    }

    [BddfyFact]
    public void ReturnsErrorMessageFromPlacementAttemptOutOfBoundsResult()
    {
        this.Given(s => s.AMockControllerWithOutOfBoundsResult())
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand("PLACE 1,2,EAST"), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.ResponseIsReturned(Constants.Messages.CannotPlaceOutsideOfGrid), "The following response is returned: \"{0}\"")
            .BDDfy();
    }

    [BddfyFact]
    public void ReturnsErrorMessageFromMovementAttemptOutOfBoundsResult()
    {
        this.Given(s => s.AMockControllerWithOutOfBoundsResult())
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand("MOVE"), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.ResponseIsReturned(Constants.Messages.CannotMove), "The following response is returned: \"{0}\"")
            .BDDfy();
    }

    [BddfyFact]
    public void ReturnsErrorMessageFromMovementAttemptPlacementMissingResult()
    {
        this.Given(s => s.AMockControllerWithPlacementMissingResult())
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand("MOVE"), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.ResponseIsReturned(Constants.Messages.CannotMoveRobotInitialPlacementMissing), "The following response is returned: \"{0}\"")
            .BDDfy();
    }

    [BddfyTheory]
    [InlineData("MOVE")]
    [InlineData("mOvE ")]
    [InlineData("move")]
    public void ProcessesMoveCommand(string command)
    {
        this.Given(s => s.AMockController())
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand(command), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.TheControllerMoveMethodIsCalled())
            .BDDfy();
    }

    [BddfyFact]
    public void ReturnsMessageFromSuccessfulMoveCommand()
    {
        this.Given(s => s.AMockControllerWithSuccessfulResult())
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand("MOVE"), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.ResponseIsReturned(Constants.Messages.Ok), "The following response is returned: \"{0}\"")
            .BDDfy();
    }

    [BddfyTheory]
    [InlineData("LEFT")]
    [InlineData("lEfT ")]
    [InlineData("left")]
    public void ProcessesLeftCommand(string command)
    {
        this.Given(s => s.AMockControllerWithSuccessfulResult())
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand(command), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.TheControllerLeftMethodIsCalled())
            .And(s => s.ResponseIsReturned(Constants.Messages.Ok))
            .BDDfy();
    }

    [BddfyTheory]
    [InlineData("RIGHT")]
    [InlineData("rIgHt ")]
    [InlineData("right")]
    public void ProcessesRightCommand(string command)
    {
        this.Given(s => s.AMockControllerWithSuccessfulResult())
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand(command), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.TheControllerRightMethodIsCalled())
            .And(s => s.ResponseIsReturned(Constants.Messages.Ok))
            .BDDfy();
    }
    
    [BddfyFact]
    public void CallingLeftWithoutPlacementReturnsError()
    {
        this.Given(s => s.AMockControllerWithPlacementMissingResult())
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand("LEFT"), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.TheControllerLeftMethodIsCalled())
            .And(s => s.ResponseIsReturned(Constants.Messages.CannotMoveRobotInitialPlacementMissing))
            .BDDfy();
    }

    [BddfyFact]
    public void CallingRightWithoutPlacementReturnsError()
    {
        this.Given(s => s.AMockControllerWithPlacementMissingResult())
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand("RIGHT"), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.TheControllerRightMethodIsCalled())
            .And(s => s.ResponseIsReturned(Constants.Messages.CannotMoveRobotInitialPlacementMissing))
            .BDDfy();
    }

    [BddfyTheory]
    [InlineData("REPORT", 2, 1, 1, "2,1,NORTH")]
    [InlineData("rEpOrT ", 1, 2, 2, "1,2,EAST")]
    [InlineData("report", 5, 3, 3, "5,3,SOUTH")]
    [InlineData("report", 7, 1, 4, "7,1,WEST")]
    public void ProcessesReportCommand(string command, int xPos, int yPos, int direction, string expectedMessage)
    {
        this.Given(s => s.AMockControllerReportingCoordinates(xPos, yPos, (Direction)direction))
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand(command), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.TheControllerReportMethodIsCalled())
            .And(s => s.ResponseIsReturned(expectedMessage))
            .BDDfy();
    }

    [BddfyFact]
    public void ReportReturnsErrorWhenPlacementMissing()
    {
        this.Given(s => s.AMockControllerWithNoCoordinatesToReport())
            .And(s => s.ADefaultSession())
            .And(s => s.ACommand("REPORT"), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.TheControllerReportMethodIsCalled())
            .And(s => s.ResponseIsReturned(Constants.Messages.CannotReportInitialPlacementMissing), 
                "and the following response is returned: \"{0}\"")
            .BDDfy();
    }

    #region BDDfy
    #region Data
    private Mock<IDefaultController> _controller;
    private DefaultSession _defaultSession;
    private string _command;
    private string _response;
    #endregion
    #region Given

    private void AMockController()
    {
        _controller = new Mock<IDefaultController>();
    }
    private void AMockControllerWithSuccessfulResult()
    {
        _controller = new Mock<IDefaultController>();
        _controller
            .Setup(c => c.Place(It.IsAny<Coordinates>()))
            .Returns(Result.Ok);
        _controller
            .Setup(c => c.Move())
            .Returns(Result.Ok);
    }
    private void AMockControllerReportingCoordinates(int xPos, int yPos, Direction direction)
    {
        _controller = new Mock<IDefaultController>();
        _controller
            .Setup(c => c.Report())
            .Returns(new Coordinates(xPos, yPos, direction));
    }
    private void AMockControllerWithNoCoordinatesToReport()
    {
        _controller = new Mock<IDefaultController>();
        _controller
            .Setup(c => c.Report())
            .Returns((Coordinates?)null);
    }
    private void AMockControllerWithOutOfBoundsResult()
    {
        _controller = new Mock<IDefaultController>();
        _controller
            .Setup(c => c.Place(It.IsAny<Coordinates>()))
            .Returns(Result.OutOfBounds);
        _controller
            .Setup(c => c.Move())
            .Returns(Result.OutOfBounds);
    }
    private void AMockControllerWithPlacementMissingResult()
    {
        _controller = new Mock<IDefaultController>();
        _controller
            .Setup(c => c.Move())
            .Returns(Result.InitialPlacementMissing);
        _controller
            .Setup(c => c.Left())
            .Returns(Result.InitialPlacementMissing);
        _controller
            .Setup(c => c.Right())
            .Returns(Result.InitialPlacementMissing);
    }
    private void ADefaultSession()
    {
        _defaultSession = new DefaultSession(_controller.Object);
    }
    private void ACommand(string command)
    {
        _command = command;
    }
    #endregion
    #region When
    private void TheCommandIsSubmitted()
    {
        _response = _defaultSession.HandleCommand(_command);
    }
    #endregion
    #region Then
    private void ResponseIsReturned(string expectedResponse)
    {
        Assert.Equal(expectedResponse, _response);
    }
    private void TheControllerPlaceMethodIsCalledWith(int expectedXPos, int expectedYPos, Direction expectedDirection)
    {
        _controller.Verify(c => c.Place(It.Is<Coordinates>(
            item => item.XPosition == expectedXPos
            && item.YPosition == expectedYPos
            && item.FDirection == expectedDirection)));
    }
    private void TheControllerMoveMethodIsCalled()
    {
        _controller.Verify(c => c.Move());
    }
    private void TheControllerLeftMethodIsCalled()
    {
        _controller.Verify(c => c.Left());
    }
    private void TheControllerRightMethodIsCalled()
    {
        _controller.Verify(c => c.Right());
    }

    private void TheControllerReportMethodIsCalled()
    {
        _controller.Verify(c => c.Report());
    }
    #endregion
    #endregion
}