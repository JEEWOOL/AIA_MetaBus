using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FlowerSpawn : MonoBehaviour
{
    public GameObject flowerPrefab; // ������ ���ӿ�����Ʈ ������

    private Transform[] cubeTransforms; // 16�� ť���� Transform �迭


    private void Start()
    {
        // �ڽ� ������Ʈ���� Transform�� �迭�� ����
        cubeTransforms = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            cubeTransforms[i] = transform.GetChild(i);
        }
    }

    public void RandomObjects()
    {
        int flowerCount = 0;
        // �� ť�꿡 ���� �ݺ�
        foreach (Transform cubeTransform in cubeTransforms)
        {
            // �̹� ������ �������� �ִ��� Ȯ��
            if (cubeTransform.childCount == 0)
            {
                // ������ Ȯ���� ���� ���� ����
                float randomValue = Random.value;

                if (randomValue <= 0.5f)
                {
                    // 1���� 5���� ť�꿡�� �ϳ��� ������ ����
                    if (cubeTransform.GetSiblingIndex() < 5)
                    {
                        CreatePrefab(cubeTransform);

                    }
                }
                else if (randomValue <= 0.75f)
                {
                    // 6���� 10���� ť�꿡�� �ϳ��� ������ ����
                    if (cubeTransform.GetSiblingIndex() < 10)
                    {
                        CreatePrefab(cubeTransform);
                    }
                }
                else if (randomValue <= 0.9f)
                {
                    // 11���� 14���� ť�꿡�� �ϳ��� ������ ����
                    if (cubeTransform.GetSiblingIndex() < 14)
                    {
                        CreatePrefab(cubeTransform);
                    }
                }
                else
                {
                    // 15���� 16���� ť�꿡�� �ϳ��� ������ ����
                    if (cubeTransform.GetSiblingIndex() >= 14)
                    {
                        CreatePrefab(cubeTransform);
                    }
                }
            }
            flowerCount += cubeTransform.childCount;
        }

        // ������ �� ����
        Debug.Log($"flower count : {flowerCount}");

        // ���� ������ ���� �ݿ�(���� �ʿ�)
        if (flowerCount < 8) // ���� �̸��� ��
            PlayListManager.gold -= flowerCount;
        else if (flowerCount > 8)
            PlayListManager.gold += (flowerCount - 8);
        Debug.Log($"credits = {PlayListManager.gold}");
    }

    private void CreatePrefab(Transform cubeTransform)
    {
        // ť�� ���� �ϳ��� ������ �������� ����
        GameObject newObject = Instantiate(flowerPrefab, cubeTransform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);

        // ������ ���� ������Ʈ�� FlowerSpawn�� �ڽ����� ����
        newObject.transform.SetParent(cubeTransform);

        Destroy(newObject, 5f);
    }
}