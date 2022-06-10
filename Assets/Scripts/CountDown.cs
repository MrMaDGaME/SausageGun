using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CountDown : MonoBehaviour
{
    public float SecondeCurrentTime;
    public float MinuteCurrentTime;
    public float startingMinuteTime = 0f;
    public float startingSecondeTime = 10f;
    
    
    [SerializeField] public TMP_Text SecondeText;
    [SerializeField] public TMP_Text MinuteText;

    
    public bool StillGood = true;

    public static CountDown instance2;
    private void Awake()
    {
        if (instance2 == null)
        {
            instance2 = this;
        }

        else if (instance2 != this)
        {
            Destroy(gameObject);
        }
    }
}
