using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ItemKnife : Item
{
    int combo = 0;
    float attackCooldown = 0.4f;
    float delayAttack = -1f;
    Animator animator;
    Collider collider;
    PhotonView PV;
    PlayerController controller;
    [SerializeField] Image reticle;
    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        collider.enabled = false;
    }
    public override void Use()
    {
        PV.RPC("Attack", RpcTarget.All);
    }
    public void SwitchCrossHair(Image crossHair)
    {
        crossHair.sprite = ((KnifeInfo)itemInfo).reticle;
    }
    public IEnumerator AttackCooldown(float duration)
    {
        float elapsed = 0.0f;
        delayAttack = duration;
        collider.enabled = true;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        delayAttack = -1f;
        collider.enabled = false;
    }
    [PunRPC]
    void Attack()
    {
        if (delayAttack == -1f)
        {
            combo++;
            AudioManager.Instance.Play(((KnifeInfo)itemInfo).name + "_Attack");
            animator.SetInteger("combo", combo);
            animator.SetTrigger("attack");
            StartCoroutine(AttackCooldown(attackCooldown));
            if (combo >= 2)
            {
                combo = 0;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                if (other.transform != controller.transform)
                {
                    other.gameObject.GetComponent<IDamageable>()?.TakeDamage(((KnifeInfo)itemInfo).damage, GlobalParameter.playerName, KillType.knife);
                    PV.RPC("CreateBlood", RpcTarget.All, other.transform.position);
                    collider.enabled = false;
                }
                break;
            default:
                break;
        }
    }
    [PunRPC]
    void CreateBlood(Vector3 pos)
    {
        CreateController.Instance.CreateBlood(pos);
    }

}
