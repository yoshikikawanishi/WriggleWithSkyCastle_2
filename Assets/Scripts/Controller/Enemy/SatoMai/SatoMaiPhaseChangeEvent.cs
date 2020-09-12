using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SatoMaiPhaseChangeEvent : MonoBehaviour {

    [SerializeField] private AudioClip true_Answer_Sound;
    [SerializeField] private AudioClip false_Answer_Sound;
    [Space]
    [SerializeField] private Canvas event_Canvas;
    [SerializeField] private GameObject selections1;
    [SerializeField] private GameObject selections2;

    private MessageDisplayCustom _message;
    private AudioSource _audio;
    private PlayerController player_Controller;
    private Rigidbody2D player_Rigid;

    private bool is_Selected_Answer = false;
    private bool is_Selected_True_Answer = false;
    private bool is_End_Event = false;


    void Start () {
        //取得
        _message = GetComponent<MessageDisplayCustom>();
        _audio = GetComponent<AudioSource>();
        GameObject player = GameObject.FindWithTag("PlayerTag");
        player_Controller = player.GetComponent<PlayerController>();
        player_Rigid = player.GetComponent<Rigidbody2D>();
        //メッセージ表示の初期設定
        _message.Set_Canvas_And_Panel_Name("SatoMaiEventCanvas", new string[2] { "MessagePanelLeft", "MessagePanelRight" });
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
        while (!player_Controller.Get_Is_Playable()) {
            yield return null;
        }
        player_Controller.Set_Is_Playable(false);
        player_Controller.Change_Animation("IdleBool");
        player_Rigid.velocity = new Vector2(0, 0);

        Appear_Window();        

        //全二問
        for (int i = 0; i < 2; i++) {
            Display_Message(1, 2, i);                           //問題文1：口上
            yield return new WaitUntil(Is_End_Message);

            Display_Answer_Selection(i);                        //選択肢表示

            Display_Message(3, 4, i);                           //問題文2：本文
            yield return new WaitUntil(Is_End_Message);                        
            yield return new WaitForSeconds(0.1f);

            Enable_Selection_Cursol(i);                         //選択肢を選択可能に

            yield return new WaitUntil(Is_Selected_Answer);     //選択されるまで待つ  
            EventSystem.current.SetSelectedGameObject(null);

            if (is_Selected_True_Answer) {                      //正解が押されたとき
                PlayerManager.Instance.Add_Life();
                Display_Message(5, 6, i);
                _audio.PlayOneShot(true_Answer_Sound);
            }            
            else {                                              //不正解が押されたとき
                PlayerManager.Instance.Reduce_Life();
                Display_Message(7, 8, i);
                _audio.PlayOneShot(false_Answer_Sound);
            }
            yield return new WaitUntil(Is_End_Message);

            Disable_Answer_Selection();
        }

        Disappear_Window();        

        //終了設定
        PauseManager.Instance.Set_Is_Pausable(true);
        player_Controller.Set_Is_Playable(true);
        is_End_Event = true;
    }


    //問題用ウィンドウを表示
    private void Appear_Window() {
        event_Canvas.gameObject.SetActive(true);
    }


    //セリフ開始
    //セリフのテキストは問題1と問題2で量を同じに、問題1はID:1～19, 問題2はID:21～39
    private void Display_Message(int beginID, int endID, int question_Number) {
        int i = question_Number;
        _message.Start_Display("SatoMaiEventText", beginID + i * 20, endID + i * 20);
    }


    //セリフ終了
    private bool Is_End_Message() {
        return _message.End_Message();
    }


    //問題の選択肢を表示
    private void Display_Answer_Selection(int question_Number) {
        if (question_Number == 0)
            selections1.SetActive(true);
        else if (question_Number == 1)
            selections2.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }


    //選択肢のカーソルを出す    
    private void Enable_Selection_Cursol(int question_Number) {
        if (question_Number == 0)
            selections1.GetComponentInChildren<Button>().Select();
        else if(question_Number == 1)
            selections2.GetComponentInChildren<Button>().Select();
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
        selections1.SetActive(false);
        selections2.SetActive(false);
    }


    //問題用ウィンドウを消す
    private void Disappear_Window() {
        event_Canvas.gameObject.SetActive(false);
    }


    //他からイベント終了を検知する
    public bool Is_End_Event() {
        return is_End_Event;
    }
	

    //====================== ボタン ======================
    
    //インスペクタで選択肢ボタンにセットすること
    //正解の場合は引数trueに
    public void Selection_Button(bool correct_Answer) {
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            is_Selected_Answer = true;
            is_Selected_True_Answer = correct_Answer;
        }
    }    


}
