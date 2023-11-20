#if MOREMOUNTAINS_CORGIENGINE
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Every frame, we check if the player is colliding with a level bound
    /// overrides of CharacterLevelBounds With MMFPlayer on collide
    /// Required: Corgi Engine 8.7
    ///
    /// @raditzlawliet
    /// </summary>
    public class CharacterLevelBoundsWithMMFPlayer : CharacterLevelBounds
    {
        public UnityEngine.Events.UnityEvent OnTop, OnBottom, OnLeft, OnRight;

        public override void LateUpdate()
        {
            _controller.State.TouchingLevelBounds = false;
            // if the player is dead, we do nothing
            if ((_character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)
                 || (!LevelManager.HasInstance))
            {
                return;
            }
            Physics2D.SyncTransforms();
            _bounds = LevelManager.Instance.LevelBounds;

            if (_bounds.size != Vector3.zero)
            {
                // when the player reaches a bound, we apply the specified bound behavior
                if ((Top != BoundsBehavior.Nothing) && (_controller.ColliderTopPosition.y > _bounds.max.y))
                {
                    _constrainedPosition.x = transform.position.x;
                    _constrainedPosition.y = _bounds.max.y - _controller.ColliderSize.y / 2 - _controller.ColliderOffset.y;
                    if (ResetForcesOnConstrain) { _controller.SetVerticalForce(0f); }
                    OnTop?.Invoke();
                    ApplyBoundsBehavior(Top, _constrainedPosition);
                }

                if ((Bottom != BoundsBehavior.Nothing) && (_controller.ColliderBottomPosition.y < _bounds.min.y))
                {
                    _constrainedPosition.x = transform.position.x;
                    _constrainedPosition.y = _bounds.min.y + _controller.ColliderSize.y / 2 + _controller.ColliderOffset.y;
                    if (ResetForcesOnConstrain) { _controller.SetVerticalForce(0f); }
                    OnBottom?.Invoke();
                    ApplyBoundsBehavior(Bottom, _constrainedPosition);
                }

                if ((Right != BoundsBehavior.Nothing) && (_controller.ColliderRightPosition.x > _bounds.max.x))
                {
                    _constrainedPosition.x = _bounds.max.x - _controller.ColliderSize.x / 2 - _controller.ColliderOffset.x;
                    _constrainedPosition.y = transform.position.y;
                    if (ResetForcesOnConstrain) { _controller.SetHorizontalForce(0f); }
                    OnRight?.Invoke();
                    ApplyBoundsBehavior(Right, _constrainedPosition);
                }

                if ((Left != BoundsBehavior.Nothing) && (_controller.ColliderLeftPosition.x < _bounds.min.x))
                {
                    _constrainedPosition.x = _bounds.min.x + _controller.ColliderSize.x / 2 + _controller.ColliderOffset.x;
                    _constrainedPosition.y = transform.position.y;
                    if (ResetForcesOnConstrain) { _controller.SetHorizontalForce(0f); }
                    OnLeft?.Invoke();
                    ApplyBoundsBehavior(Left, _constrainedPosition);
                }
            }

            // if we're in auto scroll and we get crushed, we kill the player
            if ((_oneWayLevelManager != null) && _oneWayLevelManager.OneWayLevelAutoScrolling)
            {
                bool colliding = false;
                switch (_oneWayLevelManager.OneWayLevelDirection)
                {
                    case OneWayLevelManager.OneWayLevelDirections.Right:
                        colliding = _controller.State.IsCollidingRight;
                        break;
                    case OneWayLevelManager.OneWayLevelDirections.Left:
                        colliding = _controller.State.IsCollidingLeft;
                        break;
                    case OneWayLevelManager.OneWayLevelDirections.Up:
                        colliding = _controller.State.IsCollidingAbove;
                        break;
                    case OneWayLevelManager.OneWayLevelDirections.Down:
                        colliding = _controller.State.IsCollidingBelow;
                        break;
                }
                if (colliding && _controller.State.TouchingLevelBounds)
                {
                    _character.CharacterHealth.Kill();
                    _oneWayLevelManager.SetOneWayLevelAutoScrolling(false);
                }
            }
        }
    }
}
#endif