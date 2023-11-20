using UnityEngine;
using UnityEngine.Playables;

namespace TheFlavare.Xtension
{
    // PlayableDirectorControl, Able to pause, Resume and SetSpeed Playable Director
    // 
    // @raditzlawliet
    public class PlayableDirectorControl : MonoBehaviour
    {
        PlayableDirector playableDirector;
        public void Pause()
        {
            PlayableDirector _playableDirector = playableDirector;
            if (_playableDirector == null) _playableDirector = GetComponent<PlayableDirector>();

            _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }

        public void Resume()
        {
            PlayableDirector _playableDirector = playableDirector;
            if (_playableDirector == null) _playableDirector = GetComponent<PlayableDirector>();

            _playableDirector.time = _playableDirector.time;
            _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }

        public void SetSpeed(double t)
        {
            PlayableDirector _playableDirector = playableDirector;
            if (_playableDirector == null) _playableDirector = GetComponent<PlayableDirector>();

            _playableDirector.time = _playableDirector.time;
            _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(t);
        }
    }
}