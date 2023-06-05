using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    public float range;                         // ���� ������ �ִ� �Ÿ�
    public bool pickupActivated = false;        // ���� ������ �� true;

    private RaycastHit hitInfo;                 // �浹ü ���� ����
    public LayerMask layerMask;                // ������ ���̾�� �����ϵ��� ���̾� ����ũ ����

    public Text actionText;
    public Inventory theInventory;

    private CPlayerInfo playerInfo;

    private void Start()
    {
        playerInfo = GetComponentInParent<CPlayerInfo>();
    }

    private void Update()
    {
        if (playerInfo.IS_MINE)
        {
            CheckItem();
            TryAction();
        }
    }
    void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CheckItem();
            CanPickUp();
        }
    }
    void CanPickUp()
    {
        if (pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName +
            " ȹ�� ");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }
    void CheckItem()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
        {
            InfoDisappear();
        }
    }
    void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + 
            " ȹ�� " + "<color=red>" + "(G)" + "</color>";
    }
    void InfoDisappear()
    {
        pickupActivated = false;
        if(actionText != null)
            actionText.gameObject.SetActive(false);
    }
}
