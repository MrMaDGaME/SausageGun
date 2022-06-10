using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class shoot_AR : MonoBehaviourPunCallbacks
{
    public Camera FPS_Camera;

    public float fireRate = 1f;
    
    private float fireTimer;

    public float fireRate2 = 0.5f / 3;
    
    public float knockback = 80f;

    public float damage = 10;

    public float magazine_size = 20;

    public float current_magazine;

    public bool can_shoot = true;

    public float reloading_time = 1.3f;

    public bool rafale2 = true;
    public bool rafale3 = true;
    public bool rafale4 = true;
    
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
            fireTimer = 0.0f;
            shoot();
            rafale2 = true;
            rafale3 = true;
            rafale4 = true;
            FPS_Camera.GetComponent<AudioSource>().clip = FPS_Camera.GetComponent<Sons>().AR_sound;
            FPS_Camera.GetComponent<AudioSource>().Play();
        }

        if (fireTimer >= fireRate2 && fireTimer < fireRate2 * 2 && rafale2)
        {
            rafale2 = false;
            shoot();
        }

        if (fireTimer >= fireRate2 * 2 && fireTimer < 0.5f && rafale3)
        {
            rafale3 = false;
            shoot();
        }

        if (fireTimer >= 0.5f && fireTimer < fireRate && rafale4)
        {
            rafale4 = false;
            shoot();
        }
    }

    [PunRPC]
    void Knockback_AR(Vector3 knockback_vector)
    {
        this.GetComponent<PlayerMovevement>().controller.Move((knockback_vector * Time.deltaTime));
    }

    public void shoot()
    {
        Debug.Log("tir de fusil d'assaut");

        current_magazine -= 1;
        
        Debug.Log("munition restantes : " + current_magazine);

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
                    _hit.collider.gameObject.GetComponent<PhotonView>().RPC("Knockback_AR", RpcTarget.AllBuffered, knockback_vector);
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
                    _hit.collider.gameObject.GetComponent<PhotonView>().RPC("Knockback_AR", RpcTarget.AllBuffered, knockback_vector);
                }
            }
        }
    }

    public void reload()
    {
        current_magazine = magazine_size;
        can_shoot = true;
    }
    
    [PunRPC]
    void Fire(Vector3 _firePosition)
    {
        Ray ray = FPS_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        GameObject bulletGameObject = Instantiate(BulletPrefab, firePosition.position, Quaternion.identity);
        bulletGameObject.GetComponent<BulletScript>().Initialize(ray.direction,50f);
    }
}
