﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGroundController : MonoBehaviour {

    public enum State {
        idle,
        raising,
        dropping,
        freeze,
    }
    private State state = State.idle;

    private float default_Height;
    private float raise_Height = 196f;
    private float raising_Time = 0;
    private float raise_Time_Span;
    private AnimationCurve raise_Curve;

    private float drop_Speed = 1f;

    private SpriteRenderer soul_Effect_Sprite;
    private Animator soul_Effect_Anim;

    private GameObject player;

	
	void Start () {
        default_Height = transform.position.y;
        
        soul_Effect_Sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        soul_Effect_Anim = transform.GetChild(0).GetComponent<Animator>();
        soul_Effect_Anim.gameObject.SetActive(false);

        player = GameObject.FindWithTag("PlayerTag");        
	}
	

    void Update() {
        //自機が埋まるのを防ぐ
        if(state == State.idle && Is_Overlap_Player()) {
            if(gameObject.layer != LayerMask.NameToLayer("InvincibleLayer")) {
                gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
            }
        }
        else {
            if(gameObject.layer == LayerMask.NameToLayer("InvincibleLayer")) {
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }


    void FixedUpdate() {
        //上昇
        if (state == State.raising) {
            Raise();
        }
        //落下
        else if (state == State.dropping) {
            Drop();
        }
    }


    //上昇、FiexedUpdateで呼ぶこと
    private void Raise() {
        if (raising_Time < raise_Time_Span) {
            transform.position = new Vector3(transform.position.x, raise_Curve.Evaluate(raising_Time));
            raising_Time += 0.016f * Time.timeScale;
        }
        else {
            state = State.dropping;
        }
    }


    //落下、Fixed_Updateで呼ぶこと
    private void Drop() {
        if (transform.position.y > default_Height) {
            transform.position += new Vector3(0, -drop_Speed * Time.timeScale);
        }
        else {
            transform.position = new Vector3(transform.position.x, default_Height);
            Disappear_Soul_Effect();            
            state = State.idle;
        }
    }


    //魂出現
    private void Appear_Soul_Effect() {
        soul_Effect_Anim.gameObject.SetActive(true);
        soul_Effect_Anim.SetTrigger("AppearTrigger");
    }


    //魂消滅
    private void Disappear_Soul_Effect() {
        soul_Effect_Anim.gameObject.SetActive(false);
    }


    //自機が下に埋まっているかどうか
    private bool Is_Overlap_Player() {
        Vector2 diff = player.transform.position - transform.position;
        if (Mathf.Abs(diff.x) < 24f && diff.y < -8f)
            return true;        
        return false;
    }    


    //自機が周辺にいるか
    public bool Is_Nearly_Player() {
        Vector2 diff = player.transform.position - transform.position;
        if (Mathf.Abs(diff.x) < 40f && Mathf.Abs(diff.y) < 40f)
            return true;
        return false;
    }


    //上昇開始
    public void Start_Raise(float raise_Time_Span) {
        if(state != State.idle) 
            return;

        Appear_Soul_Effect();

        raising_Time = -1.0f;
        this.raise_Time_Span = raise_Time_Span;
        raise_Curve = AnimationCurve.EaseInOut(0, default_Height, raise_Time_Span, default_Height + raise_Height);        
        state = State.raising;
    }


    //停止
    public void Freeze() {
        state = State.freeze;        
    }


    //停止解除
    public void Release_Freeze() {
        soul_Effect_Sprite.color = new Color(1, 1, 1);
        Restore_To_Original_Pos();
    }


    //元の位置に戻す
    public void Restore_To_Original_Pos() {
        if(state == State.idle) 
            return;
        state = State.dropping;
    }


    //ショット
    public IEnumerator Shoot_Cor() {
        soul_Effect_Sprite.color = new Color(1.0f, 0.3f, 0.7f);        
        yield return new WaitForSeconds(1.0f);
        ShootSystem[] shoots = GetComponentsInChildren<ShootSystem>();
        for(int i = 0; i < shoots.Length; i++) {
            shoots[i].Shoot();
        }
        soul_Effect_Sprite.color = new Color(1.0f, 1.0f, 1.0f);
    }


    public State Get_State() {
        return state;
    }
    
}
