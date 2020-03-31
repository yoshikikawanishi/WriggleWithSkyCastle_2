using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour {

    private Rigidbody2D _rigid;

    [SerializeField] private int move_Length = 32;
    [SerializeField] private float move_Speed = 1;    
    private int move_Count = 0;    


    // Use this for initialization
    void Start () {
        _rigid = GetComponent<Rigidbody2D>();
        StartCoroutine("Wolf_Action_Cor");
    }
	

    private void FixedUpdate() {
        //横移動
        if (move_Count * move_Speed < move_Length) {            
            transform.position += new Vector3(move_Speed * -transform.localScale.x, 0) * Time.timeScale;
            move_Count++;
        }
        else {
            move_Count = 0;
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        }
    }


    //ジャンプ、着地後に横移動開始
    private IEnumerator Wolf_Action_Cor() {
        //取得        
        Animator _anim = GetComponent<Animator>();
        ChildColliderTrigger body_Collider = GetComponentInChildren<ChildColliderTrigger>();

        //横移動禁止
        this.enabled = false;

        //初速、ジャンプ
        _rigid.velocity = new Vector2(0, 200f);        
        
        while(_rigid.velocity.y > -10f) { yield return null; }

        //下降        
        _anim.SetTrigger("FallTrigger");        

        //着地、横移動開始
        yield return new WaitUntil(body_Collider.Hit_Trigger);
        _anim.SetTrigger("DashTrigger");
        this.enabled = true;
    }

}
