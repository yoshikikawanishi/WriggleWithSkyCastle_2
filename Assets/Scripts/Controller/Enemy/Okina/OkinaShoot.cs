using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkinaShoot : MonoBehaviour {

    [SerializeField] private GameObject kunai_Shoot1_Obj;
    [SerializeField] private ShootSystem blue_Laser_Left;
    [SerializeField] private ShootSystem blue_Laser_Right;
    [SerializeField] private ShootSystem black_Blue_Shoot;
    [SerializeField] private GameObject blue_Pillar_Shoot_Prefab;
    [SerializeField] private GameObject red_Shoot_Obj;


    public void Shoot_Kuani_Shoot1(Vector2 position) {
        kunai_Shoot1_Obj.transform.position = position;
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

   
    public void Shoot_Blue_Pillar(float pos_X) {
        GameObject obj = Instantiate(blue_Pillar_Shoot_Prefab);
        obj.transform.position = new Vector3(pos_X, 0, 0);
        Destroy(obj, 4.0f);
    }


    public void Shoot_Red_Bullet() {
        ShootSystem[] shoots = red_Shoot_Obj.GetComponentsInChildren<ShootSystem>();
        foreach(var s in shoots) {
            s.Shoot();
        }
    }

    public void Stop_Red_Shoot() {
        ShootSystem[] shoots = red_Shoot_Obj.GetComponentsInChildren<ShootSystem>();
        foreach (var s in shoots) {
            s.Stop_Shoot();
        }
    }
}
