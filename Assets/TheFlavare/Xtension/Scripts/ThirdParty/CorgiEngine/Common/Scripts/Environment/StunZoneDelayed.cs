#if MOREMOUNTAINS_CORGIENGINE
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
	/// A stun zone will stun any character with a CharacterStun ability entering it
    /// plus a additional delay after entering (delayed stun)
    /// Required: Corgi Engine 8.7
    ///
    /// @raditzlawliet
    /// </summary>
    public class StunZoneDelayed : StunZone
    {
        [Header("Delayed")]
        public float DelayTimeInSeconds = 0f;

        [Header("Other")]
        public bool ResetForceWhenStunned = false;
        protected CorgiController _colliderCorgiController;

        /// <summary>
        /// When colliding with a gameobject, we make sure it's a target, and if yes, we stun it
        /// </summary>
        /// <param name="collider"></param>
        protected override void Colliding(GameObject collider)
        {
            if (!MMLayers.LayerInLayerMask(collider.layer, TargetLayerMask))
            {
                return;
            }

            _character = collider.GetComponent<Character>();
            if (_character != null) { _characterStun = _character.FindAbility<CharacterStun>(); }

            if (_characterStun == null)
            {
                return;
            }

            _colliderCorgiController = collider.gameObject.MMGetComponentNoAlloc<CorgiController>();

            if (DelayTimeInSeconds > 0f)
            {
                StartCoroutine(DelayStun(DelayTimeInSeconds));
            }
            else
            {
                _colliderCorgiController.SetForce(Vector2.zero);

                if (StunMode == StunModes.ForDuration)
                {
                    _characterStun.StunFor(StunDuration);
                }
                else
                {
                    _characterStun.Stun();
                }
            }

            if (DisableZoneOnStun)
            {
                this.gameObject.SetActive(false);
            }
        }

        IEnumerator DelayStun(float delay)
        {
            yield return new WaitForSeconds(delay);

            _colliderCorgiController.SetForce(Vector2.zero);

            if (StunMode == StunModes.ForDuration)
            {
                _characterStun.StunFor(StunDuration);
            }
            else
            {
                _characterStun.Stun();
            }
        }
    }
}
#endif