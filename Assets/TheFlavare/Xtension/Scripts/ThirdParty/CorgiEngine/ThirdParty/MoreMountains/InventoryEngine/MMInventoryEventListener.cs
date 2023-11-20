#if MOREMOUNTAINS_CORGIENGINE
#if ODIN_INSPECTOR || ODIN_INSPECTOR_3
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;

namespace TheFlavare.Support.InventoryEngine
{
    /// <summary>
	/// Event listener for MMInventoryEvent trigger
    /// Combined with Lua script comparator from Dialogue System
    /// Required: Odin Inspector 3, Dialogue System for Unity, Corgi Engine 8.7
    ///
    /// @raditzlawliet
    /// </summary>
    public class MMInventoryEventListener : SerializedMonoBehaviour, MMEventListener<MMInventoryEvent>
    {
        public bool debug = false;
        public bool triggerOnStart = true;

        public string targetInventoryName = ""; // "" means all
        public HashSet<MMInventoryEventType> targetEventTypes = new HashSet<MMInventoryEventType> { MMInventoryEventType.ContentChanged };

        [AssetSelector]
        public List<InventoryItem> targetEventItems;

        public Condition condition = new Condition();
        public UnityEvent actions;

        [Header("Test Event")]
        public MMInventoryEventType testEventType = MMInventoryEventType.InventoryLoaded;
        public string testInventoryName = "";
        [MMInspectorButton("TestSendEvent")]
        public bool testSendEventBool;

        public void Start()
        {
            if (triggerOnStart)
            {
                if (condition == null || condition.IsTrue(null))
                {
                    if (actions != null) actions.Invoke();
                }
            }
        }

        public void ManualTrigger() {
            if (condition == null || condition.IsTrue(null))
            {
                if (actions != null) actions.Invoke();
            }
        }

        public void OnMMEvent(MMInventoryEvent inventoryEvent)
        {
            if (debug) Debug.Log(inventoryEvent.TargetInventoryName + "=" + targetInventoryName + " | " +
                inventoryEvent.InventoryEventType + "=" + targetEventTypes + " | " +
                inventoryEvent.EventItem + "=" + targetEventItems + " | " + inventoryEvent.Quantity);

            // target inventory validate (exclude empty)
            if (inventoryEvent.TargetInventoryName != this.targetInventoryName && this.targetInventoryName != "")
            {
                return;
            }

            // target event type validate
            if (!this.targetEventTypes.Contains(inventoryEvent.InventoryEventType))
            {
                return;
            }

            // event item validate (exclude empty )
            if (targetEventItems != null && inventoryEvent.EventItem != null)
            {
                if (targetEventItems.Count > 0)
                {
                    if (!targetEventItems.Contains(inventoryEvent.EventItem))
                    {
                        return;
                    }
                }
            }

            if (condition == null || condition.IsTrue(null))
            {
                if (actions != null) actions.Invoke();
            }
        }

        void OnEnable()
        {
            this.MMEventStartListening<MMInventoryEvent>();
        }
        void OnDisable()
        {
            this.MMEventStopListening<MMInventoryEvent>();
        }

        public void TestSendEvent()
        {
            MMInventoryEvent.Trigger(testEventType, null, testInventoryName, null, 0, 0, "Player1");
        }
    }
}
#endif
#endif