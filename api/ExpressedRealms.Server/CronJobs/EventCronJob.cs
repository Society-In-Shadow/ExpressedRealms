using ExpressedRealms.Events.API.UseCases.Events.TriggerEventReminder;
using Quartz;

namespace ExpressedRealms.Server.CronJobs;

public class EventCronJob(IEventReminderHandlerUseCase eventReminderHandler) : IJob
{
    public static readonly JobKey Key = new JobKey("job-name", "group-name");

    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        await eventReminderHandler.ExecuteAsync();
    }
}
