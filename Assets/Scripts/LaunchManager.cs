using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.Audio;
using Random = UnityEngine.Random;
using TMPro;

public class LaunchManager : MonoBehaviourPunCallbacks
{
    #region Variables
    
    
    [Header("Login UI")] public GameObject LoginUIPanel;
    public InputField PlayerNameInput;
    
    [Header("Connecting Info Panel")] public GameObject ConnectingInfoUIPanel;
    
    [Header("Creating Room Info Panel")] public GameObject CreatingRoomInfoUIPanel;
    
    [Header("GameOptions Panel")] public GameObject GameOptionsUIPanel;
    
    [Header("CreateRoom Panel")] public GameObject CreateRoomUIPanel;
    public InputField RoomNameInputField;
    public string GameMode;
    public string Map;
    public byte MaxPlayer;
    public Button CreateRoomButton;

    [Header("Inside Room Panel")] public GameObject InsideRoomUIPanel;
    
    
    
    [Header("Join Random Room Panel")] public GameObject JoinRandomRoomUIPanel;

    [Header("OptionMenu Panel")] public GameObject OptionUIPanel;
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    
    [Header("Listing Player Panel")] public GameObject ListingPlayerPanel;
    public TMP_Text roomInfoText;
    public GameObject playerListPrefab;
    public GameObject playerListContent;

    [Header("TMP and normal UIObject")] public Button Button;
    public Toggle toggle1Vs1;
    public Toggle toggle2Vs2;
    public Toggle toggle3Vs3;
    
    Resolution[] resolutions;
    private Dictionary<int, GameObject> PlayerListGameobjects;
    
    #endregion

    
    
    
    #region Unity Methods
    
    // First methode to be called
   private void Awake()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Cursor.visible = true;

        ActivatePanel(LoginUIPanel.name);
        
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        
        List<string> listofresolutions = new List<string>();

        int currentResolutionindex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string r = resolutions[i].width + " x " + resolutions[i].height;
            listofresolutions.Add(r);

            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height )
                currentResolutionindex = i;
        }
        resolutionDropdown.AddOptions(listofresolutions);
        resolutionDropdown.value = currentResolutionindex;
        resolutionDropdown.RefreshShownValue();
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (LoginUIPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnLoginButtonClicked();
            }
        }
        
    }
    
    #endregion
    
    
    #region OptionsMenu Methods

    public void OntoogleClicked(string tooglename)
    {
        switch (tooglename)
        {
            case "1vs1toogle":
                toggle2Vs2.isOn = false;
                toggle3Vs3.isOn = false;
                break;
            case "2vs2toogle":
                toggle1Vs1.isOn = false;
                toggle3Vs3.isOn = false;
                break;
            case "3vs3toogle":
                toggle1Vs1.isOn = false;
                toggle2Vs2.isOn = false;
                break;
        }
        CreateRoomButton.interactable = true;
        
    }
    

    public void ResolutionChange(int resolutionindex)
    {
        Resolution resolution = resolutions[resolutionindex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void VolumeChange(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void QualityChange(int inedesxquality)
    {
        QualitySettings.SetQualityLevel(inedesxquality);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    #endregion
    
    
    
    #region ClickOnButtons 
    
    
    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            ActivatePanel(ConnectingInfoUIPanel.name);
            
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("PlayerName is invalid");
        }
    }

    public void OnCanceldButtonClicked()
    {
        ActivatePanel(GameOptionsUIPanel.name);
    }

    public void OnCreatedRoomButtonClicked()
    {
        if (GameMode != null && Map != null)
        {
            string roomName = RoomNameInputField.text;

            if (string.IsNullOrEmpty(roomName))
            {
                roomName = "Room_" + UnityEngine.Random.Range(1000, 9999);
            }

            RoomOptions roomOptions = new RoomOptions();

            string[] roomProperties = { "GameMode", "Map" };
        
            //3 Gamemodes :
            //1. 1vs1
            //2. 2vs2
            //3. 3vs3
            
            //2 Maps :
            //1. Mix & Twist = mx
            //2. Saucisse Frites = sf
            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "GameMode", GameMode}, {"Map", Map} };

            string _gamemode = GameMode;
            
            if (_gamemode == "1vs1")
                roomOptions.MaxPlayers = 2;
            else if (_gamemode == "2vs2")
                roomOptions.MaxPlayers = 4;
            else
                roomOptions.MaxPlayers = 6;

            roomOptions.CustomRoomPropertiesForLobby = roomProperties;
            roomOptions.CustomRoomProperties = customRoomProperties;
            
            
            PhotonNetwork.CreateRoom(roomName,roomOptions);

        }
        
        
    }
    
    
    public void OnJoinRandomRoomButtonClicked(string _gameMode)
    {
        GameMode = _gameMode;

        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { {"GameMode", _gameMode } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
        
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void StartTheGame()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Map"))
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("mx"))
            {
                PhotonNetwork.LoadLevel("Mix & Twist");
            }
            else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("sf"))
            {
                PhotonNetwork.LoadLevel("Saucisse Frites");
            }
        }
    }
    
    #endregion
    
    
    #region Pun Callbakcs
    
    
    public override void OnConnected()
    {
        Debug.Log(PhotonNetwork.NickName + " connected to internet.");
    }

    public override void OnConnectedToMaster()
    { 
        ActivatePanel(GameOptionsUIPanel.name);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is Connected to photon server.");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created");
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        
        
        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " ";

        if (PlayerListGameobjects == null)
        {
            PlayerListGameobjects = new Dictionary<int, GameObject>();
            
        }

        foreach (Player player in PhotonNetwork.PlayerList )
        {
            GameObject playerListGameobject = Instantiate(playerListPrefab);
            playerListGameobject.transform.SetParent(playerListContent.transform);
            playerListGameobject.transform.localScale = Vector3.one;

            playerListGameobject.transform.Find("PlayerNameText").GetComponent<TMP_Text>().text = player.NickName;

            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {    
                playerListGameobject.transform.Find("PlayerIndicator").gameObject.SetActive(true);
                
            }
            else
            {
                playerListGameobject.transform.Find("PlayerIndicator").gameObject.SetActive(false);
            }
            
            PlayerListGameobjects.Add(player.ActorNumber, playerListGameobject);
            
        }
        
        int[] teamRed = {1, 3, 5};
        if (teamRed.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            int i = 0;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties["Team"] == "red")
                {
                    i++;
                }
            }
            ExitGames.Client.Photon.Hashtable playerTeam = new ExitGames.Client.Photon.Hashtable() { {"Team", "red" } , {"NbInTheTeam" , "" + i } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerTeam);
        }
        else
        {
            int i = 0;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties["Team"] == "blue")
                {
                    i++;
                }
            }
            ExitGames.Client.Photon.Hashtable playerTeam = new ExitGames.Client.Photon.Hashtable() { {"Team", "blue" } , {"NbInTheTeam" ,"" + i } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerTeam);
        }
        
        
    }

    
    
    
    public override void OnLeftRoom()
    {
        ActivatePanel(GameOptionsUIPanel.name);
        foreach (GameObject playerListGameObject in PlayerListGameobjects.Values)
        {
            Destroy(playerListGameObject);
        }
        
        PlayerListGameobjects.Clear();
        PlayerListGameobjects = null;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        if (GameMode != null && Map != null)
        {
            string roomName = RoomNameInputField.text;

            if (string.IsNullOrEmpty(roomName))
            {
                roomName = "Room_" + UnityEngine.Random.Range(1000, 9999);
            }

            RoomOptions roomOptions = new RoomOptions();

            string[] roomProperties = { "GameMode", "Map" };
        
            //3 Gamemodes :
            //1. 1vs1
            //2. 2vs2
            //3. 3vs3
            
            //2 Maps :
            //1. Mix & Twist = mx
            //2. Saucisse Frites = sf

            roomOptions.MaxPlayers = 6;

            int n = Random.Range(0, 2);
            if (n == 1) 
                Map = "mx";
            else
                Map = "sf";

            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "GameMode", GameMode}, {"Map", Map} };
            
            string _gamemode = GameMode;
            if (_gamemode == "1vs1")
                roomOptions.MaxPlayers = 2;
            else if (_gamemode == "2vs2")
                roomOptions.MaxPlayers = 4;
            else
                roomOptions.MaxPlayers = 6;
            
            
            roomOptions.CustomRoomPropertiesForLobby = roomProperties;
            roomOptions.CustomRoomProperties = customRoomProperties;

            PhotonNetwork.CreateRoom(roomName,roomOptions);
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount);
        GameObject playerListGameobject = Instantiate(playerListPrefab);
        playerListGameobject.transform.SetParent(playerListContent.transform);
        playerListGameobject.transform.localScale = Vector3.one;

        playerListGameobject.transform.Find("PlayerNameText").GetComponent<TMP_Text>().text = newPlayer.NickName;

        if (newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {    
            playerListGameobject.transform.Find("PlayerIndicator").gameObject.SetActive(true);
                
        }
        else
        {
            playerListGameobject.transform.Find("PlayerIndicator").gameObject.SetActive(false);
        }
            
        PlayerListGameobjects.Add(newPlayer.ActorNumber, playerListGameobject);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                StartTheGame();
            }
            
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(PlayerListGameobjects[otherPlayer.ActorNumber].gameObject);
        PlayerListGameobjects.Remove(otherPlayer.ActorNumber);
        
    }
    

    #endregion
    
    
    #region Public Methods
    
    public void ActivatePanel(string panelNameToBeActivated)
    {
        LoginUIPanel.SetActive(LoginUIPanel.name.Equals(panelNameToBeActivated));
        ConnectingInfoUIPanel.SetActive(ConnectingInfoUIPanel.name.Equals(panelNameToBeActivated));
        CreatingRoomInfoUIPanel.SetActive(CreatingRoomInfoUIPanel.name.Equals(panelNameToBeActivated));
        CreateRoomUIPanel.SetActive(CreateRoomUIPanel.name.Equals(panelNameToBeActivated));
        GameOptionsUIPanel.SetActive(GameOptionsUIPanel.name.Equals(panelNameToBeActivated));
        InsideRoomUIPanel.SetActive(InsideRoomUIPanel.name.Equals(panelNameToBeActivated));
        JoinRandomRoomUIPanel.SetActive(JoinRandomRoomUIPanel.name.Equals(panelNameToBeActivated));
        OptionUIPanel.SetActive(OptionUIPanel.name.Equals(panelNameToBeActivated));
        ListingPlayerPanel.SetActive(ListingPlayerPanel.name.Equals(panelNameToBeActivated));
    }

    public void SetGameMode(string __gamemode)
    {
        GameMode = __gamemode;
    }

    public void SetMap(string __map)
    {
        Map = __map;
    }

    public void SetMaxPlayer(byte __maxplayer)
    {
        MaxPlayer = __maxplayer;
    }
    
    #endregion
}
