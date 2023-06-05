using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;          // ���̸�
    public float range;             // �����Ÿ�
    public float accuracy;          // ��Ȯ��
    public float fireRate;          // ����ӵ�
    public float reloadTime;        // ���弱 �ӵ�

    public int damage;              // �� ������

    public int reloadBulletCount;   // �Ѿ� ���弱 ����
    public int currentBulletCount;  // ���� źâ�� �����ִ� �Ѿ˰���
    public int maxBulletCount;      // �ִ� ���� ���� �Ѿ� ����
    public int carryBulletCount;    // ���� �������� �Ѿ� ����

    public float retroActionForce;  // �ݵ� ����    

    public Vector3 fineSightOriginPos;
    public Animator anim;
    public ParticleSystem muzzleFlash1;
    public ParticleSystem muzzleFlash2;

    public AudioClip fire_Sound;
}
