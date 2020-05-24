using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 自機が近付いたら落下する
/// 着地時画面の振動
/// 着地後上昇
/// </summary>
public class Tub : MonoBehaviour {

    private enum State {
        idle,
        dropping,
        landing,
        raising,
    }
    private State state = State.idle;

    private Rigidbody2D _rigid;
    private ChildColliderTrigger foot_Collision;
    private GameObject landing_Effect;
    private GameObject player;
    private CameraShake camera_Shake;

    private float default_Height;
    private float landing_Time = 0;
    private float landing_Span = 1.0f;

    [SerializeField] private Vector2 detect_Player_Range = new Vector2(-80f, 0);
    

	// Use this for initialization
	void Start () {
        //取得
        _rigid = GetComponent<Rigidbody2D>();
        foot_Collision = transform.Find("Foot").GetComponent<ChildColliderTrigger>();
        landing_Effect = transform.Find("LandingEffect").gameObject;
        player = GameObject.FindWithTag("PlayerTag");
        camera_Shake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();

        default_Height = transform.position.y;        
	}

	
	// Update is called once per frame
	void Update () {
		//自機検知
        if(state == State.idle) {
            if (Is_Detecte_Player()) {
                state = State.dropping;
                _rigid.gravityScale = 80f;
            }
        }
        //落下中
        else if(state == State.dropping) {
            if (foot_Collision.Hit_Trigger()) {
                state = State.landing;
                camera_Shake.Shake(0.2f, new Vector2(1, 1), false);
                landing_Effect.SetActive(true);
            }
        }
        //着地後
        else if(state == State.landing) {
            if(landing_Time < landing_Span) {
                landing_Time += Time.deltaTime;
            }
            else {
                state = State.raising;
                _rigid.gravityScale = 0;
                landing_Time = 0;
                landing_Effect.SetActive(false);
            }
        }
        //上昇
        else if(state == State.raising) {
            transform.position += new Vector3(0, 1);
            if(transform.position.y > default_Height) {
                state = State.idle;
                transform.position = new Vector3(transform.position.x, default_Height);
            }
        }
	}


    private bool Is_Detecte_Player() {
        if (player == null)
            return false;
        float distance = player.transform.position.x - transform.position.x;
        if(detect_Player_Range.x < distance && distance < detect_Player_Range.y) {
            return true;
        }
        return false;
    }
}
