using UnityEngine;

/// <summary>
/// �o�H�T��������L�����N�^�[�̃e�X�g
/// </summary>
public class ExamplePathfindingActor : MonoBehaviour
{
    void Start()
    {
    }

    public void Execute()
    {
        // TODO:���ۂ̓V���O���g���Ƃ��ɂ��Ă���
        PathfindingSystem system = FindObjectOfType<PathfindingSystem>();
        system.GetPath(transform.position);
    }
}
