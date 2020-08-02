using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFlyingObj : MonoBehaviour {

    [SerializeField] private GameObject screen_Effect_Prefab;
    private GameObject screen_Effect;

    private PlayerController player_Controller;

    private bool is_Hit_Player;


	// Use this for initialization
	void Start () {
        //取得
        GameObject player = GameObject.FindWithTag("PlayerTag");
        player_Controller = player.GetComponent<PlayerController>();
        //画面エフェクトの生成
        if (screen_Effect_Prefab != null) {
            GameObject main_Camera = GameObject.FindWithTag("MainCamera");
            screen_Effect = Instantiate(screen_Effect_Prefab, main_Camera.transform);
            screen_Effect.transform.localPosition = new Vector3(0, 0, 10);
            screen_Effect.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (player_Controller == null)
            return;

        if (is_Hit_Player) {            
            player_Controller.To_Disable_Ride_Beetle();
        }	
	}


    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            is_Hit_Player = true;
            Play_Disable_Effect();
        }
    }


    void OnTriggerExit2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            player_Controller.To_Enable_Ride_Beetle();
            is_Hit_Player = false;
            Play_Enable_Effect();
        }
    }


    private void Play_Disable_Effect() {
        if (screen_Effect == null)
            return;
        screen_Effect.SetActive(true);
        screen_Effect.GetComponent<Animator>().SetTrigger("AppearTrigger");
    }


    private void Play_Enable_Effect() {        
        if (screen_Effect == null)
            return;        
        screen_Effect.GetComponent<Animator>().SetTrigger("DisappearTrigger");
    }
}
