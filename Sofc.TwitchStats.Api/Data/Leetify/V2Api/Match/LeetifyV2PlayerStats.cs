using System.Text.Json.Serialization;

namespace Sofc.TwitchStats.Api.Data.Leetify.V2Api.Match;

public class LeetifyV2PlayerStats
{
    [JsonPropertyName("steam64_id")]
    public string Steam64Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("preaim")]
    public double Preaim { get; set; }

    [JsonPropertyName("reaction_time")]
    public double ReactionTime { get; set; }

    [JsonPropertyName("accuracy")]
    public double Accuracy { get; set; }

    [JsonPropertyName("accuracy_enemy_spotted")]
    public double AccuracyEnemySpotted { get; set; }

    [JsonPropertyName("accuracy_head")]
    public double AccuracyHead { get; set; }

    [JsonPropertyName("shots_fired_enemy_spotted")]
    public int ShotsFiredEnemySpotted { get; set; }

    [JsonPropertyName("shots_fired")]
    public int ShotsFired { get; set; }

    [JsonPropertyName("shots_hit_enemy_spotted")]
    public int ShotsHitEnemySpotted { get; set; }

    [JsonPropertyName("shots_hit_friend")]
    public int ShotsHitFriend { get; set; }

    [JsonPropertyName("shots_hit_friend_head")]
    public int ShotsHitFriendHead { get; set; }

    [JsonPropertyName("shots_hit_foe")]
    public int ShotsHitFoe { get; set; }

    [JsonPropertyName("shots_hit_foe_head")]
    public int ShotsHitFoeHead { get; set; }

    [JsonPropertyName("utility_on_death_avg")]
    public double UtilityOnDeathAvg { get; set; }

    [JsonPropertyName("he_foes_damage_avg")]
    public double HeFoesDamageAvg { get; set; }

    [JsonPropertyName("he_friends_damage_avg")]
    public double HeFriendsDamageAvg { get; set; }

    [JsonPropertyName("he_thrown")]
    public int HeThrown { get; set; }

    [JsonPropertyName("molotov_thrown")]
    public int MolotovThrown { get; set; }

    [JsonPropertyName("smoke_thrown")]
    public int SmokeThrown { get; set; }

    [JsonPropertyName("counter_strafing_shots_all")]
    public int CounterStrafingShotsAll { get; set; }

    [JsonPropertyName("counter_strafing_shots_bad")]
    public int CounterStrafingShotsBad { get; set; }

    [JsonPropertyName("counter_strafing_shots_good")]
    public int CounterStrafingShotsGood { get; set; }

    [JsonPropertyName("counter_strafing_shots_good_ratio")]
    public double CounterStrafingShotsGoodRatio { get; set; }

    [JsonPropertyName("flashbang_hit_foe")]
    public int FlashbangHitFoe { get; set; }

    [JsonPropertyName("flashbang_leading_to_kill")]
    public int FlashbangLeadingToKill { get; set; }

    [JsonPropertyName("flashbang_hit_foe_avg_duration")]
    public double FlashbangHitFoeAvgDuration { get; set; }

    [JsonPropertyName("flashbang_hit_friend")]
    public int FlashbangHitFriend { get; set; }

    [JsonPropertyName("flashbang_thrown")]
    public int FlashbangThrown { get; set; }

    [JsonPropertyName("flash_assist")]
    public int FlashAssist { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }

    [JsonPropertyName("initial_team_number")]
    public int InitialTeamNumber { get; set; }

    [JsonPropertyName("spray_accuracy")]
    public double SprayAccuracy { get; set; }

    [JsonPropertyName("utility_on_death_values")]
    public List<double> UtilityOnDeathValues { get; set; }

    [JsonPropertyName("he_foes_damages")]
    public List<double> HeFoesDamages { get; set; }

    [JsonPropertyName("he_friends_damages")]
    public List<double> HeFriendsDamages { get; set; }

    [JsonPropertyName("total_kills")]
    public int TotalKills { get; set; }

    [JsonPropertyName("total_deaths")]
    public int TotalDeaths { get; set; }

    [JsonPropertyName("kd_ratio")]
    public double KdRatio { get; set; }

    [JsonPropertyName("rounds_survived")]
    public int RoundsSurvived { get; set; }

    [JsonPropertyName("rounds_survived_percentage")]
    public double RoundsSurvivedPercentage { get; set; }

    [JsonPropertyName("dpr")]
    public double Dpr { get; set; }

    [JsonPropertyName("total_assists")]
    public int TotalAssists { get; set; }

    [JsonPropertyName("total_damage")]
    public int TotalDamage { get; set; }

    [JsonPropertyName("leetify_rating")]
    public double LeetifyRating { get; set; }

    [JsonPropertyName("ct_leetify_rating")]
    public double CtLeetifyRating { get; set; }

    [JsonPropertyName("t_leetify_rating")]
    public double TLeetifyRating { get; set; }

    [JsonPropertyName("multi1k")]
    public int Multi1k { get; set; }

    [JsonPropertyName("multi2k")]
    public int Multi2k { get; set; }

    [JsonPropertyName("multi3k")]
    public int Multi3k { get; set; }

    [JsonPropertyName("multi4k")]
    public int Multi4k { get; set; }

    [JsonPropertyName("multi5k")]
    public int Multi5k { get; set; }

    [JsonPropertyName("rounds_count")]
    public int RoundsCount { get; set; }

    [JsonPropertyName("rounds_won")]
    public int RoundsWon { get; set; }

    [JsonPropertyName("rounds_lost")]
    public int RoundsLost { get; set; }

    [JsonPropertyName("total_hs_kills")]
    public int TotalHsKills { get; set; }
}