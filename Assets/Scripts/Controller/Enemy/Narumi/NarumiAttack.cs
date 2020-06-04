using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarumiAttack : MonoBehaviour {

    private BGMMelody melody_Manager;
    private NarumiBlockBarrier block_Barrier;
    private MoveTwoPoints _move_Two_Points;
    private MoveConstSpeed _move_Const_Speed;
    [SerializeField] private GridGroundManager ground_Blocks;


	// Use this for initialization
	void Start () {
        melody_Manager = GetComponentInChildren<BGMMelody>();
        _move_Two_Points = GetComponent<MoveTwoPoints>();
        _move_Const_Speed = GetComponent<MoveConstSpeed>();
	}
	
	// Update is called once per frame
	void Update () {
        switch (melody_Manager.Switch_Melody_Trigger()) {
            case BGMMelody.Melody.A: StartCoroutine("Attack_In_Melody_A_Cor"); break;
            case BGMMelody.Melody.B: StartCoroutine("Attack_In_Melody_B_Cor"); break;
            case BGMMelody.Melody.C: StartCoroutine("Attack_In_Melody_C_Cor"); break;
            case BGMMelody.Melody.main: break;
        }        

	}


    //==================================================================
    private IEnumerator Attack_In_Melody_A_Cor() {
        ground_Blocks.Start_Random_Raise(0.5f);
        while(melody_Manager.Get_Now_Melody() == BGMMelody.Melody.A) {
            yield return null;
        }
        Stop_Attack_In_Melody_A();
    }
    

    public void Stop_Attack_In_Melody_A() {
        ground_Blocks.Quit_Random_Raise();
    }


    //==================================================================
    private IEnumerator Attack_In_Melody_B_Cor() {
        ground_Blocks.Start_Random_Shoot(2.0f);
        while(melody_Manager.Get_Now_Melody() == BGMMelody.Melody.B) {
            yield return null;
        }
        Stop_Attack_In_Melody_B();
    }

    public void Stop_Attack_In_Melody_B() {
        ground_Blocks.Quit_Random_Shoot();
        ground_Blocks.Restore_Blocks_To_Original_Pos();
    }


    //==================================================================
    private IEnumerator Attack_In_Melody_C_Cor() {
        //上昇
        _move_Two_Points.Start_Move(new Vector3(transform.position.x, 180f), 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        transform.position = new Vector3(196f, 180f);

        //落下
        _move_Const_Speed.Start_Move(new Vector3(transform.position.x, -48f), 0);
        yield return new WaitUntil(_move_Const_Speed.End_Move);
        //着地、衝撃波
        List<int> order = new List<int>();
        for(int i = 0; i < ground_Blocks.Num(); i++) {
            order.Add(ground_Blocks.Num() - i - 1);
        }
        ground_Blocks.Start_Blocks_Raise(order, 0.1f);
        yield return new WaitForSeconds(1.0f);
        //弾幕位置に移動
        _move_Two_Points.Start_Move(new Vector3(transform.position.x, 32f), 0);
    }
    
}
