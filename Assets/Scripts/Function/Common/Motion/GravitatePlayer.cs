using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機に吸い寄せられるオブジェクトにアタッチする
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class GravitatePlayer : MonoBehaviour {

    //自機
    private GameObject player;
    //自機との距離
    private float distance;
    //自機との角度
    private Vector2 angle;

    //吸い付き始める距離
    public float DISTANCE_BORDER = 60f;
    //吸い付きの強さ
    public float GRAVITATE_POWER = 50f;

    //コンポーネント
    private Rigidbody2D _rigid;


    // Use this for initialization
    void Start() {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        //自機に吸い付く
        if (player != null) {
            angle = player.transform.position - transform.position;
            distance = Mathf.Sqrt(angle.x * angle.x + angle.y * angle.y);
            if (distance < DISTANCE_BORDER) {
                _rigid.velocity = angle.normalized * GRAVITATE_POWER;
            }
        }
    }
}
