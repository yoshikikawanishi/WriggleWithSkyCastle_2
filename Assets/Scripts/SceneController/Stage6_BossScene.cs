﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage6_BossScene : MonoBehaviour {

    void Start() {
        BGMManager.Instance.Stop_BGM();
    }

    public void Fade_In() {
        FadeInOut.Instance.Start_Fade_In(new Color(1, 1, 1, 1), 0.02f);
    }

    public void Fade_Out() {
        FadeInOut.Instance.Start_Fade_Out(new Color(1, 1, 1, 1), 0.02f);
        BGMManager.Instance.Fade_Out();
    }
	
    public void Change_Scane() {
        SceneManager.LoadScene("Stage7_1Scene");
    }
	
}
