using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoar : Enemy {

    private GameObject player;
    private Rigidbody2D _rigid;
    private Animator _anim;

    private bool is_Rushing = false;
    private float walk_Time = 0;

    private List<string> repelled_Attack_Tags = new List<string>() {
        "PlayerAttackTag",
        "PlayerChargeAttackTag",
        "PlayerKickTag",
    };


	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
	}

	
	// Update is called once per frame
	void Update () {
        //自機を探す
        if (Is_Exist_Player_Forward()) {
            //突進開始
            if (!is_Rushing) {
                StartCoroutine("Start_Rush_Cor");
            }
            //突進
            else if (Mathf.Abs(_rigid.velocity.x) < 180f) {
                _rigid.velocity += new Vector2(-transform.localScale.x * 5, 0);
            }
        }
        else {
            is_Rushing = false;
            _anim.SetBool("DashBool", false);
            //自機を見失って減速
            if (Mathf.Abs(_rigid.velocity.x) >= 0.01f) {
                _rigid.velocity *= new Vector2(0.995f, 1);                
            }
            //自機未発見時歩く
            else {
                if(walk_Time < 2.0f) {
                    walk_Time += Time.deltaTime;
                    transform.position += new Vector3(transform.localScale.x * -0.2f, 0) * Time.timeScale;
                }
                else {
                    transform.localScale = transform.localScale * new Vector2(-1, 1);
                    walk_Time = 0;
                }
            }
        }

    }


    //正面にいる自機を見つける
    private bool Is_Exist_Player_Forward() {
        Vector2 distance = player.transform.position - transform.position;
        distance *= new Vector2(transform.localScale.x, 1);
        if(-256f < distance.x && distance.x < 0) {
            if(Mathf.Abs(distance.y) < 96f) {
                return true;
            }
        }
        return false;
    }


    //突進開始
    private IEnumerator Start_Rush_Cor() {
        yield return new WaitForSeconds(0.5f);
        is_Rushing = true;
        _anim.SetBool("DashBool", true);
    }


    public override void Damaged(int damage, string attacked_Tag) {
        int direction = transform.localScale.x.CompareTo(0);    
        //突進中は被弾時反動ではじかれる
        if (is_Rushing) {
            foreach(string tag in repelled_Attack_Tags) {
                if(attacked_Tag == tag)
                    GetComponent<Rigidbody2D>().velocity = new Vector2(direction * 100f, 5f);
            }            
        }
        base.Damaged(damage, attacked_Tag);
    }

}
