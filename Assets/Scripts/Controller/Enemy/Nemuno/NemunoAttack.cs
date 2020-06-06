using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoAttack : MonoBehaviour {

    //コンポーネント        
    private Nemuno _controller;
    private NemunoShoot _shoot;
    private NemunoBarrier _barrier;
    private NemunoAttackFunction _attack_Func;
    private MoveTwoPoints _move_Two_Points;
    
    //攻撃種類
    private enum AttackKind {
        close_Slash,
        long_Slash,
        barrier,        
    }
    private AttackKind pre_Attack = AttackKind.barrier;       

    private bool[] start_Phase = { true, true };
    public bool can_Attack = true;


    private void Awake() {
        //取得
        _controller = GetComponent<Nemuno>();
        _attack_Func = GetComponent<NemunoAttackFunction>();
        _shoot = GetComponentInChildren<NemunoShoot>();
        _barrier = GetComponentInChildren<NemunoBarrier>();              
        _move_Two_Points = GetComponent<MoveTwoPoints>();        
    }


    private void Start() {
        //初期設定
        _barrier.gameObject.SetActive(false);
        can_Attack = true;
    }


    #region Phase1
    //==========================================フェーズ１===================================
    public void Phase1(NemunoBGMTimeKeeper _BGM) {
        //フェーズ開始時の処理
        if (start_Phase[0]) {
            start_Phase[0] = false;
            _controller.Play_Battle_Effect();
        }
        
        if (!can_Attack)
            return;        

        //BGMのタイミングによって攻撃を変える
        switch (_BGM.Get_Now_Melody()) {
            case NemunoBGMTimeKeeper.Melody.A:
                Phase1_Melody_A(10);
                break;
            case NemunoBGMTimeKeeper.Melody.B:
                Phase1_Melody_B(18);
                break;
            case NemunoBGMTimeKeeper.Melody.main:
                StartCoroutine("Phase1_Melody_Main_Cor", _BGM);
                can_Attack = false;
                break;
        }       
    }

    private void Phase1_Melody_A(int bullet_Num) {
        //次の攻撃を選択する
        AttackKind next_Attack = Select_Next_Attack(pre_Attack);
        pre_Attack = next_Attack;
        //選択した攻撃を開始する
        switch (next_Attack) {
            case AttackKind.close_Slash:    _attack_Func.StartCoroutine("Close_Slash_Cor");         break;
            case AttackKind.long_Slash:     _attack_Func.StartCoroutine("Long_Slash_Cor", bullet_Num); break;
            case AttackKind.barrier:        _attack_Func.StartCoroutine("Barrier_Walk_Cor", 200f);  break;
        }

        //攻撃無効化(有効化は攻撃のコルーチン内で行う)
        can_Attack = false;
    }


    private void Phase1_Melody_B(int bullet_Num) {
        //大ジャンプとジャンプショット
        _attack_Func.StartCoroutine("Jump_Slash_Cor", bullet_Num);
        //攻撃無効化(有効化は攻撃のコルーチン内で行う)
        can_Attack = false;
    }
    

    //サビ
    private IEnumerator Phase1_Melody_Main_Cor(NemunoBGMTimeKeeper _BGM) {       

        //弾幕前溜め        
        _attack_Func.StartCoroutine("High_Jump_Cor", -1);
        yield return new WaitUntil(_attack_Func.Is_End_Move);

        //サビ開始までの時間を計算
        float wait_Time = _BGM.BGM_Time_Keeper[4] - (Time.unscaledTime  - _BGM.Get_BGM_Launch_Time()) % _BGM.BGM_Time_Keeper[5];
        if (wait_Time < 0 || wait_Time > 10)
            wait_Time = 2.0f;

        transform.localScale = new Vector3(1, 1, 1);
        _controller.Play_Charge_Effect(wait_Time);

        //移動
        _controller.Change_Animation("ShootBool");
        _controller.Change_Fly_Parameter();
        yield return new WaitForSecondsRealtime(1.0f);
        _move_Two_Points.Start_Move(new Vector3(160f, 8f), 4);
        yield return new WaitForSeconds(wait_Time - 1.0f);

        //弾幕攻撃
        _controller.Play_Burst_Effect();
        _shoot.Start_Kunai_Shoot();
        UsualSoundManager.Instance.Play_Shoot_Sound();
        yield return new WaitForSeconds(10.0f);

        if (_BGM.Get_Now_Melody() != NemunoBGMTimeKeeper.Melody.main) {
            _controller.Change_Land_Paramter();
            _controller.Change_Animation("IdleBool");
            yield return new WaitForSeconds(1.5f);
        }        

        can_Attack = true;       
    }
    

    //フェーズ１終了の処理
    private void Stop_Phase1() {
        StopAllCoroutines();
        _attack_Func.StopAllCoroutines();
        _controller.Change_Animation("IdleBool");
        _controller.Change_Land_Paramter();
        _controller.Stop_Charge_Effect();
        _controller.Quit_Battle_Effect();
        _barrier.Stop_Barrier();
        _shoot.Stop_Kunai_Shoot();
        _move_Two_Points.Stop_Move();
    }
    #endregion

    
    #region Phase2

    public void Phase2(NemunoBGMTimeKeeper _BGM) {
        //フェーズ開始時の処理
        if (start_Phase[1]) {
            Stop_Phase1();
            Raise_Move_Speed();
            start_Phase[1] = false;
            can_Attack = false;            
            StartCoroutine("Phase2_Launch_Attack");
        }

        if (!can_Attack)
            return;

        //BGMのタイミングによって攻撃を変える
        switch (_BGM.Get_Now_Melody()) {
            case NemunoBGMTimeKeeper.Melody.A:
                Phase1_Melody_A(25);
                break;
            case NemunoBGMTimeKeeper.Melody.B:
                Phase1_Melody_B(50);
                break;
            case NemunoBGMTimeKeeper.Melody.main:
                StartCoroutine("Phase2_Melody_Main_Cor", _BGM);
                can_Attack = false;
                break;
        }
    }


    //フェーズ２開始時のブロック弾幕
    private IEnumerator Phase2_Launch_Attack() {        
        //無敵化
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        yield return new WaitForSeconds(1.5f);

        //大ジャンプ
        _attack_Func.StartCoroutine("High_Jump_Cor", -1);
        yield return new WaitUntil(_attack_Func.Is_End_Move);

        //溜め
        transform.localScale = new Vector3(1, 1, 1);
        _controller.Play_Charge_Effect(2.0f);
        yield return new WaitForSeconds(2.0f);
        
        //無敵化解除
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");         

        //ブロック弾発射
        _controller.Play_Burst_Effect();
        UsualSoundManager.Instance.Play_Shoot_Sound();
        _shoot.StartCoroutine("Play_Square_Blocks_Attack");
        yield return new WaitForSeconds(8.0f);

        _controller.Play_Battle_Effect();
        can_Attack = true;
    }


    //フェーズ２サビ
    private IEnumerator Phase2_Melody_Main_Cor(NemunoBGMTimeKeeper _BGM) {

        int direction = transform.localScale.x.CompareTo(0);
        if (Mathf.Approximately(direction, 0))
            direction = 1;

        //大ジャンプ 
        _attack_Func.StartCoroutine("High_Jump_Cor", direction);
        yield return new WaitUntil(_attack_Func.Is_End_Move);

        //サビ開始までの時間を計算
        float wait_Time = _BGM.BGM_Time_Keeper[4] - (Time.unscaledTime  - _BGM.Get_BGM_Launch_Time()) % _BGM.BGM_Time_Keeper[5];
        if (wait_Time < 0 || wait_Time > 10)
            wait_Time = 2.0f;

        //溜め開始        
        _controller.Play_Charge_Effect(wait_Time);

        //移動
        _controller.Change_Animation("ShootBool");
        _controller.Change_Fly_Parameter();
        _move_Two_Points.Start_Move(new Vector3(160f * -direction, 8f), 4);
        yield return new WaitForSeconds(1.5f);

        yield return new WaitForSeconds(wait_Time - 1.5f);        

        //弾幕攻撃
        _shoot.Start_Knife_Shoot();
        yield return new WaitForSeconds(6.0f);
        
        _controller.Play_Burst_Effect();
        UsualSoundManager.Instance.Play_Shoot_Sound();
        yield return new WaitForSeconds(1.5f);

        _controller.Change_Land_Paramter();
        _controller.Change_Animation("IdleBool");
        yield return new WaitForSeconds(4.0f);

        can_Attack = true;
    }

   
    //フェーズ２終了時の処理
    public void Stop_Phase2() {        
        StopAllCoroutines();
        _attack_Func.StopAllCoroutines();
        _controller.Change_Animation("IdleBool");
        _controller.Change_Land_Paramter();
        _controller.Stop_Charge_Effect();
        _controller.Quit_Battle_Effect();
        _barrier.Stop_Barrier();
        _shoot.Stop_Knife_Shoot();
        _move_Two_Points.Stop_Move();
    }    
    #endregion


    //=======================================その他===========================================
    //次の攻撃を乱数で選ぶ
    private AttackKind Select_Next_Attack(AttackKind pre_Attack) {
        AttackKind next_Attack = pre_Attack;

        List<AttackKind> kinds = new List<AttackKind> { AttackKind.close_Slash, AttackKind.long_Slash, AttackKind.barrier };
        kinds.Remove(pre_Attack);

        next_Attack = kinds[Random.Range(0, 2)];

        return next_Attack;
    }


    //移動速度の上昇
    private void Raise_Move_Speed() {
        _move_Two_Points.Change_Paramter(0.027f, 48f, 0);    //通常ジャンプ用
        _move_Two_Points.Change_Paramter(0.014f, 0, 1);     //バリア突進用
        _move_Two_Points.Change_Paramter(0.03f, 0, 3);      //ダッシュ用
    }

}
