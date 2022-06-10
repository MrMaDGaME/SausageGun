using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovevement : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterController controller;
    public float speed = 10f;
    public float gravity = 20f;
    Vector3 MoveDirection = Vector3.zero;
    public float Saut = 8f;// donne la hauteur atteinte du saut
    public bool isOnJumpPad = false;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded)
        {
            MoveDirection = - new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            MoveDirection = transform.TransformDirection(MoveDirection);
            MoveDirection *= speed;
            
            if (Input.GetButton("Jump"))
            {
                MoveDirection.y = Saut;
            }
        }
        
        if (isOnJumpPad)
        {
            JumpPad();
            ReturnToNormal();
        }

        MoveDirection.y -= gravity * Time.deltaTime;
        
        controller.Move( MoveDirection * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("JumpPad"))
        {
            Debug.Log("Hit a JumpPad");
            isOnJumpPad = true;
        }
    }
    
    
    
    void JumpPad()
    {
        MoveDirection.y = 37f;
        gravity = 275f;
        
    }

    void ReturnToNormal()
    {
        gravity = 20f;
        isOnJumpPad = false;
    }
}
