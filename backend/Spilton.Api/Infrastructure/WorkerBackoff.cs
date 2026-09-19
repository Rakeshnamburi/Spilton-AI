namespace Spilton.Api.Infrastructure;

/// <summary>
/// Tracks consecutive hosted-service failures and returns a delay that grows
/// exponentially up to a fixed ceiling. A successful iteration resets the
/// sequence so a recovered dependency is polled at its normal interval.
/// </summary>
public sealed class WorkerBackoff(TimeSpan initialDelay, TimeSpan maximumDelay)
{
    private int consecutiveFailures;

    public int ConsecutiveFailures => consecutiveFailures;

    public TimeSpan NextFailureDelay()
    {
        if (initialDelay <= TimeSpan.Zero)
            throw new InvalidOperationException("The initial worker retry delay must be positive.");
        if (maximumDelay < initialDelay)
            throw new InvalidOperationException("The maximum worker retry delay must be at least the initial delay.");

        consecutiveFailures = Math.Min(consecutiveFailures + 1, 31);
        var multiplier = 1L << Math.Min(consecutiveFailures - 1, 30);
        var maximumTicks = maximumDelay.Ticks;
        var delayTicks = initialDelay.Ticks > maximumTicks / multiplier
            ? maximumTicks
            : initialDelay.Ticks * multiplier;
        return TimeSpan.FromTicks(Math.Min(delayTicks, maximumTicks));
    }

    public void Reset() => consecutiveFailures = 0;
}
