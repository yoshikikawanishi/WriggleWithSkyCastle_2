using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossEnemyAttack : MonoBehaviour {

    [SerializeField] protected BossEnemy boss_Enemy;
    [SerializeField] protected MelodyManager melody_Manager;

    protected int now_Phase = 0;
    protected bool can_Attack = true;

    private MelodyManager.Melody now_Melody;


    // Use this for initialization
    void Start () {
        if (melody_Manager == null)
            this.enabled = false;
	}

	
	// Update is called once per frame
	protected void Update () {
        //フェーズ切り替え時
        if (now_Phase != boss_Enemy.Get_Now_Phase()) {
            now_Phase = boss_Enemy.Get_Now_Phase();
            Action_In_Change_Phase();
        }

        if (!can_Attack) {
            return;
        }

        //メロディ切り替え時
        switch (Switch_Melody_Trigger()) {
            case MelodyManager.Melody.intro:    Start_Melody_Intro();   break;
            case MelodyManager.Melody.A:        Start_Melody_A();       break;
            case MelodyManager.Melody.B:        Start_Melody_B();       break;
            case MelodyManager.Melody.C:        Start_Melody_C();       break;
            case MelodyManager.Melody.main:     Start_Melody_Main();    break;
        } 
    }


    //メロディ切り替え検出
    private MelodyManager.Melody Switch_Melody_Trigger() {
        if(now_Melody != melody_Manager.Get_Now_Melody()) {
            now_Melody = melody_Manager.Get_Now_Melody();
            return now_Melody;
        }
        return MelodyManager.Melody.none;
    }


    //メロディ切り替えで攻撃が開始しないようにする
    public void Set_Can_Attack(bool can_Attack) {
        this.can_Attack = can_Attack;
    }
    

    //現在のメロディに対する攻撃を開始する
    protected void Restart_Attack() {
        if (!can_Attack)
            return;
        switch (melody_Manager.Get_Now_Melody()) {
            case MelodyManager.Melody.intro: Start_Melody_Intro(); break;
            case MelodyManager.Melody.A: Start_Melody_A(); break;
            case MelodyManager.Melody.B: Start_Melody_B(); break;
            case MelodyManager.Melody.C: Start_Melody_C(); break;
            case MelodyManager.Melody.main: Start_Melody_Main(); break;
        }
    }
    

    protected abstract void Start_Melody_Intro();
    protected abstract void Start_Melody_A();
    protected abstract void Start_Melody_B();
    protected abstract void Start_Melody_C();
    protected abstract void Start_Melody_Main();

    public abstract void Stop_Attack();

    /// <summary>
    /// フェーズ切り替え時に呼ばれる
    /// </summary>
    protected abstract void Action_In_Change_Phase(); 

    
}
