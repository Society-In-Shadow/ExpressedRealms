using Quartz;

namespace ExpressedRealms.Server.CronJobs;

public static class QuartzConfiguration
{
    public static void SetupQuartz(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<EventCronJob>();

        builder.Services.AddQuartz(q =>
        {
            q.AddJob<EventCronJob>(opts => opts.WithIdentity(EventCronJob.Key))
                .AddTrigger(opts =>
                    opts.ForJob(EventCronJob.Key)
                        .WithIdentity("event-trigger", "cron-group")
                        .WithCronSchedule(
                            "0 0 7 1/1 * ? *",
                            x =>
                                x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("America/Chicago"))
                        )
                ); // Every day around 7am
        });
        builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
    }
}
