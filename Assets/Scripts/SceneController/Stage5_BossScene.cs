using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage5_BossScene : MonoBehaviour {

    void Start() {
        BGMManager.Instance.Stop_BGM();
    }

    public void Fade_In() {
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
    }

    public void Fade_Out() {
        FadeInOut.Instance.Start_Fade_Out(new Color(0, 0, 0), 0.02f);
        BGMManager.Instance.Fade_Out();
    }

    public void Change_Scene() {
        SceneManager.LoadScene("Stage6_1Scene");
    }
	
}
