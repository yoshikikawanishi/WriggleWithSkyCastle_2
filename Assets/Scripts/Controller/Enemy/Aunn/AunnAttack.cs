using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AunnAttack : MonoBehaviour {

    //コンポーネント
    private AunnAttackFunction _attack_Func;

    public bool[] start_Phase = { true, true };
    public bool can_Attack = true;


    private void Awake() {
        //取得
        _attack_Func = GetComponent<AunnAttackFunction>();
    }    
	
	
    //フェーズ１
    public void Phase1(AunnBGMManager _BGM) {
        //フェーズ開始時の処理
        if (start_Phase[0]) {
            start_Phase[0] = false;

        }

        //攻撃の開始時にfalseに変更、AttackFunctionの方で攻撃終了時にtrueにもどす
        if (can_Attack == false)
            return;

        switch (_BGM.Get_Now_Melody()) {
            case AunnBGMManager.Melody.A:                
                Attack_In_Melody_A_Cor();
                break;
            case AunnBGMManager.Melody.B: break;
            case AunnBGMManager.Melody.C: break;
            case AunnBGMManager.Melody.main: break;
        }
    }

    public void Stop_Phase1() {

    }


    //フェーズ２
    public void Phase2(AunnBGMManager _BGM) {

    }

    public void Stop_Phase2() {

    }


    //Aメロ攻撃用
    private void Attack_In_Melody_A_Cor() {
        can_Attack = false;
        _attack_Func.Dive_And_Jump_Shoot();
    }
}
