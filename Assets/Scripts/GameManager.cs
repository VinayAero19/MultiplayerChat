using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public GameObject playerPrefab;
    public GameObject gameCanvas;
    public GameObject sceneCamera;
    public Text pingText;
    public GameObject disconnectUI;
    public GameObject playerFeed;
    public GameObject feeGrid;
    private bool off = false;


    [HideInInspector] public GameObject localPlayer;
    public Text respawnTimerText;
    public GameObject respawnMenu;
    private float timerAmount;
    private bool runRespwnTimer = false;

    private void Awake()
    {
        instance = this;
        gameCanvas.SetActive(true);
    }
    private void Update()
    {
        CheckInput();
        pingText.text = "Ping :" + PhotonNetwork.GetPing();
        if(runRespwnTimer)
        {
            StartRespawn();
        }
    }

    public  void EnableRespawn()
    {
        timerAmount = 5;
        runRespwnTimer = true;
        respawnMenu.SetActive(true);
    }
    private void StartRespawn()
    {
        timerAmount -= Time.deltaTime;
        respawnTimerText.text = "Respawning in :" + timerAmount.ToString("F0");

        if(timerAmount <= 0)
        {
            localPlayer.GetComponent<PhotonView>().RPC("Respawn", PhotonTargets.AllBuffered);
            localPlayer.GetComponent<Health>().EnableInput();
            ReSpwanLocation();
            respawnMenu.SetActive(false);
            runRespwnTimer = false;

        }
    }

    public void ReSpwanLocation()
    {
        float randonValue = Random.Range(-3f,5f);
        localPlayer.transform.localPosition = new Vector2(randonValue, 3f); 
    }
    private void CheckInput()
    {
        if(off && Input.GetKeyDown(KeyCode.Escape))
        {
            disconnectUI.SetActive(false);
            off = false;
        }else if(!off && Input.GetKeyDown(KeyCode.Escape))
        {
            disconnectUI.SetActive(true);
            off = true;
        }
        
    }
    public void SpwanPlayer()
    {
        float randomValue = Random.Range(-1f, 1f);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y),Quaternion.identity,0);
        gameCanvas.SetActive(false);
        sceneCamera.SetActive(false); 
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }
    public void OnPhotonPlayerConnected(GameObject player)
    {
        GameObject obj = Instantiate(playerFeed, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(feeGrid.transform, false);
        obj.GetComponent<Text>().text = player.name + " Joined the game";
        obj.GetComponent<Text>().color = Color.green;

    }

    public void OnPhotonPlayerDisConnected(GameObject player)
    {
        GameObject obj = Instantiate(playerFeed, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(feeGrid.transform, false);
        obj.GetComponent<Text>().text = player.name + " Left the game";
        obj.GetComponent<Text>().color = Color.red;

    }
}
