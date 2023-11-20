#if MOREMOUNTAINS_CORGIENGINE
using System;
using UnityEngine;
using System.Collections.Generic;
using MoreMountains.Tools;
#if ODIN_INSPECTOR || ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#endif

// Corgi Engine Extension for Advance Context Speed Multiplier
// Required Script: replace CharacterHorizontalMovement to CharacterHorizontalMovementAdvContextSpeedMultiplier  
// Required: Corgi Engine 8.7
// 
// @raditzlawliet
namespace MoreMountains.CorgiEngine
{
    [Serializable]
    public class CharacterAdvContextSpeedMultiplier
    {
        [Serializable]
        public class AdvContextSpeedMultiplier
        {
#if ODIN_INSPECTOR || ODIN_INSPECTOR_3
            [OdinSerialize]
#endif
            public Dictionary<string, float> multiplier = new Dictionary<string, float>();
            public float finalMultiplier
            {
                get
                {
                    // get highest value in multiplier
                    if (multiplier.Count == 0) return 1;

                    float finalMultiplier = 0;
                    foreach (var item in multiplier)
                    {
                        if (item.Value > finalMultiplier)
                            finalMultiplier = item.Value;
                    }
                    return finalMultiplier;
                }
            }
        }

        [Header("Info")]
#if ODIN_INSPECTOR || ODIN_INSPECTOR_3
        [ShowInInspector]
        [NonSerialized, OdinSerialize]
#endif
        protected Dictionary<string, AdvContextSpeedMultiplier> contextSpeedStack = new Dictionary<string, AdvContextSpeedMultiplier>();
        public float contextSpeedMultiplier
        {
            get
            {
                if (isCache) return cacheContextSpeedMultiplier;
                if (contextSpeedStack.Count > 0)
                {
                    float finalMultiplier = 1;
                    foreach (var item in contextSpeedStack)
                    {
                        finalMultiplier *= item.Value.finalMultiplier;
                    }
                    return finalMultiplier;
                }
                else
                    return 1;
            }
        }
        [MMReadOnly]
        public bool isCache = false;
        [MMReadOnly]
        public float cacheContextSpeedMultiplier = 1;

        protected void Caching()
        {
            isCache = false;
            cacheContextSpeedMultiplier = contextSpeedMultiplier;
            isCache = true;
        }

        public void Add(string componentName, bool stackable, string gameObjectID, float multiplier)
        {
            var id = generateID(componentName, stackable, gameObjectID);
            if (stackable)
            {
                if (!contextSpeedStack.ContainsKey(id))
                {
                    var contextSpeed = new AdvContextSpeedMultiplier();
                    contextSpeed.multiplier.Add(gameObjectID, multiplier);
                    contextSpeedStack.Add(id, contextSpeed);
                }
            }
            else
            {
                if (!contextSpeedStack.ContainsKey(id))
                {
                    var contextSpeed = new AdvContextSpeedMultiplier();
                    contextSpeed.multiplier.Add(gameObjectID, multiplier);
                    contextSpeedStack.Add(id, contextSpeed);
                }
                else
                {
                    var contextSpeed = contextSpeedStack[id];
                    if (contextSpeed.multiplier.ContainsKey(gameObjectID))
                    {
                        contextSpeed.multiplier[gameObjectID] = multiplier;
                    }
                    else
                    {
                        contextSpeed.multiplier.Add(gameObjectID, multiplier);
                    }
                }
            }
            Caching();
        }

        public void Remove(string componentName, bool stackable, string gameObjectID)
        {
            var id = generateID(componentName, stackable, gameObjectID);
            if (stackable)
            {
                contextSpeedStack.Remove(id);
            }
            else
            {
                List<string> toRemove = new List<string>();
                foreach (var item in contextSpeedStack)
                {
                    item.Value.multiplier.Remove(gameObjectID);
                    if (item.Value.multiplier.Count == 0)
                        toRemove.Add(item.Key);
                }

                foreach (var item in toRemove)
                {
                    contextSpeedStack.Remove(item);
                }
            }
            Caching();
        }

        public void RemoveAll()
        {
            contextSpeedStack.Clear();
            Caching();
        }

        protected string generateID(string componentName, bool stackable, string gameObjectID)
        {
            if (stackable)
                return $"{componentName}_{stackable}_{gameObjectID}";
            else
                return $"{componentName}_{stackable}";
        }
    }
}
#endif