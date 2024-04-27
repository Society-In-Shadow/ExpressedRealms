namespace ExpressedRealms.Server.EndPoints.NavigationEndpoints.Responses;

public record CharacterNavResponse(int Id, string Name, string Expression, string Background)
{
    public string Background { get; set; } = Background;
}
