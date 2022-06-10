using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveFlag : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject flag;
    public GameObject flag2;
    public GameObject flag3;
    
    [Header("Composant drapeau")]
    public GameObject Drapeau;
    public GameObject Socleinf;
    public GameObject Soclesup;
    public GameObject Tube;
    
    public GameObject Drapeau2;
    public GameObject Socleinf2;
    public GameObject Soclesup2;
    public GameObject Tube2;
    
    public GameObject Drapeau3;
    public GameObject Socleinf3;
    public GameObject Soclesup3;
    public GameObject Tube3;
    

    // Update is called once per frame
    private void Start()
    {
        Drapeau.SetActive(false);
        Socleinf.SetActive(false);
        Soclesup.SetActive(false);
        Tube.SetActive(false);
        
        Drapeau2.SetActive(false);
        Socleinf2.SetActive(false);
        Soclesup2.SetActive(false);
        Tube2.SetActive(false);
        
        Drapeau3.SetActive(false);
        Socleinf3.SetActive(false);
        Soclesup3.SetActive(false);
        Tube3.SetActive(false);
    }

    void Update()
    {
        if (Decompte.isActive)
        {
            Drapeau.SetActive(true);
            Socleinf.SetActive(true);
            Soclesup.SetActive(true);
            Tube.SetActive(true); 
        }
        else
        {
            Drapeau.SetActive(false);
            Socleinf.SetActive(false);
            Soclesup.SetActive(false);
            Tube.SetActive(false); 
        }
        
        if (Decompte.isActive2)
        {
            Drapeau2.SetActive(true);
            Socleinf2.SetActive(true);
            Soclesup2.SetActive(true);
            Tube2.SetActive(true); 
        }
        else
        {
            Drapeau2.SetActive(false);
            Socleinf2.SetActive(false);
            Soclesup2.SetActive(false);
            Tube2.SetActive(false); 
        }
        
        if (Decompte.isActive3)
        {
            Drapeau3.SetActive(true);
            Socleinf3.SetActive(true);
            Soclesup3.SetActive(true);
            Tube3.SetActive(true); 
        }
        else
        {
            Drapeau3.SetActive(false);
            Socleinf3.SetActive(false);
            Soclesup3.SetActive(false);
            Tube3.SetActive(false); 
        }
        
    }



}
