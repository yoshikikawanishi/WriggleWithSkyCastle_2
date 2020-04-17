using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AunnController : BossEnemy {

    //コンポーネント
    private AunnAttack _attack;    
    private Rigidbody2D _rigid;
    private CapsuleCollider2D _collider;
    private Animator _anim;

    public readonly AunnBGMManager _BGM = new AunnBGMManager();

    //戦闘開始
    private bool start_Battle = false;

    //初期値
    private float default_Gravity;
    private readonly Vector2 collider_Size_Standing = new Vector2(28f, 56f);
    private readonly Vector2 collider_Offset_Standing = new Vector2(1, -2f);
    private readonly Vector2 collider_Size_Squat = new Vector2(28f, 28f);
    private readonly Vector2 collider_Offset_Squat = new Vector2(1, -12f);

    //trueのアニメータパラメータ
    private string now_Anim_Param;


    new void Awake() {
        base.Awake();
        //取得
        _attack = GetComponent<AunnAttack>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _anim = GetComponent<Animator>();        

        default_Gravity = _rigid.gravityScale;

    }


    new void Update() {
        base.Update();
        if (start_Battle) {            
            switch (Get_Now_Phase()) {
                case 1: _attack.Phase1(_BGM); break;
                case 2: _attack.Phase2(_BGM); break;
            }
        }
    }


    //戦闘開始
    public void Start_Battle() {
        _BGM.Start_Time_Count();
        start_Battle = true;        
    }

    //クリア時の処理
    public void Clear() {
        start_Battle = false;
        _attack.Stop_Phase2();
    }


    //アニメーション変更、アニメーションに合わせて当たり判定のサイズも変更
    public void Change_Animation(string next_Param) {
        _anim.SetBool("StandingBool", false);
        _anim.SetBool("SquatBool", false);
        _anim.SetBool("DashBool", false);
        _anim.SetBool("JumpBool", false);
        _anim.SetBool("HighJumpBool", false);
        _anim.SetBool("ShootPoseBool", false);
        _anim.SetBool("DivingGroundBool", false);

        _anim.SetBool(next_Param, true);
        now_Anim_Param = next_Param;

        //当たり判定の変更
        switch (next_Param) {
            case "StangingBool":
                _collider.size = collider_Size_Standing;
                _collider.offset = collider_Offset_Standing;
                break;
            case "ShootPoseBool":
                _collider.size = collider_Size_Standing;
                _collider.offset = collider_Offset_Standing;
                break;
            default:
                _collider.size = collider_Size_Squat;
                _collider.offset = collider_Offset_Squat;
                break;
        }
        
    }


    public string Get_Now_Anim_Param() {
        return now_Anim_Param;
    }


    //地上用パラメータに
    public void Change_Land_Parameter() {
        _collider.isTrigger = false;
        _rigid.gravityScale = default_Gravity;
    }


    //空中用パラメータに
    public void Change_Fly_Parameter() {
        _collider.isTrigger = true;
        _rigid.gravityScale = 0;
        _rigid.velocity = Vector2.zero;
    }
 
}
