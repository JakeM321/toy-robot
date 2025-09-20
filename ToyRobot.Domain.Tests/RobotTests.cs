using Moq;
using TestStack.BDDfy;
using TestStack.BDDfy.Xunit;

namespace ToyRobot.Domain.Tests;

public class RobotTests
{
    [BddfyTheory]
    [InlineData(2, 2, 1, 2, 3)]
    [InlineData(2, 2, 2, 3, 2)]
    [InlineData(2, 2, 3, 2, 1)]
    [InlineData(2, 2, 4, 1, 2)]
    public void NextMoveIsCorrectlyCalculated(int initialXPos, int initialYPos, int direction, int expectedXPos, int expectedYPos)
    {
        this.Given(s => s.AMovementCoordinatorAllowingAnyMove())
            .And(s => s.ARobotPlacedAt(initialXPos, initialYPos, (Direction)direction))
            .When(s => s.TheRobotTryMoveMethodIsInvoked())
            .Then(s => s.ResultIsSuccessful())
            .And(s => s.RobotPositionIs(expectedXPos, expectedYPos, (Direction)direction), "The new position of the robot is: {0}, {1}, {2}")
            .BDDfy();
    }

    [BddfyFact]
    public void IllegalMovesAreDiscarded()
    {
        this.Given(s => s.AMovementCoordinatorNotAllowingAnyMove())
            .And(s => s.ARobotPlacedAt(0, 0, Direction.North))
            .When(s => s.TheRobotTryMoveMethodIsInvoked())
            .Then(s => s.ResultIsUnsuccessful())
            .And(s => s.RobotPositionIs(0, 0, Direction.North), "And the robot's position remains unchanged")
            .BDDfy();
    }

    #region BDDfy
    #region Data
    private Mock<IMovementCoordinator> _movementCoordinator;
    private Robot _robot;
    private bool _tryMoveResult;
    #endregion
    #region Given

    private void AMovementCoordinatorAllowingAnyMove()
    {
        _movementCoordinator = new Mock<IMovementCoordinator>();
        _movementCoordinator
            .Setup(c => c.IsMoveLegal(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
    }
    private void AMovementCoordinatorNotAllowingAnyMove()
    {
        _movementCoordinator = new Mock<IMovementCoordinator>();
        _movementCoordinator
            .Setup(c => c.IsMoveLegal(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
    }
    private void ARobotPlacedAt(int initialX, int initialY, Direction initialF)
    {
        _robot = new Robot(_movementCoordinator.Object, initialX, initialY, initialF);
    }
    #endregion
    #region When
    private void TheRobotTryMoveMethodIsInvoked()
    {
        _tryMoveResult = _robot.TryMove();
    }
    #endregion
    #region Then
    private void ResultIsSuccessful()
    {
        Assert.True(_tryMoveResult);
    }
    private void ResultIsUnsuccessful()
    {
        Assert.False(_tryMoveResult);
    }
    private void RobotPositionIs(int posX, int posY, Direction dirF)
    {
        Assert.Equal(posX, _robot.PositionX);
        Assert.Equal(posY, _robot.PositionY);
        Assert.Equal(dirF, _robot.DirectionFacing);
    }
    #endregion
    #endregion
}