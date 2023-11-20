#if MOREMOUNTAINS_CORGIENGINE
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// This ability will make your character move automatically, without having to touch the left or right inputs
    /// With extension able to temporary change direction while wall jump, and revert back after touch the ground.
    /// Good Example: Super Mario Run
    /// Required: Corgi Engine 8.7
    ///
    /// @raditzlawliet
    /// </summary>
    [MMHiddenProperties("AbilityStartFeedbacks", "AbilityStopFeedbacks")]
    [AddComponentMenu("Corgi Engine/Character/Abilities/Character Auto Movement Extend")]
    [RequireComponent(typeof(CharacterHorizontalMovement))]
    public class CharacterAutoMovementExt : CharacterAutoMovement
    {
        [Header("Automatic Direction Changes")]

        /// if this is true, doing a walljump will also cause a direction change
        [Tooltip("if this is true, doing a walljump will also save a state, and you can revert it back")]
        public bool SaveStateWhenWallJump = false;

        protected bool _walljumpTemporaryApplied = false;
        protected float _directionBeforeWallJump = 1;
        protected bool _runningBeforeWallJump = false;

        /// <summary>
        /// On init we grab our components and set them if needed, set our initial direction and run state
        /// </summary>
        protected override void Initialization()
        {
            base.Initialization();
        }
        /// <summary>
        /// When the Character walljumps, we change direction if needed
        /// </summary>
        protected override void OnWallJump()
        {
            if (ChangeDirectionOnWalljump)
            {
                SetWallJumpTemporary();
                ChangeDirection();
            }
        }

        protected virtual void SetWallJumpTemporary()
        {
            if (_walljumpTemporaryApplied) return;

            _walljumpTemporaryApplied = true;
            _directionBeforeWallJump = _currentDirection;
            _runningBeforeWallJump = _running;
        }

        public virtual void ResetWallJumpTemporary()
        {
            _walljumpTemporaryApplied = false;
        }

        // after grounded, set back
        public virtual void ApplyWallJumpTemporary()
        {
            if (!_walljumpTemporaryApplied) return;

            _walljumpTemporaryApplied = false;
            _currentDirection = _directionBeforeWallJump;
            _running = _runningBeforeWallJump;
        }
    }
}
#endif