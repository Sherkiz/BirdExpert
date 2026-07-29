using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Newtonsoft.Json;

namespace BirdExpert
{
    public class BirdsLoader : MonoBehaviour
    {
        private Dictionary<string, BirdInfo> allBirds;
        private List<string> spCodesList;
        public UnityEvent Ready;
        private LoadingSceneCanvas loadingSceneCanvas;
        public LoadingSceneCanvas LoadingSceneCanvas { set => loadingSceneCanvas = value; }

        public void StartLoading()
        {
            Ready = new UnityEvent();
            allBirds = new Dictionary<string, BirdInfo>();
            StartCoroutine(LoadAssetsFromMemory());
        }
        IEnumerator LoadAssetsFromMemory()
        {
            Debug.Log("Start retrieving basic data assets");
            loadingSceneCanvas.SetLoadingStage("loading-locations", 0.2f);
            AsyncOperationHandle<IList<IResourceLocation>> baseDataLocations = Addressables.LoadResourceLocationsAsync("BaseData", typeof(TextAsset));
            yield return baseDataLocations;
            AsyncOperationHandle<IList<IResourceLocation>> imagesLocations = Addressables.LoadResourceLocationsAsync("BaseData", typeof(Sprite));
            yield return imagesLocations;
            AsyncOperationHandle<IList<IResourceLocation>> soundsLocations = Addressables.LoadResourceLocationsAsync("BaseData", typeof(AudioClip));
            yield return soundsLocations;
            

            Debug.Log(baseDataLocations.Status.ToString());
            Debug.Log(baseDataLocations.Result.Count + " birds found in data");
            loadingSceneCanvas.SetLoadingStage("loading-birdnames", 0.3f);

            var loadOpsBasic = new List<AsyncOperationHandle>(baseDataLocations.Result.Count);
            foreach (IResourceLocation loc in baseDataLocations.Result)
            {
                AsyncOperationHandle<TextAsset> jsonFileHandle = Addressables.LoadAssetAsync<TextAsset>(loc);
                jsonFileHandle.Completed += OnBirdJsonDataLoaded;
                loadOpsBasic.Add(jsonFileHandle);
            }
            yield return Addressables.ResourceManager.CreateGenericGroupOperation(loadOpsBasic, true);
            spCodesList = allBirds.Keys.ToList();
            Names.codeNames = spCodesList;

            Debug.Log("Start retrieving bird images");
            loadingSceneCanvas.SetLoadingStage("loading-birdimages", 0.6f);

            var loadOpsImages = new List<AsyncOperationHandle>(imagesLocations.Result.Count);
            foreach (IResourceLocation loc in imagesLocations.Result)
            {
                AsyncOperationHandle<Sprite> spriteFileHandle = Addressables.LoadAssetAsync<Sprite>(loc);
                spriteFileHandle.Completed += OnBirdImageLoaded;
                loadOpsImages.Add(spriteFileHandle);
            }
            yield return Addressables.ResourceManager.CreateGenericGroupOperation(loadOpsImages, true);


            Debug.Log("Start retrieving bird sounds");
            loadingSceneCanvas.SetLoadingStage("loading-birdsounds", 0.6f);

            var loadOpsSounds = new List<AsyncOperationHandle>(soundsLocations.Result.Count);
            foreach (IResourceLocation loc in soundsLocations.Result)
            {
                AsyncOperationHandle<AudioClip> soundFileHandle = Addressables.LoadAssetAsync<AudioClip>(loc);
                soundFileHandle.Completed += OnBirdSoundLoaded;
                loadOpsSounds.Add(soundFileHandle);
            }
            yield return Addressables.ResourceManager.CreateGenericGroupOperation(loadOpsSounds, true);
            
            loadingSceneCanvas.SetLoadingStage("loading-assigning", 0.6f);
            OnAssetsLoadingFinished();
            Ready.Invoke();
        }

        private void OnBirdJsonDataLoaded(AsyncOperationHandle<TextAsset> jsonFileHandle)
        {
            if (jsonFileHandle.Status == AsyncOperationStatus.Succeeded)
            {
                CreateBirdInfo(jsonFileHandle.Result);
            }
        }

        private void CreateBirdInfo(TextAsset jsonFile)
        {
            BirdBaseData birdBaseData = (BirdBaseData) ScriptableObject.CreateInstance(typeof(BirdBaseData));
            JsonConvert.PopulateObject(jsonFile.ToString(), birdBaseData);
            if (birdBaseData != null)
            {
                BirdInfo bird = new BirdInfo(birdBaseData);
                Debug.Log("Loaded " + bird.spCode);
                Names.RegisterBird(bird);
                allBirds.Add(bird.spCode, bird);
            }
            else
            {
                Debug.Log("Bird Initialization failed for bird " + birdBaseData.code_sp);
            }
        }


        private void OnBirdImageLoaded(AsyncOperationHandle<Sprite> spriteLoadHandle)
        {
            if (spriteLoadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Sprite loadedSprite = spriteLoadHandle.Result;
                string spCode = loadedSprite.name[..6];
                BirdInfo bird = allBirds[spCode];
                Sex sex = GetSexFromImageName(loadedSprite.name);
                BirdImage birdImage = new(loadedSprite, sex);
                bird.AddImage(birdImage);
            }
        }
        private void OnBirdSoundLoaded(AsyncOperationHandle<AudioClip> soundLoadHandle)
        {
            if (soundLoadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                AudioClip loadedSound = soundLoadHandle.Result;
                string spCode = loadedSound.name[..6];
                BirdInfo bird = allBirds[spCode];
                SoundType type = GetSoundTypeFromSoundName(loadedSound.name);
                BirdSound birdSound = new(loadedSound, type);
                bird.AddSound(birdSound);
            }
        }

        private Sex GetSexFromImageName(string imageName)
        {
            return imageName.Substring(imageName.Length - 1) switch
            {
                "M" => Sex.Male,
                "F" => Sex.Female,
                _ => Sex.None,
            };
        }
        private SoundType GetSoundTypeFromSoundName(string soundName)
        {
            return soundName.Split("_").Last() switch
            {
                "cri" => SoundType.Alarm,
                "chant" => SoundType.Song,
                _ => SoundType.None,
            };
        }

        private void OnAssetsLoadingFinished()
        {
            foreach (BirdInfo bird in allBirds.Values) bird.Initialize();
            BirdsManager.allBirds = allBirds;
            Debug.Log(allBirds.Count.ToString() + " birds data loaded successfully");
        }
    }
}
