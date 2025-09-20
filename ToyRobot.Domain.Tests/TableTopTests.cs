using TestStack.BDDfy;
using TestStack.BDDfy.Xunit;

namespace ToyRobot.Domain.Tests;

public class TableTopTests
{
    [BddfyTheory]
    [InlineData(5, 5, 1, 2)]
    [InlineData(5, 5, 4, 4)]
    [InlineData(10, 8, 3, 3)]
    public void ProposedMovementsValidatedCorrectly(int tableWidth, int tableHeight, int proposedXPos, int proposedYPos)
    {
        this.Given(s => s.ATableTop(tableWidth, tableHeight), "A table of {0} x {1}")
            .When(s => s.IsMoveLegalCalled(proposedXPos, proposedYPos), "When IsMoveLegal is called with {0},{1}")
            .Then(s => s.TheProposalIsConfirmed())
            .BDDfy();
    }

    [BddfyTheory]
    [InlineData(5, 5, 5, 5)]
    [InlineData(5, 5, 1, 5)]
    [InlineData(10, 10, 11, 5)]
    public void ProposedMovementsRejectedCorrectly(int tableWidth, int tableHeight, int proposedXPos, int proposedYPos)
    {
        this.Given(s => s.ATableTop(tableWidth, tableHeight), "A table of {0} x {1}")
            .When(s => s.IsMoveLegalCalled(proposedXPos, proposedYPos), "When IsMoveLegal is called with {0},{1}")
            .Then(s => s.TheProposalIsRejected())
            .BDDfy();
    }

    #region BDDfy
    #region Data
    private TableTop _tableTop;
    private bool _result;
    #endregion
    #region Given
    private void ATableTop(int tableWidth, int tableHeight)
    {
        _tableTop =  new TableTop(tableWidth, tableHeight);
    }
    #endregion
    #region When
    private void IsMoveLegalCalled(int x, int y)
    {
        _result = _tableTop.IsMoveLegal(x, y);
    }
    #endregion
    #region Then
    private void TheProposalIsRejected()
    {
        Assert.False(_result);
    }
    private void TheProposalIsConfirmed()
    {
        Assert.True(_result);
    }
    #endregion
    #endregion
}