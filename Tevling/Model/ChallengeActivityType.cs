using Tevling.Strava;

namespace Tevling.Model;

public class ChallengeActivityType
{
    public int Id { get; set; }
    public ActivityType ActivityType { get; set; }
    public double Multiplier { get; set; } = 1.0;
    
    public int ChallengeId { get; set; }
    public Challenge? Challenge { get; set; }
}
