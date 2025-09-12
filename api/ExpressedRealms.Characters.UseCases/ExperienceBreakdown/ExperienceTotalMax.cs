namespace ExpressedRealms.Characters.UseCases.ExperienceBreakdown;

public record ExperienceTotalMax(string Name, int Total, int Max, bool IncludeInTotal = true, bool IncludeInMax = true )
{
}