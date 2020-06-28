using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningMovie : MonoBehaviour {

    [SerializeField] private GameObject flies;


    public void Start_Movie() {
        if (SceneManagement.Instance.Is_First_Visit()) {
            BGMManager.Instance.Stop_BGM();
            StartCoroutine("Opening_Movie_Cor");
        }
        else {
            BGMManager.Instance.Change_BGM("Stage1");
            Destroy(flies);
        }
    }

    private IEnumerator Opening_Movie_Cor() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        MessageDisplayCustom _message = GetComponent<MessageDisplayCustom>();

        //自機を止める、ポーズ不可
        player.GetComponent<PlayerController>().Set_Is_Playable(false);
        PauseManager.Instance.Set_Is_Pausable(false);

        //フェードイン
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.01f);
        //yield return new WaitForSeconds(0.4f);

        //ハエ登場
        flies.GetComponent<Animator>().SetTrigger("InTrigger");

        //会話
        _message.Start_Display("OpeningText", 1, 7);
        yield return new WaitUntil(_message.End_Message);

        //ハエ退場
        flies.GetComponent<Animator>().SetTrigger("OutTrigger");

        //終了設定
        player.GetComponent<PlayerController>().Set_Is_Playable(true);
        PauseManager.Instance.Set_Is_Pausable(true);
        //BGM開始
        BGMManager.Instance.Change_BGM("Stage1");
    }
}
