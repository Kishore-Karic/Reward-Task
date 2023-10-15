using RewardTask.Enum;
using RewardTask.Service;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RewardTask.Rewards
{
    public class RewardView : MonoBehaviour
    {
        [SerializeField] private RectTransform rewardTransform, currencyTransform;
        [SerializeField] private Image rewardImage, currencyImage, backgroundImage, progressImage;
        [SerializeField] private List<GameObject> statusObjectList;
        [SerializeField] private List<Sprite> backgroundSpriteList;
        [field: SerializeField] public TextMeshProUGUI GoTimer, CoolDownTimer, CurrencyValue;
        [field: SerializeField] public Button RewardButton { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }

        private RewardController rewardController;

        public void SetController(RewardController _rewardController)
        {
            rewardController = _rewardController;
            rewardController.SetProgressImage(progressImage);
        }

        public void SetImages(Image _rewardImage, Image _currencyImage)
        {
            rewardTransform.sizeDelta = new Vector2(_rewardImage.sprite.texture.width / 2.5f, _rewardImage.sprite.texture.height / 2.5f);
            rewardImage.sprite = _rewardImage.sprite;

            currencyTransform.sizeDelta = new Vector2(_currencyImage.sprite.texture.width / 2.5f, _currencyImage.sprite.texture.height / 2.5f);
            currencyImage.sprite = _currencyImage.sprite;
        }

        public void SetCurrentState(RewardStatus _rewardStatus)
        {
            for(int i = 0; i < statusObjectList.Count; i++)
            {
                statusObjectList[i].SetActive(false);
                if((int)_rewardStatus == i)
                {
                    statusObjectList[i].SetActive(true);
                }
            }
        }

        public void SetBackGround(RewardStatus _rewardStatus, int _currencyEarned)
        {
            int i = 0;

            if(_rewardStatus == RewardStatus.Go)
            {
                if(_currencyEarned == 0)
                {
                    i = 0;
                }
                else
                {
                    i = 1;
                }
            }
            else if(_rewardStatus == RewardStatus.Claim)
            {
                i = 2;
            }
            else if(_rewardStatus == RewardStatus.Cooling)
            {
                i = 3;
            }

            backgroundImage.sprite = backgroundSpriteList[i];
        }

        public void SetTime(int goTime, int coolDownTime)
        {
            int second, minute, hour;

            minute = (goTime / 60) % 60;
            second = goTime % 60;

            GoTimer.text = "" + minute + ":" + second;

            hour = (coolDownTime % 86400) / 3600;
            minute = (coolDownTime / 60) % 60;
            second = coolDownTime % 60;

            CoolDownTimer.text = "" + hour + ":" + minute + ":" + second;
        }

        public void SetCurrency(int _currency)
        {
            _currency = _currency <= 0 ? 0 : _currency;
            CurrencyValue.text = "" + _currency;
        }

        public void ChangeImageTransparency()
        {
            Color transparent = rewardImage.color;
            transparent.a = 0.3f;
            rewardImage.color = transparent;
        }

        public void OnClick()
        {
            UIService.Instance.ShowCollectReward(this);
        }

        public void RewardCollected()
        {
            rewardController.ChangeStatus();
        }
    }
}