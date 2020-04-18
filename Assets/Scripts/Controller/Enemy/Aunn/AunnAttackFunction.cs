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
    private AunnEffect _effect;
    private Rigidbody2D _rigid;
    private SpriteRenderer _sprite;
    private MoveMotion _move;
    private MoveConstSpeed _move_Const_Speed;
    private MoveTwoPoints _move_Two_Points;
    private AunnCopy _copy;
    private AunnShoot _copy_Shoot;
    private TracePlayer _trace;

    private ChildColliderTrigger foot_Collision;

    private GameObject player;

    public bool generate_Copy = false;


    void Awake() {
        //取得
        _controller = GetComponent<AunnController>();
        _attack = GetComponent<AunnAttack>();
        _shoot = GetComponentInChildren<AunnShoot>();
        _effect = GetComponentInChildren<AunnEffect>();
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _move = GetComponent<MoveMotion>();
        _move_Const_Speed = GetComponent<MoveConstSpeed>();
        _move_Two_Points = GetComponent<MoveTwoPoints>();
        _copy = GetComponentInChildren<AunnCopy>();
        _copy_Shoot = _copy.GetComponentInChildren<AunnShoot>();
        
        _trace = gameObject.AddComponent<TracePlayer>();
        _trace.kind = TracePlayer.Kind.onlyX;
        _trace.speed = 2.5f;
        _trace.enabled = false;

        foot_Collision = transform.Find("Foot").GetComponent<ChildColliderTrigger>();   
        
        player = GameObject.FindWithTag("PlayerTag");
    }    


    public void Stop_Attack() {
        StopAllCoroutines();
        _move.Stop_Move();
        _move_Const_Speed.Stop_Move();
        _move_Two_Points.Stop_Move();
        _shoot.Stop_Deposit_Purple_Bullet();
        _shoot.Stop_Dog_Bullet();
        _shoot.Stop_Long_Curve_Laser();
        _trace.enabled = false;
        _copy.Delete_Copy();
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
    //direction : 右端に行くとき.1 / 左端に行くとき-1
    public IEnumerator Dash_To_Side_Cor(int direction) {
        is_End_Move = false;
        _controller.Change_Land_Parameter();

        //予備動作
        _move.Start_Move(2);
        yield return new WaitUntil(_move.Is_End_Move);

        //方向        
        direction = direction.CompareTo(0);
        if(direction == 0) { direction = 1; }
        transform.localScale = new Vector3(-direction, 1, 1);
        //移動
        _controller.Change_Animation("DashBool");
        _move_Const_Speed.Start_Move(new Vector2(210f * direction, transform.position.y), 0);
        yield return new WaitUntil(_move_Const_Speed.End_Move);
        _controller.Change_Animation("SquatBool");

        transform.localScale = new Vector3(direction, 1, 1);

        is_End_Move = true;
    }


    //壁に張り付く
    public IEnumerator Jump_On_Wall_Cor(Vector2 next_Pos) {
        is_End_Move = false;
        _controller.Change_Fly_Parameter();

        //方向
        int direction = (next_Pos.x - transform.position.x).CompareTo(0);
        if(direction == 0) { direction = 1; }
        transform.localScale = new Vector3(-direction, 1, 1);
        //移動
        _controller.Change_Animation("JumpBool");
        _effect.Jump_And_Landing_Effect();
        yield return new WaitForSeconds(0.18f);
        _move_Two_Points.Start_Move(next_Pos, 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        _controller.Change_Animation("OnWallBool");
        transform.localScale = new Vector3(direction, 1, 1);

        is_End_Move = true;
    }


    //大ジャンプする
    public IEnumerator High_Jump_Move_Cor(Vector2 next_Pos) {
        is_End_Move = false;
        _controller.Change_Fly_Parameter();

        //向き
        int direction = (transform.position.x - next_Pos.x).CompareTo(0);
        if(direction == 0) { direction = 1; }
        transform.localScale = new Vector3(direction, 1, 1);

        //ジャンプ
        _controller.Change_Animation("JumpBool");
        _move_Two_Points.Start_Move(next_Pos, 2);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        _controller.Change_Land_Parameter();
        _controller.Change_Animation("SquatBool");
        is_End_Move = true;
    }


    //--------------------------攻撃用関数------------------------------
    // ※※※※  攻撃終了時に _attack.can_Attack をtrueにすること  ※※※※
    
    /// <summary>
    /// 潜行、地面から自機追従、ジャンプ、ショット
    /// </summary>
    public void Dive_And_Jump_Shoot() {
        StartCoroutine("Dive_And_Trace_Player_Cor");
    }


    //潜行、自機追従
    private IEnumerator Dive_And_Trace_Player_Cor() {
        //取得
        BossCollisionDetection _collision = GetComponent<BossCollisionDetection>();                
        _trace.enabled = false;       

        //地面に潜る
        _effect.Jump_And_Landing_Effect();
        _collision.Become_Invincible();
        _controller.Change_Fly_Parameter();
        _sprite.sortingOrder = -6;

        _move.Start_Move(0);
        yield return new WaitUntil(_move.Is_End_Move);

        _controller.Change_Animation("DivingGroundBool");
        yield return new WaitForSeconds(0.1f);
        _sprite.sortingOrder = 3;

        //コピーの生成
        if (generate_Copy) {
            _copy.Create_Copy(80, false, new Vector2(-transform.position.x, transform.position.y));
        }

        //自機を追いかける        
        _trace.enabled = true;
        yield return new WaitForSeconds(2.2f);
        _trace.enabled = false;

        yield return new WaitForSeconds(0.4f);

        //ジャンプショット
        _collision.Release_Invincible();
        StartCoroutine("Jump_Shoot_Cor");
    }


    //ジャンプショット
    private IEnumerator Jump_Shoot_Cor() {       
        //ジャンプ     
        _controller.Change_Animation("HighJumpBool");
        _move.Start_Move(1);        
        _effect.Play_A_Letter_Effect();
        yield return new WaitUntil(_move.Is_End_Move);

        //弾幕レーザー
        _controller.Change_Animation("ShootPoseBool");
        _shoot.Shoot_Short_Curve_Laser();
        _effect.Play_Unn_Letter_Effect();
        _effect.Play_Burst_Effect_Red();
        if (generate_Copy) {
            _copy_Shoot.Shoot_Short_Curve_Laser();
        }
        yield return new WaitForSeconds(0.5f);        

        //落下        
        _controller.Change_Land_Parameter();
        yield return new WaitUntil(foot_Collision.Hit_Trigger);
        _effect.Jump_And_Landing_Effect();
        _controller.Change_Animation("SquatBool");
        
        //コピーの消去
        if (generate_Copy)
            _copy.Delete_Copy();

        yield return new WaitForSeconds(1.2f);

        //メロディ変わったら終了
        if (_controller._BGM.Get_Now_Melody() != AunnBGMManager.Melody.A) {
            _attack.can_Attack = true;
            yield break;
        }

        //端にダッシュする
        int direction = -transform.position.x.CompareTo(0);
        StartCoroutine("Dash_To_Side_Cor", direction);
        yield return new WaitUntil(Is_End_Move);
        yield return new WaitForSeconds(0.1f);

        _attack.can_Attack = true;
    }


    
    /// <summary>
    /// 壁に張り付いてジャンプ、弾を配置
    /// </summary>
    public void Jump_On_Wall_And_Rush() {
        StartCoroutine("Jump_On_Wall_And_Rush_Cor");
    }

    private IEnumerator Jump_On_Wall_And_Rush_Cor() {
        //自機のいない方の壁に飛びつく
        int direction = (player.transform.position.x - transform.position.x).CompareTo(0);
        if (direction == 0) { direction = 1; }
        StartCoroutine("Jump_On_Wall_Cor", new Vector2(230f * -direction, 48f));
        yield return new WaitUntil(Is_End_Move);        

        //弾の配置開始
        _shoot.Start_Deposite_Purple_Bullet();
        _effect.Play_A_Letter_Effect();        

        //コピーの生成
        if (generate_Copy) {
            _copy.Create_Copy(80, true, new Vector2(-transform.position.x, transform.position.y));
            _copy_Shoot.Start_Deposite_Purple_Bullet();
        }

        yield return new WaitForSeconds(0.5f);

        //反対側の壁に飛びつく
        StartCoroutine("Jump_On_Wall_Cor", new Vector2(230f * direction, 80f));
        yield return new WaitUntil(Is_End_Move);        

        //弾の配置終了
        _shoot.Stop_Deposit_Purple_Bullet();
        _effect.Play_Unn_Letter_Effect();
        _effect.Play_Purple_Circle_Effect();

        //コピーの消去
        if (generate_Copy) {
            _copy.Delete_Copy();
            _copy_Shoot.Stop_Deposit_Purple_Bullet();
        }

        yield return new WaitForSeconds(0.5f);

        //斜め下に突進する
        _controller.Change_Animation("JumpBool");
        _rigid.velocity = new Vector2(-transform.localScale.x * 160f, -200f);
        yield return new WaitUntil(foot_Collision.Hit_Trigger);
        //着地
        _controller.Change_Land_Parameter();
        _controller.Change_Animation("SquatBool");
        _effect.Jump_And_Landing_Effect();

        yield return new WaitForSeconds(2.0f);

        _attack.can_Attack = true;
    }

    
    /// <summary>
    /// 両端をジャンプ移動で行き来する
    /// </summary>
    public void Reciprocate_Jump() {
        StartCoroutine("Reciprocate_Jump_Cor");
    }


    private IEnumerator Reciprocate_Jump_Cor() {
        //方向
        int direction = transform.position.x.CompareTo(0);
        if(direction == 0) { direction = 1; }
        transform.localScale = new Vector3(direction, 1, 1);

        //予備動作
        _effect.Play_A_Letter_Effect();
        //コピーの生成
        if (generate_Copy)
            _copy.Create_Copy(80, true, new Vector2(-transform.position.x, transform.position.y));

        yield return new WaitForSeconds(0.8f);

        //移動
        Vector2 next_Pos;
        _controller.Change_Animation("JumpBool");        
        while(true) {
            _effect.Jump_And_Landing_Effect();
            next_Pos = transform.position + new Vector3(112f * -direction, 0);
            _move_Two_Points.Start_Move(next_Pos, 1);
            yield return new WaitUntil(_move_Two_Points.End_Move);

            if (direction == 1 && transform.position.x < -88f)
                break;
            else if (direction == -1 && transform.position.x > 88f)
                break;            
        }

        //最後は場所整える
        _effect.Jump_And_Landing_Effect();
        next_Pos = new Vector2(200f * -direction, transform.position.y);
        _move_Two_Points.Start_Move(next_Pos, 1);
        yield return new WaitUntil(_move_Two_Points.End_Move);
        _controller.Change_Animation("SquatBool");
        transform.localScale = new Vector3(-direction, 1, 1);

        //コピーの消去
        if (generate_Copy)
            _copy.Delete_Copy();

        _attack.can_Attack = true;
    }


}
