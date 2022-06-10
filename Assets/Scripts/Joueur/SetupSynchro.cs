using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine.SocialPlatforms;


public class SetupSynchro : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject cameraFps;
    [SerializeField] private GameObject MyPlayerPrefab;
    [SerializeField] private TextMeshProUGUI playerNameText;
    
    public bool GameFinished = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<PlayerMovevement>().enabled = true;
            cameraFps.GetComponent<MouseLook>().enabled = true;
            cameraFps.GetComponent<Camera>().enabled = true;
            GetComponent<melee>().enabled = true;
            MyPlayerPrefab.SetActive(false);
        }
        else
        {
            transform.GetComponent<PlayerMovevement>().enabled = false;
            cameraFps.GetComponent<MouseLook>().enabled = false;
            cameraFps.GetComponent<Camera>().enabled = false;
            GetComponent<melee>().enabled = false;
        }
        SetPlayNameOnTheUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreCalculate.RedTeamScore == 2 || ScoreCalculate.BlueTeamScore == 2)
        {
            GameFinished = true;
        }
        
        if (InGameMenu.OnTheMenu || InGameMenu.OnTheOptionMenu)
        {
            if (photonView.IsMine)
            {
                cameraFps.GetComponent<MouseLook>().enabled = false;
            }
        }
        else
        {
            if (photonView.IsMine)
            {
                cameraFps.GetComponent<MouseLook>().enabled = true;
            }
        }

        if (ScoreCalculate.BlueTeamScore == 2 || ScoreCalculate.RedTeamScore == 2)
        {
            if (photonView.IsMine)
            {
                photonView.GetComponent<PlayerMovevement>().enabled = false;
                cameraFps.GetComponent<MouseLook>().enabled = false;
            }
        }
        
        //Invoke("CheckIfATeamAsNoPlayer",10f);
        
    }
    
    /*public void CheckIfATeamAsNoPlayer()
    {
        int redPlayer = 0;
        int bluePlayer = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            
            if (player.CustomProperties.ContainsValue("red"))
            {
                redPlayer++;
            }

            if (player.CustomProperties.ContainsValue("blue"))
            {
                bluePlayer++;
            }
        }
        
        if ((redPlayer == 0 || bluePlayer == 0) && !InGameMenu.OnQuitButton && !GameFinished)
        {
            if (photonView.IsMine)
            {
                photonView.GetComponent<PlayerMovevement>().enabled = false;
                cameraFps.GetComponent<MouseLook>().enabled = false;
            }
        }
    }*/

    void SetPlayNameOnTheUI()
    {
        if (playerNameText != null)
            playerNameText.text = photonView.Owner.NickName;
        if (photonView.Owner.CustomProperties.ContainsValue("red"))
        {
                
            playerNameText.color = Color.red;
        }
        else
        {
            playerNameText.color = Color.blue;
        }
        
    }
}
