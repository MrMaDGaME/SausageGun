using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animController : MonoBehaviour
{
    // Start is called before the first frame update


    public Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.S))
        {
            anim.enabled = true;
            anim.Play("Run");
        }

        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.S))
        {
            anim.enabled = false;
        }
    }
}
