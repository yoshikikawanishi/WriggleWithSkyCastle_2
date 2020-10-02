using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkinaAttack : BossEnemyAttack {

    [Space]
    [SerializeField] private GameObject fairy_Crystal_Red_Fairy;
    [SerializeField] private GameObject fairy_Crystal_Green_Fairy;

    private Okina _okina;
    private OkinaShoot _shoot;
    private OkinaEffect _effect;
    private SEManager _se;
    private MoveConstTime _move_Const_Time;
    private GameObject player;
    private PlayerController player_Controller;
    private CameraShake camera_Shake;

    private class Config {
        public readonly static Vector2 nutral_Pos = new Vector2(160f, 0);
    }    


    void Start() {        
        //取得
        _okina = GetComponent<Okina>();
        _shoot = GetComponentInChildren<OkinaShoot>();
        _effect = GetComponentInChildren<OkinaEffect>();
        _se = GetComponentInChildren<SEManager>();
        _move_Const_Time = GetComponent<MoveConstTime>();
        player = GameObject.FindWithTag("PlayerTag");
        player_Controller = player.GetComponent<PlayerController>();
        camera_Shake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        //オブジェクトプール生成
        ObjectPoolManager.Instance.Create_New_Pool(fairy_Crystal_Red_Fairy, 3);
        ObjectPoolManager.Instance.Create_New_Pool(fairy_Crystal_Green_Fairy, 3);
    }


    public override void Stop_Attack() {
        Stop_Melody_A1();
        Stop_Melody_B1();
        Stop_Melody_B2();
        Stop_Melody_Bridge();
        Stop_Melody_Chorus1();
        Stop_Melody_Chorus2();        
    }

    
    //===========================================================================================
    #region A1    

        /*
         クナイ弾出しながらランダムに移動
         */
    protected override void Start_Melody_A1() {
        StartCoroutine("Melody_A1_Cor");
    }

    private IEnumerator Melody_A1_Cor() {
        base.Set_Can_Switch_Attack(false);

        while(true) {
            //座標計算
            Vector3 pos = player.transform.position + new Vector3(-player.transform.localScale.x * 64f, 0, 0);
            if(Mathf.Abs(pos.x) > 240f) {
                pos = new Vector3(240f * pos.x.CompareTo(0), pos.y + 64f, 0);
            }
            //エフェクト
            _effect.Play_Back_Door_Effect(pos);
            _effect.Play_Power_Charge_Effect(pos, 2.0f);
            yield return new WaitForSeconds(2.0f);
            //クナイショット
            _shoot.Shoot_Kuani_Shoot1(pos);
            _effect.Play_Burst_Effect_Green(pos);
            UsualSoundManager.Instance.Play_Shoot_Sound();
            //移動
            Move_Random(48f);
            //5.5秒待つ、メロディ切り替わったら抜ける
            for(float t = 0; t < 5.5f; t += Time.deltaTime) {
                if(melody_Manager.Get_Now_Melody() != MelodyManager.Melody.A1) {
                    Stop_Melody_A1();
                    base.Set_Can_Switch_Attack(true);
                    Restart_Attack();
                }
                yield return null;
            }
            _effect.Delete_Back_Door_Effect();
            //移動
            Move_To_Nutral_Pos();
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void Move_Random(float distance) {
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 vec = distance * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        _move_Const_Time.Start_Move(transform.position + vec, 0);
    }

    private void Move_To_Nutral_Pos() {
        _move_Const_Time.Start_Move(Config.nutral_Pos, 1);
    }

    private void Stop_Melody_A1() {
        StopCoroutine("Melody_A1_Cor");
        _move_Const_Time.Stop_Move();
        _shoot.Stop_Kunai_Shoot1();
        _effect.Delete_Back_Door_Effect();
    }

    #endregion
    //===========================================================================================    
    #region B1

        /*
         無敵化、妖精のクリスタル生成
         */     
    //クリスタルの種類と座標
    private class Crystal {
        public Vector2 pos;
        public string kind;
        public Crystal(Vector2 pos, string kind) {
            this.pos = pos;
            this.kind = kind;
        }
    }

    private class ConfigB1 {
        private static readonly Crystal[] crystal_Pos1 = {
            new Crystal(new Vector2(200f, -72f), "red"),
            new Crystal(new Vector2(48f, -72f), "red"),
            new Crystal(new Vector2(-48f, -72f), "red"),
            new Crystal(new Vector2(200f, -72f), "red"),
            new Crystal(new Vector2(-200f, 24f), "green"),
            new Crystal(new Vector2(-96f, 48f), "green"),
            new Crystal(new Vector2(200f, 48f), "green"),
            new Crystal(new Vector2(96f, 24f), "green"),
        };
        private static readonly Crystal[] crystal_Pos2 = {
            new Crystal(new Vector2(-200f, -72f), "red"),
            new Crystal(new Vector2(-48f, -72f), "red"),
            new Crystal(new Vector2(48f, -72f), "red"),
            new Crystal(new Vector2(0f, -8f), "red"),
            new Crystal(new Vector2(128f, 28f), "green"),
            new Crystal(new Vector2(96f, 48f), "green"),
            new Crystal(new Vector2(64f, 28f), "green"),
            new Crystal(new Vector2(32f, 48f), "green"),            
        };
        private static readonly Crystal[] crystal_Pos3 = {
            new Crystal(new Vector2(128f, -72f), "red"),
            new Crystal(new Vector2(48f, -72f), "red"),
            new Crystal(new Vector2(-48f, -72f), "red"),
            new Crystal(new Vector2(0f, -8f), "red"),
            new Crystal(new Vector2(-128f, 64f), "green"),
            new Crystal(new Vector2(-96f, 48f), "green"),
            new Crystal(new Vector2(-64f, 24f), "green"),
            new Crystal(new Vector2(-32f, 48f), "green"),
        };
        private static readonly Crystal[] crystal_Pos4 = {
            new Crystal(new Vector2(64f, -72f), "red"),
            new Crystal(new Vector2(21f, -72f), "red"),
            new Crystal(new Vector2(-21f, -72f), "red"),
            new Crystal(new Vector2(64f, -72f), "red"),
            new Crystal(new Vector2(-64f, 63f), "green"),
            new Crystal(new Vector2(-21f, 24f), "green"),
            new Crystal(new Vector2(64f, 48f), "green"),
            new Crystal(new Vector2(21f, 48f), "green"),
        };
        public static readonly Crystal[][] positions = {
            crystal_Pos1,
            crystal_Pos2,
            crystal_Pos3,
            crystal_Pos4
        };
        public static readonly float span = 0.3f;
    }

    protected override void Start_Melody_B1() {
        StartCoroutine("Melody_B1_Cor");
    }

    private IEnumerator Melody_B1_Cor() {
        base.Set_Can_Switch_Attack(false);
        float span = ConfigB1.span;
        //消える
        _okina.Change_Animation("DisappearBool");
        yield return new WaitForSeconds(1.0f);
        //無敵化
        _okina.Become_Invincible();        
        transform.position = new Vector3(1000f, 1000f, 0);
        //自機の飛行無効化
        _effect.Play_Small_Power_Charge_Effect();
        yield return new WaitForSeconds(1.5f);
        _effect.Play_Ban_Flying_Effect();        
        yield return new WaitForSeconds(1.5f);
        while (!player_Controller.Get_Can_Ride_Beetle()) {
            yield return null;
        }
        player_Controller.To_Disable_Ride_Beetle();

        while (true) {
            //敵生成
            int r = Random.Range(0, 4);
            Crystal[] c = ConfigB1.positions[r];
            for(int i = 0; i < 8; i++) {
                Generate_Fairy_Crystal(c[i]);
                yield return new WaitForSeconds(span);
            }
            yield return new WaitForSeconds(1.0f);
            //3秒待つ、メロディ切り替わったら抜ける
            for (float t = 0; t < 3.0f; t += Time.deltaTime) {
                if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.B1) {
                    Stop_Melody_B1();
                    base.Set_Can_Switch_Attack(true);
                    base.Restart_Attack();
                }
                yield return null;
            }
            
        }
    }

    //妖精を生成
    private void Generate_Fairy_Crystal(Crystal c) {
        GameObject obj;
        if (c.kind == "red") 
            obj = ObjectPoolManager.Instance.Get_Pool(fairy_Crystal_Red_Fairy).GetObject();                    
        else 
            obj = ObjectPoolManager.Instance.Get_Pool(fairy_Crystal_Green_Fairy).GetObject();        
        obj.transform.position = c.pos;
    }

    private void Stop_Melody_B1() {
        StopCoroutine("Melody_B1_Cor");
        _okina.Release_Invincible();
        _okina.Change_Animation("AttackBool");
        transform.position = Config.nutral_Pos;
        _effect.Release_Ban_Flying_Effect();        
        player_Controller.To_Enable_Ride_Beetle();
    }

    private void Delete_Fairies() {

    }

    #endregion
    //===========================================================================================
    #region B2

        /*
         ランダム移動しながらレーザー
         */
    protected override void Start_Melody_B2() {
        StartCoroutine("Melody_B2_Cor");
    }

    private IEnumerator Melody_B2_Cor() {
        base.Set_Can_Switch_Attack(false);
        float angle = 0;

        yield return new WaitForSeconds(1.0f);

        while (true) {
            //移動            
            angle = Random.Range(0, 360f);
            _move_Const_Time.Start_Move(Config.nutral_Pos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 40f, 2);
            yield return new WaitUntil(_move_Const_Time.End_Move);
            
            //メロディ切り替わったら抜ける
            if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.B2) {
                break;
            }

            //レーザー
            _shoot.Shoot_Blue_Laser_Left();
            yield return new WaitForSeconds(0.5f);
            //移動
            angle = Random.Range(0, 360f);
            _move_Const_Time.Start_Move(Config.nutral_Pos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 40f, 2);
            yield return new WaitUntil(_move_Const_Time.End_Move);

            //メロディ切り替わったら抜ける
            if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.B2) {
                break;
            }

            //レーザー
            _shoot.Shoot_Blue_Laser_Right();
            yield return new WaitForSeconds(1.5f);

            //メロディ切り替わったら抜ける
            if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.B2) {
                break;
            }
        }

        base.Set_Can_Switch_Attack(true);
        Restart_Attack();
    }

    private void Stop_Melody_B2() {
        StopCoroutine("Molody_B2_Cor");
        _move_Const_Time.Stop_Move();
    }

    #endregion
    //===========================================================================================
    #region Bridge

        /*
         下から火柱攻撃
         */
    protected override void Start_Melody_Bridge() {
        StartCoroutine("Melody_Bridge_Cor");
    }

    private IEnumerator Melody_Bridge_Cor() {
        base.Set_Can_Switch_Attack(false);
        //溜め
        _effect.Play_Power_Charge_Effect_Blue();
        yield return new WaitForSeconds(1.0f);
        while (true) {
            //火柱予測線
            float pos_X = player.transform.position.x;
            _effect.Play_Pre_Blue_Fire_Pillar_Effect(pos_X);
            yield return new WaitForSeconds(1.0f);
            //火柱生成
            _effect.Stop_Power_Charge_Effect_Blue();
            _effect.Play_Burst_Effect_Blue();
            _shoot.Shoot_Blue_Pillar(pos_X);
            _se.Play("FirePillar");
            camera_Shake.Shake(2.0f, new Vector2(2f, 2f), true);
            yield return new WaitForSeconds(1.0f);
            //メロディ切り替わったら抜ける
            if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.bridge)
                break;
        }
        base.Set_Can_Switch_Attack(true);
        base.Restart_Attack();
    }

    private void Stop_Melody_Bridge() {
        StopCoroutine("Melody_Bridge_Cor");
    }
    #endregion
    //===========================================================================================
    #region PreChorus

        /*
         サビ弾幕の位置に移動
         */
    protected override void Start_Melody_Pre_Chorus() {
        StartCoroutine("Melody_Pre_Chorus_Cor");
    }

    private IEnumerator Melody_Pre_Chorus_Cor() {        
        //移動
        _move_Const_Time.Start_Move(Config.nutral_Pos, 0);
        //チャージ
        _effect.Play_Power_Charge_Effect();
        //少なくとも2秒は溜めること
        yield return new WaitForSeconds(2.0f);
        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.pre_Chorus) {            
            yield return null;            
        }        
        _effect.Stop_Power_Charge_Effect();
    }

    private void Stop_Melody_Pre_Chorus() {
        StopCoroutine("Melody_Pre_Chorus_Cor");
        _move_Const_Time.Stop_Move();
        _effect.Stop_Power_Charge_Effect();
    }
    #endregion
    //===========================================================================================
    #region Chorus1

    /*
     赤弾幕
     */
    protected override void Start_Melody_Chorus1() {
        StartCoroutine("Melody_Chorus1_Cor");
    }

    private IEnumerator Melody_Chorus1_Cor() {
        base.Set_Can_Switch_Attack(false);
        while (true) {
            //ショット
            _shoot.Shoot_Red_Bullet();
            _effect.Play_Burst_Effect_Red();
            yield return new WaitForSeconds(3.5f);
            //メロディ切り替わったら抜ける
            if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.chorus1)
                break;
        }
        base.Set_Can_Switch_Attack(true);
        Restart_Attack();
    }

    private void Stop_Melody_Chorus1() {
        StopCoroutine("Melody_Chorus1_Cor");
        _shoot.Stop_Red_Shoot();
    }
    #endregion
    //===========================================================================================
    #region Chorus2
    protected override void Start_Melody_Chorus2() {
        StartCoroutine("Melody_Chorus2_Cor");
    }

    private IEnumerator Melody_Chorus2_Cor() {
        base.Set_Can_Switch_Attack(false);
        //黒青弾幕
        _shoot.Shoot_Black_Blue_Bullet();
        UsualSoundManager.Instance.Play_Shoot_Sound();
        while (true) {
            yield return null;
            //メロディ切り替わったら抜ける
            if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.chorus2)
                break;
        }
        _shoot.Stop_Black_Blue_Shoot();
        yield return new WaitForSeconds(2.0f);
        base.Set_Can_Switch_Attack(true);
        Restart_Attack();
    }

    private void Stop_Melody_Chorus2() {
        _shoot.Stop_Black_Blue_Shoot();
        StopCoroutine("Melody_Chorus2_Cor");
    }
    #endregion
    //===========================================================================================

    protected override void Start_Melody_Intro() {
        throw new System.NotImplementedException();
    }
    protected override void Start_Melody_A2() {
        throw new System.NotImplementedException();
    }
    protected override void Start_Melody_C() {
        throw new System.NotImplementedException();
    }
    protected override void Action_In_Change_Phase() {

    }

}
