using System.Net;
using System.Text;
using Discord;
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

        var message = new StringBuilder();

        message.AppendLine($"# {currentEvent!.Name} Has Been Confirmed!");
        message.AppendLine(
            $"We are happy to announce that we will be attending **{currentEvent!.Name}** out in the **{currentEvent.Location}**!"
        );
        message.AppendLine(
            $"Their scheduled dates are **{currentEvent.StartDate.ToString("dddd MMMM d, yyyy")}** to **{currentEvent.EndDate.ToString("dddd MMMM d, yyyy")}**."
        );
        message.AppendLine();
        message.AppendLine("More information can be found on their website below!");

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

        if (model.PublishType != PublishType.InitialAnnouncement)
        {
            message.AppendLine(
                $"Once the event starts, everyone will have access to an additional **{currentEvent.ConExperience} XP** for this event."
            );
        }

        if (model.PublishType == PublishType.DayOfReminder)
        {
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
        }

        if (model.PublishType != PublishType.InitialAnnouncement)
        {
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

        await discordService.SendMessageToChannelAsync(
            DiscordChannel.PublicAnnouncements,
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
}
