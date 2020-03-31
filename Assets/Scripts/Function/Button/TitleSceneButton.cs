using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using MBLDefine;

public class TitleSceneButton : MonoBehaviour {

    //ひな形
    /*
     if(InputManager.Instance.GetKeyDown(Key.Jump)){
        //処理
     }
     */

    [SerializeField] private ConfirmCanvas confirm_Start_Game_Canvas;


    //初めからボタン
    public void Start_Button_Function() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            if (PlayerPrefs.HasKey("SCENE")) {
                confirm_Start_Game_Canvas.Display_Confirm_Canvas();
            }
            else {
                StartCoroutine("Start_Game_Cor");
            }
        }
    }

    
    //ゲームをはじめから開始するかどうかの確認ボタン
    public void Confirm_Button_Function(bool is_Yes) {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            if (is_Yes) {
                StartCoroutine("Start_Game_Cor");
            }
            else {
                confirm_Start_Game_Canvas.Delete_Confirm_Canvas();
            }
        }
    }


    //ゲームをはじめから開始する
    public IEnumerator Start_Game_Cor() {
        EventSystem.current.SetSelectedGameObject(null);
        FadeInOut.Instance.Start_Fade_Out(new Color(0, 0, 0), 0.02f);
        yield return new WaitForSeconds(1.0f);

        SceneManagement.Instance.Delete_Visit_Scene();  //シーン進行度
        DataManager.Instance.Initialize_Player_Data();  //自機のデータ
        PlayerPrefs.DeleteAll();

        DataManager.Instance.Load_Player_Data();
    }


    //続きからボタン
    public void Load_Button_Function() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            DataManager.Instance.Load_Player_Data();
        }        
    }

    //プレイヤーデータボタン
    public void Player_Data_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            SceneManager.LoadScene("PlayerDataScene");
        }
    }


    //設定ボタン
    public void Setting_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            SceneManager.LoadScene("ConfigScene");
        }
    }


    //遊びかたボタン
    public void Play_Guide_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            SceneManager.LoadScene("PlayGuideScene");
        }
    }


    //ゲーム終了ボタン
    public void Quit_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            UnityEngine.Application.Quit();
        }
    }

}
