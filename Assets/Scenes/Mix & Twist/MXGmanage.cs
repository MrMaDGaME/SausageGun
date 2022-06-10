using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MXGmanage : MonoBehaviourPunCallbacks
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
    
    public static Vector3[] spawn_points_mix_twist = new Vector3[9];
    
    
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
    
    public static MXGmanage instance = null;
    
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
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        GameFinished = false;
        
        spawn_points_mix_twist[0] = new Vector3(6.7f, -17.46f, -3.74f);
        spawn_points_mix_twist[1] = new Vector3(6.7f, -17.46f, 113.89f);
        spawn_points_mix_twist[2] = new Vector3(14.1f, -17.46f, -3.74f);
        spawn_points_mix_twist[3] = new Vector3(14.1f, -17.46f, 113.89f);
        spawn_points_mix_twist[4] = new Vector3(23.9f, -17.46f, -3.74f);
        spawn_points_mix_twist[5] = new Vector3(23.9f, -17.46f, 113.89f);
        spawn_points_mix_twist[6] = new Vector3(40.6f, 39f, 56.6f);
        spawn_points_mix_twist[7] = new Vector3(-15.11f, 39f, 56.24f);
        spawn_points_mix_twist[8] = new Vector3(9.75f, 68f, 57.68f);
        
        spawnPointsRed[0] = new Vector3(6.7f, -17.46f, -11.43f);
        spawnPointsBlue[0] = new Vector3(6.7f, -17.46f, 119.34f);
        spawnPointsRed[1] = new Vector3(14.1f, -17.46f, -11.43f);
        spawnPointsBlue[1] = new Vector3(14.1f, -17.46f, 119.34f);
        spawnPointsRed[2] = new Vector3(23.9f, -17.46f, -11.43f);
        spawnPointsBlue[2] = new Vector3(23.9f, -17.46f, 119.34f);
        
        EndGameMenu.SetActive(false);
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
            foreach (var point in spawn_points_mix_twist)
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
