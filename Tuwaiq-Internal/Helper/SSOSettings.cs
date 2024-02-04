namespace TuwaiqRecruitment.Helper;

public class SSOSettings
{
    public string IdentityServerUrl { get; set; } = null!;
    // public string AddAudience { get; set; }= null!;
    public string ClientId { get; set; }= null!;
    public string ClientSecret { get; set; }= null!;
    public string[] Scopes { get; set; }= null!;
}