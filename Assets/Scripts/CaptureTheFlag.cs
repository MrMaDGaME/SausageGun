using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CaptureTheFlag : MonoBehaviour
{
    public float capturing = 0;

    public static bool captured = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision avec " + other.gameObject.name);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!captured)
        {
            capturing += (1 * Time.deltaTime)/1;
            Debug.Log("Capturing the flag:" + capturing + "sec");
            if (capturing >= 5f)
            {
                Debug.Log(other.gameObject.name + " won");
                if (other.gameObject.CompareTag("PlayerRed"))
                {
                    ScoreCalculate.RedTeamScore++;
                }
                else
                {
                    ScoreCalculate.BlueTeamScore++;
                }
                captured = true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Plu de collision avec:" + other.gameObject.name);
        capturing = 0f;
    }
    
}
