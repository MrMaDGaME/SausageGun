using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CollectItems : MonoBehaviourPunCallbacks
{
    
    public enum WeaponsType
    {
        NONE, ASSAULT_RIFLE, SHOTGUN, SUBMACHINE_GUN, SNIPER, HANDGUN
    }

    public WeaponsType current_weapon = WeaponsType.NONE;

    public Camera FPS_Camera;
    
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

    public bool fork = false;

    public GameObject FusilDassaultInTheHand;
    public GameObject ArmedePoingInTheHand;
    public GameObject SniperInTheHand;
    public GameObject PompeInTheHand;
    public GameObject MitrailletteInTheHand;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        /*if (current_weapon == WeaponsType.NONE)
        {
            FusilDassaultInTheHand.SetActive(false);
            ArmedePoingInTheHand.SetActive(false);
            SniperInTheHand.SetActive(false);
            PompeInTheHand.SetActive(false);
            MitrailletteInTheHand.SetActive(false);
        }*/

        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit _collect;
            Ray ray = FPS_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out _collect, 5))
            {
                PhotonView photonView = PhotonView.Get(this);
                
                if (_collect.collider.gameObject.CompareTag("pic_a_saucisse") && !fork)
                {
                    _collect.collider.gameObject.GetComponent<PhotonView>()
                        .RPC("NDestroy", RpcTarget.AllBuffered);
                    fork = true;
                }

                if (_collect.collider.gameObject.CompareTag("arme_de_poing"))
                {
                    _collect.collider.gameObject.GetComponent<PhotonView>()
                        .RPC("NDestroy", RpcTarget.AllBuffered);
                    photonView.RPC("respawn_weapon", RpcTarget.AllBuffered, current_weapon);
                    current_weapon = WeaponsType.HANDGUN;
                    GetComponent<shoot_gun>().enabled = true;
                    photonView.RPC("ActivateAP",RpcTarget.AllBuffered);
                }
                
                if (_collect.collider.gameObject.CompareTag("fusil_d'assaut"))
                {
                    _collect.collider.gameObject.GetComponent<PhotonView>()
                        .RPC("NDestroy", RpcTarget.AllBuffered);
                    photonView.RPC("respawn_weapon", RpcTarget.AllBuffered, current_weapon);
                    current_weapon = WeaponsType.ASSAULT_RIFLE;
                    GetComponent<shoot_AR>().enabled = true;
                    photonView.RPC("ActivateAS",RpcTarget.AllBuffered);
                }
                
                if (_collect.collider.gameObject.CompareTag("mitraillette"))
                {
                    _collect.collider.gameObject.GetComponent<PhotonView>()
                        .RPC("NDestroy", RpcTarget.AllBuffered);
                    photonView.RPC("respawn_weapon", RpcTarget.AllBuffered, current_weapon);
                    current_weapon = WeaponsType.SUBMACHINE_GUN;
                    GetComponent<shoot_submachine>().enabled = true;
                    photonView.RPC("ActivateM",RpcTarget.AllBuffered);
                }
                
                if (_collect.collider.gameObject.CompareTag("pompe"))
                {
                    _collect.collider.gameObject.GetComponent<PhotonView>()
                        .RPC("NDestroy", RpcTarget.AllBuffered);
                    photonView.RPC("respawn_weapon", RpcTarget.AllBuffered, current_weapon);
                    current_weapon = WeaponsType.SHOTGUN;
                    GetComponent<shoot_shotgun>().enabled = true;
                    photonView.RPC("ActivateP",RpcTarget.AllBuffered);
                }
                
                if (_collect.collider.gameObject.CompareTag("sniper"))
                {
                    _collect.collider.gameObject.GetComponent<PhotonView>()
                        .RPC("NDestroy", RpcTarget.AllBuffered);
                    photonView.RPC("respawn_weapon", RpcTarget.AllBuffered, current_weapon);
                    current_weapon = WeaponsType.SNIPER;
                    GetComponent<shoot_sniper>().enabled = true;
                    photonView.RPC("ActivateS",RpcTarget.AllBuffered);
                }
            }
        }
    }

    [PunRPC]
    public void respawn_weapon(WeaponsType weapon)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            switch (weapon)
            {
                case WeaponsType.HANDGUN:
                    PhotonNetwork.InstantiateSceneObject(HANDGUN.name, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    GetComponent<shoot_gun>().enabled = false;
                    break;
            
                case WeaponsType.ASSAULT_RIFLE:
                    PhotonNetwork.InstantiateSceneObject(ASSAULT_RIFLE.name, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    GetComponent<shoot_AR>().enabled = false;
                    break;
            
                case WeaponsType.SNIPER: 
                    PhotonNetwork.InstantiateSceneObject(SNIPER.name, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    GetComponent<shoot_sniper>().enabled = false;
                    break;
            
                case WeaponsType.SUBMACHINE_GUN:
                    PhotonNetwork.InstantiateSceneObject(SUBMACHINE_GUN.name, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    GetComponent<shoot_submachine>().enabled = false;
                    break;
            
                case WeaponsType.SHOTGUN:
                    PhotonNetwork.InstantiateSceneObject(SHOTGUN.name, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    GetComponent<shoot_shotgun>().enabled = false;
                    break;
            
                case WeaponsType.NONE:
                    break;
            
                default:
                    Debug.Log("drop weapon : invalid weapon type");
                    break;
            }
        }
        else
        {
            switch (weapon)
            {
                case WeaponsType.HANDGUN:
                    GetComponent<shoot_gun>().enabled = false;
                    break;
            
                case WeaponsType.ASSAULT_RIFLE:
                    GetComponent<shoot_AR>().enabled = false;
                    break;
            
                case WeaponsType.SNIPER: 
                    GetComponent<shoot_sniper>().enabled = false;
                    break;
            
                case WeaponsType.SUBMACHINE_GUN:
                    GetComponent<shoot_submachine>().enabled = false;
                    break;
            
                case WeaponsType.SHOTGUN:
                    GetComponent<shoot_shotgun>().enabled = false;
                    break;
            
                case WeaponsType.NONE:
                    break;
            
                default:
                    Debug.Log("drop weapon : invalid weapon type");
                    break;
            }
        }
    }

    
    [PunRPC]
    public void ActivateAS()
    {
        FusilDassaultInTheHand.SetActive(true);
        ArmedePoingInTheHand.SetActive(false);
        SniperInTheHand.SetActive(false);
        PompeInTheHand.SetActive(false);
        MitrailletteInTheHand.SetActive(false);

    }
    
    [PunRPC]
    public void ActivateAP()
    {
        FusilDassaultInTheHand.SetActive(false);
        ArmedePoingInTheHand.SetActive(true);
        SniperInTheHand.SetActive(false);
        PompeInTheHand.SetActive(false);
        MitrailletteInTheHand.SetActive(false);
    }
    
    [PunRPC]
    public void ActivateS()
    {
        FusilDassaultInTheHand.SetActive(false);
        ArmedePoingInTheHand.SetActive(false);
        SniperInTheHand.SetActive(true);
        PompeInTheHand.SetActive(false);
        MitrailletteInTheHand.SetActive(false);
    }
    [PunRPC]
    public void ActivateP()
    {
        FusilDassaultInTheHand.SetActive(false);
        ArmedePoingInTheHand.SetActive(false);
        SniperInTheHand.SetActive(false);
        PompeInTheHand.SetActive(true);
        MitrailletteInTheHand.SetActive(false);
    }
    
    [PunRPC]
    public void ActivateM()
    {
        FusilDassaultInTheHand.SetActive(false);
        ArmedePoingInTheHand.SetActive(false);
        SniperInTheHand.SetActive(false);
        PompeInTheHand.SetActive(false);
        MitrailletteInTheHand.SetActive(true);
    }
}
