using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EternalAttack : BossEnemyAttack {

    private EternalShoot _shoot;
    private GameObject player;

    private class Config {
        public static readonly Vector2 nutral_Pos = new Vector2(160f, 0);
    }


    void Awake() {
        //取得
        _shoot = GetComponentInChildren<EternalShoot>();
        player = GameObject.FindWithTag("PlayerTag");
    }


    //ワープする
    public void Warp(Vector2 next_Pos) {
        transform.position = next_Pos;
    }

    //==========================================================
    public override void Stop_Attack() {
        Stop_Melody_A1();
        Stop_Melody_B1();
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
        yield return new WaitForSeconds(1.0f);
        //ショット
        while (true) {
            int divide_Count = boss_Enemy.Get_Now_Phase() == 1 ? 8 : 6;
            _shoot.Shoot_Vine_Shoot(divide_Count);
            //待つ、メロディ切り替わったら抜ける
            yield return new WaitForSeconds(4.0f);
            for(float t = 0; t < 5.0f; t += Time.deltaTime) {
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
            Vector2 next_Pos = new Vector2(Random.Range(30f, 180f), Random.Range(-120f, 120f));
            next_Pos = new Vector2(next_Pos.x * -player.transform.position.x.CompareTo(0), next_Pos.y);
            Warp(next_Pos);
            yield return new WaitForSeconds(0.5f);
            //ショット
            int num = boss_Enemy.Get_Now_Phase() == 1 ? 20 : 36;
            _shoot.Shoot_Ripples_Shoot(num);
            //待つ
            loop_Count++;
            if (loop_Count % 3 == 0)
                yield return new WaitForSeconds(1.5f);
            yield return new WaitForSeconds(0.5f);
        }

        base.Set_Can_Switch_Attack(true);
        Restart_Attack();
    }

    private void Stop_Melody_B1() {
        StopCoroutine("Melody_B1_Cor");
    }
    #endregion
    //==========================================================
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

    protected override void Start_Melody_Chorus1() {
        throw new System.NotImplementedException();
    }

    protected override void Start_Melody_Chorus2() {
        throw new System.NotImplementedException();
    }

    protected override void Start_Melody_Intro() {
        throw new System.NotImplementedException();
    }

    protected override void Start_Melody_Pre_Chorus() {
        throw new System.NotImplementedException();
    }
}
