using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarumiAttack : BossEnemyAttack {

    private Narumi _main;    
    private NarumiBlockBarrier block_Barrier;
    private NarumiEffects _effect;
    private NarumiShoot _shoot;
    private MoveTwoPoints _move_Two_Points;
    private MoveConstSpeed _move_Const_Speed;
    private CameraShake camera_Shake;
    [SerializeField] private GridGroundManager ground_Blocks;


	// Use this for initialization
	void Start () {        
        block_Barrier = GetComponentInChildren<NarumiBlockBarrier>();
        _effect = GetComponentInChildren<NarumiEffects>();
        _shoot = GetComponentInChildren<NarumiShoot>();
        _main = GetComponent<Narumi>();
        _move_Two_Points = GetComponent<MoveTwoPoints>();
        _move_Const_Speed = GetComponent<MoveConstSpeed>();
        camera_Shake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
	}	


    public override void Stop_Attack() {
        Stop_Attack_In_Melody_A();
        Stop_Attack_In_Melody_B();
        Stop_Attack_In_Melody_C();
        Stop_Attack_In_Melody_Main();
    }


    //===================================================================
    protected override IEnumerator Attack_In_Melody_Intro_Cor() {
        yield return null;
    }
    protected override void Stop_Attack_In_Melody_Main() {
    }
    //==================================================================
    protected override IEnumerator Attack_In_Melody_A_Cor() {
        _main.Change_Animation("AttackBool", 1);
        block_Barrier.Create_Barrier(12, 64f, 0.05f, -1);
        ground_Blocks.Start_Random_Raise(0.5f);
        while(melody_Manager.Get_Now_Melody() == BGMMelody.Melody.A) {
            yield return null;
        }
        Stop_Attack_In_Melody_A();
    }
    

    protected override void Stop_Attack_In_Melody_A() {
        _main.Change_Animation("IdleBool");
        ground_Blocks.Quit_Random_Raise();
        block_Barrier.Delete_Barrier();
    }


    //==================================================================
    protected override IEnumerator Attack_In_Melody_B_Cor() {
        yield return null;      //Stop_Attack_In_Melody_aが呼ばれるのが先にならないよう

        _main.Change_Animation("AttackBool", 1);        
        ground_Blocks.Start_Random_Shoot(2.0f);
        while(melody_Manager.Get_Now_Melody() == BGMMelody.Melody.B) {
            yield return null;
        }
        Stop_Attack_In_Melody_B();
    }

    protected override void Stop_Attack_In_Melody_B() {
        _main.Change_Animation("IdleBool");
        ground_Blocks.Quit_Random_Shoot();
        ground_Blocks.Restore_Blocks_To_Original_Pos();
        block_Barrier.Delete_Barrier();
    }


    //==================================================================
    protected override IEnumerator Attack_In_Melody_C_Cor() {
        //上昇
        _main.Change_Animation("AttackBool", 1);
        _move_Two_Points.Start_Move(new Vector3(transform.position.x, 180f), 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);

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
        _move_Two_Points.Start_Move(new Vector3(transform.position.x, 32f), 0);
        _effect.Play_Power_Charge(-1);
        yield return new WaitUntil(_move_Two_Points.End_Move);          
    }


    protected override void Stop_Attack_In_Melody_C() {
        
    }


    //======================================================================

    protected override IEnumerator Attack_In_Melody_Main_Cor() {
        //弾幕
        _effect.Stop_Power_Charge();
        for(int i = 0; i < 3; i++) {
            _shoot.Shoot_Snow_Shoot();
            yield return new WaitForSeconds(3.0f);
        }        
    }


    protected override void Stop_Attack_In_Melody_Intro() {
    }
}
