using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Photon.MonoBehaviour
{
    public bool moveDir = false;
    public float moveSpeed;
    public float destroyTime;
    public float bulletDamage;

    private void Awake()
    {
        StartCoroutine("DestroyByTime");
    }

    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(destroyTime);
        this.GetComponent<PhotonView>().RPC("DestroyObject", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    public void changeDir_left()
    {
        moveDir = true;
    }

    [PunRPC]
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
    private void Update()
    {
        if(!moveDir)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!photonView.isMine)
         return;

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();
        if(target != null && (!target.isMine || target.isSceneView))
        {
            if(target.tag == "Player")
            {
                target.RPC("ReduceHealth", PhotonTargets.AllBuffered,bulletDamage);
            }
            this.GetComponent<PhotonView>().RPC("DestroyObject",PhotonTargets.AllBuffered);
        }
    }
}
