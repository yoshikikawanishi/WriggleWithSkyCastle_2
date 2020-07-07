using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedYinBallStrong : Enemy {

    private enum State {
        none,
        tracing,
        attack
    }
    private State state = State.tracing;    

    private GameObject player;
    private GameObject main_Camera;    
    private Rigidbody2D _rigid;
    private GravitatePlayer _gravitate;
    private MoveTwoPoints _move;
    private ShootSystem _shoot;

    private const float TRACE_PLAYER_SPAN = 1.5f;       //自機追従の最低時間
    private const float START_ATTACK_DISTANCE = 240f;   //攻撃開始する自機との距離
    private const float GO_AROUND_ANGLE_DEG = 45f;      //攻撃時の移動距離     
    private const float MOVE_SPEED = 0.015f;            //攻撃時の移動速度
    private const float ARC_SIZE = 64f;                 //攻撃時の移動膨らみ具合

    private float trace_Player_Time = 0;

	
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        main_Camera = GameObject.FindWithTag("MainCamera");        
        _rigid = GetComponent<Rigidbody2D>();
        _gravitate = GetComponent<GravitatePlayer>();
        _move = GetComponent<MoveTwoPoints>();
        _shoot = GetComponentInChildren<ShootSystem>();
        _gravitate.enabled = false;
	}
	

    void Update() {
        if (!Is_Enable()) {
            return;
        }

        //自機追従
        if(state == State.tracing) {
            if (!_gravitate.enabled) {
                _gravitate.enabled = true;
            }
            if (trace_Player_Time < TRACE_PLAYER_SPAN) {
                trace_Player_Time += Time.deltaTime;
                return;
            }
            //攻撃開始
            if(Mathf.Abs(transform.position.x - player.transform.position.x) < START_ATTACK_DISTANCE) {
                state = State.attack;
                trace_Player_Time = 0;
                _gravitate.enabled = false;
                _rigid.velocity = new Vector2(0, 0);
            }
        }
        //攻撃
        else if(state == State.attack) {
            state = State.none;
            StartCoroutine("Attack_Cor");
        }
    }
    

    //行動開始の判別
    private bool Is_Enable() {
        if(Mathf.Abs(main_Camera.transform.position.x - transform.position.x) < 400f) {
            return true;
        }
        return false;
    }


    //攻撃
    //移動→溜め→弾幕発射
    private IEnumerator Attack_Cor() {
        //点滅
        StartCoroutine("Blink_Cor");
        yield return new WaitForSeconds(0.2f);
        //移動先座標
        Vector2 next_Pos = Cal_Destination();
        //移動
        int arc = 1;
        if (transform.position.y < player.transform.position.y)
            arc = -1;
        _move.Change_Paramter(MOVE_SPEED, ARC_SIZE * arc, 0);
        _move.Start_Move(next_Pos);
        yield return new WaitUntil(_move.End_Move);        
        //弾幕
        _shoot.center_Angle_Deg = new AngleCalculater().Cal_Angle_Two_Points(transform.position, player.transform.position);
        _shoot.Shoot();
        UsualSoundManager.Instance.Play_Laser_Sound();

        state = State.tracing;
    }


    //移動先計算
    //自機を中心にGO_AROUND_ANGLE_DEG度回転
    private Vector2 Cal_Destination() {
        float radius = START_ATTACK_DISTANCE;
        float angle = new AngleCalculater().Cal_Angle_Two_Points(player.transform.position, transform.position);
        int direction = 1;
        if (transform.position.y < 0 && transform.position.x < player.transform.position.x)
            direction = -1;
        else if (transform.position.y > 0 && transform.position.x > player.transform.position.x)
            direction = -1;
        angle = (angle + GO_AROUND_ANGLE_DEG * direction) * Mathf.Deg2Rad;
        Vector2 destination = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

        return destination;
    }
	

    //白く点滅
    private IEnumerator Blink_Cor() {
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        Color default_Color = _sprite.color;
        for(int i = 0; i < 3; i++) {
            _sprite.color = default_Color + new Color(0.2f, 0.2f, 0.2f, 0f);
            yield return new WaitForSeconds(0.1f);
            _sprite.color = default_Color;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
