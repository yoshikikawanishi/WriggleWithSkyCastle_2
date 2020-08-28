using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Gravity : MonoBehaviour {

    [SerializeField] private Vector2 acceleration;

    private Rigidbody2D _rigid;


	void Start () {
        _rigid = GetComponent<Rigidbody2D>();	
	}
	
	
	void LateUpdate () {
        _rigid.velocity += acceleration;
	}
}
