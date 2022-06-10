using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class melee : MonoBehaviourPunCallbacks
{
    public Camera FPS_Camera;

    public float fireRate = 1f;
    
    private float fireTimer;
    
    public float knockback = 300f;

    public float damage = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<CollectItems>().fork && damage != 100)
        {
            damage = 100;
        }
        
        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.V) && fireTimer > fireRate)
        {

            Debug.Log("corps à corps");
            
            fireTimer = 0.0f;

            RaycastHit _hit;
            Ray ray = FPS_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out _hit, 3))
            {
                if (_hit.collider.gameObject.CompareTag("PlayerRed") && !_hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("blue"))
                    {
                        Vector3 target_position = _hit.collider.transform.position;
                        Vector3 position = transform.position;
                        Vector3 knockback_vector = (target_position - position) / Vector3.Distance(target_position, position);
                        knockback_vector *= knockback;
                        knockback_vector += new Vector3(0, 10f, 0);
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakingDamage", RpcTarget.AllBuffered, damage);
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("Knockback_cac", RpcTarget.AllBuffered, knockback_vector);
                    }
                }
                
                if (_hit.collider.gameObject.CompareTag("PlayerBlue") && !_hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsValue("red"))
                    {
                        Vector3 target_position = _hit.collider.transform.position;
                        Vector3 position = transform.position;
                        Vector3 knockback_vector = (target_position - position) / Vector3.Distance(target_position, position);
                        knockback_vector *= knockback;
                        knockback_vector += new Vector3(0, 10f, 0);
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakingDamage", RpcTarget.AllBuffered, damage);
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("Knockback_cac", RpcTarget.AllBuffered, knockback_vector);
                    }
                }
            }
        }
    }

    [PunRPC]
    void Knockback_cac(Vector3 knockback_vector)
    {
        this.GetComponent<PlayerMovevement>().controller.Move((knockback_vector * Time.deltaTime));
    }
}
