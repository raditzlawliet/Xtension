#if MOREMOUNTAINS_CORGIENGINE
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
    /// <summary>
	/// Additional Event for MMCountdown, will execute event after reaching spesific countdown value
    /// Required: Corgi Engine 8.7
    ///
    /// @raditzlawliet
    /// </summary>
    [RequireComponent(typeof(MMCountdown))]
    [AddComponentMenu("More Mountains/Tools/Time/MMCountdown Event")]
    public class MMCountdownEvent : MonoBehaviour
    {
        [Tooltip("Working only for Descending")]
        public MMCountdown _countdown;
        public enum MMCountdownAtValue
        {
            Value, Percent
        }
        [Serializable]
        public class MMCountdownAtEvent
        {
            public float Value;
            [Tooltip("Percent can't work with countdown infinity")]
            public MMCountdownAtValue Type = MMCountdownAtValue.Value;
            [MMReadOnly]
            public bool Triggered = false;
            [MMReadOnly]
            public float TriggerAt = 0f;
            public UnityEvent Event;
        }
        public List<MMCountdownAtEvent> Events;
        public void Awake()
        {
            _countdown = GetComponent<MMCountdown>();
        }
        public void Update()
        {
            if (!_countdown.isCountdowning)
                return;

            foreach (MMCountdownAtEvent e in Events)
            {
                if (e.Triggered)
                    continue;

                if (_countdown.Direction == MMCountdown.MMCountdownDirections.Descending)
                {
                    if (e.Type == MMCountdownAtValue.Value)
                    {
                        if (_countdown.CurrentTime <= e.Value)
                        {
                            e.Triggered = true;
                            e.TriggerAt = _countdown.CurrentTime;
                            e.Event?.Invoke();
                        }
                    }
                    else
                    {
                        if (_countdown.Infinite)
                            return;
                        if ((_countdown.CurrentTime - _countdown.CountdownTo) / (_countdown.CountdownFrom - _countdown.CountdownTo) * 100 <= e.Value)
                        {
                            e.Triggered = true;
                            e.TriggerAt = _countdown.CurrentTime;
                            e.Event?.Invoke();
                        }
                    }
                }
            }
        }

        public void ResetEvent()
        {
            foreach (var item in Events)
            {
                item.Triggered = false;
                item.TriggerAt = 0;
            }
        }
    }
}
#endif