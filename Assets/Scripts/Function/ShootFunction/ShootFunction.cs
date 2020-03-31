using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFunction : MonoBehaviour {

    //フィールド
    private ObjectPool bullet_Pool; //弾
    private Transform parent_Obj;   //弾の親オブジェクト


    //弾のセット
    public void Set_Bullet_Pool(ObjectPool bullet_Pool, Transform parent_Obj) {
        this.bullet_Pool = bullet_Pool;
        this.parent_Obj = parent_Obj;
    }


    //弾がセットされているか
    private bool Is_Set_Pool() {
        if (bullet_Pool == null) {
            Debug.Log("Not Set Pool");
            return false;
        }
        else if (!bullet_Pool.Is_Pooled()) {
            Debug.Log("Not Pool Bullet");
            return false;
        }
        return true;
    }


    /// <summary>
    /// 弾の生成と発射
    /// </summary>
    public GameObject Shoot_Bullet(Vector2 velocity, float lifeTime) {
        if (!Is_Set_Pool()) {
            return null;
        }
        var shoot_Bullet = bullet_Pool.GetObject();             //生成
        shoot_Bullet.transform.SetParent(parent_Obj);           //親オブジェクトの設定
        shoot_Bullet.transform.position = transform.position;   //座標の設定
        shoot_Bullet.GetComponent<Rigidbody2D>().velocity = velocity;   //初速
        if (lifeTime > 0) {
            Delete_Bullet(shoot_Bullet, lifeTime);              //消滅時間
        }
        return shoot_Bullet;
    }


    /// <summary>
    /// 弾の生成、回転と発射
    /// </summary>
    public GameObject Turn_Shoot_Bullet(float speed, float angle_Deg, float lifeTime) {
        float angle_Rad = angle_Deg * Mathf.Deg2Rad;
        if (!Is_Set_Pool()) {
            return null;
        }
        var turn_Bullet = bullet_Pool.GetObject();                  //生成
        turn_Bullet.transform.SetParent(parent_Obj);                //親オブジェクト        
        turn_Bullet.transform.position  = transform.position + new Vector3(Mathf.Cos(angle_Rad), Mathf.Sin(angle_Rad));
        turn_Bullet.transform.LookAt2D(transform, Vector2.left);    //回転
        turn_Bullet.GetComponent<Rigidbody2D>().velocity = turn_Bullet.transform.right * speed;     //初速
        if (lifeTime > 0) {
            Delete_Bullet(turn_Bullet, lifeTime);                   //消滅
        }
        return turn_Bullet;
    }


    /// <summary>
    /// 奇数段
    /// </summary>
    public List<GameObject> Odd_Num_Shoot(int num, float inter_Angle_Deg, float speed, float lifeTime) {
        List<GameObject> bullet_List = new List<GameObject>();
        if (!Is_Set_Pool()) {
            return null;
        }
        int center = num / 2;
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            for (int i = 0; i < num; i++) {
                //弾の生成
                GameObject odd_Bullet = bullet_Pool.GetObject();
                odd_Bullet.transform.SetParent(parent_Obj);
                odd_Bullet.transform.position = transform.position;
                bullet_List.Add(odd_Bullet);
                //弾の方向転換
                odd_Bullet.transform.LookAt2D(player.transform, Vector2.right);
                float r = (i - center) * inter_Angle_Deg;
                odd_Bullet.transform.Rotate(0, 0, r);
                //弾の発射
                odd_Bullet.GetComponent<Rigidbody2D>().velocity = odd_Bullet.transform.right * speed;
                //弾の消去
                if (lifeTime > 0) {
                    Delete_Bullet(odd_Bullet, lifeTime);
                }
            }
        }
        else {
            Debug.Log("Can't Find Player");
        }
        return bullet_List;
    }


    /// <summary>
    /// 偶数弾
    /// </summary>
    public List<GameObject> Even_Num_Shoot(int num, float inter_Angle_Deg, float speed, float lifeTime) {
        List<GameObject> bullet_List = new List<GameObject>();
        if (!Is_Set_Pool()) {
            return null;
        }
        float center = num / 2 - 0.5f;
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            for (int i = 0; i < num; i++) {
                //弾の生成
                GameObject even_Bullet = bullet_Pool.GetObject();
                even_Bullet.transform.SetParent(parent_Obj);
                even_Bullet.transform.position = transform.position;
                bullet_List.Add(even_Bullet);
                //方向転換
                even_Bullet.transform.LookAt2D(player.transform, Vector2.right);
                float r = (i - center) * inter_Angle_Deg;
                even_Bullet.transform.Rotate(0, 0, r);
                //弾の発射
                even_Bullet.GetComponent<Rigidbody2D>().velocity = even_Bullet.transform.right * speed;
                //弾の消去
                if (lifeTime > 0) {
                    Delete_Bullet(even_Bullet, lifeTime);
                }
            }
        }
        else {
            Debug.Log("Can't Find Player");
        }
        return bullet_List;
    }



    /// <summary>
    /// 全方位弾
    /// </summary>
    public List<GameObject> Diffusion_Bullet(int num, float speed, float center_Angle_Deg, float lifeTime) {
        List<GameObject> bullet_List = new List<GameObject>();
        if (!Is_Set_Pool()) {
            return null;
        }
        for (int i = 0; i < num; i++) {
            //弾を円形に生成,発射
            float angle = i * 360f / num + center_Angle_Deg;
            GameObject bullet = Turn_Shoot_Bullet(speed, angle, lifeTime);
            bullet_List.Add(bullet);
        }
        return bullet_List;
    }


    /// <summary>
    /// nWay弾
    /// </summary>
    public List<GameObject> Some_Way_Bullet(int num, float speed, float center_Angle_Deg, float inter_Angle_Deg, float lifeTime) {
        List<GameObject> bullet_List = new List<GameObject>();
        if (!Is_Set_Pool()) {
            return null;
        }
        float center;
        //偶数wayの場合
        if (num % 2 == 0) {
            center = num / 2 - 0.5f;
        }
        //奇数wayの場合
        else {
            center = num / 2;
        }
        for (int i = 0; i < num; i++) {
            //弾の生成、発射
            float angle = center_Angle_Deg + inter_Angle_Deg * (i - center) + 180f;
            GameObject bullet = Turn_Shoot_Bullet(speed, angle, lifeTime);
            bullet_List.Add(bullet);
        }
        return bullet_List;
    }


    //弾の消去
    private void Delete_Bullet(GameObject bullet, float lifeTime) {
        Bullet b = bullet.GetComponent<Bullet>();
        if(b != null) {
            b.Set_Inactive(lifeTime);
        }
        else {
            bullet.AddComponent<Bullet>().Set_Inactive(lifeTime);
        }
    }
}
