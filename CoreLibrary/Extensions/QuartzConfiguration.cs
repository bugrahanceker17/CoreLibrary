using Microsoft.Extensions.Configuration;
using Quartz;

namespace CoreLibrary.Extensions;

public static class QuartzConfiguration
{
    public static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz, IConfiguration configuration) where T : IJob
    {
        string jobName = typeof(T).Name;

        string configKey = $"Quartz:{jobName}";
        string cronSchedule = configuration[configKey];

        if (string.IsNullOrEmpty(cronSchedule)) throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {configKey}");

        JobKey jobKey = new(jobName);
        quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

        quartz.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity($"{jobName}-trigger")
            .WithCronSchedule(cronSchedule));
    }
}