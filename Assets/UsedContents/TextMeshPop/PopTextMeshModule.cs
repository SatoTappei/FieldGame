using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���\�b�h���Ăяo�������ŔC�ӂ̕�����TextMesh�łӂ킟����
/// </summary>
public class PopTextMeshModule : MonoBehaviour
{
    static readonly int PoolQuantity = 10;

    [Header("�ݒ�ς݂�TextMesh��Prefab")]
    [SerializeField] PooledTextMesh _prefab;

    Stack<PooledTextMesh> _pool = new(PoolQuantity);

    public void Awake()
    {
        CreatePool();
    }

    /// <summary>
    /// �v�[���p�̃I�u�W�F�N�g�𐶐����A���̎q�Ƃ���TextMesh�𐶐�����
    /// </summary>
    void CreatePool()
    {
        GameObject parent = new GameObject("TextMeshPool");
        for (int i = 0; i < PoolQuantity; i++)
        {
            PooledTextMesh instance = Instantiate(_prefab, Vector3.zero, Quaternion.identity);

            // �ŏ���1�̓v�[����o�^����
            if (i == 0) instance.RegisterPool(this);

            Push(instance);
            instance.transform.SetParent(parent.transform);
        }
    }

    /// <summary>
    /// �O�����炱�̃��\�b�h���ĂԂ����ő��v
    /// �\�����镶����ƍ��W��ݒ肵�Ď��o��
    /// </summary>
    public void Pop(string line, Vector3 worldPos)
    {
        if (_pool.TryPop(out PooledTextMesh instance))
        {
            instance.OnPoped(line, worldPos);
            instance.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"TextMesh������Ȃ�: {line}");
        }
    }

    public void PopDefaultPos(string line) => Pop(line, new Vector3(0, 2, 3));

    public void Push(PooledTextMesh instance)
    {
        instance.gameObject.SetActive(false);
        _pool.Push(instance);
    }
}
