namespace RewardTask.Rewards
{
    public class RewardController
    {
        private RewardModel rewardModel;
        private RewardView rewardView;

        public RewardController(RewardModel _rewardModel, RewardView _rewardView)
        {
            rewardModel = _rewardModel;
            rewardView = _rewardView;


            rewardView.SetController(this);
        }
    }
}