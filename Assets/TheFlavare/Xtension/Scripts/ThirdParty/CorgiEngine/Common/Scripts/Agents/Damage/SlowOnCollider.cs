#if MOREMOUNTAINS_CORGIENGINE
using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Add this component to an object and it will cause slow
    /// Required Script: CharacterAdvContextSpeedMultiplier, CharacterHorizontalMovementAdvContextSpeedMultiplier
    /// Required: Corgi Engine 8.7
    /// 
    /// @raditzlawliet
    /// </summary>
    [AddComponentMenu("Corgi Engine/Character/Damage/SlowOnCollider")]
    public class SlowOnCollider : MonoBehaviour
    {
        [Header("Slow")]
        [MMInformation("This component will make your object slow to objects that collide with it. Here you can define what layers will be affected by the damage (for a standard enemy, choose Player), how much damage to give, and how much force should be applied to the object that gets the damage on hit. You can also specify how long the post-hit invincibility should last (in seconds).", MoreMountains.Tools.MMInformationAttribute.InformationType.Info, false)]

        public Collider2D _collider2D;
        [Tooltip("amount multiplier to slow, between 0-1 (0 means no slow 0%, 1 means 100%)")]
        public float baseAmountMultiplierSlow = 0.5f;
        [Tooltip("amount to apply to the multiplier each time the object is hit based on distance. 0 means further, 1 means closer middle point")]
        public AnimationCurve ratioMultiplierByDistance;
        public float applySlowEvery = 0.1f;
        [MMReadOnly]
        public bool unableApplySlow = false;

        [Header("Targets")]
        [Tooltip("the layers that will be damaged by this object")]
        public LayerMask TargetLayerMask;

        [Header("Feedback")]

        /// the owner of the DamageOnTouch zone
        [MMReadOnly]
        [Tooltip("the owner of the DamageOnTouch zone")]
        public GameObject Owner;

        /// a delegate used to communicate hit events 
        public delegate void OnSlowDelegate();
        public OnSlowDelegate OnSlow;

        [Header("Targets Ignore State")]
        /// an array containing all the blocking movement states. If the Character is in one of these states and tries to trigger this ability, it won't be permitted. Useful to prevent this ability from being used while Idle or Swimming, for example.
        [Tooltip("an array containing all the blocking movement states. If the Character is in one of these states and tries to trigger this ability, it won't be permitted. Useful to prevent this ability from being used while Idle or Swimming, for example.")]
        public CharacterStates.MovementStates[] BlockingMovementStates;
        /// an array containing all the blocking condition states. If the Character is in one of these states and tries to trigger this ability, it won't be permitted. Useful to prevent this ability from being used while dead, for example.
        [Tooltip("an array containing all the blocking condition states. If the Character is in one of these states and tries to trigger this ability, it won't be permitted. Useful to prevent this ability from being used while dead, for example.")]
        public CharacterStates.CharacterConditions[] BlockingConditionStates;

        // storage		
        protected Collider2D _collidingCollider;
        protected CorgiController _corgiController;
        protected CorgiController _colliderCorgiController;
        protected List<GameObject> _ignoredGameObjects;
        protected bool _initializedFeedbacks = false;

        /// <summary>
        /// Initialization
        /// </summary>
        protected virtual void Awake()
        {
            _ignoredGameObjects = new List<GameObject>();
            _corgiController = this.gameObject.GetComponent<CorgiController>();
            if (_collider2D == null)
                _collider2D = this.gameObject.GetComponent<Collider2D>();
            if (_collider2D != null) { _collider2D.isTrigger = true; }

            InitializeFeedbacks();
        }

        /// <summary>
        /// A public method you can use to set the controller from another class
        /// </summary>
        /// <param name="newController"></param>
        public virtual void SetCorgiController(CorgiController newController)
        {
            _corgiController = newController;
        }

        protected virtual void InitializeFeedbacks()
        {
            if (_initializedFeedbacks)
            {
                return;
            }
            _initializedFeedbacks = true;
        }

        /// <summary>
        /// OnEnable we set the start time to the current timestamp
        /// </summary>
        protected virtual void OnEnable()
        {
        }

        /// <summary>
        /// During last update, we store the position and velocity of the object
        /// </summary>
        protected virtual void Update()
        {

        }

        /// <summary>
        /// Adds the gameobject set in parameters to the ignore list
        /// </summary>
        /// <param name="newIgnoredGameObject">New ignored game object.</param>
        public virtual void IgnoreGameObject(GameObject newIgnoredGameObject)
        {
            if (_ignoredGameObjects == null)
            {
                _ignoredGameObjects = new List<GameObject>();
            }
            _ignoredGameObjects.Add(newIgnoredGameObject);
        }

        /// <summary>
        /// Removes the object set in parameters from the ignore list
        /// </summary>
        /// <param name="ignoredGameObject">Ignored game object.</param>
        public virtual void StopIgnoringObject(GameObject ignoredGameObject)
        {
            _ignoredGameObjects.Remove(ignoredGameObject);
        }

        /// <summary>
        /// Clears the ignore list.
        /// </summary>
        public virtual void ClearIgnoreList()
        {
            _ignoredGameObjects.Clear();
        }

        /// <summary>
        /// When a collision with the player is triggered, we give damage to the player and knock it back
        /// </summary>
        /// <param name="collider">what's colliding with the object.</param>
        public virtual void OnTriggerStay2D(Collider2D collider)
        {
            Colliding(collider);
        }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            Colliding(collider);
        }

        public virtual void OnTriggerExit2D(Collider2D collider)
        {
            CharacterHorizontalMovementAdvContextSpeedMultiplier _colliderContextSpeed = collider.gameObject.MMGetComponentNoAlloc<CharacterHorizontalMovementAdvContextSpeedMultiplier>();
            if (_colliderContextSpeed == null)
            {
                return;
            }

            if (_colliderContextSpeed._characterAdvContextSpeedMultiplier == null)
            {
                return;
            }

            ClearSlow(_colliderContextSpeed._characterAdvContextSpeedMultiplier);
        }

        protected virtual void Colliding(Collider2D collider)
        {
            if (!this.isActiveAndEnabled)
            {
                return;
            }

            // if the object we're colliding with is part of our ignore list, we do nothing and exit
            if (_ignoredGameObjects.Contains(collider.gameObject))
            {
                return;
            }

            // if what we're colliding with isn't part of the target layers, we do nothing and exit
            if (!MMLayers.LayerInLayerMask(collider.gameObject.layer, TargetLayerMask))
            {
                return;
            }

            Character _colliderCharacter = collider.gameObject.MMGetComponentNoAlloc<Character>();
            if (_colliderCharacter != null)
            {
                if (!isTargetNotInBlockingState(_colliderCharacter))
                    return;
            }

            CharacterHorizontalMovementAdvContextSpeedMultiplier _colliderContextSpeed = collider.gameObject.MMGetComponentNoAlloc<CharacterHorizontalMovementAdvContextSpeedMultiplier>();
            if (_colliderContextSpeed == null)
            {
                return;
            }

            if (_colliderContextSpeed._characterAdvContextSpeedMultiplier == null)
            {
                return;
            }

            // it should be ... 1 player right ?
            if (unableApplySlow)
            {
                return;
            }
            unableApplySlow = true;

            _collidingCollider = collider;

            OnCollide(_colliderContextSpeed._characterAdvContextSpeedMultiplier, collider);
        }

        /// <summary>
        /// Describes what happens when colliding with a damageable object
        /// </summary>
        /// <param name="health">Health.</param>
        protected virtual void OnCollide(CharacterAdvContextSpeedMultiplier _colliderContextSpeed, Collider2D collider)
        {
            OnSlow?.Invoke();

            float maxDistance = 0;
            if (_collider2D is CircleCollider2D)
            {
                maxDistance = ((CircleCollider2D)_collider2D).radius * transform.lossyScale.x;
            }
            else
            {
                maxDistance = (_collider2D.bounds.size.x / 2) * transform.lossyScale.x;
            }

            float distance = Vector2.Distance(_collider2D.bounds.center, collider.bounds.center);
            distance = Mathf.Min(distance, maxDistance);

            float distancePhysics2d = Physics2D.Distance(_collider2D, collider).distance;
            float multiplierSlow = ratioMultiplierByDistance.Evaluate(Mathf.Clamp(maxDistance - distance, 0, maxDistance) / maxDistance);
            // calculate multiplier slow based on base multiplier slow
            // if base multiplier slow is 1, then we don't apply any slow
            // if more than 1, accelerate. less than 1, decelerate.
            baseAmountMultiplierSlow = Mathf.Clamp(baseAmountMultiplierSlow, 0, 1);
            multiplierSlow = Mathf.Clamp(multiplierSlow, 0, 1);

            float finalMultiplierSlow = 1 - (baseAmountMultiplierSlow * multiplierSlow);
            // Debug.Log(distance + " " + maxDistance + "= " + finalMultiplierSlow + " ph2d " + distancePhysics2d + " " + transform.lossyScale.x);

            _colliderContextSpeed.Add(this.name, false, this.gameObject.GetInstanceID().ToString(), finalMultiplierSlow);

            StartCoroutine(ableApplySlowIn(applySlowEvery));
        }

        protected virtual void ClearSlow(CharacterAdvContextSpeedMultiplier _colliderContextSpeed)
        {
            OnSlow?.Invoke();

            _colliderContextSpeed.Remove(this.name, false, this.gameObject.GetInstanceID().ToString());
        }
        /// <summary>
        /// Allows the character to take damage
        /// </summary>
        public virtual IEnumerator ableApplySlowIn(float delay)
        {
            yield return MMCoroutine.WaitFor(delay);
            unableApplySlow = false;
        }

        private bool isTargetNotInBlockingState(Character _character)
        {
            if (_character != null)
            {
                if ((BlockingMovementStates != null) && (BlockingMovementStates.Length > 0))
                {
                    for (int i = 0; i < BlockingMovementStates.Length; i++)
                    {
                        if (BlockingMovementStates[i] == (_character.MovementState.CurrentState))
                        {
                            return false;
                        }
                    }
                }

                if ((BlockingConditionStates != null) && (BlockingConditionStates.Length > 0))
                {
                    for (int i = 0; i < BlockingConditionStates.Length; i++)
                    {
                        if (BlockingConditionStates[i] == (_character.ConditionState.CurrentState))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
#endif