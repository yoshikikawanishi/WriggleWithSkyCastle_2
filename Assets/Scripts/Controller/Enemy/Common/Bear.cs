using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour {

    [SerializeField] private GameObject honey_Bullet;

    private GameObject player;
    private Animator _anim;

    private bool can_Shoot = true;
    private float SHOOT_INTERVAL = 1.5f;


	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        _anim = GetComponent<Animator>();
        //弾のオブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(honey_Bullet, 1);
	}
	
	// Update is called once per frame
	void Update () {
		if(Is_Exist_Player_Forward() && can_Shoot) {
            StartCoroutine("Shoot_Cor");
            can_Shoot = false;
        }
	}


    //正面にいる自機を見つける
    private bool Is_Exist_Player_Forward() {
        Vector2 distance = player.transform.position - transform.position;
        distance *= new Vector2(transform.localScale.x, 1);
        if (-320f < distance.x && distance.x < 0) {
            if (-100f < distance.y && distance.y < 48f) {
                return true;
            }
        }
        return false;
    }


    //アニメーション再生とはちみつ弾の発射
    private IEnumerator Shoot_Cor() {
        //アニメーション
        _anim.SetTrigger("AttackTrigger");
        //弾の生成
        var bullet = ObjectPoolManager.Instance.Get_Pool(honey_Bullet).GetObject();
        bullet.transform.position = transform.position + new Vector3(-16f, 0);
        //弾の発射
        Vector2 speed = Calculate_Velocity(player.transform.position, 45f);
        bullet.GetComponent<Rigidbody2D>().velocity = speed;
        //弾の消去
        bullet.GetComponent<Bullet>().Set_Inactive(5.0f);

        yield return new WaitForSeconds(SHOOT_INTERVAL);
        can_Shoot = true;
    }


    /// <summary>
    /// 標的に命中する射出速度の計算
    /// </summary>
    /// <param name="aim_Pos">標的の座標</param>
    /// <returns>射出速度</returns>
    private Vector3 Calculate_Velocity(Vector3 aim_Pos, float angle) {
        Vector3 start_Pos = transform.position;
        float gravity = honey_Bullet.GetComponent<Rigidbody2D>().gravityScale * 10;        
        float rad = angle * Mathf.PI / 180;
        float x = start_Pos.x - aim_Pos.x;
        float y = start_Pos.y - aim_Pos.y;

        Debug.Log(gravity);
        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(gravity * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed)) {
            // 条件を満たす初速を算出できなければ
            return new Vector3(-80f, 80f);
        }
        else {
            return (new Vector3(aim_Pos.x - start_Pos.x, x * Mathf.Tan(rad), aim_Pos.z - start_Pos.z).normalized * speed);
        }
    }

}
