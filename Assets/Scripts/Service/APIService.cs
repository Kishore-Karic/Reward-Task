using Newtonsoft.Json;
using RewardTask.Rewards;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace RewardTask.APIService
{
    public class APIService : MonoBehaviour
    {
        Coroutine coroutine, coroutine2;
        public Image image, image1;
        public GameObject ImageObject, ImageObject1;
        public RectTransform rectTransform, rectTransform1;
        private RewardsList rewardsList;
        [SerializeField] private RewardService rewardService;


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
                /*Debug.Log(json);
                Debug.Log(rewardsList.status);
                foreach(RewardProperties rew in rewardsList.rewards)
                {
                    Debug.Log(rew.id);
                    Debug.Log(rew.image);
                    Debug.Log(rew.status);
                    Debug.Log(rew.award_every_minutes);
                    Debug.Log(rew.minimum_connection_minutes);
                    Debug.Log(rew.loggedin_seconds);
                    Debug.Log(rew.curr_earned);
                    Debug.Log(rew.currency_required);
                    Debug.Log(rew.currency_image);
                    Debug.Log(rew.cool_down_minutes_passed);
                }*/
                GetTex();
                //rewardService.SpawnRewards(rewardsList);
            }


        }

        private void GetTex()
        {
            if (coroutine2 != null)
            {
                StopCoroutine(coroutine2);
            }
            coroutine2 = StartCoroutine(WaitTex(rewardsList.rewards[3].image));
        }

        private void GetTex1()
        {
            if (coroutine2 != null)
            {
                StopCoroutine(coroutine2);
            }
            coroutine2 = StartCoroutine(WaitTex1(rewardsList.rewards[4].currency_image));
        }

        IEnumerator WaitTex(string text)
        {
            UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(text);

            yield return imageRequest.SendWebRequest();


            if (imageRequest.isNetworkError || imageRequest.isHttpError)
            {
                GetTex();
            }
            else
            {
                var tex = ((DownloadHandlerTexture)imageRequest.downloadHandler).texture;
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                rectTransform.sizeDelta = new Vector2(sprite.texture.width / 2.5f, sprite.texture.height / 2.5f);
                image.sprite = sprite;

                GetTex1();
            }


        }

        IEnumerator WaitTex1(string text)
        {
            UnityWebRequest imageRequest1 = UnityWebRequestTexture.GetTexture(text);

            yield return imageRequest1.SendWebRequest();


            if (imageRequest1.isNetworkError || imageRequest1.isHttpError)
            {
                GetTex1();
            }
            else
            {
                var tex = ((DownloadHandlerTexture)imageRequest1.downloadHandler).texture;
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                rectTransform1.sizeDelta = new Vector2(sprite.texture.width / 2.5f, sprite.texture.height / 2.5f);
                image1.sprite = sprite;
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
