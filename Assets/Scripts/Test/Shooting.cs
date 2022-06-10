using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SocialPlatforms;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Camera FPS_Camera;

    public float fireRate = 0.1f;
    private float fireTimer;

    
    public float knockback = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1") && fireTimer > fireRate)
        {

            fireTimer = 0.0f;

            RaycastHit _hit;
            Ray ray = FPS_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out _hit, 500))
            {
                Debug.Log(_hit.collider.gameObject.name);
                
                if (_hit.collider.gameObject.CompareTag("PlayerRed") &&!_hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("blue"))
                    {
                        Vector3 target_position = _hit.collider.transform.position;
                        Vector3 position = transform.position;
                        Vector3 knockback_vector = (target_position - position + new Vector3(0, 5f, 0)) /
                                                   Vector3.Distance(target_position, position);
                        knockback_vector *= knockback;
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakingDamage", RpcTarget.AllBuffered, 10f);
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("Knockback", RpcTarget.AllBuffered,
                            target_position - position, knockback_vector);
                    }

                }
                if (_hit.collider.gameObject.CompareTag("PlayerBlue") &&!_hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("red"))
                    {
                        Vector3 target_position = _hit.collider.transform.position;
                        Vector3 position = transform.position;
                        Vector3 knockback_vector = (target_position - position + new Vector3(0, 5f, 0)) /
                                                   Vector3.Distance(target_position, position);
                        knockback_vector *= knockback;
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakingDamage", RpcTarget.AllBuffered, 10f);
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("Knockback", RpcTarget.AllBuffered,
                            target_position - position, knockback_vector);
                    }
                }
                
            }
        }
    }

    [PunRPC]
    void Knockback(Vector3 knockback_vector)
    {
        this.GetComponent<PlayerMovevement>().controller.Move((knockback_vector * Time.deltaTime));
    }

}
