using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Okina : BossEnemy {

    [Space]
    [SerializeField] private MovieSystem before_Movie;
    [SerializeField] private MovieSystem before_Movie_Skip;
    [SerializeField] private MovieSystem clear_Movie;
    [Space]
    [SerializeField] private GameObject back_Design;

    private OkinaAttack _attack;
    private Animator _anim;


    void Start() {
        //取得
        _attack = GetComponent<OkinaAttack>();
        _anim = GetComponent<Animator>();
        //ムービー
        if (SceneManagement.Instance.Is_First_Visit()) {
            before_Movie.Start_Movie();
        }
        else {
            before_Movie_Skip.Start_Movie();
        }
    }
    

    public override void Start_Battle() {        
        Play_Battle_Effect();
        Change_Animation("AttackBool");
        GetComponentInChildren<MelodyManager>().Start_Time_Count();
        BGMManager.Instance.Change_BGM("Stage6_Boss");
        base.Start_Battle();
    }


    protected override void Clear() {
        base.Clear();
        _attack.Stop_Attack();
        Change_Animation("IdleBool");
    }


    protected override void Do_After_Clear_Process() {
        base.Do_After_Clear_Process();
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
        back_Design.transform.localScale = new Vector3(0, 0, 0);
        back_Design.SetActive(true);
    }

    //戦闘終了時の先頭エフェクト消す
    public void Delete_Battle_Effect() {
        BackGroundEffector.Instance.Change_Color_Default(1f);
        back_Design.SetActive(false);
    }


    //アニメーション変更
    public void Change_Animation(string next_Parameter) {        
        _anim.SetBool("IdleBool", false);
        _anim.SetBool("AttackBool", false);
        _anim.SetBool("DisappearBool", false);

        _anim.SetBool(next_Parameter, true);
    }
}
