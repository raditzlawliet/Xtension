using UnityEngine;
using System;

namespace TheFlavare.Xtension
{
    // Holding playable scene data
    // 
    // @raditzlawliet
    [Serializable]
    [CreateAssetMenu(fileName = "New Playable Scene", menuName = "Playable Scene", order = 51)]
    public class PlayableSceneData : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField]
        public string identity;
        public SceneReference playableScene;

        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }
    }
}