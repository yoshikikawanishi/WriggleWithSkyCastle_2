using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatoMai : BossEnemy {

    [Space]
    [SerializeField] private MovieSystem before_Movie;
    [SerializeField] private MovieSystem before_Movie_Skip;
    [SerializeField] private MovieSystem clear_Movie;
    [Space]
    [SerializeField] public GameObject satomai;
    [SerializeField] public GameObject satono;
    [SerializeField] public GameObject mai;
    [Space]
    [SerializeField] private GameObject back_Design;

    private SatoMaiAttack _attack;
    private Animator _anim;
    private MelodyManager melody_Manager;


    void Start() {
        //取得
        _attack = GetComponent<SatoMaiAttack>();
        _anim = GetComponent<Animator>();
        melody_Manager = GetComponentInChildren<MelodyManager>();
        //戦闘前ムービー開始
        if (SceneManagement.Instance.Is_First_Visit())
            before_Movie.Start_Movie();
        else
            before_Movie_Skip.Start_Movie();
    }


    protected override void Play_Damaged_Effect(string damaged_Tag) {
        base.Play_Damaged_Effect(damaged_Tag);
        StartCoroutine("Blink_Cor");
    }

    private IEnumerator Blink_Cor() {
        satomai.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.25f, 0.25f, 1);
        satono.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.25f, 0.25f, 1);
        mai.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.25f, 0.25f, 1);
        yield return new WaitForSeconds(0.1f);
        satomai.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        satono.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        mai.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
    }


    //戦闘開始時の処理
    public override void Start_Battle() {
        base.Start_Battle();
        melody_Manager.Start_Time_Count();
        Play_Battle_Effect();
        BGMManager.Instance.Change_BGM("Stage5_Boss");
    }


    //クリア時の処理
    protected override void Clear() {
        base.Clear();
        _attack.Stop_Attack();
        Become_Invincible();
    }


    //クリア後の処理
    //ムービー開始
    protected override void Do_After_Clear_Process() {
        base.Do_After_Clear_Process();
        clear_Movie.Start_Movie();        
        Delete_Battle_Effect();
    }


    public void Change_Animation(string next_Bool) {
        //座標そろえる
        if(next_Bool == "IdleBool" || next_Bool == "RollingRushing") {
            if (!satomai.activeSelf) {
                satomai.transform.position = satono.transform.position;
            }
        }
        else {
            if (satomai.activeSelf) {
                satono.transform.position = satomai.transform.position;
                mai.transform.position = satomai.transform.position;
            }
        }
        foreach(AnimatorControllerParameter param in _anim.parameters) {
            if (param.name.Contains("Bool")) {
                _anim.SetBool(param.name, false);
            }
        }
        _anim.SetBool(next_Bool, true);
    }


    //無敵化
    public void Become_Invincible() {
        satomai.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
        satono.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
        mai.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
        satomai.layer = LayerMask.NameToLayer("InvincibleLayer");
        satono.layer = LayerMask.NameToLayer("InvincibleLayer");
        mai.layer = LayerMask.NameToLayer("InvincibleLayer");
    }


    //無敵解除
    public void Release_Invincible() {
        satomai.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
        satono.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
        mai.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
        satomai.layer = LayerMask.NameToLayer("EnemyLayer");
        satono.layer = LayerMask.NameToLayer("EnemyLayer");
        mai.layer = LayerMask.NameToLayer("EnemyLayer");
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
}
