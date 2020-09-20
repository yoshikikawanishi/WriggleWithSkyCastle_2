using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkinaShoot : MonoBehaviour {

    [SerializeField] private GameObject kunai_Shoot1_Obj;
    [SerializeField] private ShootSystem blue_Laser_Left;
    [SerializeField] private ShootSystem blue_Laser_Right;
    [SerializeField] private ShootSystem black_Blue_Shoot;


    public void Shoot_Kuani_Shoot1() {
        ShootSystem[] shoots = kunai_Shoot1_Obj.GetComponentsInChildren<ShootSystem>();
        foreach(var s in shoots) {
            s.Shoot();
        }
    }

    public void Stop_Kunai_Shoot1() {
        ShootSystem[] shoots = kunai_Shoot1_Obj.GetComponentsInChildren<ShootSystem>();
        foreach (var s in shoots) {
            s.Stop_Shoot();
        }
    }


    public void Shoot_Blue_Laser_Left() {
        blue_Laser_Left.Shoot();
    }

    public void Shoot_Blue_Laser_Right() {
        blue_Laser_Right.Shoot();
    }
    

    public void Shoot_Black_Blue_Bullet() {
        black_Blue_Shoot.Shoot();
    }

    public void Stop_Black_Blue_Shoot() {
        black_Blue_Shoot.Stop_Shoot();
    }
}
