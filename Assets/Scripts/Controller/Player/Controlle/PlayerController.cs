using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBLDefine;

//***次ゲーム作るときは入力周りのことも他クラスに任せること***

public class PlayerController : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private Animator _anim;
    private PlayerTransition _transition;
    private PlayerTransitionRidingBeetle _transition_Beetle;
    private PlayerJump _jump;
    private PlayerAttack _attack;
    private PlayerChargeAttack _charge_Attack;
    private PlayerKick _kick;    
    private PlayerSquat _squat;
    private PlayerShoot _shoot;
    private PlayerGettingOnBeetle _getting_On_Beetle;
    //キー入力用
    private InputManager input;
    //収集アイテム
    private CollectionManager collection_Manager;

    //状態
    public bool is_Playable = true;    
    public bool is_Landing = true;
    public bool is_Squat = false;
    public bool is_Ride_Beetle = false;
    public bool can_Action = true;  //移動以外の無効化
    public bool can_Attack = true;      

    //現在のアニメーション
    private string now_Animator_Parameter = "IdleBool";   

    //初期値
    private float default_Gravity;

    //緑ゲージ不足の警告音は飛行中1度だけ鳴らす
    private bool is_Played_Alert = false;   


    //Awake
    private void Awake() {
        //取得
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _transition = GetComponent<PlayerTransition>();
        _transition_Beetle = GetComponent<PlayerTransitionRidingBeetle>();
        _jump = GetComponent<PlayerJump>();
        _attack = GetComponent<PlayerAttack>();
        _charge_Attack = GetComponent<PlayerChargeAttack>();
        _kick = GetComponent<PlayerKick>();        
        _squat = GetComponent<PlayerSquat>();
        _shoot = GetComponent<PlayerShoot>();
        _getting_On_Beetle = GetComponent<PlayerGettingOnBeetle>();
        input = InputManager.Instance;
        collection_Manager = CollectionManager.Instance;
        default_Gravity = _rigid.gravityScale;
    }


    // Update is called once per frame
    void Update () {

        if (!is_Playable) {
            return;
        }

        if (is_Ride_Beetle) {
            Beetle_Controlle();    
        }
        else {
            Normal_Controlle();                   
        }        
	}


    private void LateUpdate() {
        //速度の制限
        //if(Mathf.Abs(_rigid.velocity.x) > 210f) {
        //    _rigid.velocity = new Vector2(_rigid.velocity.x.CompareTo(0) * 210f, _rigid.velocity.y);
        //}
    }


    //通常時の操作
    private void Normal_Controlle() {
        //しゃがみ
        if(Input.GetAxisRaw("Vertical") < -0.05f && is_Landing) {
            if(!is_Squat)
                _squat.Squat();
        }
        else {
            if(is_Squat)
                _squat.Release_Squat();
        }        
        //移動
        if (is_Squat) {
            _transition.Slow_Down();
        }
        else {
            if (Input.GetAxisRaw("Horizontal") > 0) {
                _transition.Transition(1);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0) {
                _transition.Transition(-1);
            }
            else {
                _transition.Slow_Down();
            }            
        }

        //アクション無効化
        if(can_Action == false) {
            return;
        }

        //ジャンプ操作
        Jump();       
        //近接攻撃、キックの操作
        Attack();
        //カブトムシに乗る
        if (input.GetKeyDown(Key.Fly) && BeetlePowerManager.Instance.Get_Beetle_Power() > 0) {
            _getting_On_Beetle.Get_On_Beetle();
            is_Played_Alert = false;    //警告音を鳴らしたかどうかをリセット
        }

        //落下中重力下げる
        if(_rigid.velocity.y < -1f) {
            if(_rigid.gravityScale > default_Gravity - 5f)
                _rigid.gravityScale = default_Gravity - 20f;
        }
        else {
            if(_rigid.gravityScale < default_Gravity - 5f)
                _rigid.gravityScale = default_Gravity;
        }
    }


    //カブトムシ時の操作
    private void Beetle_Controlle() {
        //重力
        if (!Mathf.Approximately(_rigid.gravityScale, 0))
            _rigid.gravityScale = 0;

        //移動
        Vector2 direction = new Vector2(0, 0);
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));        
        if (input.GetKey(Key.Slow)) {
            direction *= 0.3f;
        }        
        _transition_Beetle.Transition(direction);        

        //ショット
        Shoot();
        //カブトムシから降りる
        if (input.GetKeyDown(Key.Fly) || BeetlePowerManager.Instance.Get_Beetle_Power() <= 0) {
            _getting_On_Beetle.Get_Off_Beetle();            
        }        
        //パワーの消費
        BeetlePowerManager.Instance.Decrease_In_Update(1f);
        //警告音
        if (BeetlePowerManager.Instance.Get_Beetle_Power() < 20f) {
            if (!is_Played_Alert) {
                is_Played_Alert = true;
                GetComponentInChildren<PlayerSoundEffect>().Play_Alert_Sound();
            }
        }
        //画面の天井より上にいるとき直す
        if (transform.position.y > 110f) {
            transform.position = new Vector3(transform.position.x, 110f);
        }
        //画面の下端より下にいるとき直す
        else if(transform.position.y < -110f) {
            transform.position = new Vector3(transform.position.x, -110f);
        }
    }


    #region JUMP
    //ジャンプボタンが押された時接地していなくても、数フレーム後に着地すればジャンプする
    private bool start_Jump_Frame_Count = false;
    private int jump_Frame_Count = 0;
    //地面から離れた後も数フレーム間はジャンプを受け付ける
    private int leave_Land_Frame_Count = 0;

    private void Jump() {        
        if (input.GetKeyDown(Key.Jump)) {
            //地面にいるときはそのままジャンプ
            if (is_Landing) {
                _jump.Jump();
            }
            //地面から離れている時
            else {
                //数フレーム前に地面にいた時はジャンプする
                if (leave_Land_Frame_Count < 3) {
                    _jump.Jump();
                    leave_Land_Frame_Count = 3;
                }
                //ボタン押下後数フレーム間着地しないか監視する
                else {
                    start_Jump_Frame_Count = true;
                    jump_Frame_Count = 0;
                }
            }
        }
        //ボタン押下後数フレーム以内に着地したときジャンプする
        if (start_Jump_Frame_Count) {
            jump_Frame_Count++;
            if (is_Landing && jump_Frame_Count < 10) {
                _jump.Jump();
                start_Jump_Frame_Count = false;
                jump_Frame_Count = 0;
            }
        }
        //地面から離れた後のフレームカウントする
        if (is_Landing)
            leave_Land_Frame_Count = 0;
        else
            leave_Land_Frame_Count++;
        //減速
        if (!input.GetKey(Key.Jump)) {            
            _jump.Release_Jumping();
        }
    }
    #endregion


    #region ATTACK
    //攻撃の入力識別用
    public bool start_Attack_Frame_Count = false;
    private int attack_Frame_Count = 0;
    //チャージアタックチャージ
    private bool start_Charge_Attack_Charge = false;
    //チャージキックチャージ
    public bool start_Charge_Kick_Charge = false;

    private void Attack() {
        //入力を受け取ったら、少しだけ待つ
        if (input.GetKeyDown(Key.Attack)) {
            start_Attack_Frame_Count = true;
        }
        //待ってる間に下が押されたらキック、1度も押されなかったら横攻撃
        if (start_Attack_Frame_Count) {
            attack_Frame_Count++;
            if (Input.GetAxisRaw("Vertical") < -0.7f) {
                _squat.Release_Squat();
                _kick.Kick();                                
            }
            else if (attack_Frame_Count > 7) {
                _attack.Attack();
                start_Charge_Attack_Charge = true;
            }
            else return;
            attack_Frame_Count = 0;
            start_Attack_Frame_Count = false;
        }

        //チャージアタック
        //Debug.Log("TODO : Charge Attack need collection item");
        if (start_Charge_Attack_Charge) {            
            _charge_Attack.Charge();
            if (!input.GetKey(Key.Attack)) {
                start_Charge_Attack_Charge = false;
                _charge_Attack.Charge_Attack();
            }
        }

        //チャージキックの溜め
        if (CollectionManager.Instance.Is_Collected("Saki")) {
            if (is_Squat) {
                _kick.Charge();
            }
            else {
                _kick.Quit_Charge(false);
            }
        }
        
    }
    #endregion


    #region SHOOT
    private bool is_Buffered_Input = false;    
    public float SHOOT_INTERVAL = 0.25f;
    private float shoot_Time = 0.2f;

    public void Shoot() {
        //通常ショット
        if (shoot_Time < _shoot.shoot_Interval) {
            shoot_Time += Time.deltaTime;
            if (input.GetKeyDown(Key.Shoot))
                is_Buffered_Input = true;
        }
        else {
            if (input.GetKeyDown(Key.Shoot) || is_Buffered_Input) {
                _shoot.Shoot();
                shoot_Time = 0;
                is_Buffered_Input = false;
            }
        }
        //チャージショット
        if (collection_Manager.Is_Collected("Yuka")) {
            if (input.GetKey(Key.Shoot)) {
                _shoot.Charge();
            }
            if (input.GetKeyUp(Key.Shoot)) {
                _shoot.Charge_Shoot();
            }
        }
    }
    #endregion


    //アニメーション変更
    //横攻撃とキックはAnyStateからのTriggerで管理
    public void Change_Animation(string next_Parameter) {
        if(now_Animator_Parameter == next_Parameter) {
            return;
        }

        _anim.SetBool("IdleBool", false);
        _anim.SetBool("DashBool", false);
        _anim.SetBool("JumpBool", false);
        _anim.SetBool("RideBeetleBool", false);
        _anim.SetBool("SquatBool", false);
        _anim.SetBool("KickBool", false);

        _anim.SetBool(next_Parameter, true);
        now_Animator_Parameter = next_Parameter;
    }


    //Setter
    public void Set_Is_Playable(bool is_Playable) {
        this.is_Playable = is_Playable;
    }

    public void Set_Can_Action(bool can_Action) {
        this.can_Action = can_Action;
    }

    //Getter
    public bool Get_Is_Playable() {
        return is_Playable;
    }

    public bool Get_Is_Ride_Beetle() {
        return is_Ride_Beetle;
    }

    //カブトムシ無効化
    public void To_Disable_Ride_Beetle() {
        _getting_On_Beetle.To_Disable();
    }

    //カブトムシ無効化解除
    public void To_Enable_Ride_Beetle() {
        _getting_On_Beetle.To_Enable();
    }

    //カブトムシ時の方向変更
    public void Change_Beetle_Direction(int scale_X) {
        _transition_Beetle.Change_Body_Direction(scale_X);
    }

    //カブトムシ時の方向取得
    public int Get_Beetle_Direction() {
        return _transition_Beetle.Get_Beetle_Direction();
    }

    //点滅
    public IEnumerator Blink(float time_Length) {
        Renderer player_Renderer = GetComponent<Renderer>();
        float t = 0;
        while (t < time_Length) {
            player_Renderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            player_Renderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
            t += 0.2f;
        }        
    }
   
}
