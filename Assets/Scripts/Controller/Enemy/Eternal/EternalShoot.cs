using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EternalShoot : MonoBehaviour {

    [SerializeField] private EternalVineShoot vine_Shoot;
    [SerializeField] private ShootSystem ripples_Shoot;
    [SerializeField] private GameObject master_Spark_Prefab;
    [SerializeField] private ShootSystem spiral_Shoot_Strong;
    [SerializeField] private ShootSystem spiral_Shoot_Weak;
    [SerializeField] private ShootSystem shoot_With_Spiral_Shoot;
    [SerializeField] private GameObject wing_Shoot_Obj;
    [SerializeField] private GameObject wing_Shoot_Reverse_Obj;
    [SerializeField] private ShootSystem shoot_With_Wing_Shoot;
    [SerializeField] private ShootSystem beetle_Power_Shoot;

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
    public void Shoot_Master_Spark(float height) {
        GameObject obj = Instantiate(master_Spark_Prefab);
        obj.transform.position = new Vector3(260f, height);       
    }
    //================================== Spiral Shoot =======================================
    public void Shoot_Spiral_Shoot_Strong() {
        spiral_Shoot_Strong.Shoot();
        shoot_With_Spiral_Shoot.Shoot();
    }

    public void Shoot_Spiral_Shoot_Weak() {
        spiral_Shoot_Weak.Shoot();
    }

    public void Stop_Spiral_Shoot_Strong() {
        spiral_Shoot_Strong.Stop_Shoot();
        shoot_With_Spiral_Shoot.Stop_Shoot();
    }

    public void Stop_Spiral_Shoot_Weak() {
        spiral_Shoot_Weak.Stop_Shoot();
    }
    //=================================== Wing Shoot ======================================
    public void Shoot_Wing_Shoot(float span) {
        StartCoroutine("Wing_Shoot_Cor", span);
    }

    private IEnumerator Wing_Shoot_Cor(float span) {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null)
            yield break;
        ShootSystem[] shoots = wing_Shoot_Obj.GetComponents<ShootSystem>();
        AngleCalculater AC = new AngleCalculater();

        while (true) {
            float angle = AC.Cal_Angle_Two_Points(transform.position, player.transform.position);
            shoots[0].center_Angle_Deg = angle - 80f;
            shoots[1].center_Angle_Deg = angle + 80f;
            shoots[0].Shoot();
            shoots[1].Shoot();
            shoot_With_Wing_Shoot.Shoot();
            yield return new WaitForSeconds(span);
        }
    }


    public void Shoot_Reverse_Wing_Shoot(float span) {
        StartCoroutine("Reverse_Wing_Shoot_Cor", span);
    }

    private IEnumerator Reverse_Wing_Shoot_Cor(float span) {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null)
            yield break;
        ShootSystem[] shoots = wing_Shoot_Reverse_Obj.GetComponents<ShootSystem>();
        AngleCalculater AC = new AngleCalculater();

        while (true) {
            float angle = AC.Cal_Angle_Two_Points(transform.position, player.transform.position);
            shoots[0].center_Angle_Deg = angle - 10f;
            shoots[1].center_Angle_Deg = angle + 10f;
            shoots[0].Shoot();
            shoots[1].Shoot();
            yield return new WaitForSeconds(span);
        }
    }

    public void Stop_Wing_Shoot() {
        StopCoroutine("Wing_Shoot_Cor");
        StopCoroutine("Reverse_Wing_Shoot_Cor");
        ShootSystem[] shoots = wing_Shoot_Obj.GetComponents<ShootSystem>();                
        foreach (ShootSystem s in shoots) {
            s.Stop_Shoot();
        }
        ShootSystem[] shoots2 = wing_Shoot_Reverse_Obj.GetComponents<ShootSystem>();
        foreach (ShootSystem s in shoots2) {
            s.Stop_Shoot();
        }
    }

    //===========================================================
    public void Shoot_Beetle_Power() {
        beetle_Power_Shoot.Shoot();
    }
    

}
