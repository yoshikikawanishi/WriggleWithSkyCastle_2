using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aunn : BossEnemy {

    //コンポーネント
    private AunnAttack _attack;    
    private Rigidbody2D _rigid;
    private CapsuleCollider2D _collider;

    public readonly AunnBGMManager _BGM = new AunnBGMManager();

    //戦闘開始
    private bool start_Battle = false;

    //初期値
    private float default_Gravity;


    new void Awake() {
        base.Awake();
        //取得
        _attack = GetComponent<AunnAttack>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        default_Gravity = _rigid.gravityScale;

    }


    new void Update() {
        base.Update();
        if (start_Battle) {
            switch (Get_Now_Phase()) {
                case 1: _attack.Phase1(_BGM); break;
                case 2: _attack.Phase2(_BGM); break;
            }
        }
    }


    //戦闘開始
    public void Start_Battle() {
        _BGM.Start_Time_Count();
        start_Battle = true;
    }

    //クリア時の処理
    public void Clear() {
        start_Battle = false;
        _attack.Stop_Phase2();
    }


    //地上用パラメータに
    public void Change_Land_Parameter() {
        _collider.isTrigger = false;
        _rigid.gravityScale = default_Gravity;
    }


    //空中用パラメータに
    public void Change_Fly_Parameter() {
        _collider.isTrigger = true;
        _rigid.gravityScale = 0;
        _rigid.velocity = Vector2.zero;
    }
     
}
