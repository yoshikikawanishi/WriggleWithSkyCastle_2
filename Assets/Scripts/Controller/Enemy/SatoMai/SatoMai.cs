using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatoMai : BossEnemy {

    [Space]
    [SerializeField] private MovieSystem before_Movie;
    [SerializeField] private MovieSystem before_Movie_Skip;
    [SerializeField] private MovieSystem clear_Movie;

    private Animator _anim;
    private MelodyManager melody_Manager;


    void Start() {
        //取得
        _anim = GetComponent<Animator>();
        melody_Manager = GetComponentInChildren<MelodyManager>();
        //戦闘前ムービー開始
        if (SceneManagement.Instance.Is_First_Visit())
            before_Movie.Start_Movie();
        else
            before_Movie_Skip.Start_Movie();
    }


    //戦闘開始時の処理
    public override void Start_Battle() {
        base.Start_Battle();
        melody_Manager.Start_Time_Count();
    }


    //クリア後の処理
    //ムービー開始
    protected override void Do_After_Clear_Process() {
        base.Do_After_Clear_Process();
        clear_Movie.Start_Movie();
    }


    public void Change_Animation(string next_Bool) {
        foreach(AnimatorControllerParameter param in _anim.parameters) {
            if (param.name.Contains("Bool")) {
                _anim.SetBool(param.name, false);
            }
        }
        _anim.SetBool(next_Bool, true);
    }
    
}
