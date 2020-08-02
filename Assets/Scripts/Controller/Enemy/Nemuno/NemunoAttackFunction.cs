using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoAttackFunction : MonoBehaviour {

    private Nemuno _controller;
    private NemunoAttack _attack;
    private NemunoSoundEffect _sound;
    private NemunoCollision _collision;
    private NemunoShoot _shoot;
    private NemunoBarrier _barrier;
    private SpriteRenderer _sprite;
    private MoveConstTime _move_Two_Points;

    private GameObject player;
    private CameraShake camera_Shake;    


    private void Awake() {
        //取得
        _controller = GetComponent<Nemuno>();
        _attack = GetComponent<NemunoAttack>();
        _sound = GetComponentInChildren<NemunoSoundEffect>();
        _collision = GetComponent<NemunoCollision>();                
        _shoot = GetComponentInChildren<NemunoShoot>();
        _barrier = GetComponentInChildren<NemunoBarrier>();        
        _sprite = GetComponent<SpriteRenderer>();
        _move_Two_Points = GetComponent<MoveConstTime>();        

        player = GameObject.FindWithTag("PlayerTag");
        camera_Shake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
    }


    //--------------------------------------- 移動用関数--------------------------------------------------
    //移動終了検知用
    private bool is_End_Move = false;
    public bool Is_End_Move() {
        if (is_End_Move) {
            is_End_Move = false;
            return true;
        }
        return false;
    }


    //バックジャンプ
    // 自機と反対側に決まった座標に飛ぶ
    public IEnumerator Back_Jump_Cor() {
        is_End_Move = false;

        int direction = -Player_Direction();
        transform.localScale = new Vector3(direction, 1, 1);

        _controller.Change_Fly_Parameter();
        _controller.Change_Animation("BackJumpBool");
        yield return new WaitForSeconds(0.2f);

        _sound.Play_Jump_Sound(0.04f);
        _move_Two_Points.Start_Move(new Vector3(160f * direction, transform.position.y), 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        _sound.Play_Land_Sound();
        _controller.Change_Land_Paramter();
        _controller.Change_Animation("IdleBool");

        is_End_Move = true;
    }


    //自機の隣にジャンプする
    public IEnumerator Jump_Next_Player_Cor() {
        is_End_Move = false;

        //方向転換とジャンプする先の座標計算
        int direction = Player_Direction();
        transform.localScale = new Vector3(-direction, 1, 1);
        float aim_Pos_X = player.transform.position.x - direction * 32f;
        if (Mathf.Abs(aim_Pos_X) > 200f)
            aim_Pos_X = aim_Pos_X.CompareTo(0) * 200f;

        //ジャンプする
        _controller.Change_Animation("ForwardJumpBool");
        _controller.Change_Fly_Parameter();
        yield return new WaitForSeconds(0.2f);

        _sound.Play_Jump_Sound(0.04f);
        _move_Two_Points.Start_Move(new Vector3(aim_Pos_X, transform.position.y), 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        _sound.Play_Land_Sound();
        _controller.Change_Land_Paramter();
        _controller.Change_Animation("IdleBool");

        is_End_Move = true;
    }


    //自機の方向にダッシュ、途中に攻撃を喰らったら確率でバックジャンプ
    public IEnumerator Dash_Cor(float dash_Distance) {
        is_End_Move = false;

        //移動先の座標        
        int direction = Player_Direction();        
        float x = transform.position.x + direction * Mathf.Abs(dash_Distance);
        if (Mathf.Abs(x) > 200f) {
            x = transform.position.x - direction * Mathf.Abs(dash_Distance);
            direction *= -1;
        }
        //方向
        transform.localScale = new Vector3(-direction, 1, 1);        

        //ダッシュ
        _controller.Change_Land_Paramter();
        _controller.Change_Animation("DashBool");
        _move_Two_Points.Start_Move(new Vector2(x, transform.position.y), 3);

        //移動中に攻撃を喰らったとき確率でバックジャンプ
        int n = Random.Range(0, 2);
        while (!_move_Two_Points.End_Move()) {
            yield return null;
            if (_collision.Damaged_Trigger() && n == 0) {
                StartCoroutine("Back_Jump_Cor");
                yield break;
            }
        }

        _controller.Change_Animation("IdleBool");
        is_End_Move = true;
    }


    //大ジャンプ
    //　direction == 1で左側に飛ぶ、-1で右
    public IEnumerator High_Jump_Cor(int direction) {
        is_End_Move = false;

        transform.localScale = new Vector3(direction, 1, 1);
        _controller.Change_Animation("ForwardJumpBool");
        _controller.Change_Fly_Parameter();
        yield return new WaitForSeconds(0.2f);

        _sound.Play_Jump_Sound(0.08f);
        _move_Two_Points.Start_Move(new Vector3(190f * -direction, -79f), 5);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        _sound.Play_Land_Sound();
        _controller.Change_Land_Paramter();
        _controller.Change_Animation("IdleBool");
        camera_Shake.Shake(0.3f, new Vector2(1f, 1f), true);
        transform.localScale = new Vector3(-direction, 1, 1);

        is_End_Move = true;
    }


    //--------------------------------------------------攻撃用関数------------------------------------------
    //近接攻撃、一回点滅後攻撃
    public IEnumerator Close_Slash_Cor() {
        //ダッシュ
        StartCoroutine("Dash_Cor", 64f);
        yield return new WaitUntil(Is_End_Move);
        //自機の隣にジャンプ
        StartCoroutine("Jump_Next_Player_Cor");
        yield return new WaitUntil(Is_End_Move);        
        //Aメロじゃなくなったらここで終了
        if(_controller._BGM.Get_Now_Melody() != NemunoBGMTimeKeeper.Melody.A) {
            _attack.can_Attack = true;
            yield break;
        }

        //攻撃
        _controller.Change_Animation("SlashTrigger");

        yield return new WaitForSeconds(Pre_Attack_Blink(1, 0.165f));
        yield return new WaitForSeconds(0.18f);

        _controller.Play_Slash_Effect();
        _sound.Play_Slash_Sound();

        yield return new WaitForSeconds(0.68f);

        _controller.Change_Animation("IdleBool");
        yield return new WaitForSeconds(0.2f);

        _attack.can_Attack = true;
    }


    //遠距離攻撃、２回点滅後攻撃、ショット
    public IEnumerator Long_Slash_Cor(int num) {
        //ダッシュ
        StartCoroutine("Dash_Cor", 64f);
        yield return new WaitUntil(Is_End_Move);
        //バックジャンプ
        StartCoroutine("Back_Jump_Cor");
        yield return new WaitUntil(Is_End_Move);
        //Aメロじゃなくなったらここで終了
        if (_controller._BGM.Get_Now_Melody() != NemunoBGMTimeKeeper.Melody.A) {
            _attack.can_Attack = true;
            yield break;
        }

        //攻撃
        _controller.Change_Animation("SlashTrigger");

        yield return new WaitForSeconds(Pre_Attack_Blink(2, 0.165f));
        yield return new WaitForSeconds(0.1f);

        _sound.Play_Slash_Sound();
        _shoot.Shoot_Shotgun(num);
        _controller.Play_Purple_Circle_Effect();
        yield return new WaitForSeconds(0.68f);

        _controller.Change_Animation("IdleBool");
        yield return new WaitForSeconds(0.2f);

        _attack.can_Attack = true;
    }


    //バリアを張って歩く
    public IEnumerator Barrier_Walk_Cor(float walk_Length) {
        //ダッシュ
        StartCoroutine("Dash_Cor", 64f);
        yield return new WaitUntil(Is_End_Move);

        //溜めモーション
        _controller.Change_Animation("BeforeBarrierTrigger");
        yield return new WaitForSeconds(Pre_Attack_Blink(3, 0.33f));

        //バリアを張る
        _controller.Play_Small_Charge_Effect();
        _controller.Change_Animation("IdleBool");
        _barrier.Start_Barrier();
        yield return new WaitForSeconds(1.03f);

        //距離と方向の計算
        int direction = -Player_Direction();
        transform.localScale = new Vector3(direction, 1, 1);
        float pos = transform.position.x - walk_Length * direction;
        if (Mathf.Abs(pos) > 200f)
            pos = 200 * pos.CompareTo(0);

        //ダッシュ
        _controller.Play_Small_Burst_Effect();
        UsualSoundManager.Instance.Play_Shoot_Sound();
        _controller.Change_Animation("DashBool");
        _move_Two_Points.Start_Move(new Vector3(pos, transform.position.y), 1);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        //バリア解除
        _controller.Change_Animation("IdleBool");
        _barrier.Stop_Barrier();
        yield return new WaitForSeconds(0.68f);

        _attack.can_Attack = true;
    }


    //ジャンプして弾幕出す
    public IEnumerator Jump_Slash_Cor(int num) {
        //大ジャンプ
        int direction = transform.position.x.CompareTo(0);
        if (Mathf.Approximately(direction, 0))
            direction = 1;
        StartCoroutine("High_Jump_Cor", direction);
        yield return new WaitUntil(Is_End_Move);
        yield return new WaitForSeconds(0.68f);
        
        //Bメロじゃなくなったらここで終了
        if (_controller._BGM.Get_Now_Melody() != NemunoBGMTimeKeeper.Melody.B) {
            _attack.can_Attack = true;
            yield break;
        }

        //垂直ジャンプ
        _controller.Change_Animation("ForwardJumpBool");
        _controller.Change_Fly_Parameter();
        _move_Two_Points.Start_Move(transform.position + new Vector3(0, 80f), 2);
        yield return new WaitUntil(_move_Two_Points.End_Move);
        _controller.Change_Fly_Parameter();

        //ショット
        _controller.Change_Animation("SlashTrigger");
        _controller.Play_Yellow_Circle_Effect();
        _sound.Play_Before_Slash_Sound();
        _shoot.StartCoroutine("Shoot_Jump_Slash_Cor", num);
        yield return new WaitForSeconds(0.68f);
        _sound.Play_Slash_Sound();
        yield return new WaitForSeconds(0.68f);

        //落下
        _controller.Change_Animation("IdleBool");
        _controller.Change_Land_Paramter();

        //Bメロじゃなくなったら着地を待つ
        if (_controller._BGM.Get_Now_Melody() != NemunoBGMTimeKeeper.Melody.B)
            yield return new WaitForSeconds(1.74f);
                    
        _attack.can_Attack = true;
    }


    //攻撃前点滅モーション
    private float Pre_Attack_Blink(int count, float span) {
        StartCoroutine(Pre_Attack_Blink_Cor(count, span));
        return span * count;
    }

    private IEnumerator Pre_Attack_Blink_Cor(int count, float span) {
        for (int i = 0; i < count; i++) {
            _sound.Play_Before_Slash_Sound();
            yield return new WaitForSeconds(span / 2);
            _sprite.color = new Color(0.7f, 0.7f, 0.7f);
            yield return new WaitForSeconds(span / 2);
            _sprite.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }


    /// <summary>
    /// 自機の方向を取得する
    /// </summary>    
    /// <returns>自機が右にいたら 1, 左にいたら-1を返す</returns>
    private int Player_Direction() {
        int direction = (player.transform.position.x - transform.position.x).CompareTo(0);
        if (direction == 0)
            direction = 1;
        return direction;
    }
}
