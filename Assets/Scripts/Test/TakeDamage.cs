using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine.SceneManagement;

public class TakeDamage : MonoBehaviourPunCallbacks
{
    public GameObject MyPrefab;
    
    public float health = 100f;

    public bool IsLow = false;
    
    public static bool IsDead = false;

    public static float healthbar;
    
    public static Vector3[] spawnPointsBlueSF = new Vector3[3];
    public static Vector3[] spawnPointsRedSF= new Vector3[3];
    
    public static Vector3[] spawnPointsBlueMX = new Vector3[3];
    public static Vector3[] spawnPointsRedMX = new Vector3[3];

    public bool Manche1 = false;
    public bool Manche2 = false;
    
    public GameObject FusilDassaultInTheHand;
    public GameObject ArmedePoingInTheHand;
    public GameObject SniperInTheHand;
    public GameObject PompeInTheHand;
    public GameObject MitrailletteInTheHand;
    
    [SerializeField]
    private Camera cam;

    private string scene_name;
    

    // Start is called before the first frame update
    void Start()
    {
        spawnPointsRedSF[0] = new Vector3(0.60f, 4f, 40.50f);
        spawnPointsRedSF[1] = new Vector3(-2.5f, 4f, 40.84f);
        spawnPointsRedSF[2] = new Vector3(3.70f, 4f, 40.99f);
        
        spawnPointsBlueSF[0] = new Vector3(-4f, 4f, -43.63f);
        spawnPointsBlueSF[1] = new Vector3(-0.16f, 4f, -43.73f);
        spawnPointsBlueSF[2] = new Vector3(-4.16f, 4f, -42);
        
        spawnPointsRedMX[0] = new Vector3(6.7f, -17.46f, -11.43f);
        spawnPointsBlueMX[0] = new Vector3(6.7f, -17.46f, 119.34f);
        spawnPointsRedMX[1] = new Vector3(14.1f, -17.46f, -11.43f);
        spawnPointsBlueMX[1] = new Vector3(14.1f, -17.46f, 119.34f);
        spawnPointsRedMX[2] = new Vector3(23.9f, -17.46f, -11.43f);
        spawnPointsBlueMX[2] = new Vector3(23.9f, -17.46f, 119.34f);
        
        health = 100;

        scene_name = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead && !IsLow && health <= 20)
        {
            PlayLowSound();
            IsLow = true;
        }

        if (photonView.IsMine)
        {
            healthbar = health;
        }

        if (!IsDead)
        {
            if (scene_name == "Saucisse Frites")
            {
                if (health <= 0f || transform.position.y < 0)
                {
                    health = 0;
                    Die();
                }
            }
            else
            {
                if (health <= 0f || transform.position.y < -20)
                {
                    health = 0;
                    Die();
                }
            }
        }
        
        if (!Manche1 &&(ScoreCalculate.BlueTeamScore == 1 || ScoreCalculate.RedTeamScore == 1))
        {
            DieWithRespawn();
            Manche1 = true;
        }
        
        if (!Manche2 &&(ScoreCalculate.BlueTeamScore == 1 && ScoreCalculate.RedTeamScore == 1))
        {
            DieWithRespawn();
            Manche2 = true;
        }
        
    }

    [PunRPC]
    public void TakingDamage(float _damage)
    {
        if (health > 0)
        {
            health -= _damage;
        }
        
    }

    [PunRPC]
    public void Reborn()
    {
        health = 100f;
        
    }

    [PunRPC]
    public void PlayerDiedInTheBlueTeam()
    {
        ScoreCalculate.BlueTeamPlayerDead++;
        if (ScoreCalculate.BlueTeamPlayerDead == PhotonNetwork.CurrentRoom.MaxPlayers / 2)
        {
            ScoreCalculate.RedTeamScore++;
            ScoreCalculate.BlueTeamPlayerDead = 0;
        }
    }
    
    [PunRPC]
    public void PlayerDiedInTheRedTeam()
    {
        ScoreCalculate.RedTeamPlayerDead++;
        if (ScoreCalculate.RedTeamPlayerDead == PhotonNetwork.CurrentRoom.MaxPlayers / 2)
        {
            ScoreCalculate.BlueTeamScore++;
            ScoreCalculate.RedTeamPlayerDead = 0;
        }
    }

    void Die()
    {
        if (photonView.IsMine)
        {
            IsDead = true;
            cam.GetComponent<AudioSource>().clip = GetComponent<Sons>().bruit_mort;
            cam.GetComponent<AudioSource>().loop = false;
            cam.GetComponent<AudioSource>().Play();
            StartCoroutine(Teleport_sub());
                
            if (photonView.CompareTag("PlayerRed"))
                photonView.RPC("PlayerDiedInTheRedTeam", RpcTarget.AllBuffered);

            else if (photonView.CompareTag("PlayerBlue"))
                photonView.RPC("PlayerDiedInTheBlueTeam", RpcTarget.AllBuffered);
        }
    }

    IEnumerator Teleport_sub()
    {
        float timer = 3f;
        while (timer > 0.0f)
        {
            yield return  new WaitForSeconds(1);
            timer--;
            Teleport();
            transform.GetComponent<PlayerMovevement>().enabled = false;
        }
    }
    void Teleport()
    {
        if (SceneManager.GetActiveScene().name == "Saucisse Frites")
            transform.position = new Vector3(-65.3f,57.8f,-48.4f);
        else
            transform.position = new Vector3(14.7f,134.5f,56f);
    }
    
    void DieWithRespawn()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        float respawnTime = 10.0f;
        while (respawnTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            RespawnPlayer();
            photonView.RPC("DesactivateWeapons",RpcTarget.AllBuffered);
            photonView.RPC("Reborn", RpcTarget.AllBuffered);
            IsDead = false;
            photonView.RPC("UnVanishPrefab",RpcTarget.AllBuffered);
            respawnTime -= 1.0f;
            GetComponent<CollectItems>().enabled = false;
            transform.GetComponent<PlayerMovevement>().enabled = false;
            GetComponent<shoot_gun>().enabled = false;
            GetComponent<shoot_AR>().enabled = false;
            GetComponent<shoot_submachine>().enabled = false;
            GetComponent<shoot_shotgun>().enabled = false;
            GetComponent<shoot_sniper>().enabled = false;
            GetComponent<CollectItems>().fork = false;
            GetComponent<CollectItems>().current_weapon = CollectItems.WeaponsType.NONE;
        }

        transform.GetComponent<PlayerMovevement>().enabled = true;
        GetComponent<CollectItems>().enabled = true;
    }


    [PunRPC]
    public void DesactivateWeapons()
    {
        FusilDassaultInTheHand.SetActive(false);
        ArmedePoingInTheHand.SetActive(false);
        SniperInTheHand.SetActive(false);
        PompeInTheHand.SetActive(false);
        MitrailletteInTheHand.SetActive(false);
    }
    
    
    public void PlayLowSound()
    {
        if (photonView.IsMine)
        {
            cam.GetComponent<AudioSource>().clip = GetComponent<Sons>().battements_de_coeur;
            cam.GetComponent<AudioSource>().loop = true;
            cam.GetComponent<AudioSource>().Play();
        }
        
    }
    

    public void RespawnPlayer()
    {
        if (SceneManager.GetActiveScene().name == "Saucisse Frites")
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("red"))
            {
                if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("0"))
                    transform.position = spawnPointsRedSF[0];
                else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("1"))
                    transform.position = spawnPointsRedSF[1];
                else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("2"))
                    transform.position = spawnPointsRedSF[2];


            }
            else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("blue"))
            {
                if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("0"))
                    transform.position = spawnPointsBlueSF[0];
                else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("1"))
                    transform.position = spawnPointsBlueSF[0];
                else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("2"))
                    transform.position = spawnPointsBlueSF[0];
            }
        }
        else
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("red"))
            {
                if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("0"))
                    transform.position = spawnPointsRedMX[0];
                else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("1"))
                    transform.position = spawnPointsRedMX[1];
                else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("2"))
                    transform.position = spawnPointsRedMX[2];


            }
            else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("blue"))
            {
                if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("0"))
                    transform.position = spawnPointsBlueMX[0];
                else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("1"))
                    transform.position = spawnPointsBlueMX[0];
                else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("2"))
                    transform.position = spawnPointsBlueMX[0];
            }
        }
        
    }
}
