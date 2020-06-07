using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3_2Scene : MonoBehaviour {
   

	// Use this for initialization
	void Start () {       
        //初回時フェードイン
        if (SceneManagement.Instance.Is_First_Visit()) {
            FadeInOut.Instance.Start_Fade_In(new Color(1, 1, 1), 0.01f);
        }
        BGMManager.Instance.Change_BGM("Stage3");
    }
	
	
}
