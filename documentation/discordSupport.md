# Discord
Expressed Realms utilizes a Discord Bot to send out event updates and scheduled events.

As of now, it will send a message to the #announcements channel when a new event is published.
It will also send reminder messages for the event, 1 month out, and 1 week out.
When the event is active, it will send a daily message talking about the day, and send 15 minute reminders about
scheduled events.

## Local Setup
Discord support is disabled by default locally. To enable it, you will need to add a Discord Bot Token to your .env file.

```ini
DISCORD_BOT_TOKEN=<TOKEN>
```
