using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad_script : MonoBehaviour
{
    
    public bool isOnJumpPad = false;
    private void Start()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        throw new NotImplementedException();
        if (isOnJumpPad)
        {
            //JumpPad();
            //Invoke(nameof(ReturnToNormal), 4);
        }
    }

    private void OnCollisionEnter(Collision other)
     {
         GameObject Pad = other.gameObject;
         Rigidbody rb = Pad.GetComponent<Rigidbody>();
         rb.AddForce((Vector3.up*500f));
          
     }
    
    private void OnTriggerEnter(Collider collisionInfo)
    {
        Debug.Log(collisionInfo.gameObject.CompareTag("JumpPad") + "/" + collisionInfo.name);
        if (collisionInfo.gameObject.CompareTag("JumpPad"))
        {
            isOnJumpPad = true;
            
        }
    }
    

    /*void JumpPad()
    {
        Saut = 80f;
        gravity = 80f;
    }

    void ReturnToNormal()
    {
        Saut = 8f;
        gravity = 20f;
        isOnJumpPad = false;
    }*/
}
