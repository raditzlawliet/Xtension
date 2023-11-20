#if MOREMOUNTAINS_CORGIENGINE
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
    /// <summary>
	/// Event listener for MMSceneLoadingManager.LoadingSceneEvent
    /// Required: Corgi Engine 8.7
    ///
    /// @raditzlawliet
    /// </summary>
    public class MMLoadingSceneEventListener : MonoBehaviour, MMEventListener<MMSceneLoadingManager.LoadingSceneEvent>
    {
        public MMSceneLoadingManager.LoadingStatus target;
        public UnityEvent action;

        public void Start()
        {
        }

        public void ManualTrigger()
        {
        }

        public void OnMMEvent(MMSceneLoadingManager.LoadingSceneEvent e)
        {
            if (target == e.Status)
            {
                action?.Invoke();
            }
        }

        protected void OnEnable()
        {
            this.MMEventStartListening<MMSceneLoadingManager.LoadingSceneEvent>();
        }
        protected void OnDisable()
        {
            this.MMEventStopListening<MMSceneLoadingManager.LoadingSceneEvent>();
        }
    }
}
#endif