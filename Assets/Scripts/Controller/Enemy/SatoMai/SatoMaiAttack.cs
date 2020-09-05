using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatoMaiAttack : BossEnemyAttack {

    [SerializeField] private GameObject satomai;
    [SerializeField] private GameObject satono;
    [SerializeField] private GameObject mai;

    private SatoMai _controller;
    private MoveConstTime satomai_Move;
    private MoveConstTime satono_Move;
    private MoveConstTime mai_Move;

    private GameObject player;


    void Start() {
        //取得
        _controller = GetComponent<SatoMai>();
        satomai_Move = satomai.GetComponent<MoveConstTime>();
        satono_Move = satono.GetComponent<MoveConstTime>();
        mai_Move = mai.GetComponent<MoveConstTime>();
        player = GameObject.FindWithTag("PlayerTag");
    }

    public override void Stop_Attack() {
        
    }

    protected override void Action_In_Change_Phase() {
        
    }

    protected override void Start_Melody_Intro() {

    }

    // ===================================================================
    #region Aメロ

    private class ConfigA {
        public readonly Vector3 nutral_Pos = new Vector3(0, 0, 0);
        public readonly Vector3 satono_Outside_Pos = new Vector3(270f, 48f);
        public readonly Vector3 mai_Outside_Pos = new Vector3(-270f, 48f);
    }
    private ConfigA configA = new ConfigA();


    protected override void Start_Melody_A() {
        StartCoroutine("Melody_A_Cor");
    }


    private IEnumerator Melody_A_Cor() {
        base.Set_Can_Switch_Attack(false);
        //画面外に分かれて出る
        _controller.Change_Animation("DivideAndGoOutBool");
        yield return new WaitForSeconds(0.5f);  
        satono_Move.Start_Move(configA.satono_Outside_Pos, 0);
        mai_Move.Start_Move(configA.mai_Outside_Pos, 0);
        yield return new WaitForSeconds(2.0f);

        //突進攻撃        
        _controller.Change_Animation("CrossRushingBool");
        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.A) {
            Rush_By_Satono();
            yield return new WaitForSeconds(0.32f);
            Rush_By_Mai();
            yield return new WaitForSeconds(1.5f);
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
    private void Rush_By_Satono() {
        Vector2 pos = player.transform.position + new Vector3(Random.Range(-32f, 32f), 0);
        int direction = (player.transform.position.x - pos.x).CompareTo(0);
        if (direction == 0) direction = 1;

        satono.transform.position = new Vector3(pos.x, 160f);
        satono.transform.localScale = new Vector3(direction, 1, 1);
        satono_Move.Start_Move(new Vector3(pos.x, -160f), 1);
    }


    //舞突進攻撃
    private void Rush_By_Mai() {
        Vector2 pos = player.transform.position + new Vector3(0, Random.Range(-8f, 40f));
        int direction = Random.Range(0, 2) == 0 ? 1 : -1;

        mai.transform.position = new Vector3(-270f * direction, pos.y);
        mai.transform.localScale = new Vector3(direction, 1, 1);
        mai_Move.Start_Move(new Vector3(270f * direction, pos.y), 1);
    }

    #endregion
    // ===================================================================
    #region Bメロ

    private class ConfigB {
        public readonly Vector3 nutral_Pos = new Vector3(0, 0, 0);
        public readonly float[] rush_Height = { -96f, 0f, 96f, 0 };
    }
    private ConfigB configB = new ConfigB();
        

    protected override void Start_Melody_B() {
        StartCoroutine("Melody_B_Cor");
    }


    private IEnumerator Melody_B_Cor() {
        base.Set_Can_Switch_Attack(false);
        //回転開始、画面外に出る
        _controller.Change_Animation("RollingRushingBool");
        yield return new WaitForSeconds(0.5f);
        satomai_Move.Start_Move(new Vector3(270, 96f), 0);
        yield return new WaitUntil(satomai_Move.End_Move);
        
        //突進
        int count = 0;
        int direction = -1;
        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.B) {
            satomai.transform.position = new Vector3(270f * -direction, configB.rush_Height[count]);
            satomai_Move.Start_Move(new Vector3(270f * direction, configB.rush_Height[count]), 1);
            //最上段の時弾幕
            if(count == 2) {

                yield return new WaitForSeconds(3.0f);
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

    #endregion
    // ===================================================================
    #region Cメロ

    protected override void Start_Melody_C() {
        
    }

    #endregion
    // ===================================================================
    #region サビ

    protected override void Start_Melody_Main() {
        
    }

    #endregion
    // ===================================================================
}
