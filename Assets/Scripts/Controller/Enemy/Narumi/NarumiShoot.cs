using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarumiShoot : MonoBehaviour {

    [SerializeField] private GameObject snow_Shoot_Obj;
    [SerializeField] private ShootSystem yellow_Talisman_Shoot;
    [SerializeField] private ShootSystem yellow_Talisman_Shoot_Strong;


    public void Shoot_Snow_Shoot() {
        ShootSystem[] shoots = snow_Shoot_Obj.GetComponentsInChildren<ShootSystem>();
        for(int i = 0;  i < shoots.Length; i++) {
            shoots[i].Shoot();
        }
    }


    public void Shoot_Yellow_Talisman_Shoot() {
        yellow_Talisman_Shoot.center_Angle_Deg = Random.Range(0, 10f);
        yellow_Talisman_Shoot.Shoot();
    }


    public void Shoot_Yellow_Talisman_Shoot_Strong() {
        yellow_Talisman_Shoot_Strong.center_Angle_Deg = Random.Range(0, 10f);
        yellow_Talisman_Shoot_Strong.Shoot();
    }
}
