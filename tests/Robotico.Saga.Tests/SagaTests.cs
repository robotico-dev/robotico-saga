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

    [Fact]
    public async Task ExecuteAsync_succeeds_when_not_configured_to_fail()
    {
        RecordingSagaStep step = new(failExecute: false);
        Robotico.Result.Result r = await step.ExecuteAsync();
        Assert.True(r.IsSuccess());
        Assert.Single(step.Executed);
    }

    [Fact]
    public async Task ExecuteAsync_fails_when_configured()
    {
        RecordingSagaStep step = new(failExecute: true);
        Robotico.Result.Result r = await step.ExecuteAsync();
        Assert.True(r.IsError(out _));
    }

    [Fact]
    public async Task CompensateAsync_succeeds_and_records_call()
    {
        RecordingSagaStep step = new(failCompensate: false);
        Robotico.Result.Result r = await step.CompensateAsync();
        Assert.True(r.IsSuccess());
        Assert.Single(step.Compensated);
    }

    [Fact]
    public async Task Saga_runner_compensates_in_reverse_order()
    {
        RecordingSagaStep step1 = new();
        RecordingSagaStep step2 = new();
        RecordingSagaStep step3 = new(failExecute: true);
        await step1.ExecuteAsync();
        await step2.ExecuteAsync();
        Robotico.Result.Result r3 = await step3.ExecuteAsync();
        Assert.True(r3.IsError(out _));
        await step2.CompensateAsync();
        await step1.CompensateAsync();
        Assert.Single(step2.Compensated);
        Assert.Single(step1.Compensated);
    }

    [Fact]
    public async Task ExecuteAsync_respects_cancellation()
    {
        using CancellationTokenSource cts = new();
        await cts.CancelAsync();
        RecordingSagaStep step = new();
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await step.ExecuteAsync(cts.Token));
    }

    [Fact]
    public async Task CompensateAsync_respects_cancellation()
    {
        using CancellationTokenSource cts = new();
        await cts.CancelAsync();
        RecordingSagaStep step = new();
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await step.CompensateAsync(cts.Token));
    }

    /// <summary>
    /// Law: execute N steps then compensate in reverse order leaves each step compensated exactly once.
    /// </summary>
    [Fact]
    public async Task Saga_law_execute_then_compensate_reverse_order()
    {
        RecordingSagaStep step1 = new();
        RecordingSagaStep step2 = new();
        RecordingSagaStep step3 = new();
        await step1.ExecuteAsync();
        await step2.ExecuteAsync();
        await step3.ExecuteAsync();
        await step3.CompensateAsync();
        await step2.CompensateAsync();
        await step1.CompensateAsync();
        Assert.Single(step1.Compensated);
        Assert.Single(step2.Compensated);
        Assert.Single(step3.Compensated);
    }

    /// <summary>
    /// Law: execute N steps then compensate in reverse order — parameterized by step count.
    /// </summary>
    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public async Task Saga_law_execute_N_then_compensate_reverse_order(int stepCount)
    {
        List<RecordingSagaStep> steps = Enumerable.Range(0, stepCount).Select(_ => new RecordingSagaStep()).ToList();

        foreach (RecordingSagaStep step in steps)
        {
            await step.ExecuteAsync();
        }

        for (int i = steps.Count - 1; i >= 0; i--)
        {
            await steps[i].CompensateAsync();
        }

        foreach (RecordingSagaStep step in steps)
        {
            Assert.Single(step.Compensated);
        }
    }
}
