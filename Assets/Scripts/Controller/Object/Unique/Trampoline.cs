using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour {

    private ChildColliderTrigger _detection;
    private Animator _anim;
    private AudioSource _audio;
    private Rigidbody2D player_Rigid;

    private const float Jump_Speed = 500f;

	
	void Start () {
        _detection = GetComponentInChildren<ChildColliderTrigger>();
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();

        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null)
            Destroy(gameObject);
        player_Rigid = player.GetComponent<Rigidbody2D>();
	}
	
	
	void Update () {
        if (_detection.Hit_Trigger()) {
            Bound();
        }
	}


    private void Bound() {
        _anim.SetTrigger("BoundTrigger");
        _audio.Play();
        player_Rigid.velocity = new Vector2(0, Jump_Speed);
    }
}
