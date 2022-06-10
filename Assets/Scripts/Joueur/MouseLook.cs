using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Start is called before the first frame update
    public float sensi = 150f;
    public Transform Corps;
    private float RotationX = 0f; // permet un déplacement de la caméra en X
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensi * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensi * Time.deltaTime;

        RotationX -= mouseY;
        RotationX = Mathf.Clamp(RotationX, -90, 90);// Borne de déplacements de la caméra
        
        transform.localRotation = Quaternion.Euler(RotationX,180,0f);// permet de rotate sur X
        Corps.Rotate(Vector3.up*mouseX);
        
        
    }
}
