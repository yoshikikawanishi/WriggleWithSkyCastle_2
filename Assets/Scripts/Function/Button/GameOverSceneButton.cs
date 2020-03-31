using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBLDefine;
using UnityEngine.SceneManagement;

public class GameOverSceneButton : MonoBehaviour {

    //ひな形
    /*
     if(InputManager.Instance.GetKeyDown(Key.Jump)){
        //処理
     }
     */


    //コンティニュー
    public void Continue_Button_Function() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            DataManager.Instance.Load_Player_Data();
        }
    }


    //タイトルに戻る
    public void Back_Title_Button_Function() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            SceneManager.LoadScene("TitleScene");
        }
    }

}
