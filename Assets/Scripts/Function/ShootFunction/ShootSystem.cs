using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : MonoBehaviour {
    
    //種類
    public enum KIND {
        Odd,
        Even,
        Diffusion,
        nWay,
        Scatter,
        Spiral
    };
    public KIND kind;

    //基本設定
    public string comment = "";
    public bool play_On_Awake = false;
    public GameObject bullet = null;
    public Transform parent = null;
    public float max_Speed;
    public float radius;
    public float speed_Noise = 0;
    public float angle_Noise = 0;
    public float lifeTime = 5;
    public Vector2 offset = new Vector2(0, 0);
    public bool default_Shoot_Sound = false;

    public bool is_Acceleration = false;
    public AnimationCurve velocity_Forward;
    public AnimationCurve velocity_Lateral;
    public AnimationCurve velocity_To_Player = AnimationCurve.Constant(0, 0.1f, 0);

    public bool is_Change_Sorting_Order = false;
    public int sorting_Order = -5;
    public int sorting_Order_Diff = 0;

    public bool is_Fixed_Rotation = false;
    public float fixed_Angle = 0;

    //その他設定
    public bool other_Param = true;    
    // 1:奇数段 / 2:偶数弾 / 3:全方位弾 / 4:nWay弾 / 5:ばらまき弾 / 6:渦巻き弾
    public int num = 1;                 //1, 2, 3, 4
    public float inter_Angle_Deg = 20;  //1, 2, 3, 4, 6
    public float center_Angle_Deg = 0;  //2, 3, 4, 5    
    public float arc_Deg = 360f;        //5
    public float shoot_Rate = 30f;      //5, 6  
    public float duration = 5.0f;       //5, 6

    //連結して出す弾の数
    public bool connect_Bullet = false;
    public int connect_Num = 1;
    public float speed_Diff = 15f;
    public float angle_Diff = 0;    

    //繰り返し時、値の変更用
    public bool looping = true;
    public int loop_Count = 1;       
    public float span = 1.0f;
    public float center_Angle_Diff = 0;    

    private ObjectPool bullet_Pool; //弾


    private void Start() {
        if (play_On_Awake) {
            Shoot();
        }
    }

    /// <summary>
    /// インスペクターで設定したショットを打つ
    /// </summary>
    public void Shoot() {
        bullet_Pool = ObjectPoolManager.Instance.Get_Pool(bullet);
        switch (kind) {
            case KIND.Scatter:  StartCoroutine("Scatter_Shoot"); break;
            case KIND.Spiral:   StartCoroutine("Spiral_Shoot"); break;
            default:            StartCoroutine("Shoot_Cor");    break;
        }
    }
    

    /// <summary>
    /// ショットを止める
    /// </summary>
    public void Stop_Shoot() {
        switch (kind) {
            case KIND.Scatter:  StopCoroutine("Scatter_Shoot"); break;
            case KIND.Spiral:   StopCoroutine("Spiral_Shoot");  break;
            default:            StopCoroutine("Shoot_Cor");     break;
        }
    }


    //ショット用のコルーチン
    private IEnumerator Shoot_Cor() {        
        for (int i = 0; i < loop_Count; i++) {            
            switch (kind) {
                case KIND.Odd: Odd_Num_Shoot(); break;
                case KIND.Even: Even_Num_Shoot(); break;
                case KIND.Diffusion: Diffusion_Shoot(); break;
                case KIND.nWay: nWay_Shoot(); break;
            }

            center_Angle_Deg += center_Angle_Diff;            
            yield return new WaitForSeconds(span - Time.deltaTime);                
        }
    }


    /// <summary>
    /// ばらまき弾
    /// </summary>
    /// <returns></returns>
    private IEnumerator Scatter_Shoot() {
        float angle = center_Angle_Deg;
        for (int i = 0; i < loop_Count; i++) {
            for (float t = 0; t < duration; t += 1 / shoot_Rate) {
                angle = center_Angle_Deg + Random.Range(-arc_Deg / 2, arc_Deg / 2);
                Turn_Shoot_Bullet(angle);
                if (default_Shoot_Sound)
                    UsualSoundManager.Instance.Play_Shoot_Sound_Small();
                
                yield return new WaitForSeconds(1 / shoot_Rate);
            }
            angle += center_Angle_Diff;
            yield return new WaitForSeconds(span);
        }
    }


    /// <summary>
    /// 渦巻き弾
    /// </summary>
    /// <returns></returns>
    private IEnumerator Spiral_Shoot() {
        float angle = center_Angle_Deg;
        for (int i = 0; i < loop_Count; i++) {
            for (float t = 0; t < duration; t += 1 / shoot_Rate) {
                Turn_Shoot_Bullet(angle);
                angle += inter_Angle_Deg;
                if (default_Shoot_Sound)
                    UsualSoundManager.Instance.Play_Shoot_Sound_Small();

                yield return new WaitForSeconds(1 / shoot_Rate);
            }
            angle += center_Angle_Diff;
            yield return new WaitForSeconds(span);
        }
    }


    //奇数段、偶数弾、全方位弾、nWay弾
    #region ShootFunction

    /// <summary>
    /// 奇数段
    /// </summary>
    public List<GameObject> Odd_Num_Shoot() {        
        List<GameObject> bullet_List = new List<GameObject>();        

        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null) {
            Debug.Log("Can't Find Player");
            return null;
        }

        AngleCalculater _angle = new AngleCalculater();
        center_Angle_Deg = _angle.Cal_Angle_Two_Points(transform.position, player.transform.position);
        float angle = center_Angle_Deg - num / 2 * inter_Angle_Deg;        

        for (int i = 0; i < num; i++) {
            bullet_List.AddRange(Turn_Shoot_Bullet(angle));            
            angle += inter_Angle_Deg;            
        }

        if (default_Shoot_Sound)
            UsualSoundManager.Instance.Play_Shoot_Sound();

        return bullet_List;
    }


    /// <summary>
    /// 偶数弾
    /// </summary>
    public List<GameObject> Even_Num_Shoot() {
        List<GameObject> bullet_List = new List<GameObject>();

        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null) {
            Debug.Log("Can't Find Player");
            return null;
        }

        AngleCalculater _angle = new AngleCalculater();
        center_Angle_Deg = _angle.Cal_Angle_Two_Points(transform.position, player.transform.position);
        float angle = center_Angle_Deg - ((num - 1) * 0.5f * inter_Angle_Deg);        

        for (int i = 0; i < num; i++) {     
            bullet_List.AddRange(Turn_Shoot_Bullet(angle));                        
            angle += inter_Angle_Deg;
        }

        if (default_Shoot_Sound)
            UsualSoundManager.Instance.Play_Shoot_Sound();

        return bullet_List;
    }


    /// <summary>
    /// 全方位弾
    /// </summary>
    public List<GameObject> Diffusion_Shoot() {
        List<GameObject> bullet_List = new List<GameObject>();

        //弾を円形に生成,発射
        for (int i = 0; i < num; i++) {
            float angle = i * 360f / num + center_Angle_Deg;            
            bullet_List.AddRange(Turn_Shoot_Bullet(angle));
            bullet_List.Add(bullet);
        }

        if (default_Shoot_Sound)
            UsualSoundManager.Instance.Play_Shoot_Sound();

        return bullet_List;
    }


    /// <summary>
    /// nWay弾
    /// </summary>
    public List<GameObject> nWay_Shoot() {
        List<GameObject> bullet_List = new List<GameObject>();

        float center;
        //偶数wayの場合
        if (num % 2 == 0) {
            center = num / 2 - 0.5f;
        }
        //奇数wayの場合
        else {
            center = num / 2;
        }
        //弾の生成、発射
        for (int i = 0; i < num; i++) {
            float angle = center_Angle_Deg + inter_Angle_Deg * (i - center);            
            bullet_List.AddRange(Turn_Shoot_Bullet(angle));            
        }

        if (default_Shoot_Sound)
            UsualSoundManager.Instance.Play_Shoot_Sound();

        return bullet_List;
    }

    #endregion


    /// <summary>
    /// 弾の生成、回転と発射
    /// </summary>
    public List<GameObject> Turn_Shoot_Bullet(float angle_Deg) {
        List<GameObject> bullet_List = new List<GameObject>();
        float speed = max_Speed;
        float angle_Rad = angle_Deg * Mathf.Deg2Rad;
        float speed_Noise = 0;
        float angle_Noise = 0;

        for (int i = 0; i < connect_Num; i++) {
            var bullet = bullet_Pool.GetObject();                               //生成            
            bullet_List.Add(bullet);
            bullet.transform.SetParent(parent);                                 //親オブジェクト        

            bullet.transform.position = transform.position + (Vector3)offset;   //座標
            if(radius > 0)
                bullet.transform.position += new Vector3(Mathf.Cos(angle_Rad), Mathf.Sin(angle_Rad)) * radius;

            bullet.transform.rotation = new Quaternion(0, 0, 0, 0);             //回転
            if(this.angle_Noise > 0)    
                angle_Noise = Random.Range(-this.angle_Noise, this.angle_Noise);    
            bullet.transform.Rotate(new Vector3(0, 0, 1), angle_Deg + angle_Noise);

            speed_Noise = Random.Range(-this.speed_Noise, this.speed_Noise);    //初速 
            Bullet_Rigid(bullet).velocity = bullet.transform.right * (speed + speed_Noise);
            //寿命
            if (lifeTime > 0) {
                Delete_Bullet(bullet, lifeTime);                       
            }
            //弾の加速            
            if (is_Acceleration) {
                StartCoroutine(Accelerate_Bullet_Cor(bullet, max_Speed));
            }
            //描画順変更
            if (is_Change_Sorting_Order) {
                bullet.GetComponent<SpriteRenderer>().sortingOrder = sorting_Order;
                sorting_Order += sorting_Order_Diff;
            }
            //角度の固定
            if (is_Fixed_Rotation) {
                bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, fixed_Angle));
            }
            //パラメータ変更
            speed -= speed_Diff;
            angle_Deg += angle_Diff;
        }
        return bullet_List;
    }


    //弾のrigidbody取得
    private Rigidbody2D Bullet_Rigid(GameObject bullet) {        
        return bullet.GetComponent<Rigidbody2D>();
    }


    //弾の消去
    private void Delete_Bullet(GameObject bullet, float lifeTime) {
        Bullet b = bullet.GetComponent<Bullet>();
        if (b != null) {
            b.Set_Inactive(lifeTime);
        }
        else {
            bullet.AddComponent<Bullet>().Set_Inactive(lifeTime);
        }
    }    


    //弾の加速(単体)
    public IEnumerator Accelerate_Bullet_Cor(GameObject bullet, float max_Speed) {
        GameObject player = GameObject.FindWithTag("PlayerTag");

        Rigidbody2D bullet_Rigid = Bullet_Rigid(bullet);
        float xt = velocity_Forward.keys[velocity_Forward.keys.Length - 1].time;
        float yt = velocity_Lateral.keys[velocity_Lateral.keys.Length - 1].time;
        float pt = velocity_To_Player.keys[velocity_To_Player.keys.Length - 1].time;
        float end_Time = Mathf.Max(xt, yt, pt);

        float forward = max_Speed;
        float lateral = 0;
        float p = 0;
        Vector2 player_Direction = new Vector2();

        for(float t = 0; t < end_Time; t += Time.deltaTime) {            
            if(!bullet.activeSelf) {
                yield break;
            }

            forward = max_Speed * velocity_Forward.Evaluate(t);                 //前方向            
            lateral = max_Speed * velocity_Lateral.Evaluate(t);                 //横方向
            bullet_Rigid.velocity = bullet.transform.right * forward +          //速度代入
                                    bullet.transform.up * lateral;
            //自機方向
            if (player != null) {
                p                     = max_Speed * velocity_To_Player.Evaluate(t);
                player_Direction      = (player.transform.position - bullet.transform.position).normalized;
                bullet_Rigid.velocity += player_Direction * p;
            }
                                    
            float dirVelocity = Mathf.Atan2(bullet_Rigid.velocity.y, bullet_Rigid.velocity.x) * Mathf.Rad2Deg;    //進行方向に回転
            bullet.transform.rotation = Quaternion.AngleAxis(dirVelocity, new Vector3(0, 0, 1));
            //描画角度の固定化
            if (is_Fixed_Rotation) {
                bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, fixed_Angle));
            }
            yield return new WaitForSeconds(0.016f);
        }
    }
    

}
