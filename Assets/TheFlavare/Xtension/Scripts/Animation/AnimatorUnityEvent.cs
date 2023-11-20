using UnityEngine;

namespace TheFlavare.Xtension
{
    // Animator helper plug and play component, can be use for UnityEvent to call Animator with string
    // e.g., SetBool(param, value) was sing two param, will be compact into single SetBool("param,value")
    // 
    // @raditzlawliet
    public class AnimatorUnityEvent : MonoBehaviour
    {
        public void SetInteger(string text)
        {
            string[] compiled = text.Split(',');
            if (compiled.Length >= 2)
            {
                GetComponent<Animator>().SetInteger(compiled[0], int.Parse(compiled[1]));
            }
        }

        public void SetBool(string text)
        {
            string[] compiled = text.Split(',');
            if (compiled.Length >= 2)
            {
                GetComponent<Animator>().SetBool(compiled[0], bool.Parse(compiled[1]));
            }
        }

        public void SetFloat(string text)
        {
            string[] compiled = text.Split(',');
            if (compiled.Length >= 2)
            {
                GetComponent<Animator>().SetFloat(compiled[0], float.Parse(compiled[1]));
            }
        }
    }
}