using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : Photon.MonoBehaviour
{
    public Player plMove;
    public GameObject Playercanvas;

    public Image fillImage;
    public float healthAmount;
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public SpriteRenderer sr;

    private void Awake()
    { 
        if(photonView.isMine)
        {
            GameManager.instance.localPlayer = this.gameObject;
        }
       
    }

    [PunRPC]
    public void  ReduceHealth(float amount)
    {
        ModifyHealth(amount);
    }

    private void CheckHealth()
    {
      
        fillImage.fillAmount = healthAmount / 100f;
        if(photonView.isMine && healthAmount <= 0)
        {
            GameManager.instance.EnableRespawn();
            plMove.disableInput = true;
            this.GetComponent<PhotonView>().RPC("Dead", PhotonTargets.AllBuffered);
        }
    }

    public void EnableInput()
    {
        plMove.disableInput = false;
    }
    [PunRPC]
    private void Dead()
    {
        rb.gravityScale = 0;
        bc.enabled = false;
        sr.enabled = false;
        Playercanvas.SetActive(false);
    }
    [PunRPC]
    private void ReSpawn()
    {
        rb.gravityScale = 1;
        bc.enabled = true;
        sr.enabled = true;
        Playercanvas.SetActive(true);
        fillImage.fillAmount = 1;
        healthAmount = 100f;
    }

    private void ModifyHealth(float amount)
    {
       if(photonView.isMine)
        {
            healthAmount -= amount;
            fillImage.fillAmount -= amount;
        }
        else
        {
            healthAmount -= amount;
            fillImage.fillAmount -= amount;
        }
        CheckHealth();
    }
}