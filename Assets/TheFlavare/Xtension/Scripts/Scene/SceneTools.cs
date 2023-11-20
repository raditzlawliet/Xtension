using UnityEngine;

namespace TheFlavare.Xtension
{
    // Scene tools for Unity Event
    // 
    // @raditzlawliet
    public class SceneTools : MonoBehaviour
    {
        public enum LoadSceneMode
        {
            Native,
        }

        public LoadSceneMode use = LoadSceneMode.Native;
        public SceneReference loadingScene;
        public void GoToScene(string targetScene)
        {
            GoToScene(use, targetScene);
        }
        public void GoToScene(SceneReference targetScene)
        {
            GoToScene(use, targetScene.GetSceneName());
        }

        public static void GoToScene(LoadSceneMode use, string targetScene)
        {
            /// Native Unity
            if (use == LoadSceneMode.Native)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene);
            }
        }

        public void GoToSceneWithLoading(string targetScene)
        {
            GoToSceneWithLoading(use, targetScene, loadingScene.GetSceneName());
        }
        public void GoToSceneWithLoading(SceneReference targetScene)
        {
            GoToSceneWithLoading(use, targetScene.GetSceneName(), loadingScene.GetSceneName());
        }
        public static void GoToSceneWithLoading(LoadSceneMode use, string targetScene, string loadingScene)
        {
            /// Native Unity
            if (use == LoadSceneMode.Native)
            {
                LoadingManager.sceneToLoad = targetScene;
                UnityEngine.SceneManagement.SceneManager.LoadScene(loadingScene);
            }
        }
    }
}
