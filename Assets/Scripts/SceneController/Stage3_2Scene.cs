using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3_2Scene : MonoBehaviour {
   
    private GameObject player;

    private AyaMovie aya_Movie;

    private bool is_Passed_Middle_Point = false;    


	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        aya_Movie = GetComponent<AyaMovie>();
        //初回時フェードイン
        if (SceneManagement.Instance.Is_First_Visit()) {
            FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.01f);
        }
        BGMManager.Instance.Change_BGM("Stage3");
    }
	
	// Update is called once per frame
	void Update () {
        if (player == null)
            return;
        if (CollectionManager.Instance.Is_Collected("Aya") && CollectionManager.Instance.Is_Collected("Momizi"))
            return;

        if (player.transform.position.x > 3364f && player.transform.position.x < 4000f) {
            if (!is_Passed_Middle_Point) {
                is_Passed_Middle_Point = true;
                BackGroundEffector.Instance.Start_Change_Color(new Color(0.4f, 0.4f, 0.4f), 0.02f);
                aya_Movie.Play_Aya_Movie();
            }
        }
	}
}
