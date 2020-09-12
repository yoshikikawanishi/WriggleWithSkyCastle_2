using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatoMaiShoot : MonoBehaviour {

    [SerializeField] private GameObject rolling_Rushing_Shoot_Obj1;
    [SerializeField] private GameObject rolling_Rushing_Shoot_Obj2;
    [SerializeField] private GameObject phase1_Laser_Shoot_Obj;
    [SerializeField] private GameObject phase1_Talisman_Shoot_Obj;
    [SerializeField] private GameObject phase2_Laser_Shoot_Obj;
    [SerializeField] private GameObject phase2_Yinball_Shoot_Obj;

    //======================================================================
    public void Shoot_In_Rolling_Rushing(bool is_Strong_Shoot) {
        ShootSystem[] shoots;
        if (is_Strong_Shoot)
             shoots = rolling_Rushing_Shoot_Obj1.GetComponents<ShootSystem>();
        else
            shoots = rolling_Rushing_Shoot_Obj2.GetComponents<ShootSystem>();

        foreach (var s in shoots) {
            s.Shoot();
        }
    }


    public void Stop_Rolling_Rushing_Shoot() {
        ShootSystem[] shoots = rolling_Rushing_Shoot_Obj1.GetComponents<ShootSystem>();
        foreach (var s in shoots) {
            s.Stop_Shoot();
        }
        shoots = rolling_Rushing_Shoot_Obj2.GetComponents<ShootSystem>();
        foreach (var s in shoots) {
            s.Stop_Shoot();
        }
    }
    //======================================================================
    public void Shoot_Phase1_Laser() {
        ShootSystem[] shoots = phase1_Laser_Shoot_Obj.GetComponents<ShootSystem>();        
        foreach(var s in shoots) {
            s.center_Angle_Deg = Random.Range(170f, 190f);
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
    public void Shoot_Phase2_Laser() {
        ShootSystem[] shoots = phase2_Laser_Shoot_Obj.GetComponents<ShootSystem>();        
        foreach (var s in shoots) {
            s.center_Angle_Deg += 20f;
            s.Shoot();
        }
    }
    //======================================================================
    public void Shoot_Phase2_Yinball_Bullet(bool is_Right_Rotation) {
        ShootSystem[] shoots = phase2_Yinball_Shoot_Obj.GetComponents<ShootSystem>();
        if (is_Right_Rotation)
            shoots[0].Shoot();
        else
            shoots[1].Shoot();
    }


    public void Delete_Yinball_Bullet() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyTag");
        foreach (var e in enemies) {
            if (e.GetComponent<SatoMaiYinball>() == null)
                continue;
            e.SetActive(false);
        }
    }
}
