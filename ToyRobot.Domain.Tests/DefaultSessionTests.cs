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
        this.Given(s => s.ADefaultSession())
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
        this.Given(s => s.ADefaultSession())
            .And(s => s.ACommand(command), "and the following command: \"{0}\"")
            .When(s => s.TheCommandIsSubmitted())
            .Then(s => s.ResponseIsReturned(expectedResponse), "The following response is returned: \"{0}\"")
            .BDDfy();
    }

    #region BDDfy
    #region Data
    private DefaultSession _defaultSession;
    private string _command;
    private string _response;
    #endregion
    #region Given
    private void ADefaultSession()
    {
        _defaultSession = new DefaultSession();
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
    #endregion
    #endregion
}