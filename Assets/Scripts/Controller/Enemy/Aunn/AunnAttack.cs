using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AunnAttack : MonoBehaviour {

    //コンポーネント
    private AunnController _controller;
    private AunnAttackFunction _attack_Func;
    private AunnShoot _shoot;
    private AunnEffect _effect;

    [HideInInspector] public bool[] start_Phase = { true, true };
    [HideInInspector] public bool can_Attack = true;


    private void Awake() {
        //取得
        _controller = GetComponent<AunnController>();
        _attack_Func = GetComponent<AunnAttackFunction>();
        _shoot = GetComponentInChildren<AunnShoot>();
        _effect = GetComponentInChildren<AunnEffect>();
    }    
	
	
    //フェーズ１
    public void Phase1(AunnBGMManager _BGM) {
        //フェーズ開始時の処理
        if (start_Phase[0]) {
            start_Phase[0] = false;
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
                StartCoroutine("Attack_In_Melody_Main_Cor", _BGM);
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
                
                break;
        }
    }

    public void Stop_Phase2() {

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
    private IEnumerator Attack_In_Melody_Main_Cor(AunnBGMManager _BGM) {
        can_Attack = false;

        AunnController _controller = GetComponent<AunnController>();
        AunnEffect _effect = GetComponentInChildren<AunnEffect>();
        MoveConstSpeed _move_Const = GetComponent<MoveConstSpeed>();
        MoveTwoPoints _move_Two_Points = GetComponent<MoveTwoPoints>();
        ChildColliderTrigger foot_Collision = transform.Find("Foot").GetComponent<ChildColliderTrigger>();

        //移動        
        _controller.Change_Fly_Parameter();
        if (transform.position.x < 32) {
            _controller.Change_Animation("JumpBool");            
            _move_Two_Points.Start_Move(new Vector3(200f, transform.position.y), 2);
            yield return new WaitUntil(_move_Two_Points.End_Move);
            _effect.Jump_And_Landing_Effect();
            yield return new WaitForSeconds(1.0f);
        }

        //サビまでの時間計測
        float wait_Time = _BGM.BGM_TIME[5] - _BGM.Get_Now_BGM_Time();
        if (wait_Time < 0 || wait_Time > 10)
            wait_Time = 2;

        //移動
        _controller.Change_Animation("ShootPoseBool");
        transform.localScale = new Vector3(1, 1, 1);
        _effect.Start_Charge_Effect(wait_Time);
        _move_Const.Start_Move(new Vector3(180f, 0), 1);
        yield return new WaitForSeconds(wait_Time);

        //弾幕
        while (_BGM.Get_Now_Melody() == AunnBGMManager.Melody.main) {
            _effect.Play_Yellow_Circle_Effect();
            _effect.Play_Burst_Effect_Red();
            _shoot.Shoot_Dog_Bullet();
            yield return new WaitForSeconds(10.0f);

            _effect.Play_Yellow_Circle_Effect();
            _effect.Play_Burst_Effect_Red();
            _shoot.Shoot_Dog_Bullet_Big();
            yield return new WaitForSeconds(8.0f);
        }
        
        _controller.Change_Land_Parameter();
        yield return new WaitUntil(foot_Collision.Hit_Trigger);
        _controller.Change_Animation("SquatBool");

        can_Attack = true;
    }


    //フェーズ切り替え時の攻撃
    private IEnumerator Phase_Change_Attack_Cor() {
        yield return new WaitForSeconds(5.0f);
        can_Attack = true;
    }
}
