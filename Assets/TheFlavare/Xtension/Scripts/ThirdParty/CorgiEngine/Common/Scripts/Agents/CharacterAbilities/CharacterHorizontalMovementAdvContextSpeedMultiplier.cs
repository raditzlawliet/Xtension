#if MOREMOUNTAINS_CORGIENGINE
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Add this ability to a Character to have it handle horizontal movement (walk, and potentially run, crawl, etc)
    /// Animator parameters : Speed (float), Walking (bool)
    /// with support CharacterAdvContextSpeedMultiplier
    /// Required: Corgi Engine 8.7
    ///
    /// @raditzlawliet
    /// </summary>
    [AddComponentMenu("Corgi Engine/Character/Abilities/Character Horizontal Movement with Advance Context Speed Multiplier")]
    public class CharacterHorizontalMovementAdvContextSpeedMultiplier : CharacterHorizontalMovement
    {
        [Header("Adv. Context Speed Multiplier")]
        public CharacterAdvContextSpeedMultiplier _characterAdvContextSpeedMultiplier = new CharacterAdvContextSpeedMultiplier();
        public float AdvContextSpeedMultiplier => _characterAdvContextSpeedMultiplier?.contextSpeedMultiplier ?? 1f;

        /// <summary>
        /// Called at Update(), handles horizontal movement
        /// </summary>
        protected override void HandleHorizontalMovement()
        {
            // if we're not walking anymore, we stop our walking sound
            if ((_movement.CurrentState != CharacterStates.MovementStates.Walking) && _startFeedbackIsPlaying)
            {
                StopStartFeedbacks();
            }

            // if movement is prevented, or if the character is dead/frozen/can't move, we exit and do nothing
            if (!ActiveAfterDeath)
            {
                if (!AbilityAuthorized
                    || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal)
                    || (_movement.CurrentState == CharacterStates.MovementStates.Gripping))
                {
                    return;
                }
            }

            // check if we just got grounded
            CheckJustGotGrounded();
            StoreLastTimeGrounded();

            bool canFlip = true;

            if (MovementForbidden)
            {
                _horizontalMovement = _character.Airborne ? _controller.Speed.x * Time.deltaTime : 0f;
                canFlip = false;
            }

            if (!_controller.State.IsGrounded && !AllowFlipInTheAir)
            {
                canFlip = false;
            }

            // If the value of the horizontal axis is positive, the character must face right.
            if (_horizontalMovement > InputThreshold)
            {
                _normalizedHorizontalSpeed = _horizontalMovement;
                if (!_character.IsFacingRight && canFlip && FlipCharacterToFaceDirection)
                {
                    _character.Flip();
                }
            }
            // If it's negative, then we're facing left
            else if (_horizontalMovement < -InputThreshold)
            {
                _normalizedHorizontalSpeed = _horizontalMovement;
                if (_character.IsFacingRight && canFlip && FlipCharacterToFaceDirection)
                {
                    _character.Flip();
                }
            }
            else
            {
                _normalizedHorizontalSpeed = 0;
            }

            /// if we're dashing, we stop there
            if (_movement.CurrentState == CharacterStates.MovementStates.Dashing)
            {
                return;
            }

            // if we're grounded and moving, and currently Idle, Dangling or Falling, we become Walking
            if ((_controller.State.IsGrounded)
                 && (_normalizedHorizontalSpeed != 0)
                 && ((_movement.CurrentState == CharacterStates.MovementStates.Idle)
                      || (_movement.CurrentState == CharacterStates.MovementStates.Dangling)
                      || (_movement.CurrentState == CharacterStates.MovementStates.Falling)))
            {
                _movement.ChangeState(CharacterStates.MovementStates.Walking);

                if (!DetectWalls(false))
                {
                    PlayAbilityStartFeedbacks();
                }
            }

            // if we're grounded, jumping but not moving up, we become idle
            if ((_controller.State.IsGrounded)
                && (_movement.CurrentState == CharacterStates.MovementStates.Jumping)
                && (_controller.TimeAirborne >= _character.AirborneMinimumTime))
            {
                _movement.ChangeState(CharacterStates.MovementStates.Idle);
            }

            // if we're walking and not moving anymore, we go back to the Idle state
            if ((_movement.CurrentState == CharacterStates.MovementStates.Walking)
                && (_normalizedHorizontalSpeed == 0))
            {
                _movement.ChangeState(CharacterStates.MovementStates.Idle);
                PlayAbilityStopFeedbacks();
            }

            // if the character is not grounded, but currently idle or walking, we change its state to Falling
            if (!_controller.State.IsGrounded
                && (
                    (_movement.CurrentState == CharacterStates.MovementStates.Walking)
                    || (_movement.CurrentState == CharacterStates.MovementStates.Idle)
                ))
            {
                _movement.ChangeState(CharacterStates.MovementStates.Falling);
            }

            // we apply instant acceleration if needed
            if (InstantAcceleration)
            {
                if (_normalizedHorizontalSpeed > 0f) { _normalizedHorizontalSpeed = 1f; }
                if (_normalizedHorizontalSpeed < 0f) { _normalizedHorizontalSpeed = -1f; }
            }

            // we pass the horizontal force that needs to be applied to the controller.
            float groundAcceleration = _controller.Parameters.SpeedAccelerationOnGround;
            float airAcceleration = _controller.Parameters.SpeedAccelerationInAir;

            if (_controller.Parameters.UseSeparateDecelerationOnGround && (Mathf.Abs(_horizontalMovement) < InputThreshold))
            {
                groundAcceleration = _controller.Parameters.SpeedDecelerationOnGround;
            }
            if (_controller.Parameters.UseSeparateDecelerationInAir && (Mathf.Abs(_horizontalMovement) < InputThreshold))
            {
                airAcceleration = _controller.Parameters.SpeedDecelerationInAir;
            }

            float movementFactor = _controller.State.IsGrounded ? groundAcceleration : airAcceleration;
            float movementSpeed = _normalizedHorizontalSpeed * MovementSpeed * _controller.Parameters.SpeedFactor * MovementSpeedMultiplier * ContextSpeedMultiplier * AdvContextSpeedMultiplier * AbilityMovementSpeedMultiplier * StateSpeedMultiplier * PushSpeedMultiplier;

            if (InstantAcceleration && (_movement.CurrentState != CharacterStates.MovementStates.WallJumping))
            {
                // if we are in instant acceleration mode, we just apply our movement speed
                _horizontalMovementForce = movementSpeed;

                // and any external forces that may be active right now
                if (Mathf.Abs(_controller.ExternalForce.x) > 0)
                {
                    _horizontalMovementForce += _controller.ExternalForce.x;
                }
            }
            else
            {
                // if we are not in instant acceleration mode, we lerp towards our movement speed
                _horizontalMovementForce = Mathf.Lerp(_controller.Speed.x, movementSpeed, Time.deltaTime * movementFactor);
            }

            // we handle friction
            _horizontalMovementForce = HandleFriction(_horizontalMovementForce);

            // we set our newly computed speed to the controller
            _controller.SetHorizontalForce(_horizontalMovementForce);

            if (_controller.State.IsGrounded)
            {
                _lastGroundedHorizontalMovement = _horizontalMovement;
            }
        }
    }
}
#endif