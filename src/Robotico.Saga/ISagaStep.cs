namespace Robotico.Saga;

/// <summary>
/// A single step in a saga. Execute forwards; compensate reverses on failure.
/// </summary>
/// <remarks>
/// The saga runner executes steps in order; on ExecuteAsync failure, it calls CompensateAsync for previously completed steps in reverse order.
/// See docs/design.adoc for orchestration vs choreography and related packages (Robotico.Repository, Robotico.Resilience, Robotico.Events).
/// </remarks>
public interface ISagaStep
{
    /// <summary>
    /// Executes the step. Returns Result; on failure, compensation may run for prior steps.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Success to continue; failure to trigger compensation of prior steps.</returns>
    /// <exception cref="OperationCanceledException">Thrown when <paramref name="cancellationToken"/> is cancelled.</exception>
    Task<Robotico.Result.Result> ExecuteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Compensates this step (undo). Called when a later step fails.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result of the compensation; should be idempotent when retried.</returns>
    /// <exception cref="OperationCanceledException">Thrown when <paramref name="cancellationToken"/> is cancelled.</exception>
    Task<Robotico.Result.Result> CompensateAsync(CancellationToken cancellationToken = default);
}
