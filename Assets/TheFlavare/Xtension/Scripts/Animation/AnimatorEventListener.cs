#if ODIN_INSPECTOR || ODIN_INSPECTOR_3
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace TheFlavare.Xtension
{
    // Event receiver for Animation using key-event values.
    // Required: Odin Inspector 3
    // 
    // @raditzlawliet
    // 
    // [ShowOdinSerializedPropertiesInInspectorAttribute]
    [System.Serializable]
    public class AnimatorEventListener : SerializedMonoBehaviour
    {
        public Dictionary<string, UnityEvent> events = new Dictionary<string, UnityEvent>();

        public void Start()
        {
        }

        [Button]
        // split by comma if needed for value
        public void OnEvent(string param)
        {
            if (!enabled) return;

            string[] keys = param.Split(',');
            if (events.ContainsKey(keys[0]))
                if (events[keys[0]] != null) events[keys[0]].Invoke();
        }
    }
}
#endif