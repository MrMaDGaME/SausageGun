using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviourPunCallbacks
{
    public enum WeaponsType
    {
        NONE, ASSAULT_RIFLE, SHOTGUN, SUBMACHINE_GUN, SNIPER, HANDGUN, FORK
    }

    //Renvoie une arme aléatoire
    public static WeaponsType GetRandomWeapon()
    {
        Array values = Enum.GetValues(typeof(WeaponsType));
        int random_index = Random.Range(1, values.Length);
        return (WeaponsType) values.GetValue(random_index);
    }

    //Nombre de projectiles tirés automatiquement avant de devoir relacher le click pour tirer à nouveau
    public int shoot_mode;

    //Nombre de secondes à attendre entre chaque tir
    public float rate;
        
    //Force de recul sur l'ennemi
    public int knockback;
        
    //Taille des chargeurs
    public int magazine_size;
        
    //Dégats par balles
    public int damage;
        
    //Dispersion des balles : Angle entre la ligne de vue du joueur et l'extremité du cône de dispersion
    public float dispersion;
        
    //Probabilité d'apparition sur la carte
    public float spawn_probability;
        
    //Angle du recul vers le haut
    public float recoil;
        
    //Nobre de secondes entre l'appui de la touche de rechargement et le remplissage du chargeur
    public float reloading_time;
        
    //Nombre de balles actuellement dans le chargeur
    public int current_magazine;
        
    //Nombre de balles en reserve : Taille du chargeur*4
    public int reserve;

    //Type de l'arme
    public WeaponsType type;

    //Représentation visuelle de l'arme
    public GameObject Weapon_Model;

    //Constructeur de classe
    public Weapon(WeaponsType type)
        {
            if (type != WeaponsType.NONE)
            {
                switch (type)
                {
                    case WeaponsType.ASSAULT_RIFLE:
                        shoot_mode = 4;
                        rate = 1;
                        //knockback =
                        magazine_size = 20;
                        damage = 10;
                        //dispersion =
                        spawn_probability = 0.3f;
                        //recoil =
                        reloading_time = 1.3f;
                        break;
                    
                    case WeaponsType.SHOTGUN:
                        shoot_mode = 1;
                        rate = 1.75f;
                        //knockback =
                        magazine_size = 4;
                        damage = 1;
                        //dispersion =
                        spawn_probability = 0.05f;
                        //recoil =
                        reloading_time = 1;
                        break;
                    
                    case WeaponsType.SUBMACHINE_GUN:
                        shoot_mode = 40;
                        rate = 0.1f;
                        //knockback =
                        magazine_size = 40;
                        damage = 1;
                        //dispersion =
                        spawn_probability = 0.3f;
                        //recoil =
                        reloading_time = 1;
                        break;
                    
                    case WeaponsType.SNIPER:
                        shoot_mode = 1;
                        rate = 2.5f;
                        //knockback =
                        magazine_size = 5;
                        damage = 70;
                        //dispersion =
                        spawn_probability = 0.2f;
                        //recoil =
                        reloading_time = 4;
                        break;
                    
                    case WeaponsType.HANDGUN:
                        shoot_mode = 1;
                        rate = 0.5f;
                        //knockback =
                        magazine_size = 8;
                        damage = 10;
                        //dispersion =
                        spawn_probability = 0.1f;
                        //recoil =
                        reloading_time = 1;
                        break;
                    
                }
                
                current_magazine = magazine_size;
                reserve = magazine_size * 4;
            }
            
            this.type = type;
        }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
