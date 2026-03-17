using System.Collections.ObjectModel;
using Robotico.Result.Errors;
using Robotico.Saga;

namespace Robotico.Saga.Tests;

/// <summary>
/// Test double that records Execute/Compensate calls and can be configured to fail on a given step.
/// </summary>
public sealed class RecordingSagaStep : ISagaStep
{
    private readonly bool _failExecute;
    private readonly bool _failCompensate;

    public RecordingSagaStep(bool failExecute = false, bool failCompensate = false)
    {
        _failExecute = failExecute;
        _failCompensate = failCompensate;
    }

    public Collection<string> Executed { get; } = [];
    public Collection<string> Compensated { get; } = [];

    public Task<Robotico.Result.Result> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Executed.Add(nameof(ExecuteAsync));
        if (_failExecute)
        {
            return Task.FromResult(Robotico.Result.Result.Error(new SimpleError("Step failed.")));
        }

        return Task.FromResult(Robotico.Result.Result.Success());
    }

    public Task<Robotico.Result.Result> CompensateAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Compensated.Add(nameof(CompensateAsync));
        if (_failCompensate)
        {
            return Task.FromResult(Robotico.Result.Result.Error(new SimpleError("Compensate failed.")));
        }

        return Task.FromResult(Robotico.Result.Result.Success());
    }
}
