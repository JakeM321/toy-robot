## ToyRobot CLI

This solution consists of three projects:

**ToyRobot.Domain**: Houses the two main layers of business logic:

1) Logic which controls the robot placement and movement across the table-top – contained in the `Session` object and its dependencies (`Robot` and `TableTop`).
2) A translation layer between (1) and the human-readable, text-based input and output required to use this logic in a CLI app or other text-based interface – contained in the `CommandLine` object.

At present, only layer (2), the `CommandLine`, is publicly exposed by the domain library. For the core robot logic to be reused in a different type of application without the CLI layer, the `Session` can be made public as well.

**ToyRobot.Application**: Simple console app for debugging the domain logic. Uses the `CommandLine` from the Domain, directly feeding user input and printing its output.

**ToyRobot.Domain.Tests**: Unit tests and component tests (XUnit) for the ToyRobot domain. See the `ComponentTests` file for proof of the system working as a whole. The component tests are the exact scenarios from the second page of the challenge worksheet.

**Concerning the unit test format**

The component tests use a simple format in which the input is submitted and the output is asserted against. E.g:

```[Fact]
public void ExampleCaseOne()
{
    Submit("PLACE 0,0,NORTH");
    Expect(Constants.Messages.Ok);

    Submit("MOVE");
    Expect(Constants.Messages.Ok);

    Submit("REPORT");
    Expect("0,1,NORTH");
}
```

The unit tests, on the other hand, use the TestStack.BDDfy fluent API for expressing the criteria in a given-then-when format:

```[BddfyFact]
public void CallingLeftWithoutPlacementReturnsError()
{
    this.Given(s => s.AMockSessionReportingMissingPlacement())
        .And(s => s.ACommandLine())
        .And(s => s.ACommand("LEFT"), "and the following command: \"{0}\"")
        .When(s => s.TheCommandIsSubmitted())
        .Then(s => s.TheSessionLeftMethodIsCalled())
        .And(s => s.ResponseIsReturned(Constants.Messages.CannotMoveRobotInitialPlacementMissing),
            "and the following response is returned: \"{0}\"")
        .BDDfy();
}
```

When these tests are executed, the runner will pretty-print the output like so:

```Scenario: Calling left without placement returns error
   	Given a mock session reporting missing placement
   	  And a command line
   	  and the following command: "LEFT"
   	When the command is submitted
   	Then the session left method is called
   	  and the following response is returned: "Cannot move robot that has not yet been placed"
```

In this example, the CLI layer is being tested with a mock `Session` that returns a “missing placement” result (`Result.InitialPlacementMissing`) when `Left()` is called. The test is asserting that the CLI layer correctly invokes the `Left()` method of the `Session`, and that the correct CLI message is returned to the user when this `Session` responds with `Result.InitialPlacementMissing`.
