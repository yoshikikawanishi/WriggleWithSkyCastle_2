using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatoMaiAttack : BossEnemyAttack {

    [SerializeField] private SatoMaiPhaseChangeEvent change_Phase_Event;

    private GameObject satomai;
    private GameObject satono;
    private GameObject mai;

    private SatoMai _controller;
    private SatoMaiShoot _shoot;
    private SatoMaiEffect _effect;
    private MoveConstTime satomai_Move;
    private MoveConstTime satono_Move;
    private MoveConstTime mai_Move;

    private GameObject player;


    void Start() {
        //取得        
        _controller = GetComponent<SatoMai>();
        _shoot = GetComponent<SatoMaiShoot>();
        _effect = GetComponentInChildren<SatoMaiEffect>();
        
        player = GameObject.FindWithTag("PlayerTag");
        satomai = _controller.satomai;
        satono = _controller.satono;
        mai = _controller.mai;

        satomai_Move = satomai.GetComponent<MoveConstTime>();
        satono_Move = satono.GetComponent<MoveConstTime>();
        mai_Move = mai.GetComponent<MoveConstTime>();
    }

    public override void Stop_Attack() {
        Stop_Melody_A1();
        Stop_Melody_B1();
        Stop_Melody_C();
        Stop_Melody_Chorus1();
        satono_Move.Stop_Move();
        mai_Move.Stop_Move();
        satomai_Move.Stop_Move();
        if (satomai.activeSelf) {
            _controller.Change_Animation("IdleBool");
        }
        else {
            _controller.Change_Animation("DivideBool");
        }
        mai.transform.localScale = new Vector3(1, 1, 1);
        satono.transform.localScale = new Vector3(1, 1, 1);
        satomai.transform.localScale = new Vector3(1, 1, 1);
    }

    // ===================================================================
    #region フェーズ切り替え
    
    private class ConfigChangePhase {
        public readonly Vector2 nutral_Pos = new Vector2(0, 0);
        public readonly Vector2 satono_Pos = new Vector2(-128f, 0);
        public readonly Vector2 mai_Pos = new Vector2(128f, 0);
    }
    private ConfigChangePhase configCP = new ConfigChangePhase();


    protected override void Action_In_Change_Phase() {
        if (_controller.Get_Now_Phase() == 2) {            
            Raise_Move_Speed();
            Stop_Attack();
            StartCoroutine("Change_Phase_Cor");
        }        
    }


    private IEnumerator Change_Phase_Cor() {
        base.Set_Can_Switch_Attack(false);        
        _controller.Become_Invincible();        
        yield return new WaitForSeconds(1.0f);
        //移動
        //一緒にいるときはまず中央に戻る
        if (satomai.activeSelf) {
            _controller.Change_Animation("IdleBool");
            satomai_Move.Start_Move(configCP.nutral_Pos, 0);
            yield return new WaitUntil(satomai_Move.End_Move);
            satono.transform.position = configCP.nutral_Pos;
            mai.transform.position = configCP.nutral_Pos;
        }
        //分裂して移動する
        _controller.Change_Animation("DivideBool");
        yield return new WaitForSeconds(0.5f);
        satono_Move.Start_Move(configCP.satono_Pos, 0);
        mai_Move.Start_Move(configCP.mai_Pos, 0);
        yield return new WaitUntil(mai_Move.End_Move);
        yield return new WaitUntil(satono_Move.End_Move);

        //クイズ開始
        change_Phase_Event.Start_Event();
        yield return new WaitUntil(change_Phase_Event.Is_End_Event);

        //戻る
        satono_Move.Start_Move(configCP.nutral_Pos, 0);
        mai_Move.Start_Move(configCP.nutral_Pos, 0);
        yield return new WaitUntil(mai_Move.End_Move);
        yield return new WaitUntil(satono_Move.End_Move);
        _controller.Change_Animation("IdleBool");
        yield return new WaitForSeconds(0.5f);
        //戦闘再開
        _controller.Release_Invincible();
        base.Set_Can_Switch_Attack(true);
        base.Restart_Attack();
    }


    private void Raise_Move_Speed() {
        satomai_Move.Change_Paramter(0.015f, 0, 1); //回転突進用
        satono_Move.Change_Paramter(0.014f, 0, 1);  //十字突進用
        mai_Move.Change_Paramter(0.014f, 0, 1);     //十字突進用
    }

    #endregion   
    // ===================================================================
    #region Aメロ
    /*
     * Aメロ攻撃、里舞分かれて画面外から十字方向に突進
     */

    private class ConfigA {
        public readonly Vector3 nutral_Pos = new Vector3(0, 0, 0);
        public readonly Vector3 satono_Outside_Pos = new Vector3(270f, 48f);
        public readonly Vector3 mai_Outside_Pos = new Vector3(-270f, 48f);        
    }
    private ConfigA configA = new ConfigA();


    protected override void Start_Melody_A1() {
        StartCoroutine("Melody_A1_Cor");
    }


    private IEnumerator Melody_A1_Cor() {
        base.Set_Can_Switch_Attack(false);
        //画面外に分かれて出る
        _controller.Change_Animation("DivideAndGoOutBool");
        yield return new WaitForSeconds(1.5f);  
        satono_Move.Start_Move(configA.satono_Outside_Pos, 0);
        mai_Move.Start_Move(configA.mai_Outside_Pos, 0);
        yield return new WaitForSeconds(2.0f);

        //突進攻撃        
        _controller.Change_Animation("CrossRushingBool");
        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.A1) {
            int direction = Random.Range(0, 2) == 0 ? 1 : -1;
            Rush_By_Satono(direction);
            _effect.Play_Satono_Cross_Rushing_Effect();
            yield return new WaitForSeconds(0.4f);
            Rush_By_Mai(direction);
            _effect.Play_Mai_Cross_Rushing_Effect(direction);
            yield return new WaitUntil(mai_Move.End_Move);
            _effect.Stop_Satono_Cross_Rushing_Effect();
            _effect.Stop_Mai_Cross_Rushing_Effect();
        }
        yield return new WaitForSeconds(1.0f);        

        //入ってきて合流
        _controller.Change_Animation("GoInsideAndMergeBool");
        satono.transform.position   = new Vector3(-configA.satono_Outside_Pos.x, configA.satono_Outside_Pos.y);
        mai.transform.position      = new Vector3(-configA.mai_Outside_Pos.x,    configA.mai_Outside_Pos.y);
        satono.transform.localScale = new Vector3(1, 1, 1);
        mai.transform.localScale    = new Vector3(1, 1, 1);
        satono_Move.Start_Move(configA.nutral_Pos, 0);
        mai_Move.Start_Move(configA.nutral_Pos, 0);
        yield return new WaitUntil(mai_Move.End_Move);

        _controller.Change_Animation("IdleBool");
        satomai.transform.position = configA.nutral_Pos;
        Set_Can_Switch_Attack(true);
        Restart_Attack();
    }


    //里乃突進攻撃
    private void Rush_By_Satono(int direction) {
        Vector2 pos = player.transform.position + new Vector3(direction * 16f, 0);      
        satono.transform.position = new Vector3(pos.x, 160f);
        satono.transform.localScale = new Vector3(-direction, 1, 1);
        satono_Move.Start_Move(new Vector3(pos.x, -380f), 1);
    }


    //舞突進攻撃
    private void Rush_By_Mai(int direction) {
        Vector2 pos = player.transform.position + new Vector3(0, Random.Range(-8f, 40f));        
        mai.transform.position = new Vector3(-250f * direction, pos.y);
        mai.transform.localScale = new Vector3(direction, 1, 1);
        mai_Move.Start_Move(new Vector3(420f * direction, pos.y), 1);
    }

    
    private void Stop_Melody_A1() {
        StopCoroutine("Melody_A1_Cor");        
        _effect.Stop_Mai_Cross_Rushing_Effect();
        _effect.Stop_Satono_Cross_Rushing_Effect();        
    }

    #endregion
    // ===================================================================
    #region Bメロ
        /*
         * Bメロ攻撃、回転しながら画面左右から突進、最上段で突進するとき弾幕を出す
         */

    private class ConfigB {
        public readonly Vector3 nutral_Pos = new Vector3(0, 0, 0);
        public readonly float[] rush_Height = { 0f, -96f, 0f, 96f };
    }
    private ConfigB configB = new ConfigB();
        

    protected override void Start_Melody_B1() {
        StartCoroutine("Melody_B1_Cor");
    }


    private IEnumerator Melody_B1_Cor() {
        base.Set_Can_Switch_Attack(false);
        yield return new WaitForSeconds(1.5f);
        //回転開始、画面外に出る
        _controller.Change_Animation("RollingRushingBool");
        yield return new WaitForSeconds(0.5f);
        satomai_Move.Start_Move(new Vector3(270, 96f), 0);
        yield return new WaitUntil(satomai_Move.End_Move);
        
        //突進
        int count = 0;
        int direction = -1;
        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.B1) {
            satomai.transform.position = new Vector3(270f * -direction, configB.rush_Height[count]);
            satomai_Move.Start_Move(new Vector3(270f * direction, configB.rush_Height[count]), 1);
            //最上段の時強弾幕
            if(count == 3) {
                _shoot.Shoot_In_Rolling_Rushing(true);
                yield return new WaitForSeconds(3.0f);
                _shoot.Stop_Rolling_Rushing_Shoot();
            }
            //中段の時弱弾幕
            else if(count == 0 || count == 2) {
                _shoot.Shoot_In_Rolling_Rushing(false);
                yield return new WaitForSeconds(2.5f);
                _shoot.Stop_Rolling_Rushing_Shoot();
            }
            yield return new WaitUntil(satomai_Move.End_Move);            
            count = (count + 1) % configB.rush_Height.Length;
            direction *= -1;
        }

        //中心に戻る        
        satomai.transform.position = new Vector3(270f * -direction, configB.nutral_Pos.y);
        satomai_Move.Start_Move(configB.nutral_Pos, 0);
        yield return new WaitUntil(satomai_Move.End_Move);
        _controller.Change_Animation("IdleBool");

        base.Set_Can_Switch_Attack(true);
        base.Restart_Attack();

    }


    private void Stop_Melody_B1() {
        StopCoroutine("Melody_B1_Cor");        
        _shoot.Stop_Rolling_Rushing_Shoot();
    }

    #endregion
    // ===================================================================
    #region Cメロ
        /*
         * 弾幕前の移動、サビパートでもし移動先の座標にいなかった時も呼ばれる関数なので注意
         */

    private class ConfigC {        
        public float power_Charge_Span = 2.0f;
    }
    private ConfigC configC = new ConfigC();


    protected override void Start_Melody_C() {
        StartCoroutine("Melody_C_Cor");
    }


    private IEnumerator Melody_C_Cor() {
        base.Set_Can_Switch_Attack(false);
        //移動
        _controller.Change_Animation("IdleBool");
        yield return new WaitForSeconds(0.1f);
        satomai.transform.localScale = new Vector3(1, 1, 1);
        satomai_Move.Start_Move(configChorus1.pos_In_Shooting, 2);
        yield return new WaitUntil(satomai_Move.End_Move);

        //溜め
        _effect.Play_Power_Charge_Effect(satomai.transform);
        float time_Count = 0;
        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.C) {
            time_Count += Time.deltaTime;
            yield return null;
        }        
        yield return new WaitForSeconds(configC.power_Charge_Span - time_Count);
        _effect.Stop_Power_Charge_Effect();

        base.Set_Can_Switch_Attack(true);
        base.Restart_Attack();
    }


    public void Stop_Melody_C() {
        StopCoroutine("Melody_C_Cor");
        _effect.Stop_Power_Charge_Effect();
    }

    #endregion
    // ===================================================================
    #region サビ
        /*
         * サビ弾幕攻撃、開始時特定の位置にいなかったらCメロの関数を呼んで移動
         */

    private class ConfigChorus1 {
        public Vector2 nutral_Pos = new Vector2(0, 0);
        public Vector3 pos_In_Shooting = new Vector3(160f, 0);
        public float laser_Span = 1.8f;
        public float talismap_Shoot_Duration = 3.0f;

    }
    private ConfigChorus1 configChorus1 = new ConfigChorus1();


    protected override void Start_Melody_Chorus1() {
        int phase = _controller.Get_Now_Phase();
        if (phase == 1)
            StartCoroutine("Melody_Chorus1_Phase1_Cor");
        else if (phase == 2)
            StartCoroutine("Melody_Chorus1_Phase2_Cor");
    }


    private IEnumerator Melody_Chorus1_Phase1_Cor() {
        base.Set_Can_Switch_Attack(false);

        //特定の位置にいなかったら移動
        if (!Is_Equals_Pos(satomai.transform.position, configChorus1.pos_In_Shooting)) {
            Start_Melody_C();
            yield break;
        }

        _controller.Change_Animation("IdleBool");

        while (melody_Manager.Get_Now_Melody() == MelodyManager.Melody.chorus1) {
            //レーザー
            for (int i = 0; i < 3; i++) {
                _shoot.Shoot_Phase1_Laser();
                yield return new WaitForSeconds(configChorus1.laser_Span);
                if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.chorus1)
                    break;
            }
            //お札弾幕
            _shoot.Shoot_Phase1_Talisman_Bullet();
            for(float t = 0; t < configChorus1.talismap_Shoot_Duration; t += Time.deltaTime) {                
                if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.chorus1)
                    break;
                yield return null;
            }            
            _shoot.Stop_Phase1_Talisman_Shoot();
            yield return new WaitForSeconds(1.0f);
        }       

        //元の位置に移動
        _controller.Change_Animation("IdleBool");
        yield return new WaitForSeconds(1.0f);
        satomai_Move.Start_Move(configChorus1.nutral_Pos, 2);
        yield return new WaitUntil(satomai_Move.End_Move);

        //攻撃再開
        base.Set_Can_Switch_Attack(true);
        base.Restart_Attack();
    }


    private IEnumerator Melody_Chorus1_Phase2_Cor() {
        base.Set_Can_Switch_Attack(false);

        //特定の位置にいなかったら移動
        if (!Is_Equals_Pos(satomai.transform.position, configChorus1.pos_In_Shooting)) {
            Start_Melody_C();
            yield break;
        }

        _controller.Change_Animation("IdleBool");

        //陰陽玉発射
        _shoot.Shoot_Phase2_Yinball_Bullet(true);
        yield return new WaitForSeconds(1.0f);
        _shoot.Shoot_Phase2_Yinball_Bullet(false);
        yield return new WaitForSeconds(7.0f);

        while (melody_Manager.Get_Now_Melody() == MelodyManager.Melody.chorus1) {
            _shoot.Shoot_Phase2_Laser();
            yield return new WaitForSeconds(configChorus1.laser_Span);
        }

        //陰陽玉消す
        _shoot.Delete_Yinball_Bullet();

        //元の位置に移動
        _controller.Change_Animation("IdleBool");
        yield return new WaitForSeconds(1.0f);
        satomai_Move.Start_Move(configChorus1.nutral_Pos, 2);
        yield return new WaitUntil(satomai_Move.End_Move);

        //攻撃再開
        base.Set_Can_Switch_Attack(true);
        base.Restart_Attack();
    }


    //座標の比較
    private bool Is_Equals_Pos(Vector3 a, Vector3 b) {
        if(Mathf.Abs(a.x - b.x) < 16f && Mathf.Abs(a.y - b.y) < 16f) {
            return true;
        }
        return false;
    }


    private void Stop_Melody_Chorus1() {
        StopCoroutine("Melody_Chorus1_Phase1_Cor");
        _shoot.Stop_Phase1_Talisman_Shoot();
        _shoot.Delete_Yinball_Bullet();        
    }

    #endregion
    // ===================================================================

    protected override void Start_Melody_Intro() {
        throw new System.NotImplementedException();
    }

    protected override void Start_Melody_A2() {
        throw new System.NotImplementedException();
    }

    protected override void Start_Melody_B2() {
        throw new System.NotImplementedException();
    }

    protected override void Start_Melody_Pre_Chorus() {
        throw new System.NotImplementedException();
    }

    protected override void Start_Melody_Chorus2() {
        throw new System.NotImplementedException();
    }

    protected override void Start_Melody_Bridge() {
        throw new System.NotImplementedException();
    }

}
