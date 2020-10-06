using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage3_BossMovie : MonoBehaviour {

    [SerializeField] private Aunn aunn;
    [SerializeField] private MovieSystem before_Boss_Movie;
    [SerializeField] private MovieSystem before_Boss_Movie_Skip;
    [SerializeField] private MovieSystem clear_Movie;


    //ボス戦前ムービー
    public void Play_Before_Boss_Movie() {
        StartCoroutine("Before_Boss_Movie_Cor");
    }

    private IEnumerator Before_Boss_Movie_Cor() {
        if (SceneManagement.Instance.Is_First_Visit()) {
            before_Boss_Movie.Start_Movie();
            yield return new WaitUntil(before_Boss_Movie.End_Movie);
        }
        else {
            before_Boss_Movie_Skip.Start_Movie();
            yield return new WaitUntil(before_Boss_Movie_Skip.End_Movie);
        }        
        aunn.Start_Battle();
    }


    //クリア後ムービー
    public void Play_Clear_Movie() {
        clear_Movie.Start_Movie();
    }


    //ムービー用画面振動
    public void Shake_Camera_Strong() {
        GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>().Shake(0.3f, new Vector2(8f, 8f), true);
        GetComponent<AudioSource>().Play();
    }

    //ムービー用画面振動
    public void Shake_Camera() {
        GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>().Shake(20.0f, new Vector2(2f, 2f), true);
    }


    //ムービー用フェードイン
    public void Fade_In() {
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
    }


    //ムービー用フェードアウト
    public void Fade_Out() {
        FadeInOut.Instance.Start_Fade_Out(new Color(0, 0, 0), 0.02f);
    }


    //ムービー用シーン遷移
    public void Change_Scene_To_Stage4() {
        SceneManager.LoadScene("Stage4_1Scene");
    }
    
}
