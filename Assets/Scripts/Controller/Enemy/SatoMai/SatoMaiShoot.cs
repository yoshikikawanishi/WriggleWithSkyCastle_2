using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatoMaiShoot : MonoBehaviour {

    [SerializeField] private GameObject rolling_Rushing_Shoot_Obj1;
    [SerializeField] private GameObject rolling_Rushing_Shoot_Obj2;
    [SerializeField] private GameObject phase1_Laser_Shoot_Obj;
    [SerializeField] private GameObject phase1_Talisman_Shoot_Obj;
    [SerializeField] private GameObject phase2_Laser_Shoot_Obj;    

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
    public void Shoot_Phase2_Laser_Pink(float height) {
        GameObject obj = phase2_Laser_Shoot_Obj.transform.GetChild(0).gameObject;
        obj.transform.position = new Vector3(260f, height, 0);     
        obj.GetComponent<ShootSystem>().Shoot();
    }

    public void Shoot_Phase2_Laser_Green(float height) {
        GameObject obj = phase2_Laser_Shoot_Obj.transform.GetChild(1).gameObject;
        obj.transform.position = new Vector3(260f, height, 0);
        obj.GetComponent<ShootSystem>().Shoot();
    }
    //======================================================================
   
}
