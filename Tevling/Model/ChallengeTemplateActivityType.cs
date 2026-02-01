using Tevling.Strava;

namespace Tevling.Model;

public class ChallengeTemplateActivityType
{
    public int Id { get; set; }
    public ActivityType ActivityType { get; set; }
    public double Multiplier { get; set; } = 1.0;
    
    public int ChallengeTemplateId { get; set; }
    public ChallengeTemplate? ChallengeTemplate { get; set; }
}
