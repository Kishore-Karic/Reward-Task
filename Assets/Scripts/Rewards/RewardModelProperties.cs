using RewardTask.Enum;
using System;

namespace RewardTask.Rewards
{
    public class RewardModelProperties
    {
        public int Id;
        public string ImageURL;
        public RewardStatus Status;
        public int AwardEveryMinutes;
        public int MinimumConnectionMinutes;
        public int LoggedinSeconds;
        public int CurrencyEarned;
        public int CurrencyRequired;
        public string CurrencyImageURL;
        public int CoolDownMinutesPassed;

        private const string Go = "go", Claim = "claim", Cooling = "cooling";

        public RewardModelProperties(RewardProperties rewardProperties)
        {
            int id, awardEveryMinute, minimumConnectionMinutes, loggedinSeconds, currencyEarned, currencyRequired, coolDownMinutesPassed;
            Int32.TryParse(rewardProperties.id, out id);
            Int32.TryParse(rewardProperties.award_every_minutes, out awardEveryMinute);
            Int32.TryParse(rewardProperties.minimum_connection_minutes, out minimumConnectionMinutes);
            Int32.TryParse(rewardProperties.loggedin_seconds, out loggedinSeconds);
            Int32.TryParse(rewardProperties.curr_earned, out currencyEarned);
            Int32.TryParse(rewardProperties.currency_required, out currencyRequired);
            Int32.TryParse(rewardProperties.cool_down_minutes_passed, out coolDownMinutesPassed);

            Id = id;
            ImageURL = rewardProperties.image;

            switch (rewardProperties.status)
            {
                case Go:
                    Status = RewardStatus.Go;
                    break;

                case Claim:
                    Status = RewardStatus.Claim;
                    break;

                case Cooling:
                    Status = RewardStatus.Cooling;
                    break;

                default:
                    break;
            }

            AwardEveryMinutes = awardEveryMinute;
            MinimumConnectionMinutes = minimumConnectionMinutes;
            LoggedinSeconds = loggedinSeconds;
            CurrencyEarned = currencyEarned;
            CurrencyRequired = currencyRequired;
            CoolDownMinutesPassed = coolDownMinutesPassed;
            CurrencyImageURL = rewardProperties.currency_image;
        }
    }
}