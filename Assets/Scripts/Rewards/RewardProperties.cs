using UnityEngine;

namespace RewardTask.Rewards
{
    [System.Serializable]
    public class RewardProperties
    {
        public string id;
        public string image;
        public string status;
        public string award_every_minutes;
        public string minimum_connection_minutes;
        public string loggedin_seconds;
        public string curr_earned;
        public string currency_required;
        public string currency_image;
        public string cool_down_minutes_passed;
    }
}