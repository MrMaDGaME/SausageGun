using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class shoot_shotgun : MonoBehaviourPunCallbacks
{
    public Camera FPS_Camera;

    public float fireRate = 1.75f;
    
    private float fireTimer;
    
    public float knockback = 300f;

    public float damage = 20;

    public float magazine_size = 4;

    public float current_magazine;

    public bool can_shoot = true;

    public float reloading_time = 2;
    
    //Bullet Gameobject
    public GameObject BulletPrefab;
    public Transform firePosition;
    
    // Start is called before the first frame update
    void Start()
    {
        current_magazine = magazine_size;
        fireTimer = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireTimer <= fireRate)
        {
            fireTimer += Time.deltaTime;
        }

        if (can_shoot && (current_magazine <= 0 || Input.GetKeyDown(KeyCode.R)))
        {
            can_shoot = false;
            Debug.Log("rechargement");
            Invoke(nameof(reload), reloading_time);
        }

        if (Input.GetButtonDown("Fire1") && fireTimer >= fireRate && can_shoot && current_magazine > 0)
        {
            FPS_Camera.GetComponent<AudioSource>().clip = FPS_Camera.GetComponent<Sons>().shotgun_sound;
            FPS_Camera.GetComponent<AudioSource>().Play();

            Debug.Log("tir de fusil à pompe");
            
            fireTimer = 0.0f;

            current_magazine -= 1;

            RaycastHit _hit;
            Ray ray = FPS_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            
            //*Bullet system*
            if (!photonView.IsMine)
            {
                return;
            }
            photonView.RPC("Fire",RpcTarget.AllBuffered,firePosition.position);
            
            if (Physics.Raycast(ray, out _hit, 500))
            {
                if (_hit.collider.gameObject.CompareTag("PlayerRed") &&!_hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("blue"))
                    {
                        Vector3 target_position = _hit.collider.transform.position;
                        Vector3 position = transform.position;
                        Vector3 knockback_vector = (target_position - position) / Vector3.Distance(target_position, position);
                        knockback_vector *= knockback;
                        knockback_vector += new Vector3(0, 10f, 0);
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakingDamage", RpcTarget.AllBuffered, damage);
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("Knockback_shotgun", RpcTarget.AllBuffered, knockback_vector);
                    }
                }
                
                if (_hit.collider.gameObject.CompareTag("PlayerBlue") &&!_hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("red"))
                    {
                        Vector3 target_position = _hit.collider.transform.position;
                        Vector3 position = transform.position;
                        Vector3 knockback_vector = (target_position - position) / Vector3.Distance(target_position, position);
                        knockback_vector *= knockback;
                        knockback_vector += new Vector3(0, 10f, 0);
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakingDamage", RpcTarget.AllBuffered, damage);
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("Knockback_shotgun", RpcTarget.AllBuffered, knockback_vector);
                    }
                }
            }
        }
    }

    [PunRPC]
    void Knockback_shotgun(Vector3 knockback_vector)
    {
        this.GetComponent<PlayerMovevement>().controller.Move((knockback_vector * Time.deltaTime));
    }

    public void reload()
    {
        current_magazine = magazine_size;
        can_shoot = true;
    }
    
    
}
