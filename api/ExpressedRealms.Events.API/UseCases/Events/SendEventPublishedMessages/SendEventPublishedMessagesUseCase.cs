using System.Net;
using System.Text;
using Discord;
using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Discord;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Shared;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using TimeZoneConverter;

namespace ExpressedRealms.Events.API.UseCases.Events.SendEventPublishedMessages;

internal sealed class SendEventPublishedMessagesUseCase(
    IEventRepository eventRepository,
    SendEventPublishedMessagesModelValidator validator,
    IDiscordService discordService,
    TimeProvider systemClock,
    CancellationToken cancellationToken
) : ISendEventPublishedMessagesUseCase
{
    public async Task<Result> ExecuteAsync(SendEventPublishedMessagesModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var currentEvent = await eventRepository.FindEventAsync(model.Id);
        var scheduleItems = await eventRepository.GetEventScheduleItems(model.Id);
        
        var today = DateOnly.FromDateTime(systemClock.GetUtcNow().DateTime);
        if (model.PublishType == PublishType.DayOfReminder && scheduleItems.All(x => x.Date != today))
        {
            return Result.Ok();
        }

        var message = new StringBuilder();

        var isInternalReminder = model.PublishType == PublishType.InternalReminder;
        if (model.PublishType == PublishType.InternalReminder)
        {
            message.AppendLine("# Reminder to Review Schedule!");
            message.AppendLine("The following message will be sent out in a week.");
            message.AppendLine();
            model.PublishType = PublishType.OneMonthReminder;
        }

        switch (model.PublishType)
        {
            case PublishType.InternalReminder:
                // Intentionally left blank
                break;
            case PublishType.InitialAnnouncement:
                message.AppendLine($"# {currentEvent!.Name} Has Been Confirmed!");
                message.AppendLine(
                    $"We are happy to announce that we will be attending **{currentEvent!.Name}** out in the **{currentEvent.Location}**!"
                );
                message.AppendLine(
                    $"Their scheduled dates are **{currentEvent.StartDate.ToString("dddd MMMM d, yyyy")}** to **{currentEvent.EndDate.ToString("dddd MMMM d, yyyy")}**."
                );
                message.AppendLine();
                message.AppendLine("More information can be found on their website below!");
                
                AppendEventAttendanceMessage(scheduleItems, currentEvent, message);
                
                message.AppendLine(
                    "The daily schedule and assigned XP will be provided one month out from the event!"
                );
                
                break;
            case PublishType.OneMonthReminder:
            case PublishType.OneWeekReminder:

                if (model.PublishType == PublishType.OneMonthReminder)
                {
                    message.AppendLine($"# {currentEvent!.Name} is about a month out!");
                }
                else
                {
                    message.AppendLine($"# {currentEvent!.Name} is about a week away!");
                }
                
                message.AppendLine(
                    $"As a reminder, we will be attending **{currentEvent!.Name}** out in the **{currentEvent.Location}**!"
                );
                message.AppendLine(
                    $"Their dates are **{currentEvent.StartDate.ToString("dddd MMMM d, yyyy")}** to **{currentEvent.EndDate.ToString("dddd MMMM d, yyyy")}**."
                );
                message.AppendLine();
                message.AppendLine("More information can be found on their website below!");
                
                AppendEventAttendanceMessage(scheduleItems, currentEvent, message);

                message.AppendLine(
                    $"Players now have access to an additional **{currentEvent.ConExperience} XP** for this event on their Primary Characters!"
                );
                
                GenerateScheduleMessage(scheduleItems, currentEvent, message);
                
                break;
            case PublishType.DayOfReminder:
                
                // First day message
                // Something about making sure their character is up to date online
                // Something about how we will be starting later
                // Something about Checkin XP Bonus
                // Something about checking in with the convention
                // 
                // Middle Day Message
                
                // Last Day Message 
                
                message.AppendLine($"# One Week Reminder for ${currentEvent.Name}!");
                message.AppendLine(
                    $"As a reminder, we will be attending **{currentEvent!.Name}** out in the **{currentEvent.Location}**!"
                );
                message.AppendLine(
                    $"Their dates are **{currentEvent.StartDate.ToString("dddd MMMM d, yyyy")}** to **{currentEvent.EndDate.ToString("dddd MMMM d, yyyy")}**."
                );
                message.AppendLine();
                message.AppendLine("More information can be found on their website below!");
                
                AppendEventAttendanceMessage(scheduleItems, currentEvent, message);

                message.AppendLine(
                    $"Event XP has been automatically assigned out to all players, in this case you are getting **{currentEvent.ConExperience} XP** for this event."
                );
                
                message.AppendLine();
                message.AppendLine(
                    "** We do need you to check in at our booth (SHQ) for the following reasons:**"
                );
                message.AppendLine(
                    "* **Bonus XP!** - We will be giving out a bonus XP for attending the event up to 5 XP.  If you bring a new player, you will get the full bonus"
                );
                message.AppendLine(
                    "* **Booklets!** - Every player needs a booklet in order to participate in the game.  These are provided to you at no cost."
                );
                message.AppendLine("* **Catching Up!** - We love to see both old and new players!");
                message.AppendLine(
                    "* **Character Help!** - If you have questions about anything regarding your character, we will be happy to help you out."
                );
                message.AppendLine(
                    "* **Boring Stuff!** - A lot of cons want us to keep track of who is playing the game, so we will need to collect badge information."
                );
                
                GenerateScheduleMessage(scheduleItems, currentEvent, message);
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        message.AppendLine();

        var siteEmbed = new EmbedBuilder()
            .WithTitle(currentEvent.WebsiteName)
            .WithUrl(currentEvent.WebsiteUrl)
            .WithDescription($"{currentEvent.Name} Website!")
            .WithColor(Color.Blue)
            .Build();

        var locationEmbed = new EmbedBuilder()
            .WithTitle(currentEvent.Location)
            .WithUrl(
                $"https://www.google.com/maps/search/?api=1&query={WebUtility.UrlEncode(currentEvent.Location)}"
            )
            .WithDescription($"{currentEvent.Name} Location!")
            .WithColor(Color.Blue)
            .Build();

        var channel = DiscordChannel.PublicAnnouncements;
        if (isInternalReminder)
        {
            channel = DiscordChannel.DevGeneralChannel;
        }
        
        await discordService.SendMessageToChannelAsync(
            channel,
            message.ToString(),
            embeds: [siteEmbed, locationEmbed]
        );

        await discordService.CreateEventAsync(
            new DiscordEvent()
            {
                Name = currentEvent.Name,
                Location = currentEvent.Location,
                StartDate = currentEvent.StartDate.ToUtc(currentEvent.TimeZoneId),
                EndDate = currentEvent.EndDate.ToUtc(currentEvent.TimeZoneId),
            }
        );

        return Result.Ok();
    }

    private static void GenerateScheduleMessage(List<EventScheduleItem> scheduleItems, Event? currentEvent, StringBuilder message)
    {
        
        var dayGroups = scheduleItems
            .OrderBy(x => x.Date)
            .ThenBy(x => x.StartTime)
            .GroupBy(x => x.Date)
            .ToList();
        
        message.AppendLine("# Schedule");
        message.AppendLine(
            $"{currentEvent.Name} is in the **{TZConvert.IanaToWindows(currentEvent.TimeZoneId)}** and our schedule below reflects that."
        );

        foreach (var dayGroup in dayGroups)
        {
            message.AppendLine($"## {dayGroup.Key.ToString("dddd")}");
            message.AppendLine();
            foreach (var scheduleItem in dayGroup)
            {
                message.AppendLine(
                    $"**{scheduleItem.StartTime.ToString("hh:mm tt")}** - {scheduleItem.EndTime.ToString("hh:mm tt")} - **{scheduleItem.Description}**"
                );
            }
        }
    }

    /// <summary>
    /// We'll be attending every day, or a subsest of days
    /// </summary>
    private static void AppendEventAttendanceMessage(List<EventScheduleItem> scheduleItems, Event? currentEvent, StringBuilder message)
    {
        var dayGroups = scheduleItems
            .OrderBy(x => x.Date)
            .ThenBy(x => x.StartTime)
            .GroupBy(x => x.Date)
            .ToList();
        
        var daysBetween =
            Math.Abs(currentEvent.EndDate.DayNumber - currentEvent.StartDate.DayNumber) + 1;
        if (dayGroups.Count == daysBetween)
        {
            message.AppendLine("# Society in Shadows will be there every day!");
        }
        else
        {
            var days = dayGroups.Select(x => x.Key.ToString("dddd")).ToList();
            var attendingDays = days.Count switch
            {
                0 => string.Empty,
                1 => days[0],
                2 => string.Join(" and ", days),
                _ => string.Join(", ", days.Take(dayGroups.Count - 1))
                     + " and "
                     + days[dayGroups.Count - 1],
            };
            message.AppendLine($"# Society in Shadows will be there on {attendingDays}!");
        }
    }
}
