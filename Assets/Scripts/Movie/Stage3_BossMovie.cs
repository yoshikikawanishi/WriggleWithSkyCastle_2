using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3_BossMovie : MonoBehaviour {

    [SerializeField] private Aunn aunn;

    private GameObject player;
    private MessageDisplay _message;

	
	void Awake () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        _message = GetComponent<MessageDisplay>();
	}
	
	
    //ボス戦前ムービー
    public void Play_Before_Boss_Movie() {
        StartCoroutine("Before_Boss_Movie_Cor");
    }

    private IEnumerator Before_Boss_Movie_Cor() {
        yield return null;
        aunn.Start_Battle();
    }


    //クリア後ムービー
    public void Play_Clear_Movie() {
        StartCoroutine("Clear_Movie_Cor");
    }

    private IEnumerator Clear_Movie_Cor() {
        yield return null;
    }
}
