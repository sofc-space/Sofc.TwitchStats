using System.Text.Json.Serialization;

namespace Sofc.TwitchStats.Api.Data.Leetify.V2Api.Profile;

public class LeetifyV2Skills
{
    [JsonPropertyName("accuracy_enemy_spotted")]
    public double AccuracyEnemySpotted { get; set; }

    [JsonPropertyName("accuracy_head")]
    public double AccuracyHead { get; set; }

    [JsonPropertyName("counter_strafing_good_shots_ratio")]
    public double CounterStrafingGoodShotsRatio { get; set; }

    [JsonPropertyName("ct_opening_aggression_success_rate")]
    public double CtOpeningAggressionSuccessRate { get; set; }

    [JsonPropertyName("ct_opening_duel_success_percentage")]
    public double CtOpeningDuelSuccessPercentage { get; set; }

    [JsonPropertyName("flashbang_hit_foe_avg_duration")]
    public double FlashbangHitFoeAvgDuration { get; set; }

    [JsonPropertyName("flashbang_hit_foe_per_flashbang")]
    public double FlashbangHitFoePerFlashbang { get; set; }

    [JsonPropertyName("flashbang_hit_friend_per_flashbang")]
    public double FlashbangHitFriendPerFlashbang { get; set; }

    [JsonPropertyName("flashbang_leading_to_kill")]
    public double FlashbangLeadingToKill { get; set; }

    [JsonPropertyName("flashbang_thrown")]
    public double FlashbangThrown { get; set; }

    [JsonPropertyName("he_foes_damage_avg")]
    public double HeFoesDamageAvg { get; set; }

    [JsonPropertyName("he_friends_damage_avg")]
    public double HeFriendsDamageAvg { get; set; }

    [JsonPropertyName("preaim")]
    public double Preaim { get; set; }

    [JsonPropertyName("reaction_time")]
    public double ReactionTime { get; set; }

    [JsonPropertyName("spray_accuracy")]
    public double SprayAccuracy { get; set; }

    [JsonPropertyName("t_opening_aggression_success_rate")]
    public double TOpeningAggressionSuccessRate { get; set; }

    [JsonPropertyName("t_opening_duel_success_percentage")]
    public double TOpeningDuelSuccessPercentage { get; set; }

    [JsonPropertyName("traded_deaths_success_percentage")]
    public double TradedDeathsSuccessPercentage { get; set; }

    [JsonPropertyName("trade_kill_opportunities_per_round")]
    public double TradeKillOpportunitiesPerRound { get; set; }

    [JsonPropertyName("trade_kills_success_percentage")]
    public double TradeKillsSuccessPercentage { get; set; }

    [JsonPropertyName("utility_on_death_avg")]
    public double UtilityOnDeathAvg { get; set; }
}