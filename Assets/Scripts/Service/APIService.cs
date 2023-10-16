using Newtonsoft.Json;
using RewardTask.Rewards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace RewardTask.Service
{
    public class APIService : MonoBehaviour
    {
        [SerializeField] private RewardService rewardService;
        [SerializeField] private string dataURL;
        [field: SerializeField] public List<Image> RewardSpritesList { get; private set; }
        [field: SerializeField] public List<Image> CurrencySpritesList { get; private set; }

        public List<RewardModelProperties> RewardModelPropertiesList { get; private set; }
        
        private Coroutine coroutine;
        private RewardsList rewardsList;
        private const int Zero = 0, Two = 2;

        void Start()
        {
            GetURL();
        }

        private void GetURL()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(GetData());
        }

        [System.Obsolete]
        IEnumerator GetData()
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(dataURL);

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                GetURL();
            }
            else
            {
                string json = webRequest.downloadHandler.text;
                rewardsList = JsonConvert.DeserializeObject<RewardsList>(json);

                IniatizeRewardModel();
            }
        }

        private void IniatizeRewardModel()
        {
            RewardModelPropertiesList = new List<RewardModelProperties>();
            for(int i = Zero; i < rewardsList.rewards.Length; i++)
            {
                RewardModelPropertiesList.Add(new RewardModelProperties(rewardsList.rewards[i]));
            }

            StartCoroutine(GetImages());
        }

        IEnumerator GetImages()
        {
            for(int i = Zero; i < rewardsList.rewards.Length; i++)
            {
                coroutine = StartCoroutine(WaitTex(rewardsList.rewards[i].image, RewardSpritesList[i]));
                yield return new WaitUntil(() => coroutine == null);

                coroutine = StartCoroutine(WaitTex(rewardsList.rewards[i].currency_image, CurrencySpritesList[i]));
                yield return new WaitUntil(() => coroutine == null);
            }

            rewardService.SpawnRewards();
        }

        IEnumerator WaitTex(string _text, Image _image)
        {
            UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(_text);

            yield return imageRequest.SendWebRequest();

            if (imageRequest.isNetworkError || imageRequest.isHttpError)
            {
                WaitTex(_text, _image);
            }
            else
            {
                var tex = ((DownloadHandlerTexture)imageRequest.downloadHandler).texture;
                Sprite sprite = Sprite.Create(tex, new Rect(Zero, Zero, tex.width, tex.height), new Vector2(tex.width / Two, tex.height / Two));
                
                _image.sprite = sprite;

                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}
