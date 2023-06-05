using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public int hp;                  // ����ü��
    public float destoryTime;       // �������� �Ҹ�ð�

    public SphereCollider col;      // ��ü �ݶ��̴�
    public GameObject go_rock;      // �Ϲݹ���
    public GameObject go_debris;    // ��������
    public GameObject go_effect;    // ä�� ����Ʈ
    public GameObject go_rock_item; // ������ ������

    public int count;              // ������ ����Ƚ��

    public AudioSource audioSource;
    public AudioClip effect_Sound1;
    public AudioClip effect_Sound2;

    public void Mining()
    {
        audioSource.clip = effect_Sound1;
        audioSource.Play();
        var clone = Instantiate(go_effect, col.bounds.center, Quaternion.identity);
        Destroy(clone, destoryTime);

        hp--;
        if(hp <= 0)
        {
            Destruction();
        }
    }
    void Destruction()
    {
        audioSource.clip = effect_Sound2;
        audioSource.Play();

        col.enabled = false;

        for(int i = 0; i <= count; i++)
        {
            Instantiate(go_rock_item, go_rock.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }        

        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destoryTime);
    }
}
