using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_1Scene : MonoBehaviour {

    private GameObject player;
    private YukaMovie _movie;

    private bool start_Movie = false;
   

	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        _movie = GetComponent<YukaMovie>();

        //初回時
        if (SceneManagement.Instance.Is_First_Visit()) {
            //フェードイン
            FadeInOut.Instance.Start_Fade_In(new Color(1, 1, 1), 0.008f);
            
            //自機をカブトムシに乗せる            
            PlayerGettingOnBeetle player_Ride_Beetle = player.GetComponent<PlayerGettingOnBeetle>();
            player_Ride_Beetle.Get_On_Beetle();
        }

        //BGM
        BGMManager.Instance.Change_BGM("Stage2");
	}


    // Update is called once per frame
    void Update() {
        //ゆうかチュートリアルのムービー
        if (player.transform.position.x > 1930f && !start_Movie) {
            start_Movie = true;
            _movie.Start_Movie();
        }
    }

}
