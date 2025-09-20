using TestStack.BDDfy;
using TestStack.BDDfy.Xunit;

namespace ToyRobot.Domain.Tests;

public class DefaultControllerTests
{
    [BddfyTheory]
    [InlineData(-1, 0)]
    [InlineData(5, 5)]
    [InlineData(8, 10)]
    public void PlacingRobotOutsideOfDefaultBoundaryReturnsOutOfBoundsResult(int xPos, int yPos)
    {
        this.Given(s => s.ADefaultController())
            .When(s => s.PlaceIsCalledWith(xPos, yPos, Direction.North))
            .Then(s => s.TheResultIs(Result.OutOfBounds))
            .BDDfy();
    }

    [BddfyTheory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(2, 2)]
    [InlineData(4, 4)]
    public void PlacingRobotWithinDefaultBoundaryReturnsOkResult(int xPos, int yPos)
    {
        this.Given(s => s.ADefaultController())
            .When(s => s.PlaceIsCalledWith(xPos, yPos, Direction.North))
            .Then(s => s.TheResultIs(Result.Ok))
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
        this.Given(s => s.ADefaultController())
            .When(s => s.PlaceIsCalledWith(xPos, yPos, directionEnum))
            .And(s => s.ReportIsCalled())
            .Then(s => s.TheReportedPositionIs(xPos, yPos, directionEnum))
            .BDDfy();
    }

    [BddfyFact]
    public void CallingReportWithoutPlacementReturnsNull()
    {
        this.Given(s => s.ADefaultController())
            .When(s => s.ReportIsCalled())
            .Then(s => s.TheReportedPositionIsNull())
            .BDDfy();
    }

    [BddfyFact]
    public void CallingMoveWithoutPlacementReturnsError()
    {
        this.Given(s => s.ADefaultController())
            .When(s => s.MoveIsCalled())
            .Then(s => s.TheResultIs(Result.InitialPlacementMissing))
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
        this.Given(s => s.ADefaultController())
            .When(s => s.PlaceIsCalledWith(initialXPos, initialYPos, directionEnum))
            .And(s => s.MoveIsCalled())
            .And(s => s.ReportIsCalled())
            .Then(s => s.TheResultIs(Result.Ok))
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
        this.Given(s => s.ADefaultController())
            .When(s => s.PlaceIsCalledWith(initialXPos, initialYPos, directionEnum))
            .And(s => s.MoveIsCalled())
            .Then(s => s.TheResultIs(Result.OutOfBounds))
            .BDDfy();
    }

    #region BDDfy
    #region Data
    private DefaultController _controller;
    private Result _result;
    private Coordinates _reportedPosition;
    #endregion
    #region Given
    private void ADefaultController()
    {
        _controller = new DefaultController();
    }
    #endregion
    #region When
    private void PlaceIsCalledWith(int xPosition, int yPosition, Direction direction)
    {
        _result = _controller.Place(new Coordinates(xPosition, yPosition, direction));
    }
    private void ReportIsCalled()
    {
        _reportedPosition = _controller.Report();
    }
    private void MoveIsCalled()
    {
        _result = _controller.Move();
    }
    #endregion
    #region Then
    private void TheResultIs(Result result)
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