using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaShootObj : MonoBehaviour {    

    //弾
    [SerializeField] private GameObject scales_Bullet;
    [SerializeField] private GameObject green_Rice_Bullet;
    [SerializeField] private GameObject red_Bullet;

    private ObjectPoolManager pool_Manager;

    private GameObject player;

    private ShootFunction _shoot;
    private BulletAccelerator _bullet_Acc;
    

	// Use this for initialization
	void Start () {
        //オブジェクトプール
        pool_Manager = ObjectPoolManager.Instance;
        pool_Manager.Create_New_Pool(scales_Bullet, 10);
        pool_Manager.Create_New_Pool(green_Rice_Bullet, 40);
        pool_Manager.Create_New_Pool(red_Bullet, 2);
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        _shoot = GetComponent<ShootFunction>();
        _bullet_Acc = GetComponent<BulletAccelerator>();
	}


    //鱗粉弾
    public void Shoot_Scales_Bullet(int num, float speed) {
        List<GameObject> bullet_List = new List<GameObject>();
        for (int i = 0; i < num; i++) {
            //弾生成
            GameObject bullet = pool_Manager.Get_Pool(scales_Bullet).GetObject();
            bullet.transform.position = transform.position + new Vector3(-16f * transform.parent.localScale.x, 12f);
            bullet_List.Add(bullet);
            //発射            
            float angle = 2 * Mathf.PI / num * i + Random.Range(0, 0.3f);
            var v = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle) + 0.5f) * speed * Random.Range(0.95f, 1.05f);
            bullet.GetComponent<Rigidbody2D>().velocity = v;
            bullet.GetComponent<Bullet>().Set_Inactive(5.0f);
            UsualSoundManager.Instance.Play_Shoot_Sound();
        }
        _bullet_Acc.Accelerat_Bullet(bullet_List, 0.98f, 0.5f);        
    }


    //フェーズ1緑弾
    public IEnumerator Shoot_Green_Bullet_Cor(int count) {
        for (int i = 0; i < count; i++) {
            //自機の位置を確認
            AngleCalculater angle_Cal = new AngleCalculater();
            float player_Angle = angle_Cal.Cal_Angle_Two_Points(transform.position, player.transform.position);            
            //ショット
            for (int j = 0; j < 4; j++) {
                Shoot_Green_Bullet_4way(j, player_Angle);
                UsualSoundManager.Instance.Play_Shoot_Sound(0.07f);
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitForSeconds(0.3f);
        }

    }

    //つながった緑米弾を4方向に撃つ
    private void Shoot_Green_Bullet_4way(int number, float player_Angle) {        
        float angle = 0;
        //内側
        angle = player_Angle + 100f + number * 23f;
        Shoot_Connect_Green_Bullet(140f, angle);
        angle = player_Angle + -100f - number * 23f;
        Shoot_Connect_Green_Bullet(140f, angle);
        //外側
        angle = player_Angle + 50f + number * 30f;
        Shoot_Connect_Green_Bullet(140f, angle);
        angle = player_Angle + -50f - number * 30f;
        Shoot_Connect_Green_Bullet(140f, angle);
    }

    //緑米弾4発つなげてうつ
    private void Shoot_Connect_Green_Bullet(float speed, float angle) {

        List<GameObject> bullet_List = new List<GameObject>();
        _shoot.Set_Bullet_Pool(pool_Manager.Get_Pool(green_Rice_Bullet), null);   
        
        for (int i = 0; i < 4; i++) {
            GameObject b = _shoot.Turn_Shoot_Bullet(speed - i * 5, angle, 7.0f);
            bullet_List.Add(b);
        }
        //減速
        _bullet_Acc.Accelerat_Bullet(bullet_List, 0.97f, 0.3f);
    }


    //フェーズ1赤弾
    public void Shoot_Red_Bullet() {
        _shoot.Set_Bullet_Pool(pool_Manager.Get_Pool(red_Bullet), null);
        List<GameObject> bullet_List = new List<GameObject>();
        bullet_List = _shoot.Odd_Num_Shoot(1, 0, 60f, 10);
        _bullet_Acc.Accelerat_Bullet(bullet_List, 1.01f, 4);
    }
	

    //フェーズ2全方位弾
    public void Shoot_Dif_Bullet() {
        UsualSoundManager.Instance.Play_Shoot_Sound();        
        GetComponent<ShootSystem>().Shoot();
    }

}
