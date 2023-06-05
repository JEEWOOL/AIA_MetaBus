using FreeNet;
using JEEWOO.NET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GunController theGunController;
    private Gun currentGun;

    public GameObject bullet_HUD;
    public Text[] text_Bullet;                  // �Ѿ� ���� ����
    public ShootingInfo hpInfo;

    // �÷��̾� HP_UI
    public int maxHp;
    public float currentHp;
    public Image hp_Gague;

    private void Start()
    {
        currentHp = maxHp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentHp -= 10;
        }
        CheckBullet();
        HpGagueUpDate();
    }

    void HpGagueUpDate()
    {
        hp_Gague.fillAmount = hpInfo.HpRate();
    }

    void CheckBullet()
    {
        currentGun = theGunController.currentGun;
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = currentGun.reloadBulletCount.ToString();
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();
    }
}
