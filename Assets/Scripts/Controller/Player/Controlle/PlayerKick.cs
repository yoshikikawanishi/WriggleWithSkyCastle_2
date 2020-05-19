using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKick : MonoBehaviour {


    //コンポーネント
    private PlayerController _controller;
    private PlayerBodyCollision player_Body;
    private PlayerEffect player_Effect;
    private PlayerSoundEffect player_SE;
    private Animator _anim;
    private Rigidbody2D _rigid;    
    private PlayerKickCollision kick_Collision;
    private PlayerManager player_Manager;

    //キック用フィールド変数
    private bool end_Kick = false;
    private bool accept_Input = true;
    private bool is_Charge_Kick_Charging = false;
    private float kick_Charge_Time = 0;


    // Use this for initialization
    void Awake() {
        //取得
        _controller = GetComponent<PlayerController>();
        player_Effect = GetComponentInChildren<PlayerEffect>();
        player_SE = GetComponentInChildren<PlayerSoundEffect>();
        player_Body = GetComponentInChildren<PlayerBodyCollision>();
        _anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();        
        kick_Collision = GetComponentInChildren<PlayerKickCollision>();
        player_Manager = PlayerManager.Instance;        
    }
   

    //チャージキックのチャージ, Updateで呼ぶこと
    public void Charge() {
        if (!is_Charge_Kick_Charging) {
            is_Charge_Kick_Charging = true;
            player_Effect.Play_Charge_Kick_Charge_Effect();
        }        
    }
    
    public void Quit_Charge() {
        if (is_Charge_Kick_Charging) {
            is_Charge_Kick_Charging = false;
            player_Effect.Stop_Charge_Kick_Charge_Effect();
        }
    }


    //キック
    public void Kick() {
        if (accept_Input) {
            kick_Collision.is_Hit_Kick = false;
            if (_controller.is_Landing)
                StartCoroutine("Kick_Cor", true);
            else
                StartCoroutine("Kick_Cor", false);
        }
    }


    private IEnumerator Kick_Cor(bool is_Sliding) {

        accept_Input = false;

        //入力受付後15フレーム以内にキック可能になればキック
        float loop_Count = 0;
        while (!_controller.can_Attack) {
            yield return null;
            loop_Count++;
            if (loop_Count > 15) {
                accept_Input = true;
                yield break;
            }
        }
        _controller.can_Attack = false;
        _controller.Set_Is_Playable(false);
        _controller.Change_Animation("KickBool");

        //キック開始
        _rigid.velocity = new Vector2(transform.localScale.x * Kick_Velocity(), -Kick_Velocity());
        kick_Collision.Make_Collider_Appear();
        player_Body.Change_Collider_Size(new Vector2(10, 12), new Vector2(0, -6));
        player_SE.Play_Kick_Sound();

        //キック中
        if (is_Sliding)
            StartCoroutine("Sliding_Cor");
        else
            StartCoroutine("Kicking_Cor");

        yield return new WaitUntil(End_Kick);

        //キック終了
        if (_controller.is_Landing)
            _controller.Change_Animation("IdleBool");
        else
            _controller.Change_Animation("JumpBool");

        _controller.Set_Is_Playable(true);
        kick_Collision.Make_Collider_Disappear();
        player_Body.Back_Default_Collider();

        float time = is_Sliding ? 0 : 0.05f;
        yield return new WaitForSeconds(time);

        _controller.can_Attack = true;
        accept_Input = true;
    }


    //キック発生中の処理
    private IEnumerator Kicking_Cor() {
        for (float t = 0; t < 0.35f; t += Time.deltaTime) {

            _rigid.velocity = new Vector2(transform.localScale.x * Kick_Velocity(), _rigid.velocity.y);
            _controller.Change_Animation("KickBool");

            //敵と衝突時の処理
            if (kick_Collision.Hit_Trigger()) {
                Do_Hit_Kick_Process();
                accept_Input = true;                    //敵に当たったとき次のキック入力を受け付ける
                yield return new WaitForSeconds(0.05f);
                break;
            }
            yield return null;
        }
        end_Kick = true;
    }

    //スライディング発生中の処理
    private IEnumerator Sliding_Cor() {
        for (float t = 0; t < 0.33f; t += Time.deltaTime) {

            _rigid.velocity = new Vector2(transform.localScale.x * Kick_Velocity(), _rigid.velocity.y);
            _controller.Change_Animation("KickBool");

            //敵と衝突時の処理
            if (kick_Collision.Hit_Trigger()) {
                Do_Hit_Kick_Process();
                yield return new WaitForSeconds(0.015f);
                break;
            }

            yield return null;
        }
        end_Kick = true;
    }


    //キックのヒット時の処理
    private void Do_Hit_Kick_Process() {
        _rigid.velocity = new Vector2(30f * -transform.localScale.x, 160f); //ノックバック        
        player_SE.Play_Hit_Attack_Sound();                                  //効果音        
    }


    //キック終了の確認用
    private bool End_Kick() {
        if (end_Kick) {
            end_Kick = false;
            return true;
        }
        return false;
    }

    //速度を変える
    private float Kick_Velocity() {
        //パワーによって変える
        int power = PlayerManager.Instance.Get_Power();
        float speed = 180f;

        if (power < 16) {
            speed = 180f;
        }
        else if (power < 32) {
            speed = 195f;
        }
        else if (power < 64) {
            speed = 210f;
        }
        else {
            speed = 225f;
        }

        //文のアイテムを持っていたら上げる
        if (CollectionManager.Instance.Is_Collected("Aya"))
            speed *= 1.5f;

        return speed;
    }
}
