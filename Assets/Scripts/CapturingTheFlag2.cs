using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturingTheFlag2 : MonoBehaviour
{
    public float capturing2 = 0;

    public static bool captured2 = false;
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision avec " + other.gameObject.name);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!captured2)
        {
            capturing2 += (1 * Time.deltaTime)/1;
            Debug.Log("Capturing the flag:" + capturing2 + "sec");
            if (capturing2 >= 5f)
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
                captured2 = true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Plu de collision avec:" + other.gameObject.name);
        capturing2 = 0f;
    }
}
