using UnityEngine;

/// <summary>
/// �r�w�C�r�A�c���[�̎g�p��
/// �ȉ���On~�n���\�b�h���C���X�y�N�^�[����MonkeyModelCallbackRegister�ɓo�^���Ă���
/// ���̃X�N���v�g���̂̓r�w�C�r�A�c���[�̓���ɕK�v�Ȃ�
/// </summary>
public class SampleMonkeyModelController : MonoBehaviour
{
    public void OnAttack()
    {
        Debug.Log("�U������");
    }

    public void OnMoveEnter()
    {
        Debug.Log("�ړ��J�n");
    }

    public void OnMoveExit()
    {
        Debug.Log("�ړ��I��");
    }

    public void OnDefeated()
    {
        Debug.Log("����");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �r�w�C�r�A�c���[�̓�����~�߂�ɂ�Complete()���ĂׂΗǂ�
            // �������A��xComplete()���ĂԂƁA�ēx���삳���邱�Ƃ͏o���Ȃ��̂Œ���
            GetComponent<MonkeyModelTree>().Complete();
        }
    }
}
