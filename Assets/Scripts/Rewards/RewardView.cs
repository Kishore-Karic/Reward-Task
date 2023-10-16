using RewardTask.Enum;
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
        [SerializeField] private float transparentValue, sizeDevisorValue;

        [field: SerializeField] public TextMeshProUGUI GoTimer, CoolDownTimer, CurrencyValue;
        [field: SerializeField] public Button RewardButton { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }

        public const int SecondsInDay = 86400, SecondsInHour = 3600, SecondsInMinute = 60, Zero = 0, One = 1, Two = 2, Three =3;
        public RewardStatus CurrentStatus;
        public int CurrencyEarned;

        private Vector3 defaultPosition;
        private const string HeartBeatAnimation = "heartbeat", BounceAnimation = "bounce", RotateAnimation = "rotate", EmptyString = "", SemicolonString = ":";
        private RewardController rewardController;

        private void Awake()
        {
            defaultPosition = rewardTransform.position;
        }

        private void OnEnable()
        {
            if(CurrentStatus == RewardStatus.Go && CurrencyEarned != Zero)
            {
                Animator.SetBool(HeartBeatAnimation, true);
            }
            else if(CurrentStatus == RewardStatus.Claim)
            {
                RewardButton.interactable = true;
                Animator.SetBool(BounceAnimation, true);
            }
            else if(CurrentStatus == RewardStatus.Cooling)
            {
                Animator.SetBool(RotateAnimation, true);
            }

            FadeImageTransparency(false);
        }

        public void SetController(RewardController _rewardController)
        {
            rewardController = _rewardController;
            rewardController.SetProgressImage(progressImage);
        }

        public void SetImages(Image _rewardImage, Image _currencyImage)
        {
            rewardTransform.sizeDelta = new Vector2(_rewardImage.sprite.texture.width / sizeDevisorValue, _rewardImage.sprite.texture.height / sizeDevisorValue);
            rewardImage.sprite = _rewardImage.sprite;

            currencyTransform.sizeDelta = new Vector2(_currencyImage.sprite.texture.width / sizeDevisorValue, _currencyImage.sprite.texture.height / sizeDevisorValue);
            currencyImage.sprite = _currencyImage.sprite;
        }

        public void SetCurrentState(RewardStatus _rewardStatus)
        {
            for(int i = Zero; i < statusObjectList.Count; i++)
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
            int i = Zero;

            if(_rewardStatus == RewardStatus.Go)
            {
                i = _currencyEarned == Zero ? Zero : One;
            }
            else if(_rewardStatus == RewardStatus.Claim)
            {
                i = Two;
            }
            else if(_rewardStatus == RewardStatus.Cooling)
            {
                i = Three;
            }

            backgroundImage.sprite = backgroundSpriteList[i];
        }

        public void SetTime(int goTime, int coolDownTime)
        {
            int second, minute, hour;

            minute = (goTime / SecondsInMinute) % SecondsInMinute;
            second = goTime % SecondsInMinute;

            GoTimer.text = EmptyString + minute + SemicolonString + second;

            hour = (coolDownTime % SecondsInDay) / SecondsInHour;
            minute = (coolDownTime / SecondsInMinute) % SecondsInMinute;
            second = coolDownTime % SecondsInMinute;

            CoolDownTimer.text = EmptyString + hour + SemicolonString + minute + SemicolonString + second;
        }

        public void SetCurrency(int _currency)
        {
            _currency = _currency <= Zero ? Zero : _currency;
            CurrencyValue.text = EmptyString + _currency;
        }

        public void FadeImageTransparency(bool _value)
        {
            
            Color transparent = rewardImage.color;
            transparent.a = _value == true ? transparentValue : One;
            rewardImage.color = transparent;
        }

        public void OnClick()
        {
            rewardController.RequestedForCollectRewards(this);
        }

        public void RewardCollected()
        {
            rewardController.ChangeStatus();
        }

        private void OnDisable()
        {
            Animator.SetBool(HeartBeatAnimation, false);
            Animator.SetBool(BounceAnimation, false);
            Animator.SetBool(RotateAnimation, false);

            rewardTransform.position = defaultPosition;
            rewardTransform.localScale = new Vector3(One, One, One);
            rewardTransform.rotation = Quaternion.Euler(Zero, Zero, Zero);
        }
    }
}