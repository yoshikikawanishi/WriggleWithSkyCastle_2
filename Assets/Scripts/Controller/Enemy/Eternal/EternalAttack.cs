using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EternalAttack : BossEnemyAttack {

    private Eternal _eternal;
    private EternalShoot _shoot;
    private EternalEffect _effect;
    private BossChildCollision _collision;
    private GameObject player;
    private PlayerController player_Controller;

    private class Config {
        public static readonly Vector2 nutral_Pos = new Vector2(160f, 0);
        public static readonly Vector2 center_Pos = new Vector2(0, 0);
    }

    private enum State {
        idle,
        warping,
        baning_Flying
    }
    private State state = State.idle;


    void Awake() {
        //取得
        _eternal = GetComponent<Eternal>();
        _shoot = GetComponentInChildren<EternalShoot>();
        _effect = GetComponentInChildren<EternalEffect>();
        _collision = GetComponentInChildren<BossChildCollision>();
        player = GameObject.FindWithTag("PlayerTag");
        player_Controller = player.GetComponent<PlayerController>();
    }


    //ワープする
    public void Warp(Vector2 next_Pos) {
        if (state == State.warping)
            return;
        state = State.warping;
        StartCoroutine("Warp_Cor", next_Pos);
    }    

    private IEnumerator Warp_Cor(Vector2 next_Pos) {
        _eternal.Change_Animation("CloseTrigger");
        _collision.Become_Invincible();
        yield return new WaitForSeconds(0.6f);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        transform.position = next_Pos;        
        _eternal.Change_Animation("OpenTrigger");
        GetComponent<SpriteRenderer>().enabled = true;
        state = State.idle;
        yield return new WaitForSeconds(0.5f);
        _collision.Release_Invincible();        
    }


    //自機の飛行禁止
    private void Ban_Player_Flying() {
        if (state == State.baning_Flying)
            return;
        state = State.baning_Flying;
        StartCoroutine("Ban_Player_Flying_Cor");
    }

    //自機の飛行禁止解除
    private void Release_Player_Flying() {
        _effect.Release_Ban_Flying_Effect();
        player_Controller.To_Enable_Ride_Beetle();
    }

    private IEnumerator Ban_Player_Flying_Cor() {
        _effect.Play_Power_Charge_Effect_Small();
        yield return new WaitForSeconds(0.5f);
        _effect.Play_Ban_Flying_Effect();
        yield return new WaitForSeconds(1.5f);
        while (!player_Controller.Get_Can_Ride_Beetle()) {
            yield return null;
        }
        player_Controller.To_Disable_Ride_Beetle();
        state = State.idle;
    }


    //座標比較
    private bool Is_Equals_Vector2(Vector2 a, Vector2 b) {
        if(Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) < 1f) {
            return true;
        }
        return false;
    }

    //==========================================================
    public override void Stop_Attack() {
        Stop_Melody_A1();
        Stop_Melody_B1();
        Stop_Melody_Chorus1();
        Stop_Melody_Pre_Chorus();
    }
    //==========================================================

    protected override void Action_In_Change_Phase() {

    }

    //==========================================================
    #region A1
    /*
        nutral_Posに移動 → ツタ弾幕 
    */
    protected override void Start_Melody_A1() {
        StartCoroutine("Melody_A1_Cor");        
    }

    private IEnumerator Melody_A1_Cor() {
        base.Set_Can_Switch_Attack(false);
        //移動
        Warp(Config.nutral_Pos);
        yield return new WaitForSeconds(1.5f);
        //ショット
        while (true) {
            int divide_Count = boss_Enemy.Get_Now_Phase() == 1 ? 8 : 6;
            _shoot.Shoot_Vine_Shoot(divide_Count);
            //待つ、メロディ切り替わったら抜ける
            yield return new WaitForSeconds(4.0f);
            for(float t = 0; t < 2.0f; t += Time.deltaTime) {
                if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.A1) {
                    Stop_Melody_A1();
                    base.Set_Can_Switch_Attack(true);
                    base.Restart_Attack();
                }
                yield return null;
            }
        }

    }

    private void Stop_Melody_A1() {
        StopCoroutine("Melody_A1_Cor");
        _shoot.Stop_Vine_Shoot();
    }
    #endregion
    //==========================================================    
    #region B1
    /*
        ワープ移動 → 波紋弾幕    
    */
    protected override void Start_Melody_B1() {
        StartCoroutine("Melody_B1_Cor");
    }

    private IEnumerator Melody_B1_Cor() {
        base.Set_Can_Switch_Attack(false);
        int loop_Count = 0;

        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.B1) {
            //ワープ
            Vector2 next_Pos = new Vector2(Random.Range(130f, 180f), Random.Range(-120f, 120f));
            next_Pos = new Vector2(next_Pos.x * -player.transform.position.x.CompareTo(0), next_Pos.y);
            Warp(next_Pos);
            yield return new WaitForSeconds(1.5f);                        
            //ショット
            int num = boss_Enemy.Get_Now_Phase() == 1 ? 20 : 36;
            _shoot.Shoot_Ripples_Shoot(num);
            //待つ
            loop_Count++;
            if (loop_Count % 3 == 0)
                yield return new WaitForSeconds(1.0f);
            yield return new WaitForSeconds(0.2f);
        }

        base.Set_Can_Switch_Attack(true);
        Restart_Attack();
    }

    private void Stop_Melody_B1() {
        StopCoroutine("Melody_B1_Cor");
    }
    #endregion
    //==========================================================
    #region PreChorus
        /*
            ワープ移動　→　溜め
         */
    protected override void Start_Melody_Pre_Chorus() {
        StartCoroutine("Melody_Pre_Chorus_Cor");
    }

    private IEnumerator Melody_Pre_Chorus_Cor() {
        base.Set_Can_Switch_Attack(false);
        //ワープ
        Vector2 pos = boss_Enemy.Get_Now_Phase() == 1 ? Config.center_Pos : Config.nutral_Pos;
        Warp(pos);
        yield return new WaitForSeconds(1.5f);
        //溜める
        _effect.Play_Power_Charge_Effect(1000f);
        yield return new WaitForSeconds(2.0f);
        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.pre_Chorus) {
            yield return null;
        }
        _effect.Stop_Power_Charge_Effect();

        base.Set_Can_Switch_Attack(true);
        base.Restart_Attack();
    }

    private void Stop_Melody_Pre_Chorus() {
        StopCoroutine("Melody_Pre_Chorus_Cor");
        _effect.Stop_Power_Charge_Effect();
    }
    #endregion
    //===================================================================
    #region Chorus1
    /*
        強うずまき弾幕　→　飛行禁止と弱うずまき弾幕
     */
    protected override void Start_Melody_Chorus1() {
        StartCoroutine("Melody_Chorus1_Cor");
    }

    private IEnumerator Melody_Chorus1_Cor() {
        base.Set_Can_Switch_Attack(false);

        //移動してなかったら移動
        if(!Is_Equals_Vector2(transform.position, Config.center_Pos)) {
            Start_Melody_Pre_Chorus();
        }
        while (true) {
            //強うずまき弾幕
            _effect.Play_Burst_Effect_White();
            _shoot.Shoot_Spiral_Shoot_Strong();
            yield return new WaitForSeconds(2.0f);
            if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.chorus1) {
                break;
            }
            _effect.Play_Power_Charge_Effect(2.0f);
            yield return new WaitForSeconds(2.0f);
            //飛行禁止と弱うずまき弾幕
            Ban_Player_Flying();
            _shoot.Stop_Spiral_Shoot_Strong();
            _shoot.Shoot_Spiral_Shoot_Weak();            
            yield return new WaitForSeconds(6.0f);            
            _shoot.Stop_Spiral_Shoot_Weak();
            Release_Player_Flying();
            if(melody_Manager.Get_Now_Melody() != MelodyManager.Melody.chorus1) {
                break;
            }
            _effect.Play_Power_Charge_Effect(2.0f);
            yield return new WaitForSeconds(2.0f);
        }

        base.Set_Can_Switch_Attack(true);
        Restart_Attack();
    }

    private void Stop_Melody_Chorus1() {
        StopCoroutine("Melody_Chorus1_Cor");
        Release_Player_Flying();
        _shoot.Stop_Spiral_Shoot_Strong();
        _shoot.Stop_Spiral_Shoot_Weak();
    }

    #endregion
    //===================================================================
    protected override void Start_Melody_A2() {
        throw new System.NotImplementedException();
    }

   
    protected override void Start_Melody_B2() {
        throw new System.NotImplementedException();
    }

    protected override void Start_Melody_Bridge() {
        throw new System.NotImplementedException();
    }

    protected override void Start_Melody_C() {
        throw new System.NotImplementedException();
    }

   

    protected override void Start_Melody_Chorus2() {
        throw new System.NotImplementedException();
    }

    protected override void Start_Melody_Intro() {
        throw new System.NotImplementedException();
    }
    
}
