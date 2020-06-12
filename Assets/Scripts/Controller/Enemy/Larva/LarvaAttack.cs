using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Tempo	    168
0.357		0.34
0.714		0.70
1.071		1.05
1.428		1.41
1.785		
2.142 		2.13

3.213		3.20

4.641		4.625

6.069		6.05
     */

public class LarvaAttack : MonoBehaviour {

    //コンポーネント
    private Larva _controller;
    private LarvaShootObj shoot_Obj;
    private MoveTwoPoints _move;

    //自機
    private GameObject player;

    private bool start_Phase1 = true;
    private bool start_Phase2 = true;
    private bool is_Direct_Player = false;



    private void Awake() {
        //取得
        _controller = GetComponent<Larva>();
        shoot_Obj = GetComponentInChildren<LarvaShootObj>();
        _move = GetComponent<MoveTwoPoints>();

        player = GameObject.FindWithTag("PlayerTag");
    }




    #region Phase1
    //フェーズ1
    public void Do_Phase1() {
        if (start_Phase1) {
            start_Phase1 = false;
            StartCoroutine("Phase1_Cor");
        }
        //自機の方向を向く
        if(player != null && is_Direct_Player) {
            Direct_At_Player();
        }
    }

    private IEnumerator Phase1_Cor() {
        //初期設定
        _controller.Play_Battle_Effect();

        while (true) {
            //弾幕1
            _controller.Play_Charge_Effect(2.13f);
            yield return new WaitForSeconds(2.13f);
            _controller.Play_Burst_Effect();
            shoot_Obj.StartCoroutine("Shoot_Green_Bullet_Cor", 2);
            yield return new WaitForSeconds(2.13f);                        

            //自機を追従、鱗粉弾発射
            Start_Trace_Player();
            for (int i = 0; i < 2; i++) {
                yield return new WaitForSeconds(2.13f);
                _controller.StartCoroutine("Pre_Action_Blink");
                _controller.Play_Small_Charge_Effect();
                yield return new WaitForSeconds(1.05f);
                shoot_Obj.Shoot_Scales_Bullet(12, 180f);
                _controller.Play_Scales_Effect();
            }
            yield return new WaitForSeconds(1.41f);
            Quit_Trace_Player();

            //突進攻撃
            StartCoroutine("Dash_Attack");
            yield return new WaitForSeconds(5.0f);            
                        
        }
    }

    //フェーズ1終了
    public void Stop_Phase1() {        
        _controller.Quit_Battle_Effect();
        _controller.Stop_Charge_Effect();
        StopAllCoroutines();
        _move.StopAllCoroutines();
        GetComponent<MoveMotion>().Stop_Move();
        shoot_Obj.StopAllCoroutines();
        Quit_Trace_Player();
    }
    #endregion





    #region Phase2
    //フェーズ2
    public void Do_Phase2() {
        if (start_Phase2) {
            start_Phase2 = false;
            Stop_Phase1();
            StartCoroutine("Phase2_Cor");
        }
        if(player != null && is_Direct_Player) {
            Direct_At_Player();
        }
    }

    private IEnumerator Phase2_Cor() {
        //無敵化、移動
        GetComponent<BossCollisionDetection>().Become_Invincible();
        _move.Start_Move(new Vector3(180f, -32f));
        yield return new WaitUntil(_move.End_Move);
        yield return new WaitForSeconds(0.7f);
        GetComponent<BossCollisionDetection>().Release_Invincible();

        //初期設定
        _controller.Play_Battle_Effect();

        while (true) {
            //自機を追従、鱗粉弾発射
            Start_Trace_Player();
            for (int i = 0; i < 2; i++) {
                yield return new WaitForSeconds(2.13f);
                _controller.StartCoroutine("Pre_Action_Blink");
                _controller.Play_Small_Charge_Effect();
                yield return new WaitForSeconds(1.05f);
                for (int j = 0; j < 2; j++) {
                    shoot_Obj.Shoot_Scales_Bullet((j+1) * 20, (j+1) * 150f);
                    _controller.Play_Scales_Effect();
                    yield return new WaitForSeconds(0.34f);
                }
            }
            yield return new WaitForSeconds(1.41f);
            Quit_Trace_Player();

            //突進攻撃
            StartCoroutine("Dash_Attack");
            yield return new WaitForSeconds(4.625f);

            //移動
            _controller.Play_Charge_Effect(2.13f);            
            _move.Start_Move(new Vector3(0, 110f));
            yield return new WaitForSeconds(2.13f);

            //弾幕2            
            shoot_Obj.Shoot_Dif_Bullet();
            _controller.Play_Burst_Effect();
            yield return new WaitForSeconds(1.05f);
            _controller.Play_Burst_Effect();
            shoot_Obj.StartCoroutine("Shoot_Green_Bullet_Cor", 3);
            yield return new WaitForSeconds(6.05f);
            
            //移動
            _move.Start_Move(new Vector3(130f, -32f));
            yield return new WaitUntil(_move.End_Move);
        }
    }
    
    //フェーズ2終了
    public void Stop_Phase2() {
        _controller.Quit_Battle_Effect();
        _controller.Stop_Charge_Effect();
        StopAllCoroutines();
        _move.StopAllCoroutines();
        GetComponent<MoveMotion>().Stop_Move();
        shoot_Obj.StopAllCoroutines();
        Quit_Trace_Player();
    }
    #endregion


    //突進攻撃
    private IEnumerator Dash_Attack() {
        int direction = transform.position.x > 0 ? -1 : 1;
        //移動
        _move.Start_Move(new Vector3(-200f * direction, 16f), 1);
        yield return new WaitUntil(_move.End_Move);        
        transform.localScale = new Vector3(-1 * direction, 1, 1);        
        //予備動作
        GoAroundMotion _around_Motion = GetComponent<GoAroundMotion>();
        _around_Motion.Start_Motion(transform.position + new Vector3(4f * direction, 0), 720f);
        yield return new WaitUntil(_around_Motion.Is_End_Motion);
        //突進
        UsualSoundManager.Instance.Play_Attack_Sound();
        _controller.Play_Dash_Attack_Effect();
        MoveMotion _motion = GetComponent<MoveMotion>();
        if (direction == 1) {
            _motion.Start_Move(0);
        }
        else {
            _motion.Start_Move(1);
        }
        yield return new WaitForSeconds(2.13f);
        transform.localScale = new Vector3(1 * direction, 1, 1);
    }



    //自機の方向を向く
    private void Direct_At_Player() {
        if(player.transform.position.x <= transform.position.x) {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }


    //自機の追従
    private void Start_Trace_Player() {
        GetComponent<GravitatePlayer>().enabled = true;
        is_Direct_Player = true;
    }

    private void Quit_Trace_Player() {
        GetComponent<GravitatePlayer>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        is_Direct_Player = false;
    }


}
