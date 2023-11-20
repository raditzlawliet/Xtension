#if MOREMOUNTAINS_CORGIENGINE
using UnityEngine;
using System.Collections.Generic;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Add this component to a trigger zone, and it'll override the CharacterJump settings for all characters that cross it 
    /// with support multiple stack JumpOverrides (that original CharacterJumpOverride)
    /// Required: Corgi Engine 8.7
    /// 
    /// @raditzlawliet
    /// </summary>
    [AddComponentMenu("Corgi Engine/Environment/Character Jump Override Stack")]
    public class CharacterJumpOverrideStack : CorgiMonoBehaviour
    {
        [Header("Jumps")]
        /// defines how high the character can jump
        [Tooltip("defines how high the character can jump")]
        public float JumpHeight = 3.025f;
        /// the minimum time in the air allowed when jumping - this is used for pressure controlled jumps
        [Tooltip("the minimum time in the air allowed when jumping - this is used for pressure controlled jumps")]
        public float JumpMinimumAirTime = 0.1f;
        /// the maximum number of jumps allowed (0 : no jump, 1 : normal jump, 2 : double jump, etc...)
        [Tooltip("the maximum number of jumps allowed (0 : no jump, 1 : normal jump, 2 : double jump, etc...)")]
        public int NumberOfJumps = 3;
        /// basic rules for jumps : where can the player jump ?
        [Tooltip("basic rules for jumps : where can the player jump ?")]
        public CharacterJump.JumpBehavior JumpRestrictions;
        /// if this is set to true, on enter/exit, the number of jumps will be set to its maximum, otherwise it'll retain the value it had on entry
        [Tooltip("if this is set to true, on enter/exit, the number of jumps will be set to its maximum, otherwise it'll retain the value it had on entry")]
        public bool ResetNumberOfJumpsLeft = true;

        ///
		[SerializeField]
        protected static Dictionary<CharacterJump, List<CharacterJumpOverrideStack>> _stack = new Dictionary<CharacterJump, List<CharacterJumpOverrideStack>>();
        protected float _previousJumpHeight;
        protected float _previousJumpMinimumAirTime;
        protected int _previousNumberOfJumps;
        protected int _previousNumberOfJumpsLeft;
        protected CharacterJump.JumpBehavior _previousJumpRestrictions;

        ///
        protected Character _character;
        protected CharacterJump _characterJump;

        protected virtual void Update()
        {
            if ((_character != null) && (_characterJump != null))
            {
                if (_character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)
                {
                    Restore();
                    _character = null;
                    _characterJump = null;
                }
            }
        }

        /// <summary>
        /// Triggered when something collides with the override zone
        /// </summary>
        /// <param name="collider">Something colliding with the override zone.</param>
        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            // we check that the object colliding with the override zone is actually a characterJump
            _characterJump = collider.GetComponent<Character>()?.FindAbility<CharacterJump>();
            _character = collider.GetComponent<Character>();

            if (_character == null)
            {
                return;
            }
            if (_characterJump == null)
            {
                return;
            }

            lock (_stack)
            {
                if (!_stack.ContainsKey(_characterJump))
                    _stack.Add(_characterJump, new List<CharacterJumpOverrideStack>());

                // Add stack
                if (!_stack[_characterJump].Contains(this))
                    _stack[_characterJump].Add(this);
            }

            // Save previous based on stack or original
            bool _savePrevious = false;
            foreach (var _override in _stack[_characterJump])
            {
                if (_override == this)
                    continue;

                _previousJumpHeight = _override._previousJumpHeight;
                _previousJumpMinimumAirTime = _override._previousJumpMinimumAirTime;
                _previousNumberOfJumps = _override._previousNumberOfJumps;
                _previousJumpRestrictions = _override._previousJumpRestrictions;
                _previousNumberOfJumpsLeft = _override._previousNumberOfJumpsLeft;

                _savePrevious = true;
                break;
            }

            if (!_savePrevious)
            {
                _previousJumpHeight = _characterJump.JumpHeight;
                _previousJumpMinimumAirTime = _characterJump.JumpMinimumAirTime;
                _previousNumberOfJumps = _characterJump.NumberOfJumps;
                _previousJumpRestrictions = _characterJump.JumpRestrictions;
                _previousNumberOfJumpsLeft = _characterJump.NumberOfJumpsLeft;
            }

            _characterJump.JumpHeight = JumpHeight;
            _characterJump.JumpMinimumAirTime = JumpMinimumAirTime;
            _characterJump.NumberOfJumps = NumberOfJumps;
            _characterJump.JumpRestrictions = JumpRestrictions;
            if (ResetNumberOfJumpsLeft)
            {
                _characterJump.SetNumberOfJumpsLeft(NumberOfJumps);
            }

            // // Example Lock
            // lock (_stack)
            // {
            // };
        }

        /// <summary>
        /// Triggered when something exits the water
        /// </summary>
        /// <param name="collider">Something colliding with the water.</param>
        protected virtual void OnTriggerExit2D(Collider2D collider)
        {
            // we check that the object colliding with the water is actually a characterJump
            _characterJump = collider.GetComponent<Character>()?.FindAbility<CharacterJump>();
            Restore();
        }

        /// <summary>
        /// Restore the initial values for the colliding CharacterJump
        /// </summary>
        protected virtual void Restore()
        {
            if (_characterJump == null)
            {
                return;
            }

            lock (_stack)
            {
                if (!_stack.ContainsKey(_characterJump))
                    _stack.Add(_characterJump, new List<CharacterJumpOverrideStack>());

                // remove
                if (_stack[_characterJump].Contains(this))
                    _stack[_characterJump].Remove(this);
            }

            if (_stack[_characterJump].Count > 0)
            {
                var _override = _stack[_characterJump][_stack[_characterJump].Count - 1];
                // restore original with latest override
                _characterJump.JumpHeight = _override.JumpHeight;
                _characterJump.JumpMinimumAirTime = _override.JumpMinimumAirTime;
                _characterJump.NumberOfJumps = _override.NumberOfJumps;
                _characterJump.JumpRestrictions = _override.JumpRestrictions;
                if (ResetNumberOfJumpsLeft)
                {
                    _characterJump.SetNumberOfJumpsLeft(_characterJump.NumberOfJumps);
                }
                else
                {
                    _characterJump.SetNumberOfJumpsLeft(_override._previousNumberOfJumpsLeft);
                }
            }
            else
            {
                // restore original with this
                _characterJump.JumpHeight = _previousJumpHeight;
                _characterJump.JumpMinimumAirTime = _previousJumpMinimumAirTime;
                _characterJump.NumberOfJumps = _previousNumberOfJumps;
                _characterJump.JumpRestrictions = _previousJumpRestrictions;
                if (ResetNumberOfJumpsLeft)
                {
                    _characterJump.SetNumberOfJumpsLeft(_characterJump.NumberOfJumps);
                }
                else
                {
                    _characterJump.SetNumberOfJumpsLeft(_previousNumberOfJumpsLeft);
                }
            }
        }
    }
}
#endif