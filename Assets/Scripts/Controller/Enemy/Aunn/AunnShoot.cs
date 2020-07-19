using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AunnShoot : MonoBehaviour {

    [SerializeField] private ShootSystem short_Curve_Laser;
    [SerializeField] private GameObject purple_Rice_Shoot_Obj;
    [SerializeField] private ShootSystem dog_Bullet_Shoot;
    [SerializeField] private ShootSystem dog_Bullet_Big_Shoot;
    [SerializeField] private ShootSystem aunn_Word_Shoot;
    [SerializeField] private GameObject long_Curve_Laser_Shoot_Obj;


    //------------------------ジャンプショット用 短レーザーショット-------------------------
    public void Shoot_Short_Curve_Laser(float span) {
        StartCoroutine("Shoot_Short_Curve_Laser_Cor", span);

    }

    private IEnumerator Shoot_Short_Curve_Laser_Cor(float span) {
        for (int i = 0; i < 4; i++) {
            short_Curve_Laser.center_Angle_Deg += 6;
            short_Curve_Laser.Shoot();
            UsualSoundManager.Instance.Play_Laser_Sound();
            UsualSoundManager.Instance.Invoke("Play_Shoot_Sound", 1.2f);
            yield return new WaitForSeconds(span);
        }
    }

    public void Stop_Short_Curve_Laser() {
        StopCoroutine("Shoot_Short_Curve_Laser_Cor");
    }


    //-------------------------突進時の弾配置用-----------------------------------------------
    private List<GameObject> deposit_Objs = new List<GameObject>();    

    public void Start_Deposite_Purple_Bullet() {
        StartCoroutine("Deposit_Purple_Bullet_Cor");
    }

    public void Stop_Deposit_Purple_Bullet() {
        StopCoroutine("Deposit_Purple_Bullet_Cor");
        foreach(var obj in deposit_Objs) {
            obj.SetActive(true);
            Destroy(obj, 10.0f);
        }
        deposit_Objs.Clear();
    }

    private IEnumerator Deposit_Purple_Bullet_Cor() {        
        while (true) {
            var shoot_Obj = Instantiate(purple_Rice_Shoot_Obj);
            shoot_Obj.transform.position = transform.position;            
            deposit_Objs.Add(shoot_Obj);
            yield return new WaitForSeconds(0.165f);
        }
    }


    //-------------------------------------山犬の散歩用-------------------------------------
    public void Shoot_Dog_Bullet() {
        dog_Bullet_Shoot.Shoot();
    }


    public void Shoot_Dog_Bullet_Big() {
        dog_Bullet_Big_Shoot.Shoot();
    }


    public void Stop_Dog_Bullet() {
        dog_Bullet_Shoot.Stop_Shoot();
        dog_Bullet_Big_Shoot.Stop_Shoot();
    }


    //------------------------------------フェーズチェンジ時の----------------------------
    public void Shoot_Aunn_Word_Bullet() {
        aunn_Word_Shoot.Shoot();
    }

    //----------------------------------フェーズ２---------------------------------------
    public void Shoot_Long_Curve_Laser() {
        long_Curve_Laser_Shoot_Obj.GetComponent<ShootSystem>().Shoot();
        long_Curve_Laser_Shoot_Obj.transform.GetChild(0).GetComponent<ShootSystem>().Shoot();        
    }

    public void Stop_Long_Curve_Laser() {
        long_Curve_Laser_Shoot_Obj.GetComponent<ShootSystem>().Stop_Shoot();
        long_Curve_Laser_Shoot_Obj.transform.GetChild(0).GetComponent<ShootSystem>().Stop_Shoot();
    }
}
