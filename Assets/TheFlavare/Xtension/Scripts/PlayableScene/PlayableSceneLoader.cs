using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace TheFlavare.Xtension
{
    // put this as singleton for the caller
    // It will help to load playable scene
    //
    // @raditzlawliet
    public class PlayableSceneLoader : Singleton<PlayableSceneLoader>
    {
        public bool unloadSceneAfterDone;
        private PlayableSceneData lastData;
        private PlayableDirector lastPlayableDirector;
        public UnityEngine.Events.UnityEvent onPlayed, onPaused, onStopped;

        public void Load(PlayableSceneData data)
        {
            StartCoroutine(LoadAsyncScene(data));
        }

        IEnumerator LoadAsyncScene(PlayableSceneData data)
        {
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(data.playableScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        IEnumerator UnloadSceneAsync(string sceneName)
        {
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName, UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        public void OnPlayablePlayed(string sceneName, PlayableDirector playableDirector)
        {
            lastPlayableDirector = playableDirector;
            onPlayed?.Invoke();
        }

        public void OnPlayableStopped(string sceneName, PlayableDirector playableDirector)
        {
            lastPlayableDirector = playableDirector;
            if (unloadSceneAfterDone)
                StartCoroutine(UnloadSceneAsync(sceneName));

            onStopped?.Invoke();
        }

        public void OnPlayablePaused(string sceneName, PlayableDirector playableDirector)
        {
            lastPlayableDirector = playableDirector;
            onPaused?.Invoke();
        }

        public void ResumePlayable()
        {
            lastPlayableDirector?.Resume();
        }

        public void StopPlayable()
        {
            lastPlayableDirector?.Stop();
        }
        public void PlayPlayable()
        {
            lastPlayableDirector?.Play();
        }

        public void TryUnloadLastScene()
        {
            StartCoroutine(UnloadSceneAsync(lastData.playableScene));
        }
    }
}