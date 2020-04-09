using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AunnCopy : Enemy {

    private Animator _anim;
    private GameObject main_Body;
    private AunnController main_Controller;

    private string now_Anim_Param;
    private bool is_Symmetry = false;
   

    void Start() {
        //取得
        _anim = GetComponent<Animator>();
        main_Body = transform.parent.gameObject;
        main_Controller = main_Body.GetComponent<AunnController>();

        //消す
        gameObject.SetActive(false);
    }


    void LateUpdate() {
        Copy_Main();
        if (is_Symmetry) {
            Symmetry_Transition();
        }
        Limit_Transition();
    }
    
    
    //本体と同じ動きをする
    private void Copy_Main() {
        //本体とアニメーション同じにする
        if (now_Anim_Param != main_Controller.Get_Now_Anim_Param()) {
            now_Anim_Param = main_Controller.Get_Now_Anim_Param();
            foreach(AnimatorControllerParameter param in _anim.parameters) {
                _anim.SetBool(param.name, false);
            }
            _anim.SetBool(now_Anim_Param, true);
        }
        //本体と当たり判定を同じにする
        if(main_Body.layer != gameObject.layer) {
            gameObject.layer = main_Body.layer;
        }

        if (is_Symmetry)
            return;

        //向き
        if (!Mathf.Approximately(transform.localScale.x, 1)) {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }


    //左右対称の動き
    private void Symmetry_Transition() {
        if (!Mathf.Approximately(transform.localScale.x, -1)) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        transform.position = new Vector3(-main_Body.transform.position.x, main_Body.transform.position.y, 1);
    }


    //画面外に出ないよう移動を制限する
    private void Limit_Transition() {
        if(transform.position.x > 220f) {
            transform.position = new Vector3(220f, transform.position.y);
        }
        else if(transform.position.x < -220f) {
            transform.position = new Vector3(-220f, transform.position.y);
        }
    }


    //生成する
    public void Create_Copy(int life, bool is_Symmetry, Vector2 position) {
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        base.Set_Life(life);
        this.is_Symmetry = is_Symmetry;
        gameObject.SetActive(true);
        transform.position = position;
    }


    //消す
    public void Delete_Copy() {
        Vanish();
    }


    //消滅時の処理
    public override void Vanish() {        
        base.Play_Vanish_Effect();
        gameObject.SetActive(false);
    }


    //存在しているか
    public bool Is_Exist() {
        return gameObject.activeSelf;
    }
}
