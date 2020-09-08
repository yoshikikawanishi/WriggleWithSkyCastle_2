using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatoMaiPhaseChangeEvent : MonoBehaviour {

    [SerializeField] private AudioClip true_Answer_Sound;
    [SerializeField] private AudioClip false_Answer_Sound;

    private MessageDisplay _message;
    private PlayerController player_Controller;

    private bool is_Selected_Answer = false;
    private bool is_Selected_True_Answer = false;
    private bool is_End_Event = false;


    void Start () {
        //取得

        player_Controller = GameObject.FindWithTag("PlayerTag").GetComponent<PlayerController>();                       
    }


    public void Start_Event() {
        //イベントは一回だけ
        if (is_End_Event)
            return;
        StartCoroutine("Event_Flow_Cor");
    }


    private IEnumerator Event_Flow_Cor() {
        //初期設定
        PauseManager.Instance.Set_Is_Pausable(false);
        player_Controller.Set_Is_Playable(false);
        //_message.Set_Canvas_And_Panel_Name( );

        Appear_Window();        

        //全二問
        for (int i = 0; i < 2; i++) {
            Display_Message(0, 0);                              //問題文1：口上
            yield return new WaitUntil(Is_End_Message);

            Display_Answer_Selection(i);                        //選択肢表示

            Display_Message(0, 0);                              //問題文2：本文
            yield return new WaitUntil(Is_End_Message);

            Enable_Selection_Cursol(null);                      //選択肢を選択可能に

            yield return new WaitUntil(Is_Selected_Answer);     //選択されるまで待つ

            if (is_Selected_True_Answer) {                      //正解が押されたとき
                PlayerManager.Instance.Add_Life();
                Display_Message(0, 0);                              
            }            
            else {                                              //不正解が押されたとき
                PlayerManager.Instance.Reduce_Life();
                Display_Message(0, 0);
            }
            yield return new WaitUntil(Is_End_Message);
        }

        Disappear_Window();

        //終了設定
        PauseManager.Instance.Set_Is_Pausable(true);
        player_Controller.Set_Is_Playable(true);
        is_End_Event = true;
    }


    //問題用ウィンドウを表示
    private void Appear_Window() {

    }


    //セリフ開始
    private void Display_Message(int beginID, int endID) {

    }


    //セリフ終了
    private bool Is_End_Message() {
        return _message.End_Message();
    }


    //問題の選択肢を表示
    private void Display_Answer_Selection(int question_Number) {

    }


    //選択肢のカーソルを出す
    private void Enable_Selection_Cursol(Button select_Button) {
        select_Button.Select();
    }


    //選択肢が選択されたらtrueを返す
    private bool Is_Selected_Answer() {
        if (is_Selected_Answer) {
            is_Selected_Answer = false;
            return true;
        }
        return false;            
    }
    

    //問題の選択肢を消す
    private void Disable_Answer_Selection() {

    }


    //問題用ウィンドウを消す
    private void Disappear_Window() {

    }


    //他からイベント終了を検知する
    public bool Is_End_Event() {
        return is_End_Event;
    }
	

    //====================== ボタン ======================

    //正解のボタンが押されたとき
    //インスペクタで正解のボタンにセットすること
    public void True_Answer_Button() {
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            is_Selected_Answer = true;
            is_Selected_True_Answer = true;
        }
    }


    //不正解のボタンが押されたとき
    //インスペクタで不正解のボタンにセットすること
    public void False_Answer_Button() {
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            is_Selected_Answer = true;
            is_Selected_True_Answer = false;
        }
    }


}
