using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_Move : MonoBehaviour
{
    // ĳ���� �̵�
    public float moveSpeed = 7f;
    float h, v;
    bool rDown;

    // ĳ���� ����
    CharacterController cc;    
    float gravity = -20f;
    float yVelocity = 0;
    public float jumpPower = 6f;
    public bool isJumping;
    Animator anim;


    CPlayerInfo playerInfo;

    // ī�޶�
    public float lookSensitivity;
    public float cameraRotationLimit;
    [SerializeField]
    private float currentCameraRotationX = 0;
    //public Camera theCamera;
    public GameObject theCamera;

    // �÷��̾� �¿�    
    public float rotSpeed = 200f;
    float mx = 0;

    private void Start()
    {
        //theCamera = FindObjectOfType<Camera>().gameObject;
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        playerInfo = GetComponent<CPlayerInfo>();
    }

    private void Update()
    {
        if (playerInfo.IS_MINE)
        {
            GetInput();
            PMove();
            CameraRotation();
            CharacterRotation();
        }
    }

    void CharacterRotation()
    {
        float mouse_X = Input.GetAxis("Mouse X");
        mx += mouse_X * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, mx, 0);
    }

    void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX += _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        //theCamera.GetComponent<FollowCam>().tarY = currentCameraRotationX;

        theCamera.transform.localEulerAngles = new Vector3(-currentCameraRotationX, 0f, 0f);
    }

    void GetInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        rDown = Input.GetKey(KeyCode.LeftShift);        
    }

    void PMove()
    {
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;
        dir = Camera.main.transform.TransformDirection(dir);

        if(dir != Vector3.zero)
            anim.SetInteger("aniStep", 1);
        else
            anim.SetInteger("aniStep", 0);
        //anim.SetBool("isRun", rDown);

        if (Input.GetButtonDown("Jump")&& !isJumping)
        {            
            anim.SetInteger("aniStep", 4);
            yVelocity = jumpPower;
            StartCoroutine(JumpMotion());
        }

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;        

        cc.Move(dir * moveSpeed * (rDown ? 1.5f : 1f) * Time.deltaTime);
    }
    IEnumerator JumpMotion()
    {
        isJumping = true;        
        yield return new WaitForSeconds(0.7f);
        isJumping = false;
    }
}
