using System.Diagnostics;
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
    private const string FullDateFormat = "dddd MMMM d, yyyy";
    private const string TimeFormat = "hh:mm tt";

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
        if (
            model.PublishType == PublishType.DayOfReminder
            && scheduleItems.All(x => x.Date != today)
        )
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
                    $"We are happy to announce that we will be attending **{currentEvent.Name}** out in the **{currentEvent.Location}**!"
                );
                message.AppendLine(
                    $"Their scheduled dates are **{currentEvent.StartDate.ToString(FullDateFormat)}** to **{currentEvent.EndDate.ToString(FullDateFormat)}**."
                );
                message.AppendLine();
                message.AppendLine("More information can be found on their website below!");

                if (!string.IsNullOrWhiteSpace(currentEvent.AdditionalNotes))
                {
                    message.AppendLine();
                    message.AppendLine(currentEvent.AdditionalNotes.Trim());
                    message.AppendLine();
                }

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
                    $"As a reminder, we will be attending **{currentEvent.Name}** out in the **{currentEvent.Location}**!"
                );
                message.AppendLine(
                    $"Their dates are **{currentEvent.StartDate.ToString(FullDateFormat)}** to **{currentEvent.EndDate.ToString(FullDateFormat)}**."
                );
                message.AppendLine();
                message.AppendLine("More information can be found on their website below!");

                if (!string.IsNullOrWhiteSpace(currentEvent.AdditionalNotes))
                {
                    message.AppendLine();
                    message.AppendLine(currentEvent.AdditionalNotes.Trim());
                    message.AppendLine();
                }

                AppendEventAttendanceMessage(scheduleItems, currentEvent, message);

                message.AppendLine(
                    $"Players now have access to an additional **{currentEvent.ConExperience} XP** for this event on their Primary Characters!"
                );

                GenerateScheduleMessage(scheduleItems, currentEvent, message);

                break;
            case PublishType.DayOfReminder:

                HandleDailyReminderMessages(scheduleItems, message, currentEvent);

                message.AppendLine();
                GenerateScheduleMessage(scheduleItems, currentEvent, message, true);

                break;
            default:
                throw new UnreachableException();
        }

        message.AppendLine();

        var siteEmbed = new EmbedBuilder()
            .WithTitle(currentEvent!.WebsiteName)
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

        if (model.PublishType == PublishType.InitialAnnouncement)
        {
            await discordService.CreateEventAsync(
                new DiscordEvent()
                {
                    Name = currentEvent.Name,
                    Location = currentEvent.Location,
                    StartDate = currentEvent.StartDate.ToUtc(currentEvent.TimeZoneId),
                    EndDate = currentEvent.EndDate.ToUtc(currentEvent.TimeZoneId),
                }
            );
        }

        return Result.Ok();
    }

    private void HandleDailyReminderMessages(
        List<EventScheduleItem> scheduleItems,
        StringBuilder message,
        Event? currentEvent
    )
    {
        var dayGroups = scheduleItems
            .OrderBy(x => x.Date)
            .ThenBy(x => x.StartTime)
            .GroupBy(x => x.Date)
            .ToList();

        var todayDate = DateOnly.FromDateTime(systemClock.GetUtcNow().DateTime);
        var dayIndex = dayGroups.FindIndex(x => x.Key == todayDate);

        switch (dayIndex)
        {
            case 0:
                message.AppendLine($"# Welcome to our First day at {currentEvent!.Name}!");
                message.AppendLine(
                    $"As a reminder, we will be attending **{currentEvent.Name}** out in the **{currentEvent.Location}**!"
                );
                message.AppendLine(
                    $"Directions to **{currentEvent.Name}** is attached down below!"
                );

                message.AppendLine();
                message.AppendLine(
                    "With the start of the event, when you log into your account, you will see a **\"Event Check-in\"** banner at the top of the page."
                );
                message.AppendLine(
                    "At it's core, this is where you will find information regarding what you need to do to get started, and as you progress through our check-in process, what you need to do next."
                );

                message.AppendLine();
                message.AppendLine(
                    "With that out of the way, have a safe trip to the convention, no need to rush on your part, we'll be here all night."
                );
                message.AppendLine();
                message.AppendLine("Once you do arrive and get settled in, stop by SHQ to:");
                message.AppendLine(
                    "* **Exchange Friends!** - Bring a Friend who is new to the game and gain 5xp (once per event)"
                );
                message.AppendLine(
                    "* **Character Review!** - Get checked in, so you can corner a GO (Game Official) to review and approve your character"
                );
                message.AppendLine(
                    "* **Booklets!** - Get a CRB (Character Reference Booklet) free of charge, which you use to play the game"
                );
                message.AppendLine("* **Catch up!** - We love to see both old and new players!");

                message.AppendLine();
                message.AppendLine(
                    "This is an exciting weekend for us, and we have lots of ground to cover!"
                );
                break;
            case 1:
                message.AppendLine($"# Welcome to day 2 at {currentEvent!.Name}!");

                message.AppendLine();
                message.AppendLine(
                    $"If you just arrived at the realm, no worries, please see yesterdays message to get started!"
                );

                message.AppendLine();
                message.AppendLine(
                    $"For those of you who managed to survive the first night, please stop by SHQ to get:"
                );

                message.AppendLine(
                    "* **Break of Dawn** - Congrats, you survived yesterday, you wake up slightly more energized!"
                );
                message.AppendLine(
                    "* **Recharges** - Some of you might still feel a little lacking, get your pick-me-up at the Nexus (SHQ)"
                );
                message.AppendLine(
                    "* **Checked Out** - Even if you survived without any sort of peril, we still need to check you out"
                );

                message.AppendLine();
                message.AppendLine(
                    "We have lots in store for you guys today, and we are excited to where we wind up."
                );

                message.AppendLine();
                message.AppendLine(
                    "As a reminder, we have a social hour at the end of the day for everyone to play games and catch up!"
                );
                break;
            case 2:
                message.AppendLine($"# Welcome to our Last Day at {currentEvent!.Name}!");

                message.AppendLine();
                message.AppendLine(
                    $"This is the last day, you gotta gather your wits and make it through, we are so close to getting out of here."
                );
                message.AppendLine();
                message.AppendLine(
                    $"Please be aware that we do have a hard stop of accepting new players at noon."
                );

                message.AppendLine();
                message.AppendLine(
                    $"For those of you who managed to survive the second night, please stop by SHQ to get:"
                );

                message.AppendLine(
                    "* **Break of Dawn** - Congrats, you survived yesterday, you wake up slightly more energized!"
                );
                message.AppendLine(
                    "* **Recharges** - Some of you might still feel a little lacking, get your pick-me-up at the Nexus (SHQ)"
                );
                message.AppendLine(
                    "* **Checked Out** - Even if you survived without any sort of peril, we still need to check you out"
                );

                message.AppendLine();
                message.AppendLine(
                    "We have lots in store for you guys today, and we are excited to where we wind up."
                );

                message.AppendLine();
                message.AppendLine(
                    "As a reminder, we have rewards / wrap up ceremony at the end of the day, see the schedule below!"
                );
                break;
        }
    }

    private void GenerateScheduleMessage(
        List<EventScheduleItem> scheduleItems,
        Event currentEvent,
        StringBuilder message,
        bool isTodayOnly = false
    )
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

        if (isTodayOnly)
        {
            var today = DateOnly.FromDateTime(systemClock.GetUtcNow().DateTime);
            dayGroups = dayGroups.Where(x => x.Key == today).ToList();
        }

        foreach (var dayGroup in dayGroups)
        {
            message.AppendLine($"## {dayGroup.Key.ToString("dddd")}");
            message.AppendLine();
            foreach (var scheduleItem in dayGroup)
            {
                message.AppendLine(
                    $"**{scheduleItem.StartTime.ToString(TimeFormat)}** - {scheduleItem.EndTime.ToString(TimeFormat)} - **{scheduleItem.Description}**"
                );
            }
        }
    }

    /// <summary>
    /// We'll be attending every day, or a subset of days
    /// </summary>
    private static void AppendEventAttendanceMessage(
        List<EventScheduleItem> scheduleItems,
        Event currentEvent,
        StringBuilder message
    )
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
