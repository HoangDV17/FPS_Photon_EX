using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IBoomEffect
{
    [SerializeField] CameraShaker cameraShaker;
    [SerializeField] Volume volume;
    [SerializeField] CanvasGroup flashAlpha;
    [SerializeField] GameObject cameraHolder, canvas, face;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] Reticle reticle;
    float verticalLookRotation;
    bool grounded = false, flashEffIsOn = false;

    Vector3 smoothMoveVelocity, moveAmount;
    Rigidbody rb;
    public PhotonView PV;

    PlayerEquipItem playerEquip;
    public PlayerManager playerManager;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        playerEquip = GetComponent<PlayerEquipItem>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }
    private void Start()
    {
        if(PV.IsMine)
        {
            playerEquip.EquipGun(0);
        }
        else
        {
            face.gameObject.layer = LayerMask.NameToLayer("Default");
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(canvas);
            Destroy(reticle.gameObject);
        }
    }

    private void Update()
    {
        if (!PV.IsMine) return;
        
        if(GameSettingPopup.instance.isSettingOpen) return;
        
        Look();
        Move();
        Jump();
       
        playerEquip.SwitchGun();
        
    }
    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        //reticle.ReticleUpdate(moveDir.sqrMagnitude);
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }
    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90, 90);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }
    private void FixedUpdate()
    {
        if (!PV.IsMine) return;
        if(GameSettingPopup.instance.isSettingOpen) { canvas.SetActive(false); return;}
        else canvas.SetActive(true);
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
        
        if(flashEffIsOn)
        {
            volume.weight -= Time.fixedDeltaTime/5;
            flashAlpha.alpha -= Time.fixedDeltaTime/5;
            if(volume.weight <= 0 && flashAlpha.alpha <= 0)
            {
                flashEffIsOn = false;
                volume.weight = 0;
                flashAlpha.alpha = 0;
            }
        }
        
    }

    public void FlashEffect()
    {
        if(!PV.IsMine) return;
        volume.weight = 1;
        flashEffIsOn = true;
        flashAlpha.alpha = 1;
        AudioManager.Instance.Play("FlashBang_Effect");
        StartCoroutine(cameraShaker.Skake(0.15f, 0.4f));
    }

    public void GenadeEffect()
    {
        if(!PV.IsMine)return;
        StartCoroutine(cameraShaker.Skake(0.15f, 0.4f));
    }
}
