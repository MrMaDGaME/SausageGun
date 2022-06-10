using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureTheFlag3 : MonoBehaviour
{
    public float capturing3 = 0;

    public static bool captured3 = false;
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
        if (!captured3)
        {
            capturing3 += (1 * Time.deltaTime)/1;
            Debug.Log("Capturing the flag:" + capturing3 + "sec");
            if (capturing3 >= 5f)
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
                captured3 = true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Plu de collision avec:" + other.gameObject.name);
        capturing3 = 0f;
    }
}
