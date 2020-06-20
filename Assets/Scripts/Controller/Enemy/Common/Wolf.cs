using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour {

    private enum STATE {
        hide,
        jump,
        dash
    }
    private STATE state = STATE.hide;

    private Rigidbody2D _rigid;
    private GameObject player;

    private const int move_Length_Dive = 64;
    private const int move_Length_Dash = 64;
    private const float move_Speed_Dive = 1.3f;
    private const float move_Speed_Dash = 2f;
    private const float interval = 1.0f;
    private const float jump_Speed = 250f;

    private int move_Length;
    private float move_Speed;
    private int move_Count = 0;    


    // Use this for initialization
    void Start () {
        _rigid = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("PlayerTag");

        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        move_Length = move_Length_Dive;
        move_Speed = move_Speed_Dive;
        move_Count = (int)((move_Length / move_Speed) / 2.0f);
    }
	

    private void FixedUpdate() {
        //隠れ中
        if (state == STATE.hide) {
            //自機が来たらジャンプ
            if (Is_Player_Above_This()) {
                StartCoroutine("Wolf_Action_Cor");
                state = STATE.jump;
            }
        }

        //横移動
        if (state != STATE.jump) {
            if (move_Count * move_Speed < move_Length) {
                transform.position += new Vector3(move_Speed * -transform.localScale.x, 0) * Time.timeScale;
                move_Count++;
            }
            else {
                move_Count = 0;
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            }
        }
    }


    //自機が上に来た時trueを返す
    private bool Is_Player_Above_This() {
        if (player == null)
            return false;

        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 16f) {
            if (player.transform.position.y < transform.position.y + 64f)
                return true;
        }
        return false;
    }


    //ジャンプ、着地後に横移動開始
    private IEnumerator Wolf_Action_Cor() {
        //取得        
        Animator _anim = GetComponent<Animator>();
        AudioSource _audio = GetComponent<AudioSource>();
        ChildColliderTrigger body_Collider = GetComponentInChildren<ChildColliderTrigger>();

        yield return new WaitForSeconds(interval);

        //ジャンプ
        _audio.Play();
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        _anim.SetTrigger("JumpTrigger");        
        _rigid.velocity = new Vector2(0, jump_Speed);        
        
        while(_rigid.velocity.y > -10f) { yield return null; }

        //下降        
        _anim.SetTrigger("FallTrigger");        

        //着地、横移動開始
        yield return new WaitUntil(body_Collider.Hit_Trigger);
        _anim.SetTrigger("DashTrigger");
        state = STATE.dash;
        move_Length = move_Length_Dash;
        move_Speed = move_Speed_Dash;
    }

}
