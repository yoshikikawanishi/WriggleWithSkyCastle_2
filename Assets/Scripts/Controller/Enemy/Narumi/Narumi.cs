﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narumi : BossEnemy {

    private Animator _anim;
    private NarumiAttack _attack;
    [SerializeField] private MelodyManager _melody;


    void Start() {
        _anim = GetComponent<Animator>();
        _attack = GetComponent<NarumiAttack>();
    }


    public override void Start_Battle() {
        base.Start_Battle();
        _melody.Start_Time_Count();
        Play_Battle_Effect();
    }


    protected override void Clear() {
        base.Clear();
        _attack.Stop_Attack();
    }


    protected override void Do_After_Clear_Process() {
        base.Do_After_Clear_Process();
        Delete_Battle_Effect();
    }


    //アニメーション変更
    public void Change_Animation(string next_Parameter) {
        _anim.SetBool("IdleBool", false);
        _anim.SetBool("AttackBool", false);
        _anim.SetBool("DropBool", false);
        _anim.SetBool("MoveForwardBool", false);
        _anim.SetBool("MoveBackBool", false);

        _anim.SetBool(next_Parameter, true);
    }    


    public void Change_Animation(string next_Parameter, int scale_X) {
        int x = scale_X.CompareTo(0);
        if (x == 0)
            x = 1;
        transform.localScale = new Vector3(scale_X, 1, 1);
        Change_Animation(next_Parameter);
    }


    //戦闘エフェクト(背景色、模様)
    public void Play_Battle_Effect() {
        BackGroundEffector.Instance.Start_Change_Color(new Color(0.3f, 0.3f, 0.38f), 1);
    }

    //戦闘終了時の先頭エフェクト消す
    public void Delete_Battle_Effect() {
        BackGroundEffector.Instance.Change_Color_Default(1f);
    }
}
