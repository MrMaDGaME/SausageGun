using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEditor;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviourPunCallbacks
{
    public static bool OnTheMenu = false;
    public static bool OnTheOptionMenu = false;
    public static bool OnQuitButton;
    public GameObject MenuInGameUI;
    public GameObject Crosshair;
    public GameObject OptionMenu;
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    // Start is called before the first frame update
    void Start()
    {
        OnQuitButton = false;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OnTheMenu)
            {
                Resume();
            }
            else if (OnTheOptionMenu)
            {
                OptionClicked();
            }
            else
            {
                Pause();
                
            }
            
        }

        if (OnTheMenu || OnTheOptionMenu)
        {
            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.S) 
                || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space))
            {
                Resume();
            }
        }
        
        
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

    public void Resume()
    {
        MenuInGameUI.SetActive(false);
        OptionMenu.SetActive(false);
        OnTheMenu = false;
        Crosshair.SetActive(true);
        OnTheOptionMenu = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        OnTheOptionMenu = false;
        MenuInGameUI.SetActive(true);
        OptionMenu.SetActive(false);
        OnTheMenu = true;
        Crosshair.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void OptionClicked()
    {
        OnTheMenu = false;
        OnTheOptionMenu = true;
        MenuInGameUI.SetActive(false);
        OptionMenu.SetActive(true);
    }

    public void BackClick()
    {
        OnTheMenu = true;
        OnTheOptionMenu = false;
        MenuInGameUI.SetActive(true);
        OptionMenu.SetActive(false);
    }
    
    
    public void OnLeaveGameButtonClicked()
    {
        ScoreCalculate.BlueTeamScore = 0;
        ScoreCalculate.RedTeamScore = 0;
        ScoreCalculate.BlueTeamPlayerDead = 0;
        ScoreCalculate.RedTeamPlayerDead = 0;
        PhotonNetwork.LeaveRoom();
        OnQuitButton = true;

    }
    
    public override void OnLeftRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LoadLevel("Launcher");
    }
    
    
}
