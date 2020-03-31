using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenFairy : MonoBehaviour {

    [SerializeField] private float move_Length = 32f;

    private Rigidbody2D _rigid;

    private float default_Pos_X;
    private float default_Size_X;
    private bool start_Action = false;


	// Use this for initialization
	void Start () {
        _rigid = GetComponent<Rigidbody2D>();
        default_Pos_X = transform.position.x;
        default_Size_X = transform.localScale.x;
    }
	
	// Update is called once per frame
	void Update () {
        if (!start_Action) {
            return;
        }
        //反転
		if(transform.position.x > default_Pos_X + move_Length) {
            _rigid.velocity = new Vector2(-40f, 0);
            transform.localScale = new Vector3(default_Size_X, transform.localScale.y);
        }
        else if(transform.position.x < default_Pos_X - move_Length) {
            _rigid.velocity = new Vector2(40f, 0);
            transform.localScale = new Vector3(-default_Size_X, transform.localScale.y);
        }
    }


    private void OnBecameVisible() {
        if (!start_Action) {
            start_Action = true;
            //初速
            _rigid.velocity = new Vector2(-40f, 0);
        }
    }
}
