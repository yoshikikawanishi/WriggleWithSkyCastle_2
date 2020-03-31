using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallFrog : MonoBehaviour {

    [SerializeField] private float jump_Span = 2.0f;

    private Rigidbody2D _rigid;
    private Animator _anim;
    private ChildColliderTrigger foot_Collision;

    private bool start_Action = false;
    private float time = 0;    

	// Use this for initialization
	void Start () {
        //取得
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        foot_Collision = GetComponentInChildren<ChildColliderTrigger>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!start_Action) 
            return;
        
        //着地
        if (foot_Collision.Hit_Trigger() && _rigid.velocity.y < -2f) {
            _anim.SetBool("JumpBool", false);
            _rigid.velocity = new Vector2(0, 0);            
        }
        //ジャンプ
        if(time < jump_Span) {
            time += Time.deltaTime;
        }
        else {
            time = 0;
            Jump();
        }
	}


    private void OnBecameVisible() {
        start_Action = true;
    }

    //ジャンプ
    private void Jump() {
        _rigid.velocity = new Vector2(-80f * transform.localScale.x, 180f);
        _anim.SetBool("JumpBool", true);
    }
}
