using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3_BossScene : MonoBehaviour {

    [SerializeField] private Aunn aunn;

    private Stage3_BossMovie _movie;


    private void Awake() {
        //取得
        _movie = GetComponent<Stage3_BossMovie>();
        BGMManager.Instance.Stop_BGM();
    }


    // Use this for initialization
    void Start () {
        //ムービー開始
        _movie.Play_Before_Boss_Movie();
	}
		
}
