using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_BossScene : MonoBehaviour {

    //コンポーネント
    private Stage2_BossMovie _movie;
    //ネムノ
    private NemunoAttack nemuno_Attack;
    private BossEnemy boss_Enemy;


	// Use this for initialization
	void Start () {
        //取得
        _movie = GetComponent<Stage2_BossMovie>();
        var nemuno = GameObject.Find("Nemuno");
        nemuno_Attack = nemuno.GetComponent<NemunoAttack>();
        boss_Enemy = nemuno.GetComponent<BossEnemy>();

        //フェードイン
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
        //ボス前ムービー開始
        _movie.Start_Before_Boss_Movie();
	}
	
	// Update is called once per frame
	void Update () {
        //クリア時
        if (boss_Enemy.Clear_Trigger()) {
            nemuno_Attack.Stop_Phase2();
            _movie.Start_Clear_Movie();
        }
	}
}
