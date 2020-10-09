using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage7_BossScene : MonoBehaviour {

    void Start() {
        BGMManager.Instance.Stop_BGM();
    }

	public void Fade_In() {
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
    }


    public void Fade_Out() {
        FadeInOut.Instance.Start_Fade_Out(new Color(1, 1, 1), 0.02f);
        BGMManager.Instance.Fade_Out();
    }


    public void Change_Scene_To_Ending() {
        SceneManager.LoadScene("EndingScene");
    }
    

    public void Appear_Eternal() {
        StartCoroutine("Appear_Eternal_Cor");
    }

    private IEnumerator Appear_Eternal_Cor() {
        SpriteRenderer eternal_Sprite = GameObject.Find("Eternal").GetComponent<SpriteRenderer>();
        while(eternal_Sprite.color.a < 1.05) {
            eternal_Sprite.color += new Color(0, 0, 0, 0.01f);
            yield return null;
        }        
    }
}
