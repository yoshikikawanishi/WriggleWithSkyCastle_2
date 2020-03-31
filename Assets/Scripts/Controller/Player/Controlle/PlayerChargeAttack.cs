using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeAttack : MonoBehaviour {

    //コンポーネント
    private PlayerManager player_Manager;
    private PlayerController _controller;
    private PlayerSoundEffect player_SE;
    private PlayerEffect player_Effect;
    private PlayerAttack _attack;
    private Animator _anim;
    private Rigidbody2D _rigid;
    private PlayerChargeAttackCollision attack_Collision;
    private CameraShake camera_Shake;

    private int player_Power;
    private int charge_Phase = 1;
    private float charge_Time = 0;
    private float[] charge_Span = new float[3] { 0.3f, 1.0f, 2.0f };
    

    private void Start() {
        //取得
        player_Manager = PlayerManager.Instance;
        _controller = GetComponent<PlayerController>();
        player_SE = GetComponentInChildren<PlayerSoundEffect>();
        player_Effect = GetComponentInChildren<PlayerEffect>();
        _attack = GetComponent<PlayerAttack>();
        _anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        attack_Collision = GetComponentInChildren<PlayerChargeAttackCollision>();
        camera_Shake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
    }


    //溜め攻撃用溜め、chargePhaseとchargeTimeを変える
    public void Charge() {        
        Change_Charge_Span();
        //0段階目
        if (charge_Time < charge_Span[0]) {
            if (charge_Phase != 0) {
                charge_Phase = 0;
                player_Effect.Start_Shoot_Charge(0);
            }
        }
        //1段階目
        else if (charge_Time < charge_Span[1]) {
            if (charge_Phase != 1) {
                charge_Phase = 1;
                player_Effect.Start_Shoot_Charge(1);
                player_SE.Start_Charge_Sound();
            }
        }
        //2段階目
        else if (charge_Time < charge_Span[2]) {
            if (charge_Phase != 2) {
                charge_Phase = 2;
                player_Effect.Start_Shoot_Charge(2);
                player_SE.Change_Charge_Sound_Pitch(1.15f);
            }
        }
        //チャージ完了
        else {
            if (charge_Phase != 3) {
                charge_Phase = 3;
                player_Effect.Start_Shoot_Charge(3);
                player_SE.Change_Charge_Sound_Pitch(1.3f);
            }
        }
        charge_Time += Time.deltaTime;
    }


    //チャージ攻撃,ChargePhase == 3で強攻撃、それ以下で通常攻撃
    public void Charge_Attack() {
        if(charge_Phase == 3) {
            StartCoroutine("Charge_Attack_Cor");
        }
        else {
            _attack.Attack();
        }
        charge_Time = 0;
        player_Effect.Start_Shoot_Charge(0);
        player_SE.Stop_Charge_Sound();
    }

    //強攻撃
    private IEnumerator Charge_Attack_Cor() {
        _anim.SetTrigger("AttackTrigger");
        attack_Collision.Make_Collider_Appear(0.18f);
        player_SE.Play_Attack_Sound();
        player_SE.Play_Hit_Attack_Sound();

        _rigid.velocity += new Vector2(transform.localScale.x * 5f, 0); //Rigidbodyのスリープ状態を解除する
        for (float t = 0; t < 0.18f; t += Time.deltaTime) {
            //敵と衝突時
            if (attack_Collision.Hit_Trigger()) {
                StartCoroutine("Do_Hit_Attack_Process");
                yield return new WaitForSeconds(0.05f);
                break;
            }
            yield return null;
        }
    }


    //パワーによってチャージ時間を変える
    private void Change_Charge_Span() {
        //値が変化したときだけ判別
        if (player_Manager.Get_Power() == player_Power) {
            return;
        }
        player_Power = player_Manager.Get_Power();

        if (player_Power < 16) {
            charge_Span = new float[3] { 0.3f, 1.0f, 2.0f };
        }
        else if (player_Power < 32) {
            charge_Span = new float[3] { 0.27f, 0.85f, 1.7f };
        }
        else if (player_Power < 64) {
            charge_Span = new float[3] { 0.24f, 0.7f, 1.4f };
        }
        else if (player_Power < 128) {
            charge_Span = new float[3] { 0.21f, 0.55f, 1.1f };
        }
        else {
            charge_Span = new float[3] { 0.2f, 0.4f, 0.8f };
        }
    }


    //敵と衝突時の処理
    private IEnumerator Do_Hit_Attack_Process() {
        float force = _controller.is_Landing ? 170f : 60f;                  //ノックバック
        _rigid.velocity = new Vector2(force * -transform.localScale.x, 20f);
        BeetlePowerManager.Instance.StartCoroutine("Increase_Cor", 25);     //緑パワーの増加
        player_SE.Play_Charge_Shoot_Sound();                                //効果音      
        camera_Shake.Shake(0.25f, new Vector2(0, 1.2f), false);
        //ヒットストップ
        float tmp = Time.timeScale;
        Time.timeScale = 0.4f;
        yield return new WaitForSeconds(0.05f);
        Time.timeScale = tmp;
    }
}
