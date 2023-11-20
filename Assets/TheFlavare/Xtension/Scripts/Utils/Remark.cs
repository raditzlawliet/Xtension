using UnityEngine;
#if ODIN_INSPECTOR || ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
#endif

// Remark on inspector
// 
// @raditzlawliet
#if ODIN_INSPECTOR || ODIN_INSPECTOR_3
[HideMonoScript]
#endif
public class Remark : MonoBehaviour
{
#if ODIN_INSPECTOR || ODIN_INSPECTOR_3
    [HideLabel]
#endif
    [TextArea]
    public string remark;
}