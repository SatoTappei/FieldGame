using System;

/// <summary>
/// BinaryHeap<T>�N���X�Œl���Ǘ����邽�߂ɕK�v�ȃC���^�[�t�F�[�X
/// </summary>
public interface IBinaryHeapCollectable<T> : IComparable<T>
{
    int BinaryHeapIndex { get; set; }
}

/// <summary>
/// �񕪃q�[�v����������N���X
/// A*�Ōo�H�T��������ۂɍŏ��R�X�g�̃m�[�h��Ԃ����߂Ɏg�p
/// </summary>
public class BinaryHeap<T> where T : IBinaryHeapCollectable<T>
{
    T[] _values;
    public int Count { get; private set; }

    public BinaryHeap(int maxHeapSize)
    {
        _values = new T[maxHeapSize];
    }

    /// <summary>
    /// �ǉ����ɓK�؂ȏꏊ�Ɉړ�������̂ŁA�l�����̏ꏊ�ɑ��݂���΃q�[�v���ɑ��݂���
    /// </summary>
    public bool Contains(T value) => Equals(_values[value.BinaryHeapIndex], value);

    /// <summary>
    /// ��Ԍ��ɒǉ����ēK�؂ȏꏊ�܂ňړ�������
    /// </summary>
    public void Add(T value)
    {
        value.BinaryHeapIndex = Count;
        _values[Count] = value;
        SortUp(value);
        Count++;
    }

    /// <summary>
    /// �擪��Ԃ��āA��Ԍ��̒l��擪�Ɏ����Ă��ēK�؂ȏꏊ�܂ňړ�������
    /// </summary>
    public T Pop()
    {
        T value = _values[0];
        Count--;
        _values[0] = _values[Count];
        _values[0].BinaryHeapIndex = 0;
        SortDown(_values[0]);
        return value;
    }

    /// <summary>
    /// �����̒l��Add()�����ۂɖ؂̉������Ɍ����ă\�[�g���Ă���
    /// </summary>
    void SortUp(T value)
    {
        int parentIndex = (value.BinaryHeapIndex - 1) / 2;

        while (true)
        {
            // ��r���Đe�ƌ�������
            T parent = _values[parentIndex];
            if (value.CompareTo(parent) > 0)
            {
                Swap(value, parent);
            }
            else
            {
                break;
            }

            // Swap()�œY�������������Ă���̂ŁA������̈ʒu�̐e�̓Y�����ɂȂ�
            parentIndex = (value.BinaryHeapIndex - 1) / 2;
        }
    }

    /// <summary>
    /// �����̒l��Pop()�����ۂɖ؂̏ォ�牺�Ɍ����ă\�[�g���Ă���
    /// </summary>
    void SortDown(T value)
    {
        while (true)
        {
            int leftChildIndex = value.BinaryHeapIndex * 2 + 1;
            int rightChildIndex = value.BinaryHeapIndex * 2 + 2;

            // ���̎q�m�[�h���؂̒��ɂ���ꍇ�̓\�[�g�̏������s��
            if (leftChildIndex < Count)
            {

                int swapIndex = leftChildIndex;
                if (rightChildIndex < Count)
                {
                    // �E�̎q�����̎q���傫���ꍇ�͂�����ƌ�������
                    if (_values[leftChildIndex].CompareTo(_values[rightChildIndex]) < 0)
                    {
                        swapIndex = rightChildIndex;
                    }
                }

                // ���E�ǂ��炩�̎q�ƌ���
                if (value.CompareTo(_values[swapIndex]) < 0)
                {
                    Swap(value, _values[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    /// <summary>
    /// ���݂̓Y�����̌��̒l������������A�Y��������������
    /// </summary>
    void Swap(T value1, T value2)
    {
        _values[value1.BinaryHeapIndex] = value2;
        _values[value2.BinaryHeapIndex] = value1;
        int itemIndex = value1.BinaryHeapIndex;
        value1.BinaryHeapIndex = value2.BinaryHeapIndex;
        value2.BinaryHeapIndex = itemIndex;
    }
}