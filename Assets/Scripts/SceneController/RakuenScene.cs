using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RakuenScene : MonoBehaviour {

    [SerializeField] private Animator hourai;

    private GameObject player;


	void Start () {
        player = GameObject.FindWithTag("PlayerTag");
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
        //BGMManager.Instance.Change_BGM("Hourai");
	}
	
	
	void Update () {
	    if(player.transform.position.x > 460f) {            
            hourai.SetTrigger("ActionTrigger");
        }
	}
}
