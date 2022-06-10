using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Decompte : MonoBehaviourPunCallbacks
{
    
    public int NumberPlayer;
    
    
    //Premiere manche
    public float _switch1 = 0f;
    private TMP_Text textDecompte;
    private float startTheCountdown = 10.0f;

    private bool StartCountdown = false;

    public float SecondeCurrentTime;
    public float MinuteCurrentTime;
    public float startingMinuteTime = 0f;
    public float startingSecondeTime = 10f;

    [SerializeField] public TMP_Text SecondeText;
    [SerializeField] public TMP_Text MinuteText;
    
    public static bool isActive = false;
    
    public GameObject Drapeau;
    public GameObject Socleinf;
    public GameObject Soclesup;
    public GameObject Tube;


    //Deuxieme manche

    public float _switch2 = 0f;
    public bool Round2 = true;
    private TMP_Text textDecompte2;
    private float startTheCountdown2 = 10.0f;

    private bool StartCountDown2 = false;

    public float SecondeCurrentTime2;
    public float MinuteCurrentTime2;

    [SerializeField] public TMP_Text SecondeText2;
    [SerializeField] public TMP_Text MinuteText2;

    public bool colorswitched = false;

    public static bool isActive2;
    
    public GameObject Drapeau2;
    public GameObject Socleinf2;
    public GameObject Soclesup2;
    public GameObject Tube2;
    
    //Troisème manche

    public float _switch3 = 0f;
    public TMP_Text textDecompte3;
    private float startTheCountdown3 = 10.0f;

    private bool StartCountDown3 = false;

    public float SecondeCurrentTime3;
    public float MinuteCurrentTime3;
    
    [SerializeField] public TMP_Text SecondeText3;
    [SerializeField] public TMP_Text MinuteText3;

    public static bool isActive3 = false;
    
    public GameObject Drapeau3;
    public GameObject Socleinf3;
    public GameObject Soclesup3;
    public GameObject Tube3;
    
    //Fin des manches
    

    public bool StillGood = true;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Saucisse Frites")
        {
            textDecompte = Gmanage.instance.Decompte;
            textDecompte2 = Gmanage.instance.Decompte2;
            textDecompte3 = Gmanage.instance.Decompte3;
            
            Drapeau = Gmanage.instance.Drapeau;
            Socleinf = Gmanage.instance.Socleinf;
            Soclesup = Gmanage.instance.Soclesup;
            Tube = Gmanage.instance.Tube;
            
            Drapeau2 = Gmanage.instance.Drapeau2;
            Socleinf2 = Gmanage.instance.Socleinf2;
            Soclesup2 = Gmanage.instance.Soclesup2;
            Tube2 = Gmanage.instance.Tube2;
            
            Drapeau3 = Gmanage.instance.Drapeau3;
            Socleinf3 = Gmanage.instance.Socleinf3;
            Soclesup3 = Gmanage.instance.Soclesup3;
            Tube3 = Gmanage.instance.Tube3;
        }
        else
        {
            textDecompte = MXGmanage.instance.Decompte;
            textDecompte2 = MXGmanage.instance.Decompte2;
            textDecompte3 = MXGmanage.instance.Decompte3;
            
            Drapeau = MXGmanage.instance.Drapeau;
            Socleinf = MXGmanage.instance.Socleinf;
            Soclesup = MXGmanage.instance.Soclesup;
            Tube = MXGmanage.instance.Tube;
            
            Drapeau2 = MXGmanage.instance.Drapeau2;
            Socleinf2 = MXGmanage.instance.Socleinf2;
            Soclesup2 = MXGmanage.instance.Soclesup2;
            Tube2 = MXGmanage.instance.Tube2;
            
            Drapeau3 = MXGmanage.instance.Drapeau3;
            Socleinf3 = MXGmanage.instance.Socleinf3;
            Soclesup3 = MXGmanage.instance.Soclesup3;
            Tube3 = MXGmanage.instance.Tube3;
        }
        
        
        startingMinuteTime = CountDown.instance2.startingMinuteTime;
        startingSecondeTime = CountDown.instance2.startingSecondeTime;
        
        SecondeCurrentTime = CountDown.instance2.SecondeCurrentTime;
        MinuteCurrentTime = CountDown.instance2.MinuteCurrentTime;

        SecondeCurrentTime2 = CountDown.instance2.SecondeCurrentTime;
        MinuteCurrentTime2 = CountDown.instance2.MinuteCurrentTime;

        SecondeCurrentTime3 = CountDown.instance2.SecondeCurrentTime;
        MinuteCurrentTime3 = CountDown.instance2.MinuteCurrentTime;


        SecondeText = CountDown.instance2.SecondeText;
        MinuteText = CountDown.instance2.MinuteText;

        SecondeText2 = CountDown.instance2.SecondeText;
        MinuteText2 = CountDown.instance2.MinuteText;

        SecondeText3 = CountDown.instance2.SecondeText;
        MinuteText3 = CountDown.instance2.MinuteText;

        StillGood = CountDown.instance2.StillGood;
        
    }

    private void Start()
    {
        MinuteCurrentTime = startingMinuteTime;
        SecondeCurrentTime = startingSecondeTime;

        MinuteCurrentTime2 = startingMinuteTime;
        SecondeCurrentTime2 = startingSecondeTime;

        MinuteCurrentTime3 = startingMinuteTime;
        SecondeCurrentTime3 = startingSecondeTime;

        if (photonView.IsMine)
        {
            GetComponent<PlayerMovevement>().enabled = false;
            GetComponent<CollectItems>().enabled = false;
        }
    }

    void Update()
    {
        NumberPlayer = PhotonNetwork.PlayerList.Length;
        if (startTheCountdown >= 0.0f)
        {
            startTheCountdown -= Time.deltaTime;
            photonView.RPC("SetText", RpcTarget.AllBuffered, startTheCountdown);
        }

        if (StillGood && MinuteCurrentTime >= 0 && SecondeCurrentTime >= 0 && StartCountdown)
        {
            SecondeCurrentTime -= Time.deltaTime;
            photonView.RPC("ChangeCountdown", RpcTarget.AllBuffered, SecondeCurrentTime, MinuteCurrentTime);
        }

        if (SecondeCurrentTime <= 0f && MinuteCurrentTime <= 0f && ScoreCalculate.BlueTeamScore == 0 && ScoreCalculate.RedTeamScore == 0)
        {
            photonView.RPC("ColorChanger",RpcTarget.AllBuffered);
        }

        if (ScoreCalculate.BlueTeamScore == 1 || ScoreCalculate.RedTeamScore == 1 && Round2)
        {
            if (startTheCountdown2 >= 0.0f)
            {
                StartCountdown = false;
                MinuteText.enabled = false;
                SecondeText.enabled = false;
                MinuteText2.enabled = true;
                SecondeText2.enabled = true;
                startTheCountdown2 -= Time.deltaTime;
                photonView.RPC("SetText2", RpcTarget.AllBuffered, startTheCountdown2);
            }

            if (!colorswitched)
            {
                MinuteText2.color = Color.white;
                SecondeText2.color = Color.white;
                colorswitched = true;
            }

            if (MinuteCurrentTime2 >= 0 && SecondeCurrentTime2 >= 0 && StartCountDown2)
            {
                SecondeCurrentTime2 -= Time.deltaTime;
                photonView.RPC("ChangeCountdown2", RpcTarget.AllBuffered, SecondeCurrentTime2, MinuteCurrentTime2);
            }


            if (SecondeCurrentTime2 <= 0f && MinuteCurrentTime2 <= 0f)
            {
                photonView.RPC("ColorChanger2", RpcTarget.AllBuffered);
            }
            
        }
        

        if (ScoreCalculate.BlueTeamScore == 1 && ScoreCalculate.RedTeamScore == 1)
        {
            Round2 = false;
            if (startTheCountdown3 >= 0.0f)
            {
                StartCountDown2 = false;
                MinuteText2.enabled = false;
                SecondeText2.enabled = false;
                MinuteText3.enabled = true;
                SecondeText3.enabled = true;
                MinuteText3.color = Color.white;
                SecondeText3.color = Color.white;
                startTheCountdown3 -= Time.deltaTime;
                photonView.RPC("SetText3", RpcTarget.AllBuffered, startTheCountdown3);
            }

            if (MinuteCurrentTime3 >= 0 && SecondeCurrentTime3 >= 0 && StartCountDown3)
            {
                SecondeCurrentTime3 -= Time.deltaTime;
                photonView.RPC("ChangeCountdown3", RpcTarget.AllBuffered, SecondeCurrentTime3, MinuteCurrentTime3);
            }
            if (SecondeCurrentTime2 <= 0f && MinuteCurrentTime2 <= 0f && ScoreCalculate.BlueTeamScore == 1 && ScoreCalculate.RedTeamScore == 1)
            {
                photonView.RPC("ColorChanger3",RpcTarget.AllBuffered);
            }
            
        }
    }

    void PassageMinute()
    {
        if (SecondeCurrentTime <= 0)
        {
            if (MinuteCurrentTime == 2.0f)
            {
                MinuteCurrentTime = 1;
            }
            else
            {
                MinuteCurrentTime = 0;
            }

            SecondeCurrentTime = 59.4444444444f;
        }
    }
    
    void PassageMinute2()
    {
        if (SecondeCurrentTime2 <= 0)
        {
            if (MinuteCurrentTime2 == 2.0f)
            {
                MinuteCurrentTime2 = 1;
            }
            else
            {
                MinuteCurrentTime2 = 0;
            }

            SecondeCurrentTime2 = 59.4444444444f;
        }
    }
    
    void PassageMinute3()
    {
        if (SecondeCurrentTime3 <= 0)
        {
            if (MinuteCurrentTime3 == 2.0f)
            {
                MinuteCurrentTime3 = 1;
            }
            else
            {
                MinuteCurrentTime3 = 0;
            }

            SecondeCurrentTime3 = 59.4444444444f;
        }
    }

    [PunRPC]
    public void SetText(float time)
    {
        if (time >= 0.0f)
        {
            if (time > 8.0f)
            {
                textDecompte.text = "";
            }

            if (time > 6.2f)
            {
                textDecompte.text = "A vos fourchettes ...";
            }
            else if (time >= 5.7f)
            {
                textDecompte.text = "";
            }
            else if (time > 4.2f)
            {
                textDecompte.text = "Pret ?";
            }
            else if (time >= 3.7f)
            {
                textDecompte.text = "";
            }
            else if (time > 2.2f)
            {
                textDecompte.text = "MANGEZ !";
                if (photonView.IsMine)
                {
                    GetComponent<PlayerMovevement>().enabled = true;
                    GetComponent<CollectItems>().enabled = true;
                }
            }

        }
        else
        {
            textDecompte.enabled = false;
            StartCountdown = true;
        }
    }

    [PunRPC]
    public void ChangeCountdown(float secondcurrentTime, float minutecurrentTime)
    {
        if (secondcurrentTime < 0 && minutecurrentTime > 0)
        {
            Invoke(nameof(PassageMinute), 0.55f);
        }

        if (minutecurrentTime <= 0 && secondcurrentTime <= 0.0f)
        {

            isActive = true;
        }

        MinuteText.text = minutecurrentTime.ToString("00");
        SecondeText.text = secondcurrentTime.ToString("00");

    }

    [PunRPC]
    public void SetText2(float time)
    {
        if (time >= 0.0f)
        {
            if (time > 8.0f)
            {
                textDecompte2.text = "";
            }

            if (time > 6.2f)
            {
                textDecompte2.text = "A vos fourchettes ...";
            }
            else if (time >= 5.7f)
            {
                textDecompte2.text = "";
            }
            else if (time > 4.2f)
            {
                textDecompte2.text = "Pret ?";
            }
            else if (time >= 3.7f)
            {
                textDecompte2.text = "";
            }
            else if (time > 2.2f)
            {
                textDecompte2.text = "MANGEZ !";
            }

        }
        else
        {
            textDecompte2.enabled = false;
            StartCountDown2 = true;
        }
    }
    
    [PunRPC]
    public void SetText3(float time)
    {
        if (time >= 0.0f)
        {
            if (time > 8.0f)
            {
                textDecompte3.text = "";
            }

            if (time > 6.2f)
            {
                textDecompte3.text = "A vos fourchettes ...";
            }
            else if (time >= 5.7f)
            {
                textDecompte3.text = "";
            }
            else if (time > 4.2f)
            {
                textDecompte3.text = "Pret ?";
            }
            else if (time >= 3.7f)
            {
                textDecompte3.text = "";
            }
            else if (time > 2.2f)
            {
                textDecompte3.text = "MANGEZ !";
            }

        }
        else
        {
            textDecompte3.enabled = false;
            StartCountDown3 = true;
        }
    }
    
    
    [PunRPC]
    public void ChangeCountdown2(float secondcurrentTime2, float minutecurrentTime2)
    {
        if (secondcurrentTime2 < 0 && minutecurrentTime2 > 0)
        {
            Invoke(nameof(PassageMinute2), 0.55f);
        }

        if (minutecurrentTime2 == 0 && secondcurrentTime2 <= 0.0f)
        {
            isActive2 = true;
        }

        MinuteText2.text = minutecurrentTime2.ToString("00");
        SecondeText2.text = secondcurrentTime2.ToString("00");

    }
    
    
    [PunRPC]
    public void ChangeCountdown3(float secondcurrentTime3, float minutecurrentTime3)
    {
        if (secondcurrentTime3 < 0 && minutecurrentTime3 > 0)
        {
            Invoke(nameof(PassageMinute3), 0.55f);
        }

        if (minutecurrentTime3 <= 0 && secondcurrentTime3 <= 0.0f)
        {

            isActive3 = true;
        }

        MinuteText3.text = minutecurrentTime3.ToString("00");
        SecondeText3.text = secondcurrentTime3.ToString("00");

    }
    
    [PunRPC]
    void ColorChanger()
    {
        _switch1 += Time.deltaTime;
        if (SecondeText.color == Color.white && _switch1 >= NumberPlayer)
        {
            MinuteText.color = Color.red;
            SecondeText.color = Color.red;
            _switch1 = 0f;
        }

        if (SecondeText.color == Color.red && _switch1 >= NumberPlayer)
        {
            MinuteText.color = Color.white;
            SecondeText.color = Color.white;
            _switch1 = 0f;
        }
    }
    
    [PunRPC]
    void ColorChanger2()
    {
        _switch2 += Time.deltaTime;
        if (SecondeText2.color == Color.white && _switch2 >= NumberPlayer)
        {
            MinuteText2.color = Color.red;
            SecondeText2.color = Color.red;
            _switch2 = 0f;
        }

        if (SecondeText2.color == Color.red && _switch2 >= NumberPlayer)
        {
            MinuteText2.color = Color.white;
            SecondeText2.color = Color.white;
            _switch2 = 0f;
        }
    }
    
    [PunRPC]
    void ColorChanger3()
    {
        _switch3 += Time.deltaTime;
        if (SecondeText3.color == Color.white && _switch3 >= NumberPlayer)
        {
            MinuteText3.color = Color.red;
            SecondeText3.color = Color.red;
            _switch3 = 0f;
        }

        if (SecondeText3.color == Color.red && _switch3 >= NumberPlayer)
        {
            MinuteText3.color = Color.white;
            SecondeText3.color = Color.white;
            _switch3 = 0f;
        }
    }
}
