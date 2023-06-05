using FreeNet;
using JEEWOO.NET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Gun currentGun;                  // ���� ������ ��
    private AudioSource audioSource;        // �ѱ� �߻� ȿ����
    private float currentFireRate;          // ���� �ӵ� ���
    private bool isReload = false;          // ������ ����Ȯ�� ����

    public Vector3 originPos;               // ���� �� ������
    private RaycastHit hitInfo;             // �����ɽ�Ʈ�� �浹�� ������ �޾ƿ�
    public GameObject theCam;                  // �����ɽ�Ʈ�� ī�޶��� ��� ������ �ޱ����ؼ� ī�޶� ������Ʈ�� �޾ƿ�
    public GameObject hit_Effect;           // �ǰ�����Ʈ
    private float gunAccuracy = 0.04f;      // �ѹ߻�� Ƣ������

    // �÷��̾� ����ź ����
    public GameObject firePos;
    public GameObject bombFactory;    
    public float throwPower = 25f;
    public int bombCount = 3;

    CPlayerInfo playerInfo;
    public HUD hud;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originPos = Vector3.zero;
        playerInfo = GetComponent<CPlayerInfo>();
        CProcessPacket.Instance.OnDispatchFire += OnReceive;
    }

    private void Update()
    {
        if (playerInfo.IS_MINE)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            Bomb();
        }
    }

    // ����ź ����
    void Bomb()
    {
        if (bombCount > 0)
        {            
            if (Input.GetMouseButtonDown(1))
            {
                bombCount--;
                GameObject bomb = Instantiate(bombFactory);
                bomb.transform.position = firePos.transform.position;

                Rigidbody rb = bomb.GetComponent<Rigidbody>();
                rb.AddForce(firePos.transform.forward * throwPower, ForceMode.Impulse);
            }
        }        
    }

    // ���� �ӵ� �ٽ� ���
    void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }
    // �߻� �õ�
    void TryFire()
    {
        if (Input.GetMouseButton(0) && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }
    // �߻� �� ���
    void Fire()
    {
        if (!isReload)
        {
            CPacket pack = CPacket.create((short)PROTOCOL.SHOT_PLAYER_REQ);
            pack.push(playerInfo.USER_ID);
            if (currentGun.currentBulletCount > 0)
            {
                pack.push_int16((short)1);
                Shoot(pack);
            }
            else
            {
                pack.push_int16((short)0);
                StartCoroutine(ReloadCoroutine());
            }
            CNetworkManager.Instance.send(pack);
        }
    }
    // �߻� �� ���
    void Shoot(CPacket pack)
    {
        currentGun.anim.SetTrigger("Attack");
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; // ����ӵ� ����
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash1.Play();
        currentGun.muzzleFlash2.Play();
        Hit(pack);

        //StopAllCoroutines();
        // �ѱ� �ݵ� �ڷ�ƾ
        //StartCoroutine(RetroActionCoroutine());        
    }
    void Shoot(Vector3 pos, Vector3 dir)
    {
        currentGun.anim.SetTrigger("Attack");
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; // ����ӵ� ����
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash1.Play();
        currentGun.muzzleFlash2.Play();
        Hit(pos, dir);

        //StopAllCoroutines();
        // �ѱ� �ݵ� �ڷ�ƾ
        //StartCoroutine(RetroActionCoroutine());        
    }

    // ���� ������� Ȯ��
    private void Hit(CPacket pack)
    {
        Vector3 pos = theCam.transform.position;
        Vector3 dir = theCam.transform.forward + new Vector3(Random.Range(-gunAccuracy - currentGun.accuracy, gunAccuracy + currentGun.accuracy),
            Random.Range(-gunAccuracy - currentGun.accuracy, gunAccuracy + currentGun.accuracy), 0);
        pack.push(pos.x);
        pack.push(pos.y);
        pack.push(pos.z);
        pack.push(dir.x);
        pack.push(dir.y);
        pack.push(dir.z);
        if (Physics.Raycast(pos, dir , out hitInfo, currentGun.range))
        {
            StartCoroutine(Hit_Effect());
        }
    }
    private void Hit(Vector3 pos, Vector3 dir)
    {
        if (Physics.Raycast(pos, dir, out hitInfo, currentGun.range))
        {
            if(hitInfo.transform.gameObject.CompareTag("Player"))// == LayerMask.NameToLayer("Player"))
            {
                ShootingInfo info = hitInfo.transform.gameObject.GetComponent<ShootingInfo>();
                info.currHp -= currentGun.damage;
                if(info.currHp < 0)
                {
                    info.PlayerDie();
                }
            }

            StartCoroutine(Hit_Effect(hitInfo, dir));
        }
    }
    IEnumerator Hit_Effect(RaycastHit hitInfo, Vector3 dir)
    {
        var clone = Instantiate(hit_Effect, hitInfo.point, Quaternion.LookRotation(-dir.normalized));
        Destroy(clone, 2.0f);
        yield return new WaitForSeconds(0.2f);
        var clone1 = Instantiate(hit_Effect, hitInfo.point, Quaternion.LookRotation(-dir.normalized));
        Destroy(clone1, 2.0f);
    }
    IEnumerator Hit_Effect()
    {
        var clone = Instantiate(hit_Effect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        Destroy(clone, 2.0f);
        yield return new WaitForSeconds(0.2f);
        var clone1 = Instantiate(hit_Effect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        Destroy(clone1, 2.0f);
    }

    // ������ �õ�
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }
    // ������
    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;

            currentGun.anim.SetTrigger("ReRoad");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("�Ѿ��� ����!");
        }
    }

    //IEnumerator RetroActionCoroutine()
    //{
    //    Vector3 recoilBack = new Vector3(originPos.x, originPos.y, currentGun.retroActionForce);

    //    currentGun.transform.localPosition = originPos;

    //    // �ѱ�ݵ�
    //    while (currentGun.transform.localPosition.z <= currentGun.retroActionForce - 0.02f)
    //    {
    //        currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
    //        yield return null;
    //    }

    //    while(currentGun.transform.localPosition != originPos)
    //    {
    //        currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
    //        yield return null;
    //    }
    //}

    // �ѱ� �߻� �Ҹ� ���
    void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    void OnReceive(string user_id, short shot, Vector3 pos, Vector3 dir)
    {
        if (playerInfo.USER_ID == user_id)
        {
            switch (shot)
            {
                case 0:
                    StartCoroutine(ReloadCoroutine());
                    break;
                case 1:
                    Shoot(pos, dir);
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        CProcessPacket.Instance.OnDispatchFire -= OnReceive;
    }
}
