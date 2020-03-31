using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyBullet : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    //自機
    private GameObject player;

    //一番近くの敵
    private GameObject target;

    [SerializeField] private float speed = 500f;
    [SerializeField] private float homing_Speed = 120f;
    [SerializeField] private float start_Homing_Time = 0f;


    // Use this for initialization
    void Awake() {
        //コンポーネント
        _rigid = GetComponent<Rigidbody2D>();
        //自機
        player = GameObject.FindWithTag("PlayerTag");
    }


    //OnEnable
    private void OnEnable() {
        transform.rotation = Quaternion.Euler(0, 0, 0);    //初期化
        if (player.transform.localScale.x < 0)
            transform.rotation = Quaternion.Euler(0, 0, 180f);

        //初めの数秒はホーミングしない
        target = null;
        Invoke("Find_Nearest_Enemy", start_Homing_Time);
    }    


    // Update is called once per frame
    void Update() {
        //敵をホーミング
        //消えていなくて、無敵化していない敵をホーミング
        if (target != null && target.activeSelf && target.layer != 10) {
            To_Homing();
        }
        else {
            _rigid.velocity = transform.right * speed;
        }
    }


    //自機より右にいる一番近くの敵を探す
    private void Find_Nearest_Enemy() {        
        target = null;
        if (player == null)
            return;

        float min_Distance = 800;
        float distance = 0;
        GameObject[] enemy_List = GameObject.FindGameObjectsWithTag("EnemyTag");
        foreach (GameObject enemy in enemy_List) {
            //無敵化している敵は無視
            if (!enemy.activeSelf && enemy.layer == 10)
                continue;
            //画面の外にいる敵は無視
            if (Mathf.Abs(enemy.transform.position.y) > 150f)
                continue;

            //一番近くの敵を探す
            distance = Vector2.Distance(enemy.transform.position, player.transform.position);            
            if (distance < min_Distance) {
                min_Distance = distance;
                target = enemy;
            }
        }
    }


    //ホーミング
    private void To_Homing() {
        transform.LookAt2D(target.transform, Vector2.right);
        _rigid.velocity += (Vector2)transform.right * homing_Speed;
        _rigid.velocity = _rigid.velocity.normalized * speed;
        float dirVelocity = Mathf.Atan2(_rigid.velocity.y, _rigid.velocity.x) * Mathf.Rad2Deg;    //進行方向に回転
        transform.rotation = Quaternion.AngleAxis(dirVelocity, new Vector3(0, 0, 1));
    }
}
