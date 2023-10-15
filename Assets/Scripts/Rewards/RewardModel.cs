using RewardTask.Enum;

namespace RewardTask.Rewards
{
    public class RewardModel
    {
        public int Id { get; private set; }
        public string ImageURL { get; private set; }
        public RewardStatus Status { get; private set; }
        public int AwardEveryMinutes { get; private set; }
        public int MinimumConnectionMinutes { get; private set; }
        public int LoggedinSeconds { get; private set; }
        public int CurrencyEarned { get; private set; }
        public int CurrencyRequired { get; private set; }
        public string CurrencyImageURL { get; private set; }
        public int CoolDownMinutesPassed { get; private set; }

        public RewardModel(RewardModelProperties rewardModelProperties)
        {
            Id = rewardModelProperties.Id;
            ImageURL = rewardModelProperties.ImageURL;
            Status = rewardModelProperties.Status;
            AwardEveryMinutes = rewardModelProperties.AwardEveryMinutes;
            MinimumConnectionMinutes = rewardModelProperties.MinimumConnectionMinutes;
            LoggedinSeconds = rewardModelProperties.LoggedinSeconds;
            CurrencyEarned = rewardModelProperties.CurrencyEarned;
            CurrencyRequired = rewardModelProperties.CurrencyRequired;
            CurrencyImageURL = rewardModelProperties.CurrencyImageURL;
            CoolDownMinutesPassed = rewardModelProperties.CoolDownMinutesPassed;
        }
    }
}