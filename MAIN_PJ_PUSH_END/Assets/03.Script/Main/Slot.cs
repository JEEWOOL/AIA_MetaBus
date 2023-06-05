using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originPos;

    public Item item;           // ȹ���� ������
    public int itemCount;       // ȹ���� �������� ����
    public Image itemImage;     // ������ �̹���

    public Text text_Count;
    public GameObject go_CountImage;
    public SlotTooltip theSlot;

    private void Start()
    {
        theSlot = FindObjectOfType<SlotTooltip>();
        originPos = transform.position;
    }

    // �̹��� ���� ����
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
    // ������ ȹ��
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType == Item.ItemType.ingredient)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }

        SetColor(1);
    }
    // ������ ���� ����
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }
    // ���� �ʱ�ȭ
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        text_Count.text = "0";
        go_CountImage.SetActive(false);        
    }

    // ���콺 ���� �� / �ƿ� �̺�Ʈ
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null)
        {
            theSlot.ShowToolTip(item, transform.position);
        }        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theSlot.HideToolTip();
    }
}