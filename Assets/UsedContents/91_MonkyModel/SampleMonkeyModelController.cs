using UnityEngine;

/// <summary>
/// ビヘイビアツリーの使用例
/// 以下のOn~系メソッドをインスペクターからMonkeyModelCallbackRegisterに登録している
/// このスクリプト自体はビヘイビアツリーの動作に必要ない
/// </summary>
public class SampleMonkeyModelController : MonoBehaviour
{
    public void OnAttack()
    {
        Debug.Log("攻撃した");
    }

    public void OnMoveEnter()
    {
        Debug.Log("移動開始");
    }

    public void OnMoveExit()
    {
        Debug.Log("移動終了");
    }

    public void OnDefeated()
    {
        Debug.Log("死んだ");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ビヘイビアツリーの動作を止めるにはComplete()を呼べば良い
            // ただし、一度Complete()を呼ぶと、再度動作させることは出来ないので注意
            GetComponent<MonkeyModelTree>().Complete();
        }
    }
}
