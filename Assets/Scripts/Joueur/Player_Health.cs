using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_Health : MonoBehaviour
{

    public int HP;

    public TMP_Text HealthPrint;
    // Start is called before the first frame update
    
    void Start()
    {
        HealthPrint.text = HP.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        HealthPrint.text = HP.ToString();
    }
    
}
