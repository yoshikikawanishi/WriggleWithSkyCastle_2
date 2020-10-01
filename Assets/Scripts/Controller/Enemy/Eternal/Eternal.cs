using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eternal : BossEnemy {

    [Space]
    [SerializeField] private MovieSystem before_Movie;
    [SerializeField] private MovieSystem before_Movie_Skip;
    [SerializeField] private MovieSystem clear_Movie;

    private Animator _anim;
    private EternalAttack _attack;    
    private MelodyManager melody_Manager;

    void Start() {
        //取得
        _anim = GetComponent<Animator>();        
        _attack = GetComponent<EternalAttack>();
        melody_Manager = GetComponentInChildren<MelodyManager>();
        //ムービー
        if (SceneManagement.Instance.Is_First_Visit()) {
            before_Movie.Start_Movie();
        }
        else {
            before_Movie_Skip.Start_Movie();
        }
    }


    public override void Start_Battle() {
        base.Start_Battle();
        Play_Battle_Effect();
        melody_Manager.Start_Time_Count();
    }


    protected override void Clear() {
        base.Clear();
        _attack.Stop_Attack();
    }


    protected override void Do_After_Clear_Process() {
        base.Do_After_Clear_Process();
        clear_Movie.Start_Movie();
        Delete_Battle_Effect();
    }


    //戦闘エフェクト(背景色、模様)
    public void Play_Battle_Effect() {
        
    }

    //戦闘終了時の先頭エフェクト消す
    public void Delete_Battle_Effect() {
        
    }


    public void Change_Animation(string next_Parameter) {
        foreach (AnimatorControllerParameter param in _anim.parameters) {
            if (param.name.Contains("Bool")) {
                _anim.SetBool(param.name, false);
            }
        }
        if (next_Parameter.Contains("Bool")) {
            _anim.SetBool(next_Parameter, true);
        }
        else if (next_Parameter.Contains("Trigger")) {            
            _anim.SetTrigger(next_Parameter);            
        }
    }
}
