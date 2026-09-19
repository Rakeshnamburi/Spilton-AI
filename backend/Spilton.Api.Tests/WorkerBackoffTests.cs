using Spilton.Api.Infrastructure;

namespace Spilton.Api.Tests;

public sealed class WorkerBackoffTests
{
    [Fact]
    public void Failure_delay_grows_exponentially_and_stops_at_ceiling()
    {
        var backoff = new WorkerBackoff(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10));

        Assert.Equal(TimeSpan.FromSeconds(2), backoff.NextFailureDelay());
        Assert.Equal(TimeSpan.FromSeconds(4), backoff.NextFailureDelay());
        Assert.Equal(TimeSpan.FromSeconds(8), backoff.NextFailureDelay());
        Assert.Equal(TimeSpan.FromSeconds(10), backoff.NextFailureDelay());
        Assert.Equal(TimeSpan.FromSeconds(10), backoff.NextFailureDelay());
        Assert.Equal(5, backoff.ConsecutiveFailures);
    }

    [Fact]
    public void Success_reset_returns_next_failure_to_initial_delay()
    {
        var backoff = new WorkerBackoff(TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1));
        _ = backoff.NextFailureDelay();
        _ = backoff.NextFailureDelay();

        backoff.Reset();

        Assert.Equal(0, backoff.ConsecutiveFailures);
        Assert.Equal(TimeSpan.FromSeconds(1), backoff.NextFailureDelay());
    }

    [Fact]
    public void Invalid_bounds_are_rejected_before_a_delay_is_used()
    {
        Assert.Throws<InvalidOperationException>(() => new WorkerBackoff(TimeSpan.Zero, TimeSpan.FromSeconds(1)).NextFailureDelay());
        Assert.Throws<InvalidOperationException>(() => new WorkerBackoff(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(1)).NextFailureDelay());
    }
}
