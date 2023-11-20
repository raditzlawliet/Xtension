using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace TheFlavare.Xtension
{
    // Loading Manager for Unity Event
    // 
    // @raditzlawliet
    public class LoadingManager : MonoBehaviour
    {
        public static string sceneToLoad;

        public UnityEvent OnLoadBegin;
        public UnityEvent OnLoadComplete;

        public UnityEvent<float> OnProgressUpdate;
        public UnityEvent<float> OnProgressUpdateReverse; // 1f - progress

        public float loadCompleteDelay;
        public float minimumLoadingTime = 1f;

        float _time;

        void Start()
        {
            _time = 0;
            StartCoroutine(LoadAsyncScene(sceneToLoad));
            sceneToLoad = "";
        }

        IEnumerator LoadAsyncScene(string _sceneToLoad)
        {
            if (_sceneToLoad != "" && _sceneToLoad != null)
            {
                OnLoadBegin?.Invoke();

                // The Application loads the Scene in the background at the same time as the current Scene.
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneToLoad);
                // Don't allow the scene to activate until you allow it to
                asyncLoad.allowSceneActivation = false;
                // While the scene is loading, update the loading progress
                while (!asyncLoad.isDone)
                {
                    _time += Time.deltaTime;
                    // Calculate the progress and update the loading screen
                    float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Divide by 0.9f to account for the build's loading progress
                    UpdateLoadingScreen(progress);

                    // If the loading is complete, activate the scene
                    if (asyncLoad.progress >= 0.9f)
                    {
                        if (_time >= minimumLoadingTime)
                        {
                            OnLoadComplete?.Invoke();
                            yield return new WaitForSeconds(loadCompleteDelay);

                            asyncLoad.allowSceneActivation = true;
                        }
                    }

                    yield return null;
                }
            }
            else
            {
                _sceneToLoad = "";
            }
        }

        void UpdateLoadingScreen(float progress)
        {
            // Update the loading screen UI elements (e.g., progress bar, text, etc.) based on the progress value
            // You can use the Unity UI APIs to manipulate the UI elements as per your design
            OnProgressUpdate.Invoke(progress);
            OnProgressUpdateReverse.Invoke(1f - progress);
        }
    }
}