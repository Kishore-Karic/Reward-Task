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
        Coroutine coroutine, coroutine2;
        public Sprite sprite1;
        public Image image, image1;
        public GameObject ImageObject, ImageObject1, dummy;
        public RectTransform rectTransform, rectTransform1;
        private RewardsList rewardsList;
        [SerializeField] private RewardService rewardService;

        [field: SerializeField] public List<Image> RewardSpritesList { get; private set; }
        [field: SerializeField] public List<Image> CurrencySpritesList { get; private set; }

        public List<RewardModelProperties> RewardModelPropertiesList { get; private set; }
        private bool gotImageSprite, gotCurrencyImageSprite;

        void Start()
        {
            Debug.Log(image.sprite);
            GetURL();
        }

        private void GetURL()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(Wait());
        }

        IEnumerator Wait()
        {
            UnityWebRequest webRequest = UnityWebRequest.Get("https://epicmindarena.com/inteview_api/get_hourly_rewards_2.json");

            yield return webRequest.SendWebRequest();


            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                GetURL();
            }
            else
            {
                string json = webRequest.downloadHandler.text;
                rewardsList = JsonConvert.DeserializeObject<RewardsList>(json);
                Debug.Log(json);
                Debug.Log(rewardsList.status);

                IniatizeRewardModel();
                //rewardService.SpawnRewards(rewardsList);
            }


        }

        private void IniatizeRewardModel()
        {
            RewardModelPropertiesList = new List<RewardModelProperties>();
            for(int i = 0; i < rewardsList.rewards.Length; i++)
            {
                RewardModelPropertiesList.Add(new RewardModelProperties(rewardsList.rewards[i]));
            }

            StartCoroutine(GetImages());
        }

        IEnumerator GetImages()
        {
            for(int i = 0; i < rewardsList.rewards.Length; i++)
            {
                coroutine = StartCoroutine(WaitTex(rewardsList.rewards[i].image, RewardSpritesList[i]));
                yield return new WaitUntil(() => coroutine == null);
                coroutine = StartCoroutine(WaitTex(rewardsList.rewards[i].currency_image, CurrencySpritesList[i]));
                yield return new WaitUntil(() => coroutine == null);
                //GetTex(rewardsList.rewards[i].image, rewardsList.rewards[i].image_sprite);
                //GetTex1(i);
                //yield return (gotImageSprite == true);

                //gotImageSprite = false;

                //GetTex(rewardsList.rewards[i].currency_image, rewardsList.rewards[i].currency_image_sprite);
                //yield return (gotImageSprite == true);

                Debug.Log(i);
                //gotImageSprite = false;
            }

            foreach (RewardModelProperties rew in RewardModelPropertiesList)
            {
                Debug.Log(rew.Id);
                Debug.Log(rew.ImageURL);
                Debug.Log(rew.Status);
                Debug.Log(rew.AwardEveryMinutes);
                Debug.Log(rew.MinimumConnectionMinutes);
                Debug.Log(rew.LoggedinSeconds);
                Debug.Log(rew.CurrencyEarned);
                Debug.Log(rew.CurrencyRequired);
                Debug.Log(rew.CurrencyImageURL);
                Debug.Log(rew.CoolDownMinutesPassed);
            }
            rewardService.SpawnRewards();
        }

        private void GetTex(string _text, Image _image)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(WaitTex(_text, _image));
        }

        private void GetTex1(int j)
        {
            if (coroutine2 != null)
            {
                StopCoroutine(coroutine2);
            }
            coroutine2 = StartCoroutine(WaitTex1(j));
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
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                //rectTransform.sizeDelta = new Vector2(sprite.texture.width / 2.5f, sprite.texture.height / 2.5f);
                //Debug.Log(sprite);
                
                _image.sprite = sprite;
                //image1.sprite = _sprite;
                //Debug.Log(image1.sprite);

                coroutine = null;
                gotImageSprite = true;
            }
        }

        IEnumerator WaitTex1(int j)
        {
            UnityWebRequest imageRequest1 = UnityWebRequestTexture.GetTexture(rewardsList.rewards[j].currency_image);

            yield return imageRequest1.SendWebRequest();

            if (imageRequest1.isNetworkError || imageRequest1.isHttpError)
            {
                GetTex1(j);
            }
            else
            {
                var tex = ((DownloadHandlerTexture)imageRequest1.downloadHandler).texture;
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                //rectTransform1.sizeDelta = new Vector2(sprite.texture.width / 2.5f, sprite.texture.height / 2.5f);
                //rewardsList.rewards[j].currency_image_sprite = sprite;

                gotCurrencyImageSprite = true;
            }
        }
    }
}
/*[System.Serializable]
public class List
{
    public string status;
    public Rewards[] rewards;
}

[System.Serializable]
public class Rewards
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
}*/
