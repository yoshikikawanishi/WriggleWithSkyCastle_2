using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage3_BossMovie : MonoBehaviour {

    [SerializeField] private AunnController aunn;
        	
	
    //ボス戦前ムービー
    public void Play_Before_Boss_Movie() {
        StartCoroutine("Before_Boss_Movie_Cor");
    }

    private IEnumerator Before_Boss_Movie_Cor() {
        yield return new WaitForSeconds(1.0f);
        aunn.Start_Battle();
    }


    //クリア後ムービー
    public void Play_Clear_Movie() {
        StartCoroutine("Clear_Movie_Cor");
    }

    private IEnumerator Clear_Movie_Cor() {
        yield return new WaitForSeconds(2.0f);
        FadeInOut.Instance.Start_Fade_Out(new Color(0, 0, 0), 0.02f);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Stage4_1Scene");
    }
}
