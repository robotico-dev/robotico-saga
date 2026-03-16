namespace Robotico.Saga;

/// <summary>
/// A single step in a saga. Execute forwards; compensate reverses on failure.
/// </summary>
public interface ISagaStep
{
    /// <summary>
    /// Executes the step. Returns Result; on failure, compensation may run for prior steps.
    /// </summary>
    Task<Robotico.Result.Result> ExecuteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Compensates this step (undo). Called when a later step fails.
    /// </summary>
    Task<Robotico.Result.Result> CompensateAsync(CancellationToken cancellationToken = default);
}
