using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningMovie : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Start_Movie() {
        StartCoroutine("Opening_Movie_Cor");
    }

    private IEnumerator Opening_Movie_Cor() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        MessageDisplay _message = GetComponent<MessageDisplay>();
        GuideWindowDisplayer _guide = GetComponent<GuideWindowDisplayer>();

        //自機を止める、ポーズ不可
        player.GetComponent<PlayerController>().Set_Is_Playable(false);
        PauseManager.Instance.Set_Is_Pausable(false);

        //フェードイン
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
        yield return new WaitForSeconds(0.5f);
        //会話
        _message.Start_Display("OpeningText", 1, 2);
        yield return new WaitUntil(_message.End_Message);
        //ガイドウィンドウ
        _guide.Open_Window("UI/GuideOpening");
        yield return new WaitForSeconds(0.1f);

        //終了設定
        player.GetComponent<PlayerController>().Set_Is_Playable(true);
        PauseManager.Instance.Set_Is_Pausable(true);

    }
}
