using TestStack.BDDfy;
using TestStack.BDDfy.Xunit;

namespace ToyRobot.Domain.Tests;

public class SessionTests
{
    [BddfyTheory]
    [InlineData(-1, 0)]
    [InlineData(5, 5)]
    [InlineData(8, 10)]
    public void PlacingRobotOutsideOfDefaultBoundaryReturnsOutOfBoundsResult(int xPos, int yPos)
    {
        this.Given(s => s.ASession())
            .When(s => s.PlaceIsCalledWith(xPos, yPos, Direction.North))
            .Then(s => s.TheResultIs(CommandResult.OutOfBounds))
            .BDDfy();
    }

    [BddfyTheory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(2, 2)]
    [InlineData(4, 4)]
    public void PlacingRobotWithinDefaultBoundaryReturnsOkResult(int xPos, int yPos)
    {
        this.Given(s => s.ASession())
            .When(s => s.PlaceIsCalledWith(xPos, yPos, Direction.North))
            .Then(s => s.TheResultIs(CommandResult.Ok))
            .BDDfy();
    }

    [BddfyTheory]
    [InlineData(0, 0, 1)]
    [InlineData(0, 1, 2)]
    [InlineData(2, 2, 3)]
    [InlineData(4, 4, 4)]
    public void PlacingRobotWithinDefaultBoundarySetsPosition(int xPos, int yPos, int direction)
    {
        var directionEnum = (Direction)direction;
        this.Given(s => s.ASession())
            .When(s => s.PlaceIsCalledWith(xPos, yPos, directionEnum))
            .And(s => s.ReportIsCalled())
            .Then(s => s.TheReportedPositionIs(xPos, yPos, directionEnum))
            .BDDfy();
    }

    [BddfyFact]
    public void CallingReportWithoutPlacementReturnsNull()
    {
        this.Given(s => s.ASession())
            .When(s => s.ReportIsCalled())
            .Then(s => s.TheReportedPositionIsNull())
            .BDDfy();
    }

    [BddfyFact]
    public void CallingMoveWithoutPlacementReturnsError()
    {
        this.Given(s => s.ASession())
            .When(s => s.MoveIsCalled())
            .Then(s => s.TheResultIs(CommandResult.InitialPlacementMissing))
            .BDDfy();
    }

    [BddfyTheory]
    [InlineData(0, 0, 1, 0, 1)]
    [InlineData(1, 1, 2, 2, 1)]
    [InlineData(2, 2, 3, 2, 1)]
    [InlineData(3, 3, 4, 2, 3)]
    public void CallingMoveAfterPlacementAdvancesRobotPosition(int initialXPos, int initialYPos, int direction, int expectedXPos, int expectedYPos)
    {
        var directionEnum = (Direction)direction;
        this.Given(s => s.ASession())
            .When(s => s.PlaceIsCalledWith(initialXPos, initialYPos, directionEnum))
            .And(s => s.MoveIsCalled())
            .And(s => s.ReportIsCalled())
            .Then(s => s.TheResultIs(CommandResult.Ok))
            .And(s => s.TheReportedPositionIs(expectedXPos, expectedYPos, directionEnum))
            .BDDfy();
    }

    [BddfyTheory]
    [InlineData(0, 4, 1)]
    [InlineData(4, 0, 2)]
    [InlineData(0, 0, 3)]
    [InlineData(0, 0, 4)]
    public void CallingIllegalMoveReturnsError(int initialXPos, int initialYPos, int direction)
    {
        var directionEnum = (Direction)direction;
        this.Given(s => s.ASession())
            .When(s => s.PlaceIsCalledWith(initialXPos, initialYPos, directionEnum))
            .And(s => s.MoveIsCalled())
            .Then(s => s.TheResultIs(CommandResult.OutOfBounds))
            .BDDfy();
    }

    [BddfyFact]
    public void CallingLeftWithoutPlacementReturnsError()
    {
        this.Given(s => s.ASession())
            .When(s => s.LeftIsCalled())
            .Then(s => s.TheResultIs(CommandResult.InitialPlacementMissing))
            .BDDfy();
    }

    [BddfyTheory]
    [InlineData(1, 4)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(4, 3)]
    public void CallingLeftUpdatesDirectionAndReturnsOk(int initialDirection, int expectedDirection)
    {
        var initialDirectionEnum = (Direction)initialDirection;
        var expectedDirectionEnum = (Direction)expectedDirection;
        this.Given(s => s.ASession())
            .When(s => s.PlaceIsCalledWith(0, 0, initialDirectionEnum))
            .And(s => s.LeftIsCalled())
            .And(s => s.ReportIsCalled())
            .Then(s => s.TheReportedPositionIs(0, 0, expectedDirectionEnum))
            .And(s => s.TheResultIs(CommandResult.Ok))
            .BDDfy();
    }

    [BddfyFact]
    public void CallingRightWithoutPlacementReturnsError()
    {
        this.Given(s => s.ASession())
            .When(s => s.RightIsCalled())
            .Then(s => s.TheResultIs(CommandResult.InitialPlacementMissing))
            .BDDfy();
    }

    [BddfyTheory]
    [InlineData(1, 2)]
    [InlineData(2, 3)]
    [InlineData(3, 4)]
    [InlineData(4, 1)]
    public void CallingRightUpdatesDirectionAndReturnsOk(int initialDirection, int expectedDirection)
    {
        var initialDirectionEnum = (Direction)initialDirection;
        var expectedDirectionEnum = (Direction)expectedDirection;
        this.Given(s => s.ASession())
            .When(s => s.PlaceIsCalledWith(0, 0, initialDirectionEnum))
            .And(s => s.RightIsCalled())
            .And(s => s.ReportIsCalled())
            .Then(s => s.TheReportedPositionIs(0, 0, expectedDirectionEnum))
            .And(s => s.TheResultIs(CommandResult.Ok))
            .BDDfy();
    }

    #region BDDfy
    #region Data
    private Session _session;
    private CommandResult _result;
    private Coordinates _reportedPosition;
    #endregion
    #region Given
    private void ASession()
    {
        _session = new Session();
    }
    #endregion
    #region When
    private void PlaceIsCalledWith(int xPosition, int yPosition, Direction direction)
    {
        _result = _session.Place(new Coordinates(xPosition, yPosition, direction));
    }
    private void ReportIsCalled()
    {
        _reportedPosition = _session.Report();
    }
    private void MoveIsCalled()
    {
        _result = _session.Move();
    }
    private void LeftIsCalled()
    {
        _result = _session.Left();
    }
    private void RightIsCalled()
    {
        _result = _session.Right();
    }
    #endregion
    #region Then
    private void TheResultIs(CommandResult result)
    {
        Assert.Equal(result, _result);
    }
    private void TheReportedPositionIs(int xPos, int yPos, Direction direction)
    {
        Assert.Equal(_reportedPosition.XPosition, xPos);
        Assert.Equal(_reportedPosition.YPosition, yPos);
        Assert.Equal(_reportedPosition.FDirection, direction);
    }
    private void TheReportedPositionIsNull()
    {
        Assert.Null(_reportedPosition);
    }
    #endregion
    #endregion
}