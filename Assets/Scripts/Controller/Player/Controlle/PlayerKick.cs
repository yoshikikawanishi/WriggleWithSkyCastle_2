using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKick : MonoBehaviour {

    private enum Kind {
        kick,
        sliding,
        charge_Kick,
        charge_Sliding
    }
    private Kind kind;

    private enum State {
        idle,
        charging,
        full_Charge,
    }
    private State state;

    //コンポーネント
    private PlayerController _controller;
    private PlayerShoot _shoot;
    private PlayerBodyCollision player_Body;
    private PlayerEffect player_Effect;
    private PlayerSoundEffect player_SE;
    private Rigidbody2D _rigid;    
    private PlayerKickCollision kick_Collision;
    
    private bool end_Kick = false;
    private bool accept_Input = true;
    private float charge_Time = 0;
    private float full_Charge_Span = 1.0f;


    // Use this for initialization
    void Awake() {
        //取得
        _controller = GetComponent<PlayerController>();
        _shoot = GetComponent<PlayerShoot>();
        player_Effect = GetComponentInChildren<PlayerEffect>();
        player_SE = GetComponentInChildren<PlayerSoundEffect>();
        player_Body = GetComponentInChildren<PlayerBodyCollision>();        
        _rigid = GetComponent<Rigidbody2D>();        
        kick_Collision = GetComponentInChildren<PlayerKickCollision>();             
    }
   

    //チャージキックのチャージ, Updateで呼ぶこと
    public void Charge() {
        if (Is_Complete_Charge())
            return;
        //チャージ開始
        if (state == State.idle) {
            charge_Time += Time.deltaTime;
            if (charge_Time > 0.5f) {
                state = State.charging;
                player_Effect.Play_Charge_Kick_Charge_Effect();
            }
        }
        //チャージ
        if (charge_Time < full_Charge_Span) {
            charge_Time += Time.deltaTime;            
        }
        //チャージ完了
        else if (state != State.full_Charge) {            
            player_Effect.Start_Full_Charge_Blink();
            player_SE.Finish_Charge_Kick_Charge();
            state = State.full_Charge;            
        }
    }
    
    
    public void Quit_Charge(bool release_Full_Charge) {
        if(state == State.idle) {
            return;
        }
        else if (state == State.charging) {         
            player_Effect.Stop_Charge_Kick_Charge_Effect();
            charge_Time = 0;
            state = State.idle;
        }
        else if(state == State.full_Charge) {
            if (release_Full_Charge) {
                player_Effect.Quit_Full_Charge_Blink();
                charge_Time = 0;
                state = State.idle;
            }
        }                
    }


    public bool Is_Complete_Charge() {
        if(state == State.full_Charge) {
            return true;
        }
        return false;
    }


    //キック
    public void Kick() {        
        if (accept_Input) {
            kick_Collision.is_Hit_Kick = false;
            if (Is_Complete_Charge()) {
                if (_controller.is_Landing)
                    StartCoroutine("Kick_Cor", Kind.charge_Sliding);
                else
                    StartCoroutine("Kick_Cor", Kind.charge_Kick);
            }
            else {
                if (_controller.is_Landing)
                    StartCoroutine("Kick_Cor", Kind.sliding);
                else
                    StartCoroutine("Kick_Cor", Kind.kick);
            }
        }
    }


    private IEnumerator Kick_Cor(Kind kind) {        
        accept_Input = false;

        //入力受付後500フレーム以内にキック可能になればキック
        float loop_Count = 0;
        while (!_controller.can_Attack) {
            yield return null;
            loop_Count++;
            if (loop_Count > 500) {
                accept_Input = true;
                yield break;
            }
        }

        //キック開始        
        _controller.can_Attack = false;
        _controller.Set_Is_Playable(false);
        _controller.Change_Animation("KickBool");
        
        player_Body.Change_Collider_Size(new Vector2(10, 12), new Vector2(0, -6));
        player_SE.Play_Kick_Sound();

        if (kind == Kind.kick || kind == Kind.sliding) {
            kick_Collision.Make_Collider_Appear(false);
            StartCoroutine("Kicking_Cor");
        }
        else {
            kick_Collision.Make_Collider_Appear(true);
            player_SE.Play_Charge_Shoot_Sound();
            Quit_Charge(true);
            player_Body.Become_Invincible();
            StartCoroutine("Charge_Kicking_Cor");
        }       
       
        yield return new WaitUntil(End_Kick);

        //キック終了
        if (_controller.is_Landing)
            _controller.Change_Animation("IdleBool");
        else
            _controller.Change_Animation("JumpBool");

        _controller.Set_Is_Playable(true);
        kick_Collision.Make_Collider_Disappear();
        player_Body.Back_Default_Collider();

        if (kind == Kind.charge_Kick || kind == Kind.charge_Sliding) {
            player_Body.Release_Invincible();
        }

        _controller.can_Attack = true;
        accept_Input = true;
    }


    //キック発生中の処理
    private IEnumerator Kicking_Cor() {
        float speed = Kick_Velocity();
        _rigid.velocity = new Vector2(speed * transform.localScale.x, -speed);
        
        for (float t = 0; t < 0.35f; t += Time.deltaTime) {

            _rigid.velocity = new Vector2(speed * transform.localScale.x, _rigid.velocity.y);
            _controller.Change_Animation("KickBool");

            //敵と衝突時の処理
            if (kick_Collision.Hit_Trigger()) {
                Do_Hit_Kick_Process();
                accept_Input = true;
                yield return new WaitForSeconds(0.05f);
                break;
            }
            yield return null;
        }
        end_Kick = true;
    }

   
    //チャージキック
    private IEnumerator Charge_Kicking_Cor() {                       
        float speed = Charge_Kick_Velocity();
        _rigid.velocity = new Vector2(speed * transform.localScale.x, -speed);

        for (float t = 0; t < 0.5f; t += Time.deltaTime) {

            _rigid.velocity = new Vector2(speed * transform.localScale.x, _rigid.velocity.y);
            _controller.Change_Animation("KickBool");

            //敵と衝突時の処理
            if (kick_Collision.Hit_Trigger()) {
                Do_Hit_Charge_Kick_Process();
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

    private void Do_Hit_Charge_Kick_Process() {
        _shoot.Shoot_Charge_Kick_Shoot();
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

        if (power < 100) {
            speed = 180f;
        }
        else if (power < 200) {
            speed = 195f;
        }
        else if (power < 300) {
            speed = 210f;
        }
        else {
            speed = 225f;
        }

        //文のアイテムを持っていたら上げる
        if (CollectionManager.Instance.Is_Collected("Aya"))
            speed *= 1.2f;

        return speed;
    }


    //チャージキックの速度
    private float Charge_Kick_Velocity() {
        return 400f;
    }
}
