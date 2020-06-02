using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_BossScene : MonoBehaviour {

    //コンポーネント
    private Stage2_BossMovie _movie;
    

	// Use this for initialization
	void Start () {
        //取得
        _movie = GetComponent<Stage2_BossMovie>();        
    
        //フェードイン
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
        //ボス前ムービー開始
        _movie.Start_Before_Boss_Movie();
	}
	
}
