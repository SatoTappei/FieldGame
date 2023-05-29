using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// “G‚Ì’e‚ÌƒNƒ‰ƒX
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    void Start()
    {
        this.OnTriggerEnterAsObservable().Subscribe(_ =>
        {
            MessageBroker.Default.Publish(new DamageData(transform.position, 3.0f, DamageData.TargetTag.Player));
        });
    }

    void Update()
    {
        
    }
}
