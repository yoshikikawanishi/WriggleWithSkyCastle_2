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
            case MelodyManager.Melody.intro: Start_Melody_Intro(); break;
            case MelodyManager.Melody.A1: Start_Melody_A1(); break;
            case MelodyManager.Melody.A2: Start_Melody_A2(); break;
            case MelodyManager.Melody.B1: Start_Melody_B1(); break;
            case MelodyManager.Melody.B2: Start_Melody_B2(); break;
            case MelodyManager.Melody.pre_Chorus: Start_Melody_Pre_Chorus(); break;
            case MelodyManager.Melody.chorus1: Start_Melody_Chorus1(); break;
            case MelodyManager.Melody.chorus2: Start_Melody_Chorus2(); break;
            case MelodyManager.Melody.bridge: Start_Melody_Bridge(); break;
            case MelodyManager.Melody.C: Start_Melody_C(); break;
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


    /// <summary>
    /// メロディ切り替えで攻撃が開始しないようにする
    /// </summary>    
    public void Set_Can_Switch_Attack(bool can_Attack) {
        this.can_Attack = can_Attack;
    }


    /// <summary>
    /// 現在のメロディに対する攻撃を開始する
    /// </summary>
    protected void Restart_Attack() {
        if (!can_Attack)
            return;
        switch (melody_Manager.Get_Now_Melody()) {
            case MelodyManager.Melody.intro: Start_Melody_Intro(); break;
            case MelodyManager.Melody.A1: Start_Melody_A1(); break;
            case MelodyManager.Melody.A2: Start_Melody_A2(); break;
            case MelodyManager.Melody.B1: Start_Melody_B1(); break;
            case MelodyManager.Melody.B2: Start_Melody_B2(); break;
            case MelodyManager.Melody.pre_Chorus: Start_Melody_Pre_Chorus(); break;
            case MelodyManager.Melody.chorus1: Start_Melody_Chorus1(); break;
            case MelodyManager.Melody.chorus2: Start_Melody_Chorus2(); break;
            case MelodyManager.Melody.bridge: Start_Melody_Bridge(); break;
            case MelodyManager.Melody.C: Start_Melody_C(); break;            
        }
    }
    

    protected abstract void Start_Melody_Intro();
    protected abstract void Start_Melody_A1();
    protected abstract void Start_Melody_A2();
    protected abstract void Start_Melody_B1();
    protected abstract void Start_Melody_B2();
    protected abstract void Start_Melody_Pre_Chorus();
    protected abstract void Start_Melody_Chorus1();
    protected abstract void Start_Melody_Chorus2();
    protected abstract void Start_Melody_Bridge();
    protected abstract void Start_Melody_C();
    

    /// <summary>
    /// 全攻撃の終了
    /// </summary>
    public abstract void Stop_Attack();

    /// <summary>
    /// フェーズ切り替え時に呼ばれる
    /// </summary>
    protected abstract void Action_In_Change_Phase(); 

    
}
