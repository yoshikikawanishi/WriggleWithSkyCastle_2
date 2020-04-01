using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3_BossScene : MonoBehaviour {

    [SerializeField] private AunnController aunn;

    private Stage3_BossMovie _movie;


    private void Awake() {
        //取得
        _movie = GetComponent<Stage3_BossMovie>();
    }


    // Use this for initialization
    void Start () {
        //ムービー開始
        _movie.Play_Before_Boss_Movie();
	}
	

	// Update is called once per frame
	void Update () {
        //クリア時
        if (aunn.Clear_Trigger()) {
            _movie.Play_Clear_Movie();
            aunn.Clear();
        }
	}
}
