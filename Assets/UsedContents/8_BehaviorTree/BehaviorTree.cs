using System.Collections.Generic;
using UnityEngine;
using State = BehaviorTreeNode.State;

/// <summary>
/// BehaviorTree�{�̂̃N���X
/// </summary>
public class BehaviorTree : MonoBehaviour
{
    // �Q�l����Ō��݂̃A�N�V�����̏�Ԃ��Ǘ����邽�߂̗񋓌^���`���Ă���
    public enum ActionState 
    {
        Idle,
        Working,
    };
    ActionState _state = ActionState.Idle;

    // TODO:���̒n�_�Ԃ��s��������悤�ɂ�����
    [SerializeField] Transform _startPoint;
    [SerializeField] Transform _item;

    public RootNode _rootNode = new();
    public State _treeState = State.Runnning;
    public List<BehaviorTreeNode> _nodeList = new();

    void Start()
    {
        // �A�C�e���Ɍ������Ĉړ�������A�X�^�[�g�n�_�܂Ŗ߂�Sequence
        SequenceNode sequence = new ();
        LinerMoveToPosAction moveToItem = new(transform, _item.position, 10.0f);
        LinerMoveToPosAction moveToStartPoint = new(transform, _startPoint.position, 3.0f);
        sequence.AddChild(moveToItem);
        sequence.AddChild(moveToStartPoint);

        // ���[�g�m�[�h�̎q��Sequence���Ԃ牺����
        _rootNode._child = sequence;
    }

    // �Q�l����ł̕��j
    // �؂𗘗p������̂ё��Ƀ��\�b�h��p�ӂ��ăf���Q�[�g�ŏ��n���Ă���B
    // �������邱�ƂŊe�m�[�h���K�v�ɂȂ�Q�ƂȂǂ̏�񂪌��点��
    State GoToLocation(Vector3 destination)
    {
        // ���ݒn�_����^�[�Q�b�g�܂ł̋���
        float distance = Vector3.Distance(destination, transform.position);
        
        if (_state == ActionState.Idle)
        {
            // ���̒n�_��
            _state = ActionState.Working;
        }
        // else if(�^�[�Q�b�g�܂ł���������ꍇ)
        // state = ActionState.Idle;
        // return State.Failure;
        
        // else if(�^�[�Q�b�g�܂ł̋��������ȉ��Ȃ�)
        // state = ActionState.Idle;
        // return State.Success;

        return State.Runnning;
    }

    // �Q�l����ł͂����������Ă��邩�ǂ����̊֐�
    // bool�^�ł͂Ȃ�State�^�ŕԂ�
    public State HasMoney()
    {
        return State.Success;
    }

    void Update()
    {
        _rootNode.Update();
    }
}
