using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage6_1Scene : MonoBehaviour {

	void Start () {
        if (SceneManagement.Instance.Is_First_Visit()) {
            FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
        }
        BGMManager.Instance.Change_BGM("Stage4");
    }
	
	
}
