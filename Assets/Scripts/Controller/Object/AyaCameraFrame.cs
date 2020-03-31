using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AyaCameraFrame : MonoBehaviour {

    private Animator _anim;

    private void Awake() {
        _anim = GetComponent<Animator>();
    }   
	

    public void Appear() {
        _anim.SetTrigger("AppearTrigger");
    }

    public void Disappear() {
        _anim.SetTrigger("DisappearTrigger");
    }
	
}
