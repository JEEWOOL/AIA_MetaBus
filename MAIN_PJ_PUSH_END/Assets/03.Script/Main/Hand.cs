using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public string handName;                 // ��
    public float range;                     // ���� ����
    public int damage;                      // ���ݷ�
    public float workSpeed;                 // �۾� �ӵ�
    public float attackDelay;               // ���� ������
    public float attackDelayA;              // ���� Ȱ��ȭ ����
    public float attackDelayB;              // ���� ��Ȱ��ȭ ����

    public Animator anim;
}