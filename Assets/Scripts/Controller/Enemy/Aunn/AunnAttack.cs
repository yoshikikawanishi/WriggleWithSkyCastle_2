using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AunnAttack : BossEnemyAttack {

    //コンポーネント
    private Aunn _controller;
    private SpriteRenderer _sprite;
    private Rigidbody2D _rigid;    
    private AunnShoot _shoot;
    private AunnCopy _copy;
    private AunnShoot _copy_Shoot;
    private AunnEffect _effect;
    private SEManager _se;
    private ChildColliderTrigger foot_Collider;
    private MoveConstSpeed _move_Const;
    private MoveMotion _move_Motion;
    private MoveTwoPoints _move_Two_Points;

    private GameObject player;

    [HideInInspector] public bool[] start_Phase = { true, true };    


    private void Awake() {
        //取得
        _controller = GetComponent<Aunn>();
        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();        
        _shoot = GetComponentInChildren<AunnShoot>();
        _copy = GetComponentInChildren<AunnCopy>();
        _copy_Shoot = _copy.GetComponentInChildren<AunnShoot>();
        _effect = GetComponentInChildren<AunnEffect>();
        _se = GetComponentInChildren<SEManager>();
        _move_Const = GetComponent<MoveConstSpeed>();
        _move_Motion = GetComponent<MoveMotion>();
        _move_Two_Points = GetComponent<MoveTwoPoints>();
        foot_Collider = transform.Find("Foot").GetComponent<ChildColliderTrigger>();
        player = GameObject.FindWithTag("PlayerTag");
    }


    public override void Stop_Attack() {
        Stop_Attack_A();
        Stop_Attack_C();
        Stop_Attack_Intro();
        Stop_Attack_Main();
    }


    // =======================================================================================================
    #region intro
    protected override void Start_Melody_Intro() {        
        StartCoroutine("Attack_In_Melody_Intro_Cor");
    }


    private IEnumerator Attack_In_Melody_Intro_Cor() {
        Set_Can_Switch_Attack(false);

        //取得
        BossCollisionDetection _collision = GetComponent<BossCollisionDetection>();
        TracePlayer _trace = Get_Trace_Player();
        _trace.enabled = false;

        //地面に潜る
        _effect.Jump_And_Landing_Effect();
        _collision.Become_Invincible();
        _controller.Change_Fly_Parameter();
        _sprite.sortingOrder = -6;
        _se.Play("Dive");

        _move_Motion.Start_Move(0);
        yield return new WaitUntil(_move_Motion.Is_End_Move);
        _controller.Change_Animation("DivingGroundBool");
        yield return new WaitForSeconds(0.1f);
        _sprite.sortingOrder = 3;

        //コピーの生成
        if (_controller.Get_Now_Phase() == 2) {
            Vector2 position = new Vector2(-transform.position.x, transform.position.y);
            if (Mathf.Abs(transform.position.x) < 5f)
                position = new Vector2(120f, transform.position.y);
            _copy.Create_Copy(80, false, position);
        }

        //自機を追いかける        
        _trace.enabled = true;
        yield return new WaitForSeconds(2.09f);
        _trace.enabled = false;

        yield return new WaitForSeconds(0.33f);

        //ジャンプ
        _collision.Release_Invincible();        
        _controller.Change_Animation("HighJumpBool");
        _move_Motion.Start_Move(1);
        _effect.Play_A_Letter_Effect();
        _effect.Play_Burst_Effect_Red();
        UsualSoundManager.Instance.Play_Attack_Sound();
        yield return new WaitForSeconds(1.03f);

        //レーザー
        _controller.Change_Animation("ShootPoseBool");
        _effect.Play_Unn_Letter_Effect();
        _effect.Play_Burst_Effect_Red();
        _shoot.Shoot_Short_Curve_Laser(0.33f);
        if (_copy.gameObject.activeSelf) {
            _copy_Shoot.Shoot_Short_Curve_Laser(0.33f);
        }
        yield return new WaitForSeconds(1.65f);

        //落下   
        _controller.Change_Land_Parameter();
        yield return new WaitUntil(foot_Collider.Hit_Trigger);
        _effect.Jump_And_Landing_Effect();
        _controller.Change_Animation("SquatBool");

        //コピーの消去
        if (_copy.gameObject.activeSelf)
            _copy.Delete_Copy();

        yield return new WaitForSeconds(1.65f);

        //次の攻撃へ
        Set_Can_Switch_Attack(true);
        Restart_Attack();
    }


    private void Stop_Attack_Intro() {
        StopCoroutine("Attack_In_Melody_Intro_Cor");
        _shoot.Stop_Short_Curve_Laser();
        _move_Motion.Stop_Move();
        //潜行中の場合
        if (Get_Trace_Player().enabled) {
            Get_Trace_Player().enabled = false;
            transform.position += new Vector3(0, 32f);
            _controller.Change_Land_Parameter();            
        }
        _controller.Change_Animation("SquatBool");
        GetComponent<BossCollisionDetection>().Release_Invincible();
        //コピーの消去
        if (_copy.gameObject.activeSelf)
            _copy.Delete_Copy();
    }


    //TracePlayerをアタッチ、初期設定
    private TracePlayer Get_Trace_Player() {
        TracePlayer _trace = GetComponent<TracePlayer>();
        if (_trace == null) {
            _trace = gameObject.AddComponent<TracePlayer>();
            _trace.kind = TracePlayer.Kind.onlyX;
            _trace.speed = 2.5f;
        }
        return _trace;
    }

    #endregion
    // =======================================================================================================
    #region A
    protected override void Start_Melody_A() {
        StartCoroutine("Attack_In_Melody_A_Cor");
    }


    private IEnumerator Attack_In_Melody_A_Cor() {
        Set_Can_Switch_Attack(false);

        //自機のいない方の壁に飛びつく
        int direction = (player.transform.position.x - transform.position.x).CompareTo(0);
        if (direction == 0) { direction = 1; }
        StartCoroutine("Jump_On_Wall_Cor", new Vector2(230f * -direction, 48f));        
        yield return new WaitForSeconds(0.68f);
        _effect.Play_A_Letter_Effect();
        _effect.Play_Purple_Circle_Effect();

        //コピーの生成
        if (_controller.Get_Now_Phase() == 2) {
            _copy.Create_Copy(80, true, new Vector2(-transform.position.x, transform.position.y));
        }

        //反対側の壁に飛びつく
        StartCoroutine("Jump_On_Wall_Cor", new Vector2(230f * direction, 80f));
        //弾の配置開始
        _shoot.Start_Deposite_Purple_Bullet();
        if(_copy.gameObject.activeSelf)
            _copy_Shoot.Start_Deposite_Purple_Bullet();

        yield return new WaitForSeconds(1.03f);

        //弾の配置終了
        _shoot.Stop_Deposit_Purple_Bullet();
        _copy_Shoot.Stop_Deposit_Purple_Bullet();
        _effect.Play_Unn_Letter_Effect();
        _effect.Play_Purple_Circle_Effect();
        _se.Play("Charge");
        
        //コピーの消去
        if (_copy.gameObject.activeSelf) {
            _copy.Delete_Copy();            
        }

        yield return new WaitForSeconds(0.5f);

        //斜め下に突進する
        _controller.Change_Animation("JumpBool");
        _rigid.velocity = new Vector2(-transform.localScale.x * 160f, -200f);
        UsualSoundManager.Instance.Play_Attack_Sound();
        yield return new WaitUntil(foot_Collider.Hit_Trigger);

        //着地
        _controller.Change_Land_Parameter();
        _controller.Change_Animation("SquatBool");
        _effect.Jump_And_Landing_Effect();
        _se.Play("Kick");

        yield return new WaitForSeconds(2.0f);

        //次の攻撃へ
        Set_Can_Switch_Attack(true);
        Restart_Attack();
    }

    //壁に張り付く
    public IEnumerator Jump_On_Wall_Cor(Vector2 next_Pos) {        
        _controller.Change_Fly_Parameter();

        //方向
        int direction = (next_Pos.x - transform.position.x).CompareTo(0);
        if (direction == 0) { direction = 1; }
        transform.localScale = new Vector3(-direction, 1, 1);
        //移動
        _controller.Change_Animation("JumpBool");
        _effect.Jump_And_Landing_Effect();
        UsualSoundManager.Instance.Play_Attack_Sound();
        yield return new WaitForSeconds(0.18f);
        _move_Two_Points.Start_Move(next_Pos, 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        _controller.Change_Animation("OnWallBool");
        transform.localScale = new Vector3(direction, 1, 1);        
    }


    private void Stop_Attack_A() {
        StopCoroutine("Attack_In_Melody_A_Cor");
        StopCoroutine("Jump_On_Wall_Cor");
        _move_Two_Points.Stop_Move();
        _controller.Change_Land_Parameter();
        _controller.Change_Animation("SquatBool");
        _shoot.Stop_Deposit_Purple_Bullet();
        _copy_Shoot.Stop_Deposit_Purple_Bullet();
        if (_copy.gameObject.activeSelf)
            _copy.Delete_Copy();
    }

    #endregion
    // =======================================================================================================
    #region B
    protected override void Start_Melody_B() {
        
    }

    #endregion
    // =======================================================================================================
    #region C
    protected override void Start_Melody_C() {
        StartCoroutine("Attack_In_Melody_C_Cor");
    }


    private IEnumerator Attack_In_Melody_C_Cor() {
        Set_Can_Switch_Attack(false);

        //ジャンプ移動        
        StartCoroutine("High_Jump_Move_Cor", new Vector2(200f, transform.position.y + 4));
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(foot_Collider.Hit_Trigger);
        transform.localScale = new Vector3(1, 1, 1);

        yield return new WaitForSeconds(1.03f);
        
        //空中移動
        _controller.Change_Fly_Parameter();
        _controller.Change_Animation("ShootPoseBool");
        transform.localScale = new Vector3(1, 1, 1);
        _move_Const.Start_Move(new Vector3(180f, 0), 1);
        yield return new WaitUntil(_move_Const.End_Move);
        
        //TODO : 弾幕
        //溜め
        _effect.Start_Charge_Effect(10);

        while (melody_Manager.Get_Now_Melody() == MelodyManager.Melody.C) { yield return null; }

        //TODO : 弾幕中止
        //ため中止
        _effect.Stop_Charge_Effect();       

        //次の攻撃に
        Set_Can_Switch_Attack(true);
        Restart_Attack();
    }


    //大ジャンプする
    private IEnumerator High_Jump_Move_Cor(Vector2 next_Pos) {        
        _controller.Change_Fly_Parameter();

        //向き
        int direction = (transform.position.x - next_Pos.x).CompareTo(0);
        if (direction == 0) { direction = 1; }
        transform.localScale = new Vector3(direction, 1, 1);

        //ジャンプ
        _controller.Change_Animation("JumpBool");
        _move_Two_Points.Start_Move(next_Pos, 2);
        UsualSoundManager.Instance.Play_Shoot_Sound();
        yield return new WaitUntil(_move_Two_Points.End_Move);
        
        _controller.Change_Land_Parameter();
        _controller.Change_Animation("SquatBool");        
    }


    private void Stop_Attack_C() {        
        StopCoroutine("Attack_In_Melody_C_Cor");
        StopCoroutine("High_Jump_Move_Cor");
        _move_Const.Stop_Move();
        _effect.Stop_Charge_Effect();
        _move_Two_Points.Stop_Move();
        _controller.Change_Land_Parameter();
        _controller.Change_Animation("SquatBool");
    }
    #endregion
    // =======================================================================================================
    #region main
    protected override void Start_Melody_Main() {
        StartCoroutine("Attack_In_Melody_Main_Cor");
    }


    private IEnumerator Attack_In_Melody_Main_Cor() {
        if(transform.position.x < 175f && Mathf.Abs(transform.position.y) > 5f) {
            Start_Melody_C();
            yield break;
        }

        Set_Can_Switch_Attack(false);

        //弾幕
        do {
            //フェーズ１
            if (_controller.Get_Now_Phase() == 1) {
                _effect.Play_Yellow_Circle_Effect();
                _effect.Play_Burst_Effect_Red();
                _shoot.Shoot_Dog_Bullet();
                yield return new WaitForSeconds(15.0f);

                if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.main)
                    break;

                _effect.Play_Yellow_Circle_Effect();
                _effect.Play_Burst_Effect_Red();
                _shoot.Shoot_Dog_Bullet_Big();
                yield return new WaitForSeconds(10.0f);
            }
            //フェーズ２
            else {
                _effect.Play_Burst_Effect_Red();
                _shoot.Shoot_Long_Curve_Laser();
                yield return new WaitForSeconds(9.0f);
            }
        } while (melody_Manager.Get_Now_Melody() == MelodyManager.Melody.main);

        //落下
        _controller.Change_Land_Parameter();
        yield return new WaitUntil(foot_Collider.Hit_Trigger);
        _controller.Change_Animation("SquatBool");

        //次の攻撃に        
        Set_Can_Switch_Attack(true);
        Restart_Attack();        
    }


    private void Stop_Attack_Main() {        
        StopCoroutine("Attack_In_Melody_Main_Cor");
        _shoot.Stop_Dog_Bullet();
        _shoot.Stop_Long_Curve_Laser();       
    }
    #endregion
    // =======================================================================================================    
    #region ChangePhase
    protected override void Action_In_Change_Phase() {
        if (_controller.Get_Now_Phase() == 2) {
            Stop_Attack();
            StartCoroutine("Phase_Change_Attack_Cor");
        }
    }


    //フェーズ切り替え時の攻撃
    private IEnumerator Phase_Change_Attack_Cor() {
        Set_Can_Switch_Attack(false);

        //取得
        BossCollisionDetection _collision = GetComponent<BossCollisionDetection>();
        //無敵化
        _collision.Become_Invincible();
        foot_Collider.Delete_Collision();
        yield return new WaitForSeconds(1.0f);

        //移動
        StartCoroutine("High_Jump_Move_Cor", new Vector2(0, transform.position.y + 4));        
        yield return new WaitForSeconds(2.5f);        
        
        _controller.Change_Fly_Parameter();
        _controller.Change_Animation("ShootPoseBool");
        _move_Const.Start_Move(new Vector3(0, transform.position.y + 80f), 1);
        yield return new WaitUntil(_move_Const.End_Move);

        //ショット
        _shoot.Shoot_Aunn_Word_Bullet();
        yield return new WaitForSeconds(3.0f);

        _controller.Change_Land_Parameter();        
        _collision.Release_Invincible();
        foot_Collider.Activate_Collision();

        yield return new WaitForSeconds(4.0f);

        //攻撃再開
        Set_Can_Switch_Attack(true);
        Restart_Attack();
    }
    #endregion


}
