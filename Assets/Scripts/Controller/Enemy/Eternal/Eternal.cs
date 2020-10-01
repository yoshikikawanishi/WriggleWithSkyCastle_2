using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eternal : BossEnemy {

    [Space]
    [SerializeField] private MovieSystem before_Movie;
    [SerializeField] private MovieSystem before_Movie_Skip;
    [SerializeField] private MovieSystem clear_Movie;

    private EternalAttack _attack;
    private MelodyManager melody_Manager;

    void Start() {
        //取得
        melody_Manager = GetComponentInChildren<MelodyManager>();
        _attack = GetComponent<EternalAttack>();
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
}
