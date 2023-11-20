#if MOREMOUNTAINS_CORGIENGINE
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// This zone lets you impact a Character equipped with an AutoMovement ability
    /// You'll be able to use it to change its direction, stop it in its tracks, or make it start/stop running
    /// with support temporary run
    /// Required: Corgi Engine 8.7
    /// 
    /// @raditzlawliet
    /// </summary>
    [AddComponentMenu("Corgi Engine/Environment/AutoMovementControlZone")]
    [RequireComponent(typeof(Collider2D))]
    public class AutoMovementRunModeZone : CorgiMonoBehaviour
    {
        /// the possible modes you can change the run state
        public enum RunModes { None, Toggle, ForceRun, ForceRunTemporary, ForceWalk, ForceWalkTemporary }

        /// the selected run mode
        /// none : does nothing
        /// toggle : runs if walking, walks if running
        /// force run : makes the character run
        /// force walk : makes the character walk
        /// force run temporary: make charater run in area and back after leaving area
        /// force walk temporary: make charater walk in area and back after leaving area
        [Tooltip("the selected run mode\n" +
                 "- none : does nothing\n" +
                 "- toggle : runs if walking, walks if running\n" +
                 "- force run : makes the character run\n" +
                 "- force run temporary: make charater run in area and back after leaving area\n" +
                 "- force walk : makes the character walk\n" +
                 "- force walk : make charater walk in area and back after leaving area")]
        public RunModes RunMode = RunModes.None;

        protected Collider2D _collider2D;
        protected CharacterAutoMovement _characterAutoMovement;

        /// <summary>
        /// On awake grabs the Collider2D and sets it correctly to is trigger
        /// </summary>
        protected virtual void Awake()
        {
            _collider2D = this.gameObject.GetComponent<Collider2D>();
            _collider2D.isTrigger = true;
        }

        /// <summary>
        /// On trigger enter, we handle our collision
        /// </summary>
        /// <param name="collider"></param>
        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            HandleCollisionEnter(collider);
        }

        /// <summary>
        /// On trigger enter, we handle our collision
        /// </summary>
        /// <param name="collider"></param>
        protected virtual void OnTriggerExit2D(Collider2D collider)
        {
            HandleCollisionStay(collider);
        }

        /// <summary>
        /// On trigger enter, we handle our collision
        /// </summary>
        /// <param name="collider"></param>
        protected virtual void OnTriggerStay2D(Collider2D collider)
        {
            HandleCollisionExit(collider);
        }

        /// <summary>
        /// Tests if we're colliding with a CharacterAutoMovement and interacts with it if needed
        /// </summary>
        /// <param name="collider"></param>
        protected virtual void HandleCollisionEnter(Collider2D collider)
        {
            _characterAutoMovement = collider.gameObject.MMGetComponentNoAlloc<Character>()?.FindAbility<CharacterAutoMovement>();

            if (_characterAutoMovement == null)
            {
                return;
            }

            switch (RunMode)
            {
                case RunModes.Toggle:
                    _characterAutoMovement.ToggleRun();
                    break;
                case RunModes.ForceRun:
                case RunModes.ForceRunTemporary:
                    _characterAutoMovement.ForceRun(true);
                    break;
                case RunModes.ForceWalk:
                case RunModes.ForceWalkTemporary:
                    _characterAutoMovement.ForceRun(false);
                    break;
            }
        }

        /// <summary>
        /// Tests if we're colliding with a CharacterAutoMovement and interacts with it if needed
        /// </summary>
        /// <param name="collider"></param>
        protected virtual void HandleCollisionStay(Collider2D collider)
        {
            _characterAutoMovement = collider.gameObject.MMGetComponentNoAlloc<Character>()?.FindAbility<CharacterAutoMovement>();

            if (_characterAutoMovement == null)
            {
                return;
            }

            switch (RunMode)
            {
                case RunModes.ForceRunTemporary:
                    _characterAutoMovement.ForceRun(true);
                    break;
                case RunModes.ForceWalkTemporary:
                    _characterAutoMovement.ForceRun(false);
                    break;
            }
        }

        /// <summary>
        /// Tests if we're colliding with a CharacterAutoMovement and interacts with it if needed
        /// </summary>
        /// <param name="collider"></param>
        protected virtual void HandleCollisionExit(Collider2D collider)
        {
            _characterAutoMovement = collider.gameObject.MMGetComponentNoAlloc<Character>()?.FindAbility<CharacterAutoMovement>();

            if (_characterAutoMovement == null)
            {
                return;
            }
            
            // List<Collider2D> a = new List<Collider2D>();
            // collider.GetContacts(a);
            // foreach (var item in a)
            // {
            //     if (item.gameObject.MMGetComponentNoAlloc<AutoMovementRunModeZone>())
            // }

            switch (RunMode)
            {
                case RunModes.ForceRunTemporary:
                    _characterAutoMovement.ForceRun(false);
                    break;
                case RunModes.ForceWalkTemporary:
                    _characterAutoMovement.ForceRun(true);
                    break;
            }
        }
    }
}
#endif