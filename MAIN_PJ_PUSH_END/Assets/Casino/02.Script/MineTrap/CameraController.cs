using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target; // ī�޶� ����ٴ� ���
    public float rotationSpeed = 10f; // ȸ�� �ӵ�
    public float distance = 7f; // ������ �Ÿ�

    private bool isRotating = false; // ȸ�� ������ ���θ� ��Ÿ���� �÷���


    private void Awake()
    {
        // target ������Ʈ �ʱ�ȭ
        target = GameObject.FindWithTag("MineTrap");
    }

    private void Update()
    {
        if (isRotating)
        {
            // ī�޶� ȸ��
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            transform.RotateAround(target.transform.position, Vector3.up, mouseX);
            transform.RotateAround(target.transform.position, -transform.right, mouseY);
        }
    }

    public void RotateCamera()
    {
        /*
        // ī�޶� ��ġ�� ȸ�� �ʱ�ȭ
        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;*/

        // ȸ�� ������ ���·� ����
        isRotating = true;
    }

    public void StopRotateCamera()
    {
        // ȸ�� ����
        isRotating = false;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // ��� ������Ʈ �տ� ī�޶� ��ġ ����
            Vector3 desiredPosition = target.transform.position - target.transform.forward * distance;
            transform.position = desiredPosition;
            transform.LookAt(target.transform.position);
        }
    }
}
