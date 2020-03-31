using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudFairy : Enemy {

    private Animator _anim;
    private VerticalVibeMotion _vertical_Move;

    private float attack_Time = 0;
    private bool is_Damaged = false;


	// Use this for initialization
	void Start () {
        //取得
        _anim = GetComponent<Animator>();
        _vertical_Move = GetComponent<VerticalVibeMotion>();
	}
	
	// Update is called once per frame
	void Update () {        
        if (is_Damaged) {
            //被弾後一定時間待つ
            if (attack_Time < 0.01f) {
                attack_Time += Time.deltaTime;
            }
            //攻撃
            else {
                attack_Time = 0;    
                Attack();           
                is_Damaged = false;
                _vertical_Move.enabled = true;
            }
        }
	}

    public override void Damaged(int damage, string attacked_Tag) {
        base.Damaged(damage, attacked_Tag);
        //打ち返し開始
        if (!is_Damaged && attacked_Tag != "Poison") {
            is_Damaged = true;
            _anim.SetTrigger("DamagedTrigger");
            StartCoroutine("White_Blink_Cor");
            _vertical_Move.enabled = false;
        }
    }


    private void Attack() {
        GetComponentInChildren<ShootSystem>().Shoot();
    }


    private IEnumerator White_Blink_Cor() {
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        for(int i = 0; i < 3; i++) {
            _sprite.color = new Color(0.7f, 0.7f, 0.7f);
            yield return new WaitForSeconds(0.1f);
            _sprite.color = new Color(0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
