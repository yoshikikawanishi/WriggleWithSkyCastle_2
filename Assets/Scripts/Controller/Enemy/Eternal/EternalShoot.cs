using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EternalShoot : MonoBehaviour {

    [SerializeField] private EternalVineShoot vine_Shoot;
    [SerializeField] private ShootSystem ripples_Shoot;


    //================================ Vine Shoot =========================================
    public void Shoot_Vine_Shoot(int divide_Count) {
        vine_Shoot.Shoot_Vine_Shoot(divide_Count);
    }

    public void Stop_Vine_Shoot() {
        vine_Shoot.Stop_Vine_Shoot();
    }

    //================================= Ripples Shoot ======================================
    public void Shoot_Ripples_Shoot(int num) {
        ripples_Shoot.num = num;
        ripples_Shoot.inter_Angle_Deg = 360f / num;
        ripples_Shoot.Shoot();
    }
    
}
