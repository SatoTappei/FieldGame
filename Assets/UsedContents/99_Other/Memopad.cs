using UnityEngine;

#if UNITY_EDITOR
public class Memopad : MonoBehaviour
{
    [TextArea(40, 50)]
    [SerializeField] string _memo;
}
#endif