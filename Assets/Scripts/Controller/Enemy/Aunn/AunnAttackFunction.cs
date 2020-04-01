using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃の関数置き場
/// </summary>
public class AunnAttackFunction : MonoBehaviour {

    private AunnController _controller;
    private AunnAttack _attack;
    private AunnShoot _shoot;
    private Rigidbody2D _rigid;
    private MoveMotion _move;
    private MoveConstSpeed _move_Const_Speed;
    private MoveTwoPoints _move_Two_Points;

    private ChildColliderTrigger foot_Collision;

    private GameObject player;


    void Awake() {
        //取得
        _controller = GetComponent<AunnController>();
        _attack = GetComponent<AunnAttack>();
        _shoot = GetComponentInChildren<AunnShoot>();
        _rigid = GetComponent<Rigidbody2D>();
        _move = GetComponent<MoveMotion>();
        _move_Const_Speed = GetComponent<MoveConstSpeed>();
        _move_Two_Points = GetComponent<MoveTwoPoints>();
        foot_Collision = transform.Find("Foot").GetComponent<ChildColliderTrigger>();
        player = GameObject.FindWithTag("PlayerTag");
    }

    void Start() {
            
    }


    //--------------------------移動用関数------------------------------
    private bool is_End_Move = false;
    public bool Is_End_Move() {
        if (is_End_Move) {
            is_End_Move = false;
            return true;
        }
        return false;
    }


    //端にダッシュする
    private IEnumerator Dash_To_Side_Cor() {
        is_End_Move = false;
        _controller.Change_Land_Parameter();

        //方向
        int direction = transform.position.x.CompareTo(0);
        if(direction == 0) { direction = 1; }
        transform.localScale = new Vector3(-direction, 1, 1);
        //移動
        _move_Const_Speed.Start_Move(new Vector2(210f * direction, transform.position.y), 0);
        yield return new WaitUntil(_move_Const_Speed.End_Move);

        transform.localScale = new Vector3(direction, 1, 1);

        is_End_Move = true;
    }


    //壁に張り付く
    private IEnumerator Jump_On_Wall_Cor(Vector2 next_Pos) {
        is_End_Move = false;
        _controller.Change_Fly_Parameter();

        //方向
        int direction = (next_Pos.x - transform.position.x).CompareTo(0);
        if(direction == 0) { direction = 1; }
        transform.localScale = new Vector3(-direction, 1, 1);
        //移動
        _move_Two_Points.Start_Move(next_Pos, 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        transform.localScale = new Vector3(direction, 1, 1);

        is_End_Move = true;
    }
   

    //--------------------------攻撃用関数------------------------------
    // ※※※※  攻撃終了時に _attack.can_Attack をtrueにすること  ※※※※


    //潜行、地面から自機追従、ジャンプ、ショット
    public void Dive_And_Jump_Shoot() {
        StartCoroutine("Dive_And_Jump_Shoot_Cor");
    }

    private IEnumerator Dive_And_Jump_Shoot_Cor() {
        //取得
        BossCollisionDetection _collision = GetComponent<BossCollisionDetection>();
        TracePlayer _trace = GetComponent<TracePlayer>();
        if(_trace == null) {
            _trace = gameObject.AddComponent<TracePlayer>();
            _trace.kind = TracePlayer.Kind.onlyX;
            _trace.speed = 2.5f;
        }
        _trace.enabled = false;


        //端にダッシュする
        StartCoroutine("Dash_To_Side_Cor");
        yield return new WaitUntil(Is_End_Move);

        if(_controller._BGM.Get_Now_Melody() != AunnBGMManager.Melody.A) {
            _attack.can_Attack = true;
            yield break;
        }

        //地面に潜る
        _controller.Change_Fly_Parameter();
        _move.Start_Move(0);
        yield return new WaitUntil(_move.Is_End_Move);

        //当たり判定を消して自機を追いかける
        _collision.Become_Invincible();
        _trace.enabled = true;
        yield return new WaitForSeconds(2.2f);
        _trace.enabled = false;

        yield return new WaitForSeconds(0.8f);

        //ジャンプして弾幕発射
        _collision.Release_Invincible();
        _move.Start_Move(1);
        yield return new WaitUntil(_move.Is_End_Move);
        _shoot.Shoot_Short_Curve_Laser();
        yield return new WaitForSeconds(0.5f);

        //落下
        _controller.Change_Land_Parameter();
        yield return new WaitUntil(foot_Collision.Hit_Trigger);

        yield return new WaitForSeconds(0.8f);

        _attack.can_Attack = true;
    }


    //壁に張り付いて突進、弾を配置
    public void Jump_On_Wall_And_Rush() {
        StartCoroutine("Jump_On_Wall_And_Rush_Cor");
    }

    private IEnumerator Jump_On_Wall_And_Rush_Cor() {
        //自機のいない方の壁に飛びつく
        int direction = (player.transform.position.x - transform.position.x).CompareTo(0);
        if (direction == 0) { direction = 1; }
        StartCoroutine("Jump_On_Wall_Cor", new Vector2(230f * -direction, 48f));
        yield return new WaitUntil(Is_End_Move);
        yield return new WaitForSeconds(0.5f);

        //弾の配置開始
        _shoot.Start_Deposite_Purple_Bullet();

        //反対側の壁に飛びつく
        StartCoroutine("Jump_On_Wall_Cor", new Vector2(230f * direction, 80f));
        yield return new WaitUntil(Is_End_Move);

        //弾の配置終了
        _shoot.Stop_Deposit_Purple_Bullet();

        yield return new WaitForSeconds(0.5f);

        //斜め下に突進する
        _rigid.velocity = new Vector2(-transform.localScale.x * 160f, -200f);
        yield return new WaitUntil(foot_Collision.Hit_Trigger);
        //着地
        _controller.Change_Land_Parameter();                

        yield return new WaitForSeconds(2.0f);

        _attack.can_Attack = true;
    }

}
