using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Okina : BossEnemy {

    [Space]
    [SerializeField] private MovieSystem before_Movie;
    [SerializeField] private MovieSystem before_Movie_Skip;
    [SerializeField] private MovieSystem clear_Movie;


    void Start() {
        if (SceneManagement.Instance.Is_First_Visit()) {
            before_Movie.Start_Movie();
        }
        else {
            before_Movie_Skip.Start_Movie();
        }
    }


    public override void Start_Battle() {
        //BGMManager.Instance.Change_BGM()
        Play_Battle_Effect();
        GetComponentInChildren<MelodyManager>().Start_Time_Count();
        base.Start_Battle();
    }


    protected override void Clear() {
        base.Clear();
    }


    protected override void Do_After_Clear_Process() {
        clear_Movie.Start_Movie();
        Delete_Battle_Effect();
    }

    
    public void Become_Invincible() {
        GetComponentInChildren<BossChildCollision>().Become_Invincible();
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
    }


    public void Release_Invincible() {
        GetComponentInChildren<BossChildCollision>().Release_Invincible();
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }


    //戦闘エフェクト(背景色、模様)
    public void Play_Battle_Effect() {
        BackGroundEffector.Instance.Start_Change_Color(new Color(0.4f, 0.4f, 0.4f), 1);
    }

    //戦闘終了時の先頭エフェクト消す
    public void Delete_Battle_Effect() {
        BackGroundEffector.Instance.Change_Color_Default(1f);
    }
}
