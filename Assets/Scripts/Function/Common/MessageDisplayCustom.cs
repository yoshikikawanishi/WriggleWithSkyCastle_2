using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// メッセージを表示する
/// シーン内にキャンバスと表示用のパネル複数を用意すること
/// 
/// ※パネルの構成
///         ﾊﾟﾈﾙ
///           |
///         |￣￣|
///     ﾒｯｾﾃｷｽﾄ　ﾈｰﾑﾃｷｽﾄ
/// </summary>
public class MessageDisplayCustom : MonoBehaviour {
    
    private GameObject[] message_Panels;
    private Text[] message_Texts;
    private Text[] name_Texts;

    private bool end_Message = true;
    private bool is_Auto = false;

    private float text_Speed = 0.02f;

    private string canvas_Name = "Canvas";
    private string[] panel_Names = { "MessagePanelLeft", "MessagePanelRight" };

    private TextFileReader text = new TextFileReader();    


    // Update is called once per frame
    private void Update() {
        if (end_Message || is_Auto)
            return;

        //表示スピード
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            text_Speed = 0.005f;
        }
        if (InputManager.Instance.GetKeyUp(MBLDefine.Key.Jump)) {
            text_Speed = 0.07f;
        }
    }

    
    //表示開始
    public void Start_Display(string file_Name, int start_ID, int end_ID) {
        is_Auto = false;
        Setting_Panel_And_Text();
        Read_Text_File(file_Name);
        StartCoroutine(Display_Message_Cor(start_ID, end_ID));
    }


    //表示開始
    public void Start_Display_Auto(string file_Name, int start_ID, int end_ID, float word_Span, float sentence_Span) {
        is_Auto = true;
        Start_Display(file_Name, start_ID, end_ID);
    }


    ///メッセージ表示終了を他スクリプトで検知する用
    public bool End_Message() {
        if (end_Message) {
            end_Message = false;
            return true;
        }
        return false;
    }


    //表示用キャンバスとパネルの変更
    public void Set_Canvas_And_Panel_Name(string canvas_Name, string[] panel_Names) {
        this.canvas_Name = canvas_Name;
        this.panel_Names = panel_Names;
    }

    
    //表示の強制終了
    public void Quit_Message() {
        StopAllCoroutines();
        for (int i = 0; i < message_Panels.Length; i++)
            message_Panels[i].SetActive(false);
        end_Message = true;
    }


    //===============================================================================================

    //パネルとテキストの取得、初期化
    private void Setting_Panel_And_Text() {
        GameObject canvas = GameObject.Find(canvas_Name);
        
        int panel_Num = panel_Names.Length;
        message_Panels = new GameObject[panel_Num];
        message_Texts = new Text[panel_Num];
        name_Texts = new Text[panel_Num];

        for(int i = 0; i < panel_Num; i++) {
            message_Panels[i] = canvas.transform.Find(panel_Names[i]).gameObject;            
            message_Texts[i] = message_Panels[i].transform.GetChild(0).GetComponent<Text>();
            name_Texts[i] = message_Panels[i].transform.GetChild(1).GetComponent<Text>();

            message_Panels[i].SetActive(true);
            message_Texts[i].text = "";
            name_Texts[i].text = "";
        }
    }


    //テキストファイル読み込み
    private void Read_Text_File(string file_Name) {
        text.Read_Text_File(file_Name);
    }


    //表示ルーチン
    private IEnumerator Display_Message_Cor(int start_ID, int end_ID) {
        //効果音の取得
        AudioSource sound = message_Panels[0].GetComponent<AudioSource>();

        end_Message = false;
        int index = 0;

        //1行ずつ表示
        for (int i = start_ID; i <= end_ID; i++) {
            //パネル番号            
            try {
                index = int.Parse(text.textWords[i, 2]);
            }
            catch (System.FormatException) {
                index = 0;
            }

            //名前とアイコン
            name_Texts[index].text = text.textWords[i, 1];
            //セリフ
            int lineLength = text.textWords[i, 3].Length;
            for (int j = 0; j < lineLength; j++) {
                if (text.textWords[i, 3][j] == '/') {
                    message_Texts[index].text += "\n";
                }
                else {
                    message_Texts[index].text += text.textWords[i, 3][j];
                    sound.Play();
                }
                yield return new WaitForSecondsRealtime(text_Speed);
            }
            //1行分表示後決定が押されるのを待つ
            yield return new WaitUntil(Wait_Input_Z);
            //終了
            if (i == end_ID)
                break;
            //次の行へ
            message_Texts[index].text = "";
        }

        for (int i = 0; i < message_Panels.Length; i++)
            message_Panels[i].gameObject.SetActive(false);

        end_Message = true;
    }


    //Zが入力されるのを待つ
    private bool Wait_Input_Z() {
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            return true;
        }
        return false;
    }

}
