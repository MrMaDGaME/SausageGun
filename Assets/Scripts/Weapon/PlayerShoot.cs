using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerShoot : MonoBehaviourPunCallbacks
{
    public arme_test arme;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;
    
    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("no camera on player");
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    
    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 500f, mask))
        {
            Debug.Log("objet touché" + hit.collider.name);
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("damage : " + arme_test.damage);
            }
        }
    }
}
