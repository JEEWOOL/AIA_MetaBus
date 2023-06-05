using JEEWOO.NET;
using FreeNet;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> playerList = new List<GameObject>();
    public GameObject cameraPref;
    public Camera maincam; // ����ã��� ī�޶��Ҵ�

    public Text actionText;
    public GameObject inventory;
    private static GameManager _instance = null;
    Transform startPoint;

    public static GameObject slotMachineUI;   // ���Ըӽ� UI
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
        startPoint = GameObject.Find("StartPos").GetComponent<Transform>();
        //GameObject.Instantiate(player, startPoint.position, startPoint.rotation);

        CreateLocalPlayer();
        SendReqSpawnedPlayer();

        slotMachineUI = GameObject.Find("Canvas - SlotMachine");
        if (slotMachineUI != null)
        {
            slotMachineUI.SetActive(false);
        }
    }


    void CreateLocalPlayer()
    {
        GameObject localPlayer = GameObject.Instantiate(player, startPoint.position, startPoint.rotation);

        string user_id = CProcessPacket.Instance.USER_ID;
        localPlayer.AddComponent<CPlayerInfo>().USER_ID = user_id;
        localPlayer.GetComponent<CPlayerInfo>().IS_MINE = true;
        GameObject cameraObj = Instantiate(cameraPref, localPlayer.transform);
        maincam = cameraObj.GetComponent<Camera>(); // ����ã�� ī�޶��Ҵ�
        //cameraObj.AddComponent<FollowCam>().target = localPlayer.transform;
        if (localPlayer.GetComponent<SP_Move>() != null)
            localPlayer.GetComponent<SP_Move>().theCamera = cameraObj;
        ActionController ac = cameraObj.GetComponent<ActionController>();
        if(ac != null)
        {
            ac.actionText = this.actionText;
            ac.theInventory = this.inventory.GetComponent<Inventory>();
        }

        // Player List�� ���
        playerList.Add(localPlayer);
    }

    // ������ Local Player ���������� ����
    void SendReqSpawnedPlayer()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.SPAWN_PLAYER_REQ);
        string user_id = CProcessPacket.Instance.USER_ID;
        msg.push(user_id);

        CNetworkManager.Instance.send(msg);
    }

    // �ٸ� ����� Local Player�� Remote Player�� �� ������ ����
    public void CreateFriendPlayer(CPacket msg)
    {
        short count = msg.pop_int16();
        //if(count >= 2) {
        //    CPacket pack = CPacket.create((short)PROTOCOL.GAME_START_REQ);

        //    CNetworkManager.Instance.send();
        //}
        for (short i = 0; i < count; i++)
        {
            string user_id = msg.pop_string();
            bool isCreate = true;
            foreach (var player in playerList)
            {
                // �̹� ���� Player�� �����Ǿ� �����Ѵ�.
                if (player.GetComponent<CPlayerInfo>().USER_ID == user_id)
                    isCreate = false;
            }
            // Player�� �������� ���� ���� ����
            if (isCreate)
            {
                Vector3 pos = startPoint.position;
                Quaternion rot = startPoint.rotation;
                GameObject playerObj = Instantiate(player, pos, rot);
                playerObj.AddComponent<CPlayerInfo>().USER_ID = user_id;
                playerObj.GetComponent<CPlayerInfo>().IS_MINE = false;

                // Player List�� ���
                playerList.Add(playerObj);
            }
        }
    }
}