using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarumiAttack : BossEnemyAttack {

    private Narumi _main;
    private NarumiBlockBarrier block_Barrier;
    private ObjectRectFormationGenerator rect_Block_Gen;
    private NarumiEffects _effect;
    private NarumiShoot _shoot;
    private MoveConstTime _move_Const_Time;
    private MoveConstSpeed _move_Const_Speed;
    private CameraShake camera_Shake;
    [SerializeField] private GridGroundManager ground_Blocks;
    [SerializeField] private BossBattleScroll scroll;
    [SerializeField] private GridGroundManager scroll_Ground_Blocks_First;
    [SerializeField] private GridGroundManager scroll_Ground_Blocks_Second;

    private bool is_Phase2 = false;

    
    void Start() {
        block_Barrier = GetComponentInChildren<NarumiBlockBarrier>();
        rect_Block_Gen = GetComponentInChildren<ObjectRectFormationGenerator>();
        _effect = GetComponentInChildren<NarumiEffects>();
        _shoot = GetComponentInChildren<NarumiShoot>();
        _main = GetComponent<Narumi>();
        _move_Const_Time = GetComponent<MoveConstTime>();
        _move_Const_Speed = GetComponent<MoveConstSpeed>();
        camera_Shake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
    }    


    public override void Stop_Attack() {
        Stop_Melody_A_Phase1();
        Stop_Melody_B_Phase1();
        Stop_Melody_C_Phase1();
        Stop_Melody_Main_Phase1();
        Stop_Melody_A_Phase2();
        Stop_Melody_B_Phase2();
        Stop_Melody_C_Phase2();
        Stop_Melody_Main_Phase2();
        scroll.Stop_Scroll();
    }


    protected override void Start_Melody_Intro() {
    }


    protected override void Start_Melody_A1() {
        if(now_Phase == 1) {
            StartCoroutine("Attack_Melody_A_Phase1_Cor");            
        }
        else {
            StartCoroutine("Attack_Melody_A_Phase2_Cor");
        }
    }


    protected override void Start_Melody_B1() {
        if (now_Phase == 1) {
            StartCoroutine("Attack_Melody_B_Phase1_Cor");            
        }
        else {
            StartCoroutine("Attack_Melody_B_Phase2_Cor");
        }
    }


    protected override void Start_Melody_C() {
        if (now_Phase == 1) {
            StartCoroutine("Attack_Melody_C_Phase1_Cor");
        }
        else {
            StartCoroutine("Attack_Melody_C_Phase2_Cor");
        }
    }


    protected override void Start_Melody_Chorus1() {
        if (now_Phase == 1) {
            StartCoroutine("Attack_Melody_Main_Phase1_Cor");
        }
        else {
            StartCoroutine("Attack_Melody_Main_Phase2_Cor");
        }
    }


    protected override void Action_In_Change_Phase() {
        //フェーズ２開始時
        if(now_Phase == 2) {
            Stop_Attack();
            StartCoroutine("Start_Phase2_Cor");
        }
    }

    //==================================================================
    private IEnumerator Attack_Melody_A_Phase1_Cor() {
        //中心まで移動
        _move_Const_Speed.Start_Move(new Vector3(0, 0), 1);
        yield return new WaitUntil(_move_Const_Speed.End_Move);
        //溜めエフェクト
        _effect.Play_Power_Charge_Small();
        yield return new WaitForSeconds(1.0f);
        _effect.Play_Burst();
        
        _main.Change_Animation("AttackBool", 1);
        //ブロック生成
        block_Barrier.Create_Barrier(12, 64f, 0.05f, -1);
        //地面移動開始
        ground_Blocks.Start_Random_Raise(1.0f);
        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.A1) {
            yield return null;
        }
        Stop_Melody_A_Phase1();
    }
    

    private void Stop_Melody_A_Phase1() {
        _main.Change_Animation("IdleBool");
        ground_Blocks.Quit_Random_Raise();
        block_Barrier.Delete_Barrier();
        _move_Const_Speed.Stop_Move();
    }


    //==================================================================
    private IEnumerator Attack_Melody_B_Phase1_Cor() {
        yield return null;      //Stop_Attack_In_Melody_Aができてからになるよう

        _main.Change_Animation("AttackBool", 1);        
        ground_Blocks.Start_Random_Shoot(1.5f);
        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.B1) {
            yield return null;
        }
        Stop_Melody_B_Phase1();
    }


    private void Stop_Melody_B_Phase1() {
        _main.Change_Animation("IdleBool");
        ground_Blocks.Quit_Random_Shoot();
        ground_Blocks.Restore_Blocks_To_Original_Pos();
        block_Barrier.Delete_Barrier();
    }


    //==================================================================
    private IEnumerator Attack_Melody_C_Phase1_Cor() {
        //上昇
        _main.Change_Animation("AttackBool", 1);
        _move_Const_Time.Start_Move(new Vector3(transform.position.x, 180f), 0);
        yield return new WaitUntil(_move_Const_Time.End_Move);

        transform.position = new Vector3(196f, 180f);
        yield return new WaitForSeconds(1.0f);

        //落下
        _main.Change_Animation("DropBool", 1);
        _move_Const_Speed.Start_Move(new Vector3(transform.position.x, -48f), 0);
        yield return new WaitUntil(_move_Const_Speed.End_Move);
        //着地、衝撃波
        List<int> order = new List<int>();
        for(int i = 0; i < ground_Blocks.Num(); i++) {
            order.Add(ground_Blocks.Num() - i - 1);
        }
        ground_Blocks.Start_Blocks_Raise(order, 0.1f);
        camera_Shake.Shake(0.5f, new Vector2(1.5f, 1.5f), true);
        yield return new WaitForSeconds(1.0f);
        //弾幕位置に移動
        _main.Change_Animation("AttackBool", 1);
        _move_Const_Time.Start_Move(new Vector3(transform.position.x, 32f), 0);
        _effect.Play_Power_Charge(-1);
        yield return new WaitUntil(_move_Const_Time.End_Move);          
    }

    
    private void Stop_Melody_C_Phase1() {
        StopCoroutine("Attack_Melody_C_Phase1_Cor");
        _effect.Stop_Power_Charge();
        _move_Const_Speed.Stop_Move();
        _move_Const_Time.Stop_Move();        
    }


    //======================================================================
    private IEnumerator Attack_Melody_Main_Phase1_Cor() {
        //弾幕
        _effect.Stop_Power_Charge();
        _shoot.Shoot_Snow_Shoot();
        _effect.Play_Burst();
        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.chorus1) { yield return null; }
        _shoot.Stop_Snow_Shoot();                 
    }


    private void Stop_Melody_Main_Phase1() {
        StopCoroutine("Attack_In_Melody_Main_Phase1_Cor");
        _shoot.Stop_Snow_Shoot();
    }


    //====================================================================
    private IEnumerator Start_Phase2_Cor() {
        //取得
        GameObject main_Camera = GameObject.FindWithTag("MainCamera");
        CameraController camera_Controller = main_Camera.GetComponent<CameraController>();
        //攻撃無効化
        Stop_Attack();
        base.Set_Can_Switch_Attack(false);
        //無敵化
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        //インターバル
        yield return new WaitForSeconds(1.5f);
        Stop_Attack();
        //移動
        _move_Const_Time.Start_Move(new Vector3(150f, 0), 1);
        _main.Change_Animation("MoveForwardBool", -1);
        yield return new WaitUntil(_move_Const_Time.End_Move);
        //攻撃再開
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        base.Set_Can_Switch_Attack(true);
        base.Restart_Attack();
        //カメラの子に
        transform.SetParent(main_Camera.transform);        
        //カメラを移動        
        if(camera_Controller != null)
            camera_Controller.enabled = false;
        while (main_Camera.transform.position.x < 512f) {
            main_Camera.transform.position += new Vector3(Time.deltaTime * 70f, 0, 0);
            yield return null;
        }
        //スクロール開始
        scroll.Start_Scroll();
        _main.Change_Animation("MoveBackBool", 1);
    }


    //====================================================================
    private IEnumerator Attack_Melody_A_Phase2_Cor() {
        yield return new WaitForSeconds(2.0f);
        scroll_Ground_Blocks_First.Start_Random_Raise(1.1f);
        scroll_Ground_Blocks_Second.Start_Random_Raise(1.1f);
        _shoot.Start_Jizo_Bullet_Dropping(2.0f);
        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.A1) {
            yield return null;
        }
        _shoot.Stop_Jizo_Bullet_Dropping();
        Stop_Melody_A_Phase2();
    }

    
    private void Stop_Melody_A_Phase2() {
        StopCoroutine("Attack_Melody_A_Phase2_Cor");
        scroll_Ground_Blocks_First.Quit_Random_Raise();
        scroll_Ground_Blocks_Second.Quit_Random_Raise();
        _shoot.Stop_Jizo_Bullet_Dropping();
    }


    //====================================================================
    private IEnumerator Attack_Melody_B_Phase2_Cor() {
        GameObject main_Camera = GameObject.FindWithTag("MainCamera");

        yield return new WaitForSeconds(2.0f);
        //ショット開始
        StartCoroutine("Yellow_Talisman_Shoot_Cor");

        //ブロック生成
        Vector3 pos = new Vector2(260f, -80f);
        
        while (melody_Manager.Get_Now_Melody() == MelodyManager.Melody.B1) {
            yield return new WaitForSeconds(Random.Range(4.0f, 6.0f));
            rect_Block_Gen.Generate(main_Camera.transform.position + pos);            
        }
        Stop_Melody_B_Phase2();
    }


    private IEnumerator Yellow_Talisman_Shoot_Cor() {
        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.B1) {
            yield return new WaitForSeconds(2.0f);
            _effect.Play_Power_Charge_Small();
            yield return new WaitForSeconds(1.0f);
            _effect.Play_Yellow_Circle();
            _shoot.Shoot_Yellow_Talisman_Shoot_Strong();            
        }
    }


    private void Stop_Melody_B_Phase2() {
        StopCoroutine("Attack_Melody_B_Phase2_Cor");
        StopCoroutine("Yellow_Talisman_Shoot_Cor");
    }


    //====================================================================
    private IEnumerator Attack_Melody_C_Phase2_Cor() {
        yield return null;
    }


    private void Stop_Melody_C_Phase2() {

    }


    //====================================================================
    private IEnumerator Attack_Melody_Main_Phase2_Cor() {
        _effect.Play_Power_Charge_Red(1.0f);

        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.chorus1) {
            _effect.Play_Power_Charge_Small();
            yield return new WaitForSeconds(1.0f);
            Vector3 pos = new Vector3(Random.Range(-50f, 100f), Random.Range(-100f, 100f));
            _shoot.Shoot_Big_Bullet(pos);
            _effect.Play_Burst_Red();
            yield return new WaitForSeconds(4.0f);
        }
        yield return null;
    }
    //====================================================================

    private void Stop_Melody_Main_Phase2() {

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
