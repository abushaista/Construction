using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Domain.Config;

public class RabbitMqSettings
{
    public string Host { get; set; }
    public ushort Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public TimeoutSettings TimeoutSettings { get; set; }
    public KillSwitchSettings KillSwitchSettings { get; set; }
    public CircuitBreakerSettings CircuitBreakerSettings { get; set; }
    public RateLimiterSettings RateLimiterSettings { get; set; }
    public PrefetchSettings PrefetchSettings { get; set; }
    public ushort? DefaultQuorumSize { get; set; }
    public ushort? BusQuorumSize { get; set; }
}

public class PrefetchSettings
{
    public int GlobalPrefetchCount { get; set; }
    public int BusPrefetchCount { get; set; }
    public Dictionary<string, int> PerConsumerConfig { get; set; }
}

public class ConsumerPrefetchSetting
{
    public string ConsumerName { get; set; }
    public int PrefetchCount { get; set; }
}

public class KillSwitchSettings
{
    public int TrackingPeriodInSeconds { get; set; }
    public int TripThresholdInPercentage { get; set; }
    public int ActivationThreshold { get; set; }
    public int RestartTimeoutInSeconds { get; set; }
}

public class CircuitBreakerSettings
{
    public int TrackingPeriodInSeconds { get; set; }
    public int TripThresholdInPercentage { get; set; }
    public int ActiveThreshold { get; set; }
    public int ResetIntervalInSeconds { get; set; }
}

public class RateLimiterSettings
{
    public int RateLimit { get; set; }
    public int IntervalInSeconds { get; set; }
}
