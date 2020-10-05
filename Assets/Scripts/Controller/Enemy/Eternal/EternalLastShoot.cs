using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EternalLastShoot : MonoBehaviour {

    [SerializeField] private GameObject wing_Shoot_Obj;
    [SerializeField] private GameObject first_Shoot_Obj;
    [SerializeField] private GameObject second_Shoot_Obj;
    [SerializeField] private GameObject third_Shoot_Obj;
    [SerializeField] private GameObject forth_Shoot_Obj;


    public void Start_First_Shoot() {
        first_Shoot_Obj.SetActive(true);
    }


    public void Start_Second_Shoot() {
        second_Shoot_Obj.SetActive(true);
    }


    public void Start_Third_Shoot() {
        third_Shoot_Obj.SetActive(true);
    }


    public void Start_Forth_Shoot() {
        forth_Shoot_Obj.SetActive(true);
    }


    public void Start_Wing_Shoot() {
        StartCoroutine("Wing_Shoot_Cor");
    }


    public void Stop_Shoot() {
        StopCoroutine("Wing_Shoot_Cor");
        first_Shoot_Obj.SetActive(false);
        second_Shoot_Obj.SetActive(false);
        third_Shoot_Obj.SetActive(false);
        forth_Shoot_Obj.SetActive(false);
    }


    private IEnumerator Wing_Shoot_Cor() {
        //変数取得
        GameObject player = GameObject.FindWithTag("PlayerTag");
        AngleCalculater AC = new AngleCalculater();
        ShootSystem[,] shoots = new ShootSystem[5, 2];
        for (int i = 0; i < 5; i++) {
            shoots[i, 0] = wing_Shoot_Obj.transform.GetChild(i).GetComponents<ShootSystem>()[0];
            shoots[i, 1] = wing_Shoot_Obj.transform.GetChild(i).GetComponents<ShootSystem>()[1];
        }
        //ショット
        while (true) {
            float angle = AC.Cal_Angle_Two_Points(transform.position, player.transform.position);
            for(int i = 0; i < 5; i++) {
                shoots[i, 0].center_Angle_Deg = angle - 80;
                shoots[i, 1].center_Angle_Deg = angle + 80;
                shoots[i, 0].Shoot();
                shoots[i, 1].Shoot();
            }
            yield return new WaitForSeconds(3.4f);
        }
    }
}
