using RewardTask.Enum;
using RewardTask.Service;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace RewardTask.Rewards
{
    public class RewardController
    {
        private RewardModel rewardModel;
        private RewardView rewardView;
        private int goUnlockTimeInSeconds, coolUnlockTimeInSeconds, currentTimer, currentCurrency;
        private RewardStatus currentStatus;
        private Image progressImage;
        private bool currencyNotEarned;

        public RewardController(RewardModel _rewardModel, RewardView _rewardView, Image _rewardImage, Image _currencyImage)
        {
            rewardModel = _rewardModel;
            rewardView = _rewardView;


            rewardView.SetController(this);
            rewardView.SetImages(_rewardImage, _currencyImage);
            rewardView.SetCurrentState(rewardModel.Status);
            rewardView.SetBackGround(rewardModel.Status, rewardModel.CurrencyEarned);

            goUnlockTimeInSeconds = rewardModel.MinimumConnectionMinutes * 60; // 60 seconds
            coolUnlockTimeInSeconds = (rewardModel.AwardEveryMinutes - rewardModel.CoolDownMinutesPassed) * 60;
            rewardView.SetTime(goUnlockTimeInSeconds, coolUnlockTimeInSeconds);

            currentCurrency = rewardModel.CurrencyRequired;
            rewardView.SetCurrency(currentCurrency);

            currentStatus = rewardModel.Status;

            if (currentStatus == RewardStatus.Go)
            {
                currencyNotEarned = (rewardModel.CurrencyEarned == 0);
                currentTimer = goUnlockTimeInSeconds;
                EventService.Instance.onSubtractCoins += SubtractCoins;
            }
            else if(currentStatus == RewardStatus.Claim)
            {
                rewardView.RewardButton.interactable = true;
                rewardView.Animator.SetBool("bounce", true);
            }
            else if(currentStatus == RewardStatus.Cooling)
            {
                currentTimer = coolUnlockTimeInSeconds;
                rewardView.Animator.SetBool("rotate", true);
            }

            StartTimer();
        }

        public void SetProgressImage(Image _image)
        {
            progressImage = _image;
        }

        private async void StartTimer()
        {
            int hour, minute, second, totalSeconds;
            totalSeconds = rewardModel.AwardEveryMinutes * 60;

            while (currentTimer > -1)
            {
                hour = (currentTimer % 86400) / 3600;
                minute = (currentTimer / 60) % 60;
                second = currentTimer % 60;

                if(currentStatus == RewardStatus.Go)
                {
                    rewardView.GoTimer.text = "" + minute + ":" + second;
                }
                else if(currentStatus == RewardStatus.Cooling)
                {
                    rewardView.CoolDownTimer.text = "" + hour + ":" + minute + ":" + second;
                    progressImage.fillAmount = (float)(totalSeconds - currentTimer) / totalSeconds;
                }

                await Task.Delay(1000);

                currentTimer -= 1;
            }

            if (currentTimer <= 0 && currentCurrency <= 0 && currentStatus == RewardStatus.Go)
            {
                ChangeStatus();
            }
        }

        public void ChangeStatus()
        {
            switch (currentStatus)
            {
                case RewardStatus.Go:
                    rewardView.SetCurrentState(RewardStatus.Claim);
                    rewardView.SetBackGround(RewardStatus.Claim, rewardModel.CurrencyEarned);
                    rewardView.RewardButton.interactable = true;
                    currentStatus = RewardStatus.Claim;
                    rewardView.Animator.SetBool("bounce", true);
                    break;

                case RewardStatus.Claim:
                    rewardView.SetCurrentState(RewardStatus.Cooling);
                    rewardView.SetBackGround(RewardStatus.Cooling, rewardModel.CurrencyEarned);
                    rewardView.ChangeImageTransparency();
                    rewardView.RewardButton.interactable = false;
                    currentTimer = coolUnlockTimeInSeconds;
                    StartTimer();
                    currentStatus = RewardStatus.Cooling;
                    rewardView.Animator.SetBool("bounce", false);
                    rewardView.Animator.SetBool("rotate", true);
                    break;

                default:
                    break;
            }
        }

        private void SubtractCoins()
        {
            currentCurrency -= 5;
            rewardView.SetCurrency(currentCurrency);

            if (currencyNotEarned)
            {
                rewardView.SetBackGround(rewardModel.Status, 1);
                currencyNotEarned = false;
            }

            if(currentCurrency <= 0)
            {
                EventService.Instance.onSubtractCoins -= SubtractCoins;
            }

            if (currentTimer <= 0 && currentCurrency <= 0 && currentStatus == RewardStatus.Go)
            {
                ChangeStatus();
            }
        }
    }
}