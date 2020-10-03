using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage7_BossScene : MonoBehaviour {

	public void Fade_In() {
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
    }


    public void Fade_Out() {
        FadeInOut.Instance.Start_Fade_Out(new Color(1, 1, 1), 0.02f);
    }


    public void Change_Scene_To_Bad_Ending() {
        SceneManager.LoadScene("BadEndingScene");
    }
}
