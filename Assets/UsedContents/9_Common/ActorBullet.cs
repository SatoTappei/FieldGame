using UnityEngine;
using UniRx;
using UniRx.Triggers;
using TargetTag = DamageData.TargetTag;

/// <summary>
/// ActorBulletPoolにプーリングされているキャラクターの弾のクラス
/// 描画はECS側が行うのでコライダーのみでRendererが無い
/// </summary>
public class ActorBullet : MonoBehaviour
{
    [SerializeField] float _lifeTime = 1.5f;
    [SerializeField] float _speed = 5.0f;
    [SerializeField] float _radius = 3.0f;
    [SerializeField] TargetTag _targetTag;

    ActorBulletPool _bulletPool;
    Transform _transform;
    Vector3 _dir;
    float _timer;

    void Awake()
    {
        this.OnTriggerEnterAsObservable().Subscribe(_ =>
        {
            MessageBroker.Default.Publish(new DamageData(transform.position, _radius, _targetTag));
        });

        _transform = transform;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _lifeTime)
        {
            _bulletPool.Return(this);
            OnReturn();
        }
        else
        {
            _transform.Translate(_dir * _speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// ActorBulletPoolで生成時に呼ばれる
    /// </summary>
    public void InitOnCreate(ActorBulletPool pool) => _bulletPool = pool;

    /// <summary>
    /// プールから取り出した際に飛んでいく方向をキャラクターの前方向に設定する
    /// </summary>
    public void OnRent(Transform model, Vector3 muzzlePos)
    {
        _transform.position = muzzlePos;
        _dir = model.forward;
    }

    void OnReturn()
    {
        _timer = 0;
    }
}
