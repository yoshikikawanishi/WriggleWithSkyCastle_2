using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarumiShoot : MonoBehaviour {

    [SerializeField] private GameObject snow_Shoot_Obj;


	public void Shoot_Snow_Shoot() {
        ShootSystem[] shoots = snow_Shoot_Obj.GetComponentsInChildren<ShootSystem>();
        for(int i = 0;  i < shoots.Length; i++) {
            shoots[i].Shoot();
        }
    }
}
