using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenYinBall : MonoBehaviour {

    //コンポーネント
    private GravitatePlayer _gravitate_Player;
    private ShootFunction _shoot;

    //弾
    private ObjectPool bullet_Pool;


    private void Awake() {
        //取得
        _gravitate_Player = GetComponent<GravitatePlayer>();
        _shoot = GetComponent<ShootFunction>();
    }


    // Use this for initialization
    void Start () {
        //弾のオブジェクトプール
        GameObject bullet = Resources.Load("Bullet/PurpleBullet") as GameObject;
        bullet_Pool = ObjectPoolManager.Instance.Get_Pool(bullet);        
	}


    private void OnBecameVisible() {
        StartCoroutine("Shoot_Bullet");    
    }

    private void OnBecameInvisible() {
        StopCoroutine("Shoot_Bullet");
    }


    private IEnumerator Shoot_Bullet() {
        while (true) {
            _gravitate_Player.enabled = true;
            yield return new WaitForSeconds(1.0f);

            //予備動作
            StartCoroutine("Blink");
            _gravitate_Player.enabled = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

            yield return new WaitForSeconds(1.2f);

            //ショット
            _shoot.Set_Bullet_Pool(bullet_Pool, null);
            _shoot.Odd_Num_Shoot(1, 0, 50f, 10);
            UsualSoundManager.Instance.Play_Shoot_Sound();

            yield return new WaitForSeconds(0.8f);

            _gravitate_Player.enabled = true;

            yield return new WaitForSeconds(4.0f);
        }
    }


    //点滅
    private IEnumerator Blink() {
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        for(int i = 0; i < 4; i++) {
            _sprite.color = new Color(0.7f, 0.7f, 0.7f, 1);
            yield return new WaitForSeconds(0.1f);
            _sprite.color = new Color(0.5f, 0.5f, 0.5f, 1);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
