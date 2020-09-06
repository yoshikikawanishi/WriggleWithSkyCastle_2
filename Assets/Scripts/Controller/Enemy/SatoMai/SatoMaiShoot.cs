using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatoMaiShoot : MonoBehaviour {

    [SerializeField] private GameObject rolling_Rushing_Shoot_Obj;
    [SerializeField] private GameObject phase1_Laser_Shoot_Obj;
    [SerializeField] private GameObject phase1_Talisman_Shoot_Obj;

    //======================================================================
    public void Shoot_In_Rolling_Rushing() {
        ShootSystem[] shoots = rolling_Rushing_Shoot_Obj.GetComponents<ShootSystem>();
        foreach(var s in shoots) {
            s.Shoot();
        }
    }


    public void Stop_Rolling_Rushing_Shoot() {
        ShootSystem[] shoots = rolling_Rushing_Shoot_Obj.GetComponents<ShootSystem>();
        foreach (var s in shoots) {
            s.Stop_Shoot();
        }
    }
    //======================================================================
    public void Shoot_Phase1_Laser() {
        ShootSystem[] shoots = phase1_Laser_Shoot_Obj.GetComponents<ShootSystem>();
        float center_Angle = Random.Range(0, 90f);
        foreach(var s in shoots) {
            s.center_Angle_Deg = center_Angle;
            s.Shoot();
        }
    }
    //======================================================================
    public void Shoot_Phase1_Talisman_Bullet() {
        ShootSystem[] shoots = phase1_Talisman_Shoot_Obj.GetComponents<ShootSystem>();
        foreach (var s in shoots) {
            s.Shoot();
        }
    }


    public void Stop_Phase1_Talisman_Shoot() {
        ShootSystem[] shoots = phase1_Talisman_Shoot_Obj.GetComponents<ShootSystem>();
        foreach (var s in shoots) {
            s.Stop_Shoot();
        }
    }
    //======================================================================
}
