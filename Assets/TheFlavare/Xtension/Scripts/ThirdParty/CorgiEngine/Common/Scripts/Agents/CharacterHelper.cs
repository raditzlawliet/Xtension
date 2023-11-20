#if MOREMOUNTAINS_CORGIENGINE
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Character Helper for Unity Event
    /// Required: Corgi Engine 8.7
    /// 
    /// @raditzlawliet
    /// </summary>
    public class CharacterHelper : MonoBehaviour
    {
        public Character.CharacterTypes CharacterType = Character.CharacterTypes.Player;
        public string PlayerID = "Player1";

        protected Character _character;

        protected void findCharacter()
        {
            if (_character != null)
            {
                if (_character.gameObject.activeSelf)
                    return;
            }

            foreach (var c in FindObjectsOfType<Character>())
            {
                if (c.CharacterType == CharacterType && c.PlayerID == PlayerID)
                {
                    _character = c;
                    break;
                }
            }
        }

        public void PauseAutoMovement()
        {
            if (!isCharacterValid())
                return;

            _character.FindAbility<CharacterAutoMovement>().PauseMovement();
        }

        public void ResumeAutoMovement()
        {
            if (!isCharacterValid())
                return;

            _character.FindAbility<CharacterAutoMovement>().ResumeMovement();
        }

        public void Kill()
        {
            if (!isCharacterValid())
                return;

            _character.GetComponent<Health>()?.Kill();
        }

        public void Freeze(bool freeze)
        {
            if (!isCharacterValid())
                return;

            if (freeze)
                _character.Freeze();
            else
                _character.UnFreeze();
        }
        public bool isCharacterValid()
        {
            findCharacter();
            return _character != null;
        }
    }
}
#endif