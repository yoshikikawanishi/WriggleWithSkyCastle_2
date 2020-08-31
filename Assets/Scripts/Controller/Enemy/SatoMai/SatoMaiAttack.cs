using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatoMaiAttack : BossEnemyAttack {

    [SerializeField] private GameObject satomai;
    [SerializeField] private GameObject satono;
    [SerializeField] private GameObject mai;

    private SatoMai _controller;
    private MoveConstTime satono_Move;
    private MoveConstTime mai_Move;


    void Start() {
        //取得
        _controller = GetComponent<SatoMai>();
        satono_Move = satono.GetComponent<MoveConstTime>();
        mai_Move = mai.GetComponent<MoveConstTime>();
    }

    public override void Stop_Attack() {
        
    }

    protected override void Action_In_Change_Phase() {
        
    }

    protected override void Start_Melody_Intro() {

    }

    // ===================================================================
    protected override void Start_Melody_A() {
        
    }

    private IEnumerator Melody_A_Cor() {
        Set_Can_Switch_Attack(false);
        //画面外に分かれて出る
        _controller.Change_Animation("DivideAndGoOut");
        satono_Move.Start_Move(new Vector3(250f, 48f), 0);
        mai_Move.Start_Move(new Vector3(-250f, 48f), 0);
        yield return new WaitForSeconds(2.0f);

        //突進攻撃
        while(melody_Manager.Get_Now_Melody() == MelodyManager.Melody.A) {
            yield return null;
        }
        
        //TODO : 入ってきて合流

        Set_Can_Switch_Attack(true);
        Restart_Attack();
    }

    // ===================================================================

    protected override void Start_Melody_B() {
        
    }

    protected override void Start_Melody_C() {
        
    }

    protected override void Start_Melody_Main() {
        
    }
}
