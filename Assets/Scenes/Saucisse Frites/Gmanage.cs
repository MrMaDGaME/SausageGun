using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class Gmanage : MonoBehaviourPunCallbacks
{
    [SerializeField] 
    GameObject playerPrefab;
    [SerializeField] 
    GameObject playerPrefab2;
    [SerializeField]
    GameObject ASSAULT_RIFLE;
    [SerializeField]
    GameObject SHOTGUN;
    [SerializeField]
    GameObject SUBMACHINE_GUN;
    [SerializeField]
    GameObject SNIPER;
    [SerializeField]
    GameObject HANDGUN;
    [SerializeField]
    GameObject FORK;

    private PhotonView view;
    private int actorNumber;
    

    public TMP_Text Decompte;
    public TMP_Text Decompte2;
    public TMP_Text Decompte3;

    public TMP_Text GraphicBlueScore;
    public TMP_Text GraphicRedScore;
    
    public static Vector3[] spawn_points_saucisse_frites = new Vector3[9];
    
    public static Vector3[] spawnPointsBlue = new Vector3[3];
    public static Vector3[] spawnPointsRed = new Vector3[3];
    
    public GameObject Drapeau;
    public GameObject Socleinf;
    public GameObject Soclesup;
    public GameObject Tube;
    
    public GameObject Drapeau2;
    public GameObject Socleinf2;
    public GameObject Soclesup2;
    public GameObject Tube2;
    
    public GameObject Drapeau3;
    public GameObject Socleinf3;
    public GameObject Soclesup3;
    public GameObject Tube3;

    public TMP_Text Hp;


    private bool Manche1Weapons = true;
    private bool Manche2Weapons = true;
    
    #region Singeleton Implementation
    
    public static Gmanage instance = null;

    public GameObject EndGameMenu;
    public GameObject EndGameMenu2;
    public GameObject InGameMenu;
    public GameObject Crosshair;

    public bool GameFinished;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
    }
    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        GameFinished = false;
        spawn_points_saucisse_frites[0] = new Vector3(3.66f, 4f, 40.84f);
        spawn_points_saucisse_frites[1] = new Vector3(0.2f, 4f, 40.99f);
        spawn_points_saucisse_frites[2] = new Vector3(-3.38f, 4f, 41.22f);
        spawn_points_saucisse_frites[3] = new Vector3(-4.16f, 4f, -43.63f);
        spawn_points_saucisse_frites[4] = new Vector3(-0.65f, 4f, -43.73f);
        spawn_points_saucisse_frites[5] = new Vector3(2.94f, 4f, -43.81f);
        spawn_points_saucisse_frites[6] = new Vector3(-43.09f, 4f, 9.76f);
        spawn_points_saucisse_frites[7] = new Vector3(41.68f, 4f, 8.16f);
        spawn_points_saucisse_frites[8] = new Vector3(6.98f, 8f, -0.11f);
        
        
        spawnPointsRed[0] = new Vector3(0.60f, 4f, 40.50f);
        spawnPointsRed[1] = new Vector3(-2.5f, 4f, 40.84f);
        spawnPointsRed[2] = new Vector3(3.70f, 4f, 40.99f);
        
        spawnPointsBlue[0] = new Vector3(-4f, 4f, -43.63f);
        spawnPointsBlue[1] = new Vector3(-0.16f, 4f, -43.73f);
        spawnPointsBlue[2] = new Vector3(-4.16f, 4f, -42);
        
        EndGameMenu.SetActive(false);
        EndGameMenu2.SetActive(false);
        InGameMenu.SetActive(true);
        Crosshair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        
        //instancer le personnages juste après le chargement de scène
        if (PhotonNetwork.IsConnected)
        {
            view = GetComponent<PhotonView>();
            actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            if (playerPrefab != null)
            {
                Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["Team"] + "/" + PhotonNetwork.LocalPlayer.CustomProperties["NbInTheTeam"]);
                
                if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("red"))
                {
                    if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("0"))
                        PhotonNetwork.Instantiate(playerPrefab.name, spawnPointsRed[0], Quaternion.identity);
                    else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("1"))
                        PhotonNetwork.Instantiate(playerPrefab.name, spawnPointsRed[1], Quaternion.identity);
                    else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("2"))
                        PhotonNetwork.Instantiate(playerPrefab.name, spawnPointsRed[2], Quaternion.identity);

                    

                }
                else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("blue"))
                {
                    if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("0"))
                        PhotonNetwork.Instantiate(playerPrefab2.name, spawnPointsBlue[0], Quaternion.identity);
                    else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("1"))
                        PhotonNetwork.Instantiate(playerPrefab2.name, spawnPointsBlue[1], Quaternion.identity);
                    else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("2"))
                        PhotonNetwork.Instantiate(playerPrefab2.name, spawnPointsBlue[2], Quaternion.identity);

                    
                }
            }
            
            if (PhotonNetwork.IsMasterClient)
                InstantiateWeapons();
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        GraphicBlueScore.text = ScoreCalculate.BlueTeamScore.ToString();
        GraphicRedScore.text = ScoreCalculate.RedTeamScore.ToString();
        Hp.text = TakeDamage.healthbar.ToString();
        
        
        Debug.Log(ScoreCalculate.RedTeamScore + " / " + ScoreCalculate.BlueTeamScore);
        if ((ScoreCalculate.BlueTeamScore == 1 || ScoreCalculate.RedTeamScore == 1) && Manche1Weapons)
        {
            if (PhotonNetwork.IsMasterClient)
                RespawnWeapon();
            Manche1Weapons = false;
            global::Decompte.isActive = false;

        }
        
        if ((ScoreCalculate.BlueTeamScore == 1 && ScoreCalculate.RedTeamScore == 1) && Manche2Weapons)
        {
            if (PhotonNetwork.IsMasterClient)
                RespawnWeapon();
            Manche2Weapons = false;
            global::Decompte.isActive2 = false;
        }

        if (ScoreCalculate.BlueTeamScore == 2)
        {
            EndGameMenu.SetActive(true);
            InGameMenu.SetActive(false);
            Crosshair.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        
        if (ScoreCalculate.RedTeamScore == 2)
        {
            EndGameMenu2.SetActive(true);
            InGameMenu.SetActive(false);
            Crosshair.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        if (ScoreCalculate.RedTeamScore == 2 || ScoreCalculate.BlueTeamScore == 2)
        {
            GameFinished = true;
        }
        
        //Invoke("CheckIfATeamAsNoPlayer", 10f);

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

        if (redPlayer == 0 && !global::InGameMenu.OnQuitButton && !GameFinished)
        {
            EndGameMenu.SetActive(true);
            InGameMenu.SetActive(false);
            Crosshair.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        if (bluePlayer == 0 && !global::InGameMenu.OnQuitButton && !GameFinished)
        {
            EndGameMenu2.SetActive(true);
            InGameMenu.SetActive(false);
            Crosshair.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }*/
    

    public void RespawnWeapon()
    {
        GameObject[] OnTheGroundPic = GameObject.FindGameObjectsWithTag("pic_a_saucisse"); 
        GameObject[] OnTheGroundAP = GameObject.FindGameObjectsWithTag("arme_de_poing");
        GameObject[] OnTheGroundAS = GameObject.FindGameObjectsWithTag("fusil_d'assaut");
        GameObject[] OnTheGroundM = GameObject.FindGameObjectsWithTag("mitraillette");
        GameObject[] OnTheGroundP = GameObject.FindGameObjectsWithTag("pompe");
        GameObject[] OnTheGroundS = GameObject.FindGameObjectsWithTag("sniper");
                
        foreach (var pic in OnTheGroundPic)
        {
            PhotonNetwork.Destroy(pic.gameObject);
        }
        foreach (var ap in OnTheGroundAP)
        {
            PhotonNetwork.Destroy(ap.gameObject);
        }
        foreach (var As in OnTheGroundAS)
        {
            PhotonNetwork.Destroy(As.gameObject);
        }
        foreach (var M in OnTheGroundM)
        {
            PhotonNetwork.Destroy(M.gameObject);
        }
        foreach (var P in OnTheGroundP)
        {
            PhotonNetwork.Destroy(P.gameObject);
        }
        foreach (var S in OnTheGroundS)
        {
            PhotonNetwork.Destroy(S.gameObject);
        }
        
        InstantiateWeapons();
    }
    
    
    public void InstantiateWeapons()
    {
        if (ASSAULT_RIFLE != null && SHOTGUN != null && SUBMACHINE_GUN != null && SNIPER != null &&
            HANDGUN != null && FORK != null)
        {
            foreach (var point in spawn_points_saucisse_frites)
            {
                Weapon.WeaponsType type = Weapon.GetRandomWeapon();

                if (type == Weapon.WeaponsType.ASSAULT_RIFLE)
                {
                    PhotonNetwork.InstantiateSceneObject(ASSAULT_RIFLE.name, point, Quaternion.identity);
                }
                else if (type == Weapon.WeaponsType.SHOTGUN)
                {
                    PhotonNetwork.InstantiateSceneObject(SHOTGUN.name, point, Quaternion.identity);
                }
                else if (type == Weapon.WeaponsType.SUBMACHINE_GUN)
                {
                    PhotonNetwork.InstantiateSceneObject(SUBMACHINE_GUN.name, point, Quaternion.identity);
                }
                else if (type == Weapon.WeaponsType.SNIPER)
                {
                    PhotonNetwork.InstantiateSceneObject(SNIPER.name, point, Quaternion.identity);
                }
                else if (type == Weapon.WeaponsType.HANDGUN)
                {
                    PhotonNetwork.InstantiateSceneObject(HANDGUN.name, point, Quaternion.identity);
                }
                else if (type == Weapon.WeaponsType.FORK)
                {
                    PhotonNetwork.InstantiateSceneObject(FORK.name, point, Quaternion.identity);
                }
            }
        }
    }
    
    
    public void OnQuitButtonClicked()
    {
        ScoreCalculate.BlueTeamScore = 0;
        ScoreCalculate.RedTeamScore = 0;
        ScoreCalculate.BlueTeamPlayerDead = 0;
        ScoreCalculate.RedTeamPlayerDead = 0;
        PhotonNetwork.LeaveRoom();
        
    }

    #region PUN2 Callbacks

    public override void OnLeftRoom()
    {
        //PhotonNetwork.DestroyAll();
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LoadLevel("Launcher");
    }
    

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
    
    
    #endregion
}
