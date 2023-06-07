using UnityEngine;

/// <summary>
/// �U�����s���A�N�V�����m�[�h
/// </summary>
public class FireAction : BehaviorTreeNode
{
    public FireAction(string nodeName, BehaviorTreeBlackBoard blackBoard) 
        : base(nodeName, blackBoard) { }

    protected override void OnEnter()
    {
        //var v = GameObject.FindGameObjectWithTag()
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        BlackBoard.FireParticle.Play();
        TriggerByMonoBroker.Instance.AddShootData(ShootData.BulletType.Enemy, 
            BlackBoard.Transform.position, BlackBoard.Model.forward);
        
        // �R���C�_�[�𔭎˂��鏈�����K�v
        // Transform��Model�ւ̎Q�Ƃ͍��ɂ���
        // Pool����e�����o���Ĕ��˂�����
        // Pool�ւ̎Q�Ƃǂ����邩���
        // ���̃m�[�h�͎g���܂킵�����A�R�[���o�b�N��n���Ď��s����悤�ɂ���΋ߐڍU���ɂ��Ή��ł���H
        // ��1: ���̃N���X�Ƀv�[����static�Ŏ�������

        // ���D��:�U�����ǂ����邩�H���̃Q�[���ɂ͉������U�������������͊m�肵�Ă���B�ߐڍU���͍l�����Ȃ��ŗǂ�

        return State.Success;
    }
}
