using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarletFairy : FairyEnemy {

    [Space]
    [SerializeField] private ShootSystem distante_Shoot;
    [SerializeField] private ShootSystem close_Shoot;
    [SerializeField] private ChildColliderTrigger side_Collision;

    private enum State {
        walk,
        distante_Shoot,
        close_Shoot,
    }
    private State state = State.walk;
    private State pre_Action = State.close_Shoot;

    private Renderer _renderer;
    private Animator _anim;
    private GameObject player;

    private float player_Close_Border = 96f;
    private Vector2 player_Distance;
    private bool is_Close_Player = false;

    private float MOVE_SPEED = 0.5f;
    private float move_Speed;


    void Start() {
        //取得
        _renderer = GetComponent<Renderer>();
        _anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("PlayerTag");
        //初期値
        move_Speed = MOVE_SPEED;
    }


    void Update() {
        if (!_renderer.isVisible) {
            return;
        }

        //歩く
        transform.position += new Vector3(-transform.localScale.x * move_Speed, 0, 0);
        //方向転換
        if (side_Collision.Hit_Trigger()) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
        }

        //ショット
        if (state == State.walk) {
            //自機が近付いた時
            if (Is_Close_Player() && pre_Action != State.close_Shoot) {
                state = State.close_Shoot;                
                StartCoroutine("Start_Close_Shoot_Cor");
            }
            //自機が遠い時
            else {                
                state = State.distante_Shoot;                
                StartCoroutine("Start_Distante_Shoot_Cor");
            }            
        }
    }


    //自機が近くにいる時true
    private bool Is_Close_Player() {
        player_Distance = player.transform.position - transform.position;
        if(Mathf.Pow(player_Distance.x, 2) + Mathf.Pow(player_Distance.y, 2) < Mathf.Pow(player_Close_Border, 2)) {
            return true;
        }
        return false;
    }

    
    //遠くにいる自機に対するショット
    private IEnumerator Start_Distante_Shoot_Cor() {
        StartCoroutine(Blink_Cor(2, false));
        yield return new WaitForSeconds(0.9f);

        //自機が近付いたら変更
        if (Is_Close_Player() && pre_Action != State.close_Shoot) {
            StartCoroutine("Start_Close_Shoot_Cor");
            yield break;
        }

        distante_Shoot.center_Angle_Deg = 90f + 30f * transform.localScale.x;
        distante_Shoot.Shoot();
        yield return new WaitForSeconds(0.6f);        

        state = State.walk;
        pre_Action = State.distante_Shoot;
    }


    //近くにいる自機に対するショット
    private IEnumerator Start_Close_Shoot_Cor() {
        move_Speed = 0;        
        _anim.SetBool("AttackBool", true);

        StartCoroutine(Blink_Cor(3, true));
        yield return new WaitForSeconds(0.6f);

        close_Shoot.Shoot();
        yield return new WaitForSeconds(1.5f);
        
        _anim.SetBool("AttackBool", false);
        move_Speed = MOVE_SPEED;
        state = State.walk;
        pre_Action = State.close_Shoot;
    }


    //白点滅
    private IEnumerator Blink_Cor(int count, bool play_SE) {
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        SEManager _se = GetComponentInChildren<SEManager>();

        for(int i = 0; i < count; i++) {
            _sprite.color = new Color(0.7f, 0.7f, 0.7f, 1);
            if(play_SE)
                _se.Play("Flash");
            yield return new WaitForSeconds(0.1f);
            _sprite.color = new Color(0.5f, 0.5f, 0.5f, 1);
            yield return new WaitForSeconds(0.1f);
        }
    }


    public override void Vanish() {
        close_Shoot.transform.SetParent(null);
        close_Shoot.Stop_Shoot();
        Destroy(close_Shoot, 2.0f);
        base.Vanish();
    }

}
