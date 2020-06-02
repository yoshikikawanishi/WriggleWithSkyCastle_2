using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nemuon : BossEnemy {

    //背景エフェクト
    [SerializeField] private GameObject back_Design;

    //コンポーネント
    private Animator _anim;
    private Rigidbody2D _rigid;
    private CapsuleCollider2D _collider;    
    private NemunoAttack _attack;
    public NemunoBGMTimeKeeper _BGM;

    //戦闘開始
    private bool start_Battle = false;


    new void Awake() {
        base.Awake();
        //取得
        _anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();        
        _attack = GetComponent<NemunoAttack>();
        _BGM = new NemunoBGMTimeKeeper();
    }
   

	// Update is called once per frame
	new void Update () {
        base.Update();
        if (state == State.battle) {
            switch (Get_Now_Phase()) {
                case 1: _attack.Phase1(_BGM); break;
                case 2: _attack.Phase2(_BGM); break;
            }
        }
	}


    //アニメーション変更
    public void Change_Animation(string next) {
        foreach(AnimatorControllerParameter param in _anim.parameters) {
            if(param.name.Contains("Bool"))
                _anim.SetBool(param.name, false);            
        }
        if(next.Contains("Bool"))
            _anim.SetBool(next, true);
        if (next.Contains("Trigger"))
            _anim.SetTrigger(next);
    }


    //戦闘開始
    public override void Start_Battle() {
        base.Start_Battle();
        start_Battle = true;
        _BGM.Start_Time_Count();
    }


    //クリア時の処理
    protected override void Do_After_Clear_Process() {
        base.Do_After_Clear_Process();    
        _attack.Stop_Phase2();
        GameObject.Find("Scripts").GetComponent<Stage2_BossMovie>().Start_Clear_Movie();
    }


    //空に飛ぶとき、重力を消してあたりはんていをtriggerにする
    public void Change_Fly_Parameter() {
        _rigid.gravityScale = 0;
        _rigid.velocity = Vector2.zero;
        _collider.isTrigger = true;
    }

    //地上に降りるとき、重力を付けて当たり判定をつける
    public void Change_Land_Paramter() {
        _rigid.gravityScale = 32f;
        _rigid.velocity = Vector2.zero;
        _collider.isTrigger = false;
    }


    //-------------エフェクト--------------
    public void Play_Charge_Effect(float duration) {
        transform.Find("Effects").GetChild(0).gameObject.SetActive(true);
        Invoke("Stop_Charge_Effect", duration);
    }

    public void Stop_Charge_Effect() {
        transform.Find("Effects").GetChild(0).gameObject.SetActive(false);
    }

    public void Play_Small_Charge_Effect() {
        transform.Find("Effects").GetChild(1).GetComponent<ParticleSystem>().Play();
    }

    public void Play_Burst_Effect() {
        transform.Find("Effects").GetChild(2).GetComponent<ParticleSystem>().Play();
    }

    public void Play_Small_Burst_Effect() {
        transform.Find("Effects").GetChild(3).GetComponent<ParticleSystem>().Play();
    }

    public void Play_Yellow_Circle_Effect() {
        var orgin = transform.Find("Effects").GetChild(4).gameObject;
        var obj = Instantiate(orgin);
        obj.transform.position = transform.position;
        obj.SetActive(true);        
    }

    public void Play_Slash_Effect() {
        var obj = transform.Find("Effects").GetChild(5).gameObject;        
        obj.GetComponent<Animator>().SetTrigger("PlayEffectTrigger");
    }

    public void Play_Purple_Circle_Effect() {
        var orgin = transform.Find("Effects").GetChild(6).gameObject;
        var obj = Instantiate(orgin);
        obj.transform.position = transform.position;
        obj.SetActive(true);
    }

    //戦闘中の画面エフェクト
    public void Play_Battle_Effect() {
        back_Design.SetActive(true);
        back_Design.transform.localScale = new Vector3(0, 0, 1);
        BackGroundEffector.Instance.Start_Change_Color(new Color(0.45f, 0.35f, 0.35f), 0.1f);
    }

    //戦闘中の画面エフェクト消す
    public void Quit_Battle_Effect() {
        back_Design.SetActive(false);
        BackGroundEffector.Instance.Start_Change_Color(new Color(0.85f, 0.8f, 0.7f), 0.1f);
    }

}
