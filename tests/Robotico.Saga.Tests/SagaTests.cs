using Robotico.Saga;
using Xunit;

namespace Robotico.Saga.Tests;

public sealed class SagaTests
{
    [Fact]
    public void ISagaStep_contract_exists()
    {
        Assert.True(typeof(ISagaStep).IsInterface);
    }
}
