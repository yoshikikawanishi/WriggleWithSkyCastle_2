using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossEnemyAttack : MonoBehaviour {

    [SerializeField] protected BGMMelody melody_Manager;


	// Use this for initialization
	void Start () {
        if (melody_Manager == null)
            this.enabled = false;
	}

	
	// Update is called once per frame
	void Update () {
        switch (melody_Manager.Switch_Melody_Trigger()) {
            case BGMMelody.Melody.intro: StartCoroutine("Attack_In_Melody_Intro_Cor"); break;
            case BGMMelody.Melody.A: StartCoroutine("Attack_In_Melody_A_Cor"); break;
            case BGMMelody.Melody.B: StartCoroutine("Attack_In_Melody_B_Cor"); break;
            case BGMMelody.Melody.C: StartCoroutine("Attack_In_Melody_C_Cor"); break;
            case BGMMelody.Melody.main: StartCoroutine("Attack_In_Melody_Main_Cor"); break;
        }
    }


    public abstract void Stop_Attack();
    protected abstract void Stop_Attack_In_Melody_Intro();
    protected abstract void Stop_Attack_In_Melody_A();
    protected abstract void Stop_Attack_In_Melody_B();
    protected abstract void Stop_Attack_In_Melody_C();
    protected abstract void Stop_Attack_In_Melody_Main();

    protected abstract IEnumerator Attack_In_Melody_Intro_Cor();
    protected abstract IEnumerator Attack_In_Melody_A_Cor();
    protected abstract IEnumerator Attack_In_Melody_B_Cor();
    protected abstract IEnumerator Attack_In_Melody_C_Cor();
    protected abstract IEnumerator Attack_In_Melody_Main_Cor();

}
