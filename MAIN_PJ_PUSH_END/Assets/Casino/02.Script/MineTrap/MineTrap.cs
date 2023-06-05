//using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;

public class MineTrap : MonoBehaviour
{
    public GameObject trapPrefab; // ���� ť�� ������
    public Material trapMaterial; // ���� ť�� ��Ƽ����
    public Material safeMaterial; // ���� ��Ƽ����
    public Material normalMaterial; // �ʱ�ȭ ��Ƽ����

    private Transform[] cubeTransforms; // 16���� ť�� �迭 
    private Transform trapTransform; // ���� ť���� Transform
    private Transform safeTransform; // Ŭ���� ������ ť���� Transform
    private bool isGameActive = true; // ���� ����

    private GameManager gameManager; // GameManager ����
    public Camera maincam;
    private int safecount = 0; // ����ť�� Ŭ��Ƚ��

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>(); // ���ӸŴ��� ����
        // 16���� ť�� �迭�� �ʱ�ȭ
        cubeTransforms = new Transform[16];

        // �ڽ� ť����� Transform�� �迭�� ����
        for (int i = 0; i < transform.childCount; i++)
        {
            cubeTransforms[i] = transform.GetChild(i);
            
        }

        // �������� �ϳ��� ť�긦 ���� ť��� ����
        int trapIndex = Random.Range(0, 16);
        trapTransform = cubeTransforms[trapIndex];
    }

    private void Update()
    {
        if (isGameActive && Input.GetMouseButtonDown(0))
        {
            int layerMask = 1 << LayerMask.NameToLayer("Player");    // LayerMask
            layerMask = ~layerMask;    // �÷��̾� ����ĳ��Ʈ �����ϱ����ؼ�

            RaycastHit hit;

            // Mathf.Infinity - Ray�Ÿ��� ���������� �ξ��⶧���� �Ŀ� ����
            bool ray = Physics.Raycast(maincam.transform.position, maincam.transform.forward, out hit, Mathf.Infinity, layerMask);
                        
            if (ray)
            {
                GameObject clickedObject = hit.collider.gameObject;
                OnCubeClick(clickedObject);
            }
        }
    }

    public void OnCubeClick(GameObject cube)
    {
        maincam = gameManager.maincam.GetComponent<Camera>(); // ���ӸŴ����� �Ҵ�� maincam��������

        if (System.Array.IndexOf(cubeTransforms, cube.transform) != -1)
        {
            if (cube.transform == trapTransform)
            {
                // ���� ť�긦 Ŭ���� ���
                cube.GetComponent<Renderer>().material = trapMaterial; // ��Ƽ���� ����
                StartCoroutine(ResetGameAfterDelay(5f));
                Debug.Log($"�ִ��� -{safecount}");
            }
            else
            {
                // �Ϲ� ť�긦 Ŭ���� ���
                cube.GetComponent<Renderer>().material = safeMaterial; // ��Ƽ���� ����                
                
                // Ŭ����ť��(������ť��)�� �ƴҰ�� ī��Ʈ����
                if (cube.transform != safeTransform)
                {
                    safecount++;
                }
                // Ŭ���� ť��(������ ť��)�� �ٲ�
                safeTransform = cube.transform;
                Debug.Log($"safecount -{safecount}");                
            }
        }
    }

    private IEnumerator ResetGameAfterDelay(float delay)
    {
        isGameActive = false; // ���� ���¸� ��Ȱ��ȭ

        yield return new WaitForSeconds(delay);

        // ���� �����
        // ���� ť�긦 Ŭ������ ��
        for (int i = 0; i < cubeTransforms.Length; i++)
        {
            cubeTransforms[i].GetComponent<Renderer>().material = normalMaterial;

        }
        // ���� ���¸� �ٽ� Ȱ��ȭ
        // �������� �ϳ��� ť�긦 ���� ť��� ����
        int trapIndex = Random.Range(0, 16);
        trapTransform = cubeTransforms[trapIndex];
        safecount = 0; // Ƚ�� ��� �ʱ�ȭ
        isGameActive = true;
    }
}
