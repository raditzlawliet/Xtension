using UnityEngine;

namespace TheFlavare.Xtension
{
    // Animator helper plug and play component, can be use for setter for UnityEvent to change animator param like transition progress, blended or else. 
    // e.g., some component have callback transition function(int progress) can pass with this component
    // 
    // @raditzlawliet
    public class AnimatorUnityEventSingleParam : MonoBehaviour
    {
        public Animator animator;
        public string paramName;
        public void Awake()
        {
            if (animator == null)
                animator = GetComponent<Animator>();
        }
        public void SetInteger(int v)
        {
            if (animator != null)
                animator.SetInteger(paramName, v);
        }

        public void SetFloat(float v)
        {
            if (animator != null)
                animator.SetFloat(paramName, v);
        }

        public void SetBool(bool v)
        {
            if (animator != null)
                animator.SetBool(paramName, v);
        }
    }
}