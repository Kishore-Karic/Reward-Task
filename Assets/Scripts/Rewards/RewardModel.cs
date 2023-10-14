using RewardTask.Enum;
using System;

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

        public RewardModel(RewardProperties rewardProperties)
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
                case "go":
                    Status = RewardStatus.Go;
                    break;

                case "claim":
                    Status = RewardStatus.Claim;
                    break;

                case "cooling":
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
        }
    }
}