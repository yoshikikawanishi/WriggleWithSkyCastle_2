using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSunFlowerFairyBullet : MonoBehaviour {

    [SerializeField] private GameObject main_Purple_Bullet;

    private ShootSystem _shoot;

    private bool is_Shooting = false;   //Updata内で１度だけ呼ぶよう、enableをfalseにするとObjectPoolのあれこれでよくない


    private void Awake() {
        _shoot = GetComponent<ShootSystem>();
    }

    private void OnEnable() {
        main_Purple_Bullet.SetActive(true);
        StartCoroutine("Shoot_Cor");
        is_Shooting = true;
    }    


    private void Update() {
        if (!main_Purple_Bullet.activeSelf && is_Shooting) {
            _shoot.Stop_Shoot();
            is_Shooting = false;
        }
    }

    private IEnumerator Shoot_Cor() {
        yield return null;        
        _shoot.center_Angle_Deg = transform.parent.localEulerAngles.z;
        _shoot.Shoot();
    }

}
