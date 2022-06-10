using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    
    public void Initialize(Vector3 direction, float speed)
    {
        transform.forward = direction;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = direction * speed;
    }
}
