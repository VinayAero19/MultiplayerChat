using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Photon.MonoBehaviour
{
    public PhotonView photonView;
    public  Rigidbody2D rb;
    public GameObject playerCamera;
    public SpriteRenderer sr;
    public Text playerNameText;
    public bool isGrounded = false;
    public float moveSpeed;
    public float jumpForce;

    public GameObject bulletObj;
    public Transform firePos;

    public bool disableInput = false;
    private void Awake()
    {
        if(photonView.isMine)
        {
            playerCamera.SetActive(true);
            playerNameText.text = PhotonNetwork.playerName;
        }
        else
        {
            playerNameText.text = photonView.owner.name;
            playerNameText.color = Color.cyan;
        }
    }

    private void Update()
    {
        if(photonView.isMine && !disableInput)
        {
            CheckInput();
        }
       
    }
    private void CheckInput()
    {
        
        var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += move * moveSpeed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            Shoot();
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            photonView.RPC("FlipTrue",PhotonTargets.AllBuffered);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            photonView.RPC("FlipFalse", PhotonTargets.AllBuffered);
        }
    }

    private void Shoot()
    {
        if(sr.flipX == false)
        {
            GameObject obj = PhotonNetwork.Instantiate(bulletObj.name, new Vector2(firePos.transform.position.x, firePos.transform.position.y), Quaternion.identity, 0);
        }
        if (sr.flipX == true)
        {
            GameObject obj = PhotonNetwork.Instantiate(bulletObj.name, new Vector2(firePos.transform.position.x, firePos.transform.position.y), Quaternion.identity, 0);
            obj.GetComponent<PhotonView>().RPC("changeDir_left", PhotonTargets.AllBuffered);
        }

    }

    [PunRPC]
    private void FlipTrue()
    {
        sr.flipX = true;
    }
    [PunRPC]
    private void FlipFalse()
    {
        sr.flipX = false ;
    }
}
