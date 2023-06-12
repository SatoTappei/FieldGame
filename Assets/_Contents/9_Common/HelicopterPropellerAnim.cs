using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// ヘリコプターのプロペラのアニメーション
/// </summary>
public class HelicopterPropellerAnim : MonoBehaviour
{
    static readonly float Speed = 1200;

    [SerializeField] Transform _mainProp;
    [SerializeField] Transform _smallProp;

    void Awake()
    {
        this.UpdateAsObservable().Subscribe(_ => 
        {
            _mainProp.Rotate(new Vector3(0, Speed * Time.deltaTime, 0));
            _smallProp.Rotate(new Vector3(Speed * Time.deltaTime, 0, 0));
        });
    }
}
