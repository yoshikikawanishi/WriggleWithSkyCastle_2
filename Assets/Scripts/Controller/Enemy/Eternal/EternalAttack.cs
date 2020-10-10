using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EternalAttack : BossEnemyAttack {    

    private Eternal _eternal;
    private EternalLastAttack _last_Attack;
    private EternalShoot _shoot;
    private EternalEffect _effect;
    private SEManager _se;
    private SmallLarveGenerator _small_Larve_Gen;
    private BossChildCollision _collision;
    private MoveConstTime _move;
    private GameObject player;
    private PlayerController player_Controller;

    private class Config {
        public static readonly Vector2 nutral_Pos = new Vector2(160f, 0);
        public static readonly Vector2 center_Pos = new Vector2(0, 0);
        public static readonly Vector2 rightside_Pos = new Vector2(220f, 0);
        public static readonly Vector2 upside_Pos = new Vector2(0, 110f);
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
        _last_Attack = GetComponent<EternalLastAttack>();
        _shoot = GetComponentInChildren<EternalShoot>();
        _effect = GetComponentInChildren<EternalEffect>();
        _se = GetComponentInChildren<SEManager>();
        _collision = GetComponentInChildren<BossChildCollision>();
        _small_Larve_Gen = GetComponentInChildren<SmallLarveGenerator>();
        _move = GetComponent<MoveConstTime>();
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
        _se.Play("Wing");
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
            yield return new WaitForSeconds(0.016f);
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
        Stop_Melody_B2();
        Stop_Melody_Chorus1();        
        Stop_Melody_Chorus2();
        Stop_Melody_Pre_Chorus();
        Stop_Melody_C();
    }
    //==========================================================
    #region ChangePhase
    protected override void Action_In_Change_Phase() {
        if(boss_Enemy.Get_Now_Phase() == 2) {
            StartCoroutine("Change_Phase_Cor");
        }
    }
   
    private IEnumerator Change_Phase_Cor() {
        base.Set_Can_Switch_Attack(false);
        _collision.Become_Invincible();
        Stop_Attack();
        yield return new WaitForSeconds(1.5f);
        Warp(Config.nutral_Pos);        
        yield return new WaitForSeconds(2.0f);
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        _collision.Become_Invincible();
        _last_Attack.Start_Last_Battle_Movie();
        _shoot.Shoot_Beetle_Power();
        
    }

    #endregion
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
            _effect.Play_Power_Charge_Effect_Small();
            yield return new WaitForSeconds(1.0f);
            _effect.Play_Burst_Effect(Color.green);
            UsualSoundManager.Instance.Play_Shoot_Sound();
            _shoot.Shoot_Vine_Shoot(6);
            //待つ、メロディ切り替わったら抜ける
            yield return new WaitForSeconds(2.0f);
            for(float t = 0; t < 3.0f; t += Time.deltaTime) {
                if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.A1) {                    
                    base.Set_Can_Switch_Attack(true);
                    base.Restart_Attack();
                    Stop_Melody_A1();
                }
                yield return new WaitForSeconds(0);
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

    private class ConfigB1 {
        public static readonly Vector2[] positions = new Vector2[5] {
            new Vector2(0, 0),
            new Vector2(200f, 64f),            
            new Vector2(220f, -54f),
            new Vector2(-200f, 100f),
            new Vector2(-220f, -54f),
        };
        public static readonly int bullet_Num = 15;
    }
    
    protected override void Start_Melody_B1() {
        StartCoroutine("Melody_B1_Cor");
    }

    private IEnumerator Melody_B1_Cor() {
        base.Set_Can_Switch_Attack(false);
        int loop_Count = 0;
        int position_Index = -1;

        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.B1) {
            int index;
            do {
                index = Random.Range(0, 5);
            } while (index == position_Index);
            position_Index = index;
            //ワープ
            Warp(ConfigB1.positions[index]);
            yield return new WaitForSeconds(1.5f);                        
            //ショット            
            _shoot.Shoot_Ripples_Shoot(ConfigB1.bullet_Num);
            _effect.Play_Burst_Effect(new Color(1f, 1f, 0.4f));
            //待つ
            loop_Count++;
            if (loop_Count % 3 == 0)
                yield return new WaitForSeconds(1.0f);
            else
                yield return new WaitForSeconds(0.286f);
        }

        base.Set_Can_Switch_Attack(true);
        Restart_Attack();
    }


    private void Stop_Melody_B1() {
        StopCoroutine("Melody_B1_Cor");
    }
    #endregion
    //===================================================================
    #region B2
    /*
        右端で上下移動　→　マスタースパーク
     */

    private class Config_B2 {
        public static readonly float[] heights = new float[3] {
            -72f, 0f, 72f
        };
    }

    protected override void Start_Melody_B2() {
        StartCoroutine("Melody_B2_Cor");
    }

    private IEnumerator Melody_B2_Cor() {
        base.Set_Can_Switch_Attack(false);

        //移動してなかったら移動
        if (!Is_Equals_Vector2(transform.position, Config.nutral_Pos)) {
            Start_Melody_C();
            yield break;
        }
        int height_Index = -1;
        while (true) {
            //移動先決定
            int index = Random.Range(0, 3);
            while (index == height_Index) {
                index = Random.Range(0, 3);
            }
            height_Index = index;
            //移動
            _move.Start_Move(new Vector3(Config.rightside_Pos.x, Config_B2.heights[height_Index]), 0);
            yield return new WaitUntil(_move.End_Move);
            //発射
            for (int i = 0; i < 3; i++) {
                if (i == index)
                    continue;
                _shoot.Shoot_Master_Spark(Config_B2.heights[i]);
                _shoot.Shoot_Beetle_Power();
            }
            yield return new WaitForSeconds(1.5f);
            //目リディ切り替わってたら抜ける
            if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.B2) {
                break;
            }
        }
        yield return new WaitForSeconds(1.0f);

        base.Set_Can_Switch_Attack(true);
        Restart_Attack();
    }

    private void Stop_Melody_B2() {
        StopCoroutine("Melody_B2_Cor");
        _move.Stop_Move();
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
        Vector2 pos = Config.center_Pos;
        Warp(pos);
        yield return new WaitForSeconds(1.5f);
        //溜める
        _effect.Play_Power_Charge_Effect(1000f);
        _eternal.Change_Animation("CloseTrigger");
        yield return new WaitForSeconds(2.0f);
        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.pre_Chorus) {
            yield return new WaitForSeconds(0.016f);
        }
        _effect.Stop_Power_Charge_Effect();
        _eternal.Change_Animation("OpenTrigger");

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
            yield break;
        }
        while (true) {
            //強うずまき弾幕
            _effect.Play_Burst_Effect(Color.red);
            UsualSoundManager.Instance.Play_Shoot_Sound();
            _shoot.Shoot_Spiral_Shoot_Strong();
            //待ち
            yield return new WaitForSeconds(2.857f);
            if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.chorus1)
                break;
            //溜め
            _effect.Play_Power_Charge_Effect(2.857f);
            yield return new WaitForSeconds(2.857f);
            //飛行禁止と弱うずまき弾幕
            Ban_Player_Flying();
            _effect.Play_Burst_Effect(Color.blue);
            _shoot.Stop_Spiral_Shoot_Strong();
            yield return new WaitForSeconds(1.8f);
            _shoot.Shoot_Spiral_Shoot_Weak();
            UsualSoundManager.Instance.Play_Shoot_Sound();
            //待ち
            yield return new WaitForSeconds(6.429f);
            if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.chorus1)
                break;
            //開放
            Release_Player_Flying();            
            _effect.Play_Power_Charge_Effect(2.857f);
            yield return new WaitForSeconds(2.857f);
        }

        base.Set_Can_Switch_Attack(true);
        Restart_Attack();
        Stop_Melody_Chorus1();
    }

    private void Stop_Melody_Chorus1() {
        StopCoroutine("Melody_Chorus1_Cor");
        StopCoroutine("Ban_Player_Flying_Cor");
        Release_Player_Flying();
        _shoot.Stop_Spiral_Shoot_Strong();
        _shoot.Stop_Spiral_Shoot_Weak();
    }

    #endregion
    //===================================================================
    #region C
    protected override void Start_Melody_C() {
        StartCoroutine("Melody_C_Cor");
    }

    private IEnumerator Melody_C_Cor() {
        base.Set_Can_Switch_Attack(false);
        //ワープ
        Vector2 pos = Config.nutral_Pos;
        Warp(pos);
        yield return new WaitForSeconds(1.5f);
        //溜める
        _effect.Play_Power_Charge_Effect(1000f);
        _eternal.Change_Animation("CloseTrigger");
        yield return new WaitForSeconds(2.0f);
        while (melody_Manager.Get_Now_Melody() == MelodyManager.Melody.C) {
            yield return new WaitForSeconds(0);
        }
        _effect.Stop_Power_Charge_Effect();
        _eternal.Change_Animation("OpenTrigger");

        base.Set_Can_Switch_Attack(true);
        base.Restart_Attack();
    }

    private void Stop_Melody_C() {
        StopCoroutine("Melody_C_Cor");
        _effect.Stop_Power_Charge_Effect();
    }
    #endregion
    //===================================================================
    #region Chorus2

    /*
        上側にワープ　→　弾幕
     */
    protected override void Start_Melody_Chorus2() {
        StartCoroutine("Melody_Chorus2_Cor");
    }

    private IEnumerator Melody_Chorus2_Cor() {
        base.Set_Can_Switch_Attack(false);
        //移動してなかったらワープ
        if (!Is_Equals_Vector2(Config.nutral_Pos, transform.position)) {
            Start_Melody_C();
            yield break;
        }
        //ショット
        _effect.Play_Burst_Effect(new Color(0.6f, 1f, 0.1f));
        UsualSoundManager.Instance.Play_Shoot_Sound();
        _shoot.Shoot_Wing_Shoot(2.3f);
        _shoot.Shoot_Reverse_Wing_Shoot(3.7f);
        yield return new WaitForSeconds(7.0f);
        _shoot.Stop_Wing_Shoot();                
        //メロディー切り替わってたら抜ける
        if(melody_Manager.Get_Now_Melody() != MelodyManager.Melody.chorus2) {
            base.Set_Can_Switch_Attack(true);
            base.Restart_Attack();
            yield break;
        }
        //ミニラルバ生成
        _small_Larve_Gen.Start_Gen(2.0f);
        yield return new WaitForSeconds(4.0f);
        _small_Larve_Gen.Stop_Gen();
        _effect.Play_Power_Charge_Effect(2.0f);
        yield return new WaitForSeconds(2.0f);

        base.Set_Can_Switch_Attack(true);
        base.Restart_Attack();
    }

    private void Stop_Melody_Chorus2() {
        StopCoroutine("Melody_Chorsu2_Cor");
        _shoot.Stop_Wing_Shoot();
        _small_Larve_Gen.Disable_All_Larva();
    }
    #endregion
    //===================================================================
 
    protected override void Start_Melody_A2() {
        throw new System.NotImplementedException();
    }    

    protected override void Start_Melody_Bridge() {
        throw new System.NotImplementedException();
    }
   

    protected override void Start_Melody_Intro() {
        throw new System.NotImplementedException();
    }
    
}
