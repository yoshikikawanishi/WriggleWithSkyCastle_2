using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MBLDefine;

public class PauseCanvasButton : MonoBehaviour {

    //再開ボタン
    public void Back_Game_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            PauseManager.Instance.Release_Pause_Game();
        }
    }


    //設定ボタン
    public void Setting_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            PauseManager.Instance.Display_Setting_Canvas();
        }
    }

	//タイトルに戻るボタン
    public void Back_Title_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)){
            PauseManager.Instance.Release_Pause_Game();
            SceneManager.LoadScene("TitleScene");
        }
    }


    //設定終了ボタン
    public void Finish_Setting_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            PauseManager.Instance.Hide_Setting_Canvas();
        }
    }
}
