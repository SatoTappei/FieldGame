using System;
using System.Buffers;
using UnityEngine;

/// <summary>
/// .NetStandard2.1�̐V�@�\������
/// </summary>
public class DotNetStandard2Dot1 : MonoBehaviour, IExampleInterface
{
    void Start()
    {
        ArrayPoolExample();
    }

    /// <summary>
    /// �ꎞ�I�ȃo�b�t�@���p�ɂɍ쐬�E�j������ꍇ�Ɏg��
    /// �������A���P�[�V������GC�R�X�g�̍팸�����҂ł���B
    /// </summary>
    void ArrayPoolExample()
    {
        //using System;
        //using System.Buffers;

        // �Œ�512�̒���"�ȏ�"�̔z����v�[������؂��
        int[] buffer = ArrayPool<int>.Shared.Rent(512);
        Debug.Log(buffer.Length); // 512

        // �K�v�ȃT�C�Y(Start����Length�̒���)��Span�Ŏ擾����1�Ŗ��߂�
        Span<int> span = buffer.AsSpan(0, 256);
        span.Fill(1);

        // 1��256��\�������
        foreach (int i in span)
        {
            Debug.Log(i);
        }

        // Return()����ۂɏ���������Ȃ��̂ŎQ�ƌ^��Clear()���邱��
        // ���������[�N�̌����ɂȂ�
        span.Clear();

        // �����������̂�0��256��\�������
        foreach (int i in span)
        {
            Debug.Log(i);
        }

        // �؂肽��Ԃ�
        ArrayPool<int>.Shared.Return(buffer);
    }

    /// <summary>
    /// �z��͈͍̔\��
    /// ..�Ōq���Ƃ��͈̗̔͂v�f���A^��t����ƌ�납��w��ł���
    /// </summary>
    void RangeStructExample()
    {
        int[] array1 = new int[5] { 1, 2, 3, 4, 5 };
        int[] array2 = array1[0..2];
        int[] array3 = array1[1..^0];
    }
}

/// <summary>
/// Interface��Default�̎������������邱�Ƃ��o����悤�ɂȂ���
/// �g�������悭�킩��Ȃ��A�������Ȃ���ԂŃ��\�b�h���Ăяo�����Ƃ͏o���Ȃ��炵���B
/// </summary>
public interface IExampleInterface
{
    public void ExampleMethod() => Debug.Log("Interface��Default�̎������������邱�Ƃ��o����");
}