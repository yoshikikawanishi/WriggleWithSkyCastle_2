using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EternalShoot : MonoBehaviour {

    [SerializeField] private EternalVineShoot vine_Shoot;
    [SerializeField] private ShootSystem ripples_Shoot;
    [SerializeField] private GameObject master_Spark;
    [SerializeField] private ShootSystem spiral_Shoot_Strong;
    [SerializeField] private ShootSystem spiral_Shoot_Weak;

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
    //================================== Master Spark =======================================

    //================================== Spiral Shoot =======================================
    public void Shoot_Spiral_Shoot_Strong() {
        spiral_Shoot_Strong.Shoot();
    }

    public void Shoot_Spiral_Shoot_Weak() {
        spiral_Shoot_Weak.Shoot();
    }

    public void Stop_Spiral_Shoot_Strong() {
        spiral_Shoot_Strong.Stop_Shoot();
    }

    public void Stop_Spiral_Shoot_Weak() {
        spiral_Shoot_Weak.Stop_Shoot();
    }
}
