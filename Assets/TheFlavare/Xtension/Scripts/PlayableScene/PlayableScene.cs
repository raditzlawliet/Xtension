using UnityEngine;
using UnityEngine.Playables;

namespace TheFlavare.Xtension
{
    // Put this on scene as manager
    // it will help to sync data between addicitive scene and loader
    //
    // @raditzlawliet
    public class PlayableScene : MonoBehaviour
    {
        private string currentScene;
        private PlayableDirector director;

        bool playedTriggered;

        public void Awake()
        {
            currentScene = gameObject.scene.name;
            director = GetComponent<PlayableDirector>();

            director.played += OnTimelinePlayed;
            director.paused += OnTimelinePaused;
            director.stopped += OnTimelineStopped;
        }

        public void Start()
        {
            // anticipate director play on awake, but no triggering this
            if (director.state == PlayState.Playing && !playedTriggered)
                OnTimelinePlayed(director);
        }

        private void OnTimelineStopped(PlayableDirector playableDirector)
        {
            PlayableSceneLoader.Instance.OnPlayableStopped(currentScene, playableDirector);
        }

        private void OnTimelinePlayed(PlayableDirector playableDirector)
        {
            playedTriggered = true;
            PlayableSceneLoader.Instance.OnPlayablePlayed(currentScene, playableDirector);
        }

        private void OnTimelinePaused(PlayableDirector playableDirector)
        {
            PlayableSceneLoader.Instance.OnPlayablePaused(currentScene, playableDirector);
        }
    }
}