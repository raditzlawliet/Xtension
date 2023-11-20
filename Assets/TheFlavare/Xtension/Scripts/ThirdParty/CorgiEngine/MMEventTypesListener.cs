#if MOREMOUNTAINS_CORGIENGINE
#if ODIN_INSPECTOR || ODIN_INSPECTOR_3
using System.Collections.Generic;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
	/// Event listener for CorgiEngineEvent
    /// Required: Corgi Engine 8.7, Odin Inspector 3
    ///
    /// @raditzlawliet
    /// </summary>
    public class MMEventTypesListener : SerializedMonoBehaviour, MMEventListener<CorgiEngineEvent>
    {
        public Dictionary<CorgiEngineEventTypes, MMFeedbacks> feedbacks;

        public virtual void OnMMEvent(CorgiEngineEvent corgiEngineEvent)
        {
            if (feedbacks == null)
                return;
            if (!feedbacks.ContainsKey(corgiEngineEvent.EventType))
                return;

            feedbacks[corgiEngineEvent.EventType]?.PlayFeedbacks();
        }

        /// <summary>
        /// On enable, we start listening to events
        /// </summary>
        protected virtual void OnEnable()
        {
            this.MMEventStartListening<CorgiEngineEvent>();
        }

        /// <summary>
        /// On disable, we stop listening to events
        /// </summary>
        protected virtual void OnDisable()
        {
            this.MMEventStopListening<CorgiEngineEvent>();
        }
    }
}
#endif
#endif