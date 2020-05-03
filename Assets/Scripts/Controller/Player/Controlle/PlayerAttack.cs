using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    [SerializeField] private GameObject mantis_Attack_Bullet;

    //コンポーネント
    private PlayerController _controller;
    private PlayerSoundEffect player_SE;
    private Animator _anim;
    private Rigidbody2D _rigid;
    private PlayerAttackCollision attack_Collision;
    private PlayerKickCollision kick_Collision;
    private PlayerManager player_Manager;

    private bool can_Attack = true;    


    // Use this for initialization
    void Awake () {
        //取得
        _controller = GetComponent<PlayerController>();
        player_SE = GetComponentInChildren<PlayerSoundEffect>();
        _anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        attack_Collision = GetComponentInChildren<PlayerAttackCollision>();
        kick_Collision = GetComponentInChildren<PlayerKickCollision>();
        player_Manager = PlayerManager.Instance;
        //オブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(mantis_Attack_Bullet, 5);
    }


    #region Attack

    //攻撃用フィールド変数
    private float attack_Time = 0.18f;
    private float attack_Span = 0.17f;
    private bool knock_Back = true;

    private void Set_Attack_Status(float attack_Time, float attack_Span, bool knock_Back) {
        this.attack_Time = attack_Time;
        this.attack_Span = attack_Span;
        this.knock_Back = knock_Back;
    }


    //攻撃
    public void Attack() {
        if (can_Attack) {
            //オプションによって変える
            switch (player_Manager.Get_Option()) {
                case PlayerManager.Option.none:     Set_Attack_Status(0.18f, 0.17f, true); break;
                case PlayerManager.Option.bee:      Set_Attack_Status(0.18f, 0.01f, true); break;
                case PlayerManager.Option.butterfly: Set_Attack_Status(0.18f, 0.13f, false); break;
                case PlayerManager.Option.mantis:   Set_Attack_Status(0.24f, 0.17f, true); break;
                case PlayerManager.Option.spider:   Set_Attack_Status(0.18f, 0.17f, true); break;
            }
            StartCoroutine("Attack_Cor");
        }
    }


    //攻撃用コルーチン
    public IEnumerator Attack_Cor() {
        can_Attack = false;       
        _anim.SetTrigger("AttackTrigger");
        attack_Collision.Make_Collider_Appear(attack_Time);        
        player_SE.Play_Attack_Sound();

        if (player_Manager.Get_Option() == PlayerManager.Option.bee)    //オプションが蜂の時ショット
            Bee_Shoot();
        else if (player_Manager.Get_Option() == PlayerManager.Option.mantis) //オプションがカマキリの時ショット
            Mantis_Shoot();
        else if (player_Manager.Get_Option() == PlayerManager.Option.butterfly)  //オプションがチョウの時浮く
            _rigid.velocity = new Vector2(_rigid.velocity.x, 200f);
        
        _rigid.velocity += new Vector2(transform.localScale.x * 5f, 0); //Rigidbodyのスリープ状態を解除する        
        for (float t = 0; t < attack_Time; t += Time.deltaTime) {
            //敵と衝突時
            if (attack_Collision.Hit_Trigger()) {                
                StartCoroutine("Do_Hit_Attack_Process");
                yield return new WaitForSeconds(0.05f);                
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(attack_Span);
        can_Attack = true;
    }

    //オプションが蜂の時のショット    
    private void Bee_Shoot() {
        ObjectPool bullet_Pool = ObjectPoolManager.Instance.Get_Pool("PlayerBeeBullet");
        for (int i = 0; i < 3; i++) {
            var bullet = bullet_Pool.GetObject();
            bullet.transform.position = transform.position + new Vector3(0, -6f + i * 6f);
            bullet.transform.localScale = transform.localScale;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(900f * transform.localScale.x, 0);
        }
        player_SE.Play_Shoot_Sound();
    }

    //オプションがカマキリの時ショット
    private void Mantis_Shoot() {
        ObjectPool bullet_Pool = ObjectPoolManager.Instance.Get_Pool("PlayerMantisAttackBullet");
        float angle;
        Vector3 pos;
        for(int i = 0; i < 8; i++) {
            var bullet = bullet_Pool.GetObject();
            angle = Random.Range(-30f, 70f) * Mathf.Deg2Rad;
            pos = new Vector2(Mathf.Cos(angle) * transform.localScale.x, Mathf.Sin(angle) * 0.5f) * 24f;
            bullet.transform.position = transform.position + pos;            
            bullet.GetComponent<Rigidbody2D>().velocity = (bullet.transform.position - transform.position) * Random.Range(20f, 120f);
            bullet.GetComponent<Rigidbody2D>().velocity += _rigid.velocity;
            bullet.GetComponent<Bullet>().Set_Inactive(5.0f);
        }
        player_SE.Play_Shoot_Sound();
    }
    
    //敵と衝突時の処理
    private IEnumerator Do_Hit_Attack_Process() {
        if (knock_Back) {
            float force = _controller.is_Landing ? 170f : 30f;                  //ノックバック
            _rigid.velocity = new Vector2(force * -transform.localScale.x, 10f);
        }        
        player_SE.Play_Hit_Attack_Sound();                                  //効果音                                                               
        float tmp = Time.timeScale;                                         //ヒットストップ        
        Time.timeScale = 0.4f;
        yield return new WaitForSeconds(0.06f);
        Time.timeScale = tmp;

    }

    #endregion 


    #region Kick  

    //キック用フィールド変数
    private bool end_Kick = false;
    private bool accept_Input = true;

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
        while (!can_Attack) {
            yield return null;
            loop_Count++;
            if (loop_Count > 15) {
                accept_Input = true;
                yield break;
            }
        }
        can_Attack = false;
        _controller.Set_Is_Playable(false);
        _controller.Change_Animation("KickBool");

        //キック開始
        _rigid.velocity = new Vector2(transform.localScale.x * Kick_Velocity(), -Kick_Velocity());
        kick_Collision.Make_Collider_Appear();
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

        float time = is_Sliding ? 0 : 0.05f;
        yield return new WaitForSeconds(time);
        
        can_Attack = true;
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
    #endregion
}
