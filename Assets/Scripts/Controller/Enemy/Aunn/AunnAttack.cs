using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AunnAttack : MonoBehaviour {

    //コンポーネント
    private Aunn _controller;
    private AunnAttackFunction _attack_Func;
    private AunnShoot _shoot;
    private AunnEffect _effect;
    private ChildColliderTrigger foot_Collider;

    [HideInInspector] public bool[] start_Phase = { true, true };
    [HideInInspector] public bool can_Attack = true;


    private void Awake() {
        //取得
        _controller = GetComponent<Aunn>();
        _attack_Func = GetComponent<AunnAttackFunction>();
        _shoot = GetComponentInChildren<AunnShoot>();
        _effect = GetComponentInChildren<AunnEffect>();
        foot_Collider = transform.Find("Foot").GetComponent<ChildColliderTrigger>();
    }    
	
	
    //フェーズ１
    public void Phase1(AunnBGMManager _BGM) {
        //フェーズ開始時の処理
        if (start_Phase[0]) {
            start_Phase[0] = false;
            _effect.Play_Battle_Effect();
        }

        //攻撃の開始時にfalseに変更、AttackFunctionの方で攻撃終了時にtrueにもどす
        if (can_Attack == false)
            return;

        switch (_BGM.Get_Now_Melody()) {
            case AunnBGMManager.Melody.A:                
                Attack_In_Melody_A();
                break;
            case AunnBGMManager.Melody.B:
                Attack_In_Melody_B();
                break;
            case AunnBGMManager.Melody.C:
                Attack_In_Melody_C();
                break;
            case AunnBGMManager.Melody.main:
                Attack_In_Melody_Main(true, _BGM);
                break;
        }
    }

    public void Stop_Phase1() {
        _attack_Func.Stop_Attack();
        StopAllCoroutines();
        if(_controller.Get_Now_Anim_Param() == "DivingGroundBool") {
            transform.position += new Vector3(0, 50f);
        }
        _controller.Change_Land_Parameter();
        _controller.Change_Animation("StandingBool");
        _effect.Stop_Charge_Effect();
        _effect.Delete_Battle_Effect();
    }


    //フェーズ２
    public void Phase2(AunnBGMManager _BGM) {
        //フェーズ開始時の処理
        if (start_Phase[1]) {
            start_Phase[1] = false;
            Stop_Phase1();
            _attack_Func.generate_Copy = true;
            can_Attack = false;
            StartCoroutine("Phase_Change_Attack_Cor");
            _effect.Play_Battle_Effect();
        }

        //攻撃の開始時にfalseに変更、AttackFunctionの方で攻撃終了時にtrueにもどす
        if (can_Attack == false)
            return;

        switch (_BGM.Get_Now_Melody()) {
            case AunnBGMManager.Melody.A:
                Attack_In_Melody_A();
                break;
            case AunnBGMManager.Melody.B:
                Attack_In_Melody_B();
                break;
            case AunnBGMManager.Melody.C:
                Attack_In_Melody_C();
                break;
            case AunnBGMManager.Melody.main:
                Attack_In_Melody_Main(false, _BGM);
                break;
        }
    }

    public void Stop_Phase2() {
        _attack_Func.Stop_Attack();
        StopAllCoroutines();
        if (_controller.Get_Now_Anim_Param() == "DivingGroundBool") {
            transform.position += new Vector3(0, 50f);
        }
        _controller.Change_Land_Parameter();
        _controller.Change_Animation("StandingBool");
        _effect.Stop_Charge_Effect();
        _effect.Delete_Battle_Effect();
    }


    //Aメロ攻撃用
    private void Attack_In_Melody_A() {
        can_Attack = false;
        _attack_Func.Dive_And_Jump_Shoot();
    }


    //Bメロ攻撃用
    private void Attack_In_Melody_B() {
        can_Attack = false;
        _attack_Func.Jump_On_Wall_And_Rush();
    }


    //Cメロ攻撃用
    private void Attack_In_Melody_C() {
        can_Attack = false;
        _attack_Func.Reciprocate_Jump();
    }


    //サビ前、サビ弾幕用
    private void Attack_In_Melody_Main(bool is_Phase1, AunnBGMManager _BGM) {
        StartCoroutine(Attack_In_Melody_Main_Cor(is_Phase1, _BGM));
    }
    
    private IEnumerator Attack_In_Melody_Main_Cor(bool is_Phase1, AunnBGMManager _BGM) {
        can_Attack = false;
        Aunn _controller = GetComponent<Aunn>();
        AunnEffect _effect = GetComponentInChildren<AunnEffect>();
        MoveConstSpeed _move_Const = GetComponent<MoveConstSpeed>();
        ChildColliderTrigger foot_Collision = transform.Find("Foot").GetComponent<ChildColliderTrigger>();

        //移動        
        _attack_Func.StartCoroutine("High_Jump_Move_Cor", new Vector2(200f, transform.position.y + 4));
        yield return new WaitUntil(_attack_Func.Is_End_Move);
        transform.localScale = new Vector3(1, 1, 1);

        //サビまでの時間計測
        float wait_Time = _BGM.BGM_TIME[5] - _BGM.Get_Now_BGM_Time();
        if (wait_Time < 0 || wait_Time > 10)
            wait_Time = 2;

        //移動
        _controller.Change_Fly_Parameter();
        _controller.Change_Animation("ShootPoseBool");
        transform.localScale = new Vector3(1, 1, 1);
        _effect.Start_Charge_Effect(wait_Time);
        _move_Const.Start_Move(new Vector3(180f, 0), 1);
        yield return new WaitForSeconds(wait_Time);

        //弾幕
        do {
            //フェーズ１
            if (is_Phase1) {
                _effect.Play_Yellow_Circle_Effect();
                _effect.Play_Burst_Effect_Red();
                _shoot.Shoot_Dog_Bullet();
                yield return new WaitForSeconds(15.0f);

                if (_BGM.Get_Now_Melody() != AunnBGMManager.Melody.main)
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
        } while (_BGM.Get_Now_Melody() == AunnBGMManager.Melody.main);

        _controller.Change_Land_Parameter();
        yield return new WaitUntil(foot_Collision.Hit_Trigger);
        _controller.Change_Animation("SquatBool");

        can_Attack = true;
    }


    //フェーズ切り替え時の攻撃
    private IEnumerator Phase_Change_Attack_Cor() {
        //取得
        MoveConstSpeed _move_Const = GetComponent<MoveConstSpeed>();

        //無敵化
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        foot_Collider.Delete_Collision();
        yield return new WaitForSeconds(1.0f);

        //移動
        _attack_Func.StartCoroutine("High_Jump_Move_Cor", new Vector2(0, transform.position.y + 4));
        yield return new WaitUntil(_attack_Func.Is_End_Move);
        yield return new WaitForSeconds(0.3f);
        _controller.Change_Fly_Parameter();
        _controller.Change_Animation("ShootPoseBool");
        _move_Const.Start_Move(new Vector3(0, transform.position.y + 80f), 1);
        yield return new WaitUntil(_move_Const.End_Move);

        //ショット
        _shoot.Shoot_Aunn_Word_Bullet();
        yield return new WaitForSeconds(3.0f);

        _controller.Change_Land_Parameter();
        yield return new WaitForSeconds(3.0f);

        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        foot_Collider.Activate_Collision();
        can_Attack = true;
    }
}
