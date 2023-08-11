using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Tool;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Type = System.Type;

namespace Core.Scripts.Network
{
    public class BundleManager : MonoBehaviour
    {
        private static BundleManager _bundleManager;
        public static BundleManager Instance => _bundleManager;

        [SerializeField] private List<DataBundle> _dataBundles;
        public List<DataBundle> DataBundles => _dataBundles;

        //private string _bundlePath = "https://devrouter.fumydata.ru/game/bear";
        private string _bundlePath = "https://router.fumydata.ru/game/bear";
        private bool isRoutineLoadBundlesPlayed = false;

        public void Awake()
        {
            _bundleManager = this;
        }
        public void AddBundle(string bundleUrl,string bundleName)
        {
            string tempBundleUrl = _bundlePath + bundleUrl;
            bool isAdd = true;
            foreach (var dataBundle in DataBundles)
            {
                if (dataBundle.BundleUrl == tempBundleUrl)
                {
                    isAdd = false;
                    break;
                }
            }

            if (isAdd)
            {
                ManagerLog.Log("Bundle add + " + tempBundleUrl);
                var tempBundle = new DataBundle();
                tempBundle.BundleUrl = tempBundleUrl;
                tempBundle.BundleName = bundleName;
                _dataBundles.Add(tempBundle);
            }
        }
        public IEnumerator RoutineLoadIcon(RawImage rawImage, string url)
        {
            WWW www = new WWW(_bundlePath + url);
            yield return www;

            Texture2D texure = new Texture2D (1, 1);
            texure.LoadImage (www.bytes);
            texure.Apply ();
            rawImage.texture = texure;
        }

        public void LoadBundles()
        {
             StartCoroutine(RoutineLoadBundles());
             //bundles ability ability_id
             //в скллах добавить бандлы
        }
        
        private IEnumerator RoutineLoadBundles()
        {
            isRoutineLoadBundlesPlayed = true;
            
            while (true)
            {
                bool isLoading = false;
                yield return new WaitForSecondsRealtime(0.1f);
                foreach (var dataBundle in DataBundles)
                {
                    ManagerLog.Log("databundle==="+dataBundle.BundleUrl);
                    if (dataBundle.BundleLoadProgress != 1)//
                    {
                        isLoading = true;
                        //while (!Caching.ready) {Debug.Log("111"); yield return null; }

                        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(dataBundle.BundleUrl))
                        {
                            www.SendWebRequest();

                            //var progress = 0f;

                            using (UnityWebRequest request = UnityWebRequest.Head(dataBundle.BundleUrl))
                            {
                                yield return request.SendWebRequest();
                            }

                            while (!www.isDone)
                            {

                                // GraphicManager.Instance.BattleCanvas.LoadScreen.SetInformation(string.Format(
                                //     "Download package {0} [{1}MB]",
                                //     dataBundle.BundleName, (www.downloadedBytes / 1_048_576f).ToString("F2")));
                                // GraphicManager.Instance.BattleCanvas.LoadScreen.SetProgressBar(www.downloadProgress);
                                //DataManager.Log(string.Format("{0}% Bytes:{1}", www.downloadProgress * 100, www.downloadedBytes));
                                //Debug.LogError(path + "Progress load =" + progress);
                                yield return new WaitForEndOfFrame();
                            }

                            if (www.result != UnityWebRequest.Result.Success)
                            {
                                //DataManager.Log(www.error + dataBundle.BundleUrl);
                            }
                            else
                            {
                                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

                                string modelName = bundle.GetAllAssetNames()[0];

                                //DataManager.Log("Загруженный бандл " + modelName);

                                //TODO Save bundle to Prefab for spawn
                                dataBundle.BundleName = modelName;
                                dataBundle.BundleLoadProgress = 1;
                                dataBundle.BundleIsLoaded = true;
                                //GameObject tempBundle=bundle.LoadAssetAsync(modelName, typeof(GameObject)).asset as GameObject;
                                //Instantiate(tempBundle).transform.parent=GameObject.Find("Location").transform;

                                dataBundle.BundlePrefab = bundle.LoadAssetAsync(modelName, typeof(GameObject)).asset as GameObject;
                                
                                Renderer[] rens = dataBundle.BundlePrefab.GetComponentsInChildren<Renderer>();
                                
                                // for each material in each renderer, reattach the same shader if you can find it.
                                for (int i = 0; i < rens.Length; i++)
                                {
                                    bool shared = true;
                                    //foreach (Material mat in shared? rens[i].sharedMaterials : rens[i].materials)
                                    foreach (Material mat in shared? rens[i].sharedMaterials : rens[i].materials)
                                    {
                                        var shd = Shader.Find(mat.shader.name);
                                        if (null == shd)
                                        {
                                            //ManagerData.Log("Cannot refresh the shader on GameObject:" + go.name + " shader name: " + mat.shader.name + ". Applying standard shader. If not okay, add this shader to Resources folder.");
                                            shd = Shader.Find("Particles/Additive");
                                        }
                                        mat.shader = shd;
                                    }
                                }

                                
                                //GameObject.Find("[LOCATION]").GetComponent<AudioListener>().enabled = false;

                                //Instantiate(location).transform.parent = GameObject.Find("[LOCATION]").transform;

                                // GameObject tempModel = Instantiate(dataBundle.BundlePrefab,
                                //     DataManager.Instance.Location.transform);
                                //
                                // DataManager.Instance.Models.Add(tempModel);
                                //location.SetActive(true);

                                //bundle.Unload(false);
                                

                                //bundle.Unload(false);

                                //locationIsLoad = true;
                                //RefreshShaders(location, true);
                                //StartCoroutine(DownloadAndCache());
                                ManagerLog.Log("end");
                            }

                            ManagerLog.Log("dispose");
                            //www.disposeDownloadHandlerOnDispose = true;
                            //www.disposeCertificateHandlerOnDispose = true;
                            //www.Dispose();
                        }


                    }
                }

                if (!isLoading)
                {
                    break;
                }

            }
            
            isRoutineLoadBundlesPlayed = false;


        }



    }
    
    

        [Serializable]
    public class DataBundle
    {
        public string BundleUrl = "";
        public string BundleName = "";
        public float BundleLoadProgress = 0;
        public GameObject BundlePrefab = null;
        public bool BundleIsLoaded = false;
    }
}