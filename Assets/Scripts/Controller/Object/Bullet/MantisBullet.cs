using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisBullet : MonoBehaviour {

    private List<string> refrect_Obj_Tags = new List<string> {        
        "EnemyTag",
        "GroundTag",
        "SandbackTag",
        "SandbackGroundTag",
        "DamagedGroundTag",
    };

    private GameObject main_Camera;
    private Rigidbody2D _rigid;

    private bool is_First_Frame = true;
    private bool is_Refrected = false;
    private int start_Direction = 1;


    private void Awake() {
        //取得
        main_Camera = GameObject.FindWithTag("MainCamera");
        _rigid = GetComponent<Rigidbody2D>();
    }


    private void OnEnable() {
        is_First_Frame = true;
        is_Refrected = false;
    }


    private void FixedUpdate() {
        //方向の取得
        if (is_First_Frame) {
            start_Direction = _rigid.velocity.x.CompareTo(0);
            is_First_Frame = false;
        }
        _rigid.AddForce(new Vector2(-start_Direction * 1000, 0));
        //消す
        if ((main_Camera.transform.position.x - transform.position.x) * start_Direction > 260f)
            gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        //１度だけ反射
        if (is_Refrected)
            return;

        foreach (string tag in refrect_Obj_Tags) {
            if(collision.tag == tag) {
                _rigid.velocity = new Vector2(-start_Direction * 50, 0);
                is_Refrected = true;
            }
        }    
    
    }

}
