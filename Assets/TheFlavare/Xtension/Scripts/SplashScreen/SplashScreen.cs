using UnityEngine;
using TheFlavare.Xtension;

namespace TheFlavare.SplashScreen
{
    // Boilerplate splash screen
    // 
    // @raditzlawliet
    public class SplashScreen : MonoBehaviour
    {
        [Header("Target & Loading Scene")]
        public SceneReference scene;
        public SceneReference sceneMobile;
        public SceneReference sceneLoading;

        [Header("On Start Run Animation")]
        public Animator animator;
        public string animatorTrigger = "Start";

        void Awake()
        {
            if (animator == null) animator = GetComponent<Animator>();
            if (sceneMobile == null) sceneMobile = scene;
        }

        void Start()
        {
            animator.SetTrigger(animatorTrigger);
        }

        public void SwitchScene()
        {
#if UNITY_ANDROID
            /// Run your Android only code here.
            SceneTools.GoToSceneWithLoading(SceneTools.LoadSceneMode.Native, sceneMobile.GetSceneName(), sceneLoading.GetSceneName());
#else
            /// Run your Others than Mobile.
            SceneTools.GoToSceneWithLoading(SceneTools.LoadSceneMode.Native, scene.GetSceneName(), sceneLoading.GetSceneName());
#endif
        }
    }
}