using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AunnCopy : Enemy {

    private Animator _anim;
    private GameObject main_Body;
    private AunnController main_Controller;

    private string now_Anim_Param;
    private bool is_Symmetry = false;

    private Vector3 main_Pos;
   

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
        Limit_Transition();
    }


    //本体と同じ動きをする
    private void Copy_Main() {
        //本体とアニメーション同じにする
        if (now_Anim_Param != main_Controller.Get_Now_Anim_Param()) {
            now_Anim_Param = main_Controller.Get_Now_Anim_Param();
            foreach (AnimatorControllerParameter param in _anim.parameters) {
                _anim.SetBool(param.name, false);
            }
            _anim.SetBool(now_Anim_Param, true);
        }
        //本体と当たり判定を同じにする
        if (main_Body.layer != gameObject.layer) {
            gameObject.layer = main_Body.layer;
        }

        //向き、座標
        if (is_Symmetry) {
            transform.localScale = new Vector3(-main_Body.transform.localScale.x, 1, 1);
            Symmetry_Transition();
        }
        else {
            transform.localScale = new Vector3(main_Body.transform.localScale.x, 1, 1);
            Copy_Transition();
        }
    }


    //左右対称の動き
    private void Symmetry_Transition() {   
        transform.position = new Vector3(-main_Body.transform.position.x, main_Body.transform.position.y, 1);
    }


    //本体と同じ動き
    private void Copy_Transition() {
        Vector3 move_Diff = main_Body.transform.position - main_Pos;
        main_Pos = main_Body.transform.position;
        transform.position += move_Diff;
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
        transform.SetParent(null);

        transform.position = position;
        main_Pos = main_Body.transform.position;        

        //生成直後は判定消す
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        StartCoroutine("Release_Invincible_Cor");
    }

    private IEnumerator Release_Invincible_Cor() {
        yield return new WaitForSeconds(1.5f);
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
    }


    //消す
    public void Delete_Copy() {
        Vanish();
    }


    //消滅時の処理
    public override void Vanish() {        
        base.Play_Vanish_Effect();
        transform.SetParent(main_Body.transform);
        gameObject.SetActive(false);
    }


    //存在しているか
    public bool Is_Exist() {
        return gameObject.activeSelf;
    }
}
