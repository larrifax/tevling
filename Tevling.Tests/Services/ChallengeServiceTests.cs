using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using Tevling.Data;
using Tevling.Model;
using Tevling.Strava;
using Xunit;

namespace Tevling.Services;

public class ChallengeServiceTests
{
    [Fact]
    public async Task GetChallengesAsync_should_return_only_joined_challenges()
    {

    }

    [Fact]
    public async Task GetChallengesAsync_should_not_return_private_challenges()
    {

    }

    [Fact]
    public async Task GetChallengesAsync_should_return_private_challenges_if_invited()
    {

    }

    [Fact]
    public async Task GetChallengesAsync_should_return_authored_and_joined_challenges()
    {

    }

    [Fact]
    public async Task GetChallengesAsync_should_search_challenges_by_title()
    {

    }

    [Fact]
    public async Task GetScoreBoardAsync_should_apply_activity_type_multipliers()
    {
        // Arrange
        InMemoryDataContextFactory dataContextFactory = new();
        IDbContextFactory<DataContext> factory = dataContextFactory;
        ChallengeService service = new(new NullLogger<ChallengeService>(), factory);

        Athlete athlete1 = new() { Id = 1, Name = "Athlete 1", StravaId = 1 };
        Athlete athlete2 = new() { Id = 2, Name = "Athlete 2", StravaId = 2 };
        
        await using (DataContext context = await factory.CreateDbContextAsync())
        {
            await context.Athletes.AddAsync(athlete1);
            await context.Athletes.AddAsync(athlete2);
            await context.SaveChangesAsync();
        }

        Challenge challenge = new()
        {
            Title = "Test Challenge",
            Description = "Test Description",
            Start = DateTimeOffset.UtcNow.AddDays(-7),
            End = DateTimeOffset.UtcNow.AddDays(7),
            Measurement = ChallengeMeasurement.Distance,
            CreatedById = 1,
            ChallengeActivityTypes = new List<ChallengeActivityType>
            {
                new() { ActivityType = ActivityType.Run, Multiplier = 1.0 },
                new() { ActivityType = ActivityType.Ride, Multiplier = 0.5 }
            }
        };

        await using (DataContext context = await factory.CreateDbContextAsync())
        {
            context.Attach(athlete1);
            context.Attach(athlete2);
            challenge = await context.AddChallengeAsync(challenge);
            challenge.Athletes = new List<Athlete> { athlete1, athlete2 };
            await context.UpdateChallengeAsync(challenge);
        }

        // Add activities for athletes
        await using (DataContext context = await factory.CreateDbContextAsync())
        {
            // Athlete 1: 10km running (multiplier 1.0) = 10km
            await context.Activities.AddAsync(new Activity
            {
                AthleteId = 1,
                StravaId = 1,
                Details = new ActivityDetails
                {
                    Type = ActivityType.Run,
                    DistanceInMeters = 10000,
                    StartDate = DateTimeOffset.UtcNow.AddDays(-1)
                }
            });

            // Athlete 2: 20km biking (multiplier 0.5) = 10km effective
            await context.Activities.AddAsync(new Activity
            {
                AthleteId = 2,
                StravaId = 2,
                Details = new ActivityDetails
                {
                    Type = ActivityType.Ride,
                    DistanceInMeters = 20000,
                    StartDate = DateTimeOffset.UtcNow.AddDays(-1)
                }
            });

            await context.SaveChangesAsync();
        }

        // Act
        ScoreBoard scoreBoard = await service.GetScoreBoardAsync(challenge.Id);

        // Assert
        scoreBoard.Scores.Count.ShouldBe(2);
        scoreBoard.Scores[0].ScoreValue.ShouldBe(10f, 0.01f); // Both should have 10km effective
        scoreBoard.Scores[1].ScoreValue.ShouldBe(10f, 0.01f);
    }
}
