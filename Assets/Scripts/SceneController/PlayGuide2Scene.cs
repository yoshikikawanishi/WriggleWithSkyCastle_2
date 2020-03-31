using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGuide2Scene : MonoBehaviour {

    [SerializeField] private ControlleGuideText guide_Text;
    [SerializeField] private GameObject guide_Arrow;    


    private void Start() {
        //フェードアウト
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
        StartCoroutine("Display_Guide_Window");
    }

    // Update is called once per frame
    void Update () {        
        //ガイド終了時矢印看板を表示する
        if (guide_Text.End_Guide_Trigger()) {
            guide_Arrow.SetActive(true);            
        }

	}


    //飛行のガイドウィンドウ表示
    private IEnumerator Display_Guide_Window() {
        yield return new WaitForSeconds(1.0f);
        GetComponent<GuideWindowDisplayer>().Open_Window("UI/GuideFly");
    }
}
