using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// メッセージを表示する
/// シーン内にキャンバスと表示用のパネルを用意すること
/// 
/// ※パネルの構成
///         ﾊﾟﾈﾙ
///           |
///         |￣￣|
///     ﾒｯｾﾃｷｽﾄ　ﾈｰﾑﾃｷｽﾄ
/// </summary>
public class MessageDisplay : MonoBehaviour {
    
    //テキストの複数列を入れる2次元配列
    public string[,] textWords;

    //表示する用のパネル
    GameObject messagePanel;
    //メッセージ表示のテキストコンポーネント
    private Text messageText;
    //キャラ名表示のテキストコンポーネント
    private Text nameText;

    //表示するID番号
    private int start_ID = 1;
    private int end_ID = 1;

    //表示終了、
    private bool endMessage = false;
    
    //メッセージ表示の速度
    private float textSpeed = 0.07f;

    //キャンバスとメッセージパネルの名前
    private string canvas_Name = "Canvas";
    private string panel_Name = "MessagePanel";

    //メッセージ表示後に選択画面を生成するか
    public bool is_Display_Selection_After_Message = false;
    //メッセージ表示後に生成する選択画面、シーン上に配置する
    [SerializeField] private Canvas selection_Canvas;
   

    // Update is called once per frame
    private void Update() {
        //表示スピード
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            textSpeed = 0.005f;
        }
        if (InputManager.Instance.GetKeyUp(MBLDefine.Key.Jump)) {
            textSpeed = 0.07f;
        }
    }


    //=================================================== public =======================================================
    //表示開始
    public void Start_Display(string fileName, int start_ID, int end_ID) {
        endMessage = false;
        //テキストファイルの読み込み
        Read_Text(fileName);
        //セリフ枠の表示、テキスト、アイコンの取得
        Display_Panel();
        //番号の代入
        this.start_ID = start_ID;
        this.end_ID = end_ID;
        //セリフの表示
        StartCoroutine("Print_Message");
    }

    //表示開始    
    public void Start_Display(string fileName, int start_ID, int end_ID, bool is_Overwrite) {
        if (is_Overwrite)
            StopAllCoroutines();
        else if (messagePanel.activeSelf)
            return;
        
        Start_Display(fileName, start_ID, end_ID);
    }


    //表示開始
    public void Start_Display_Auto(string fileName, int start_ID, int end_ID, float waitingTime, float speed) {
        endMessage = false;
        //テキストファイルの読み込み
        Read_Text(fileName);
        //セリフ枠の表示、テキスト、アイコンの取得
        Display_Panel();
        //番号の代入
        this.start_ID = start_ID;
        this.end_ID = end_ID;
        //セリフの表示
        StartCoroutine(Print_Message_Auto(waitingTime, speed));
    }

    //表示開始
    public void Start_Display_Auto(string fileName, int start_ID, int end_ID, float waitingTime, float speed, bool is_Overwrite) {
        if (is_Overwrite)
            StopAllCoroutines();
        else if (messagePanel != null && messagePanel.activeSelf)            
            return;

        Start_Display_Auto(fileName, start_ID, end_ID, waitingTime, speed);
    }


    //表示用キャンバスとパネルの変更
    public void Set_Canvas_And_Panel_Name(string canvas_Name, string panel_Name) {
        this.canvas_Name = canvas_Name;
        this.panel_Name = panel_Name;
    }

    //表示の強制終了
    public void Quit_Message() {
        StopAllCoroutines();
        messagePanel.SetActive(false);
        endMessage = true;
    }

    //==================================================== private ====================================================
    //テキストファイルの読み込み
    private void Read_Text(string fileName) {
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(fileName);
        textWords = text.textWords;
    }


    //メッセージパネルの表示
    private void Display_Panel() {
        GameObject canvas = GameObject.Find(canvas_Name);
        messagePanel = canvas.transform.Find(panel_Name).gameObject;
        if (messagePanel == null)
            messagePanel = Instantiate(Resources.Load("UI/MessagePanel") as GameObject, canvas.transform);
        messagePanel.SetActive(true);
        messagePanel.transform.SetAsLastSibling();
        //テキストを取得
        messageText = messagePanel.transform.GetChild(0).GetComponent<Text>();
        messageText.text = "";
        //キャラ名表示のテキストを取得
        nameText = messagePanel.transform.GetChild(1).GetComponent<Text>();
        nameText.text = "";
    }


    //メッセージ表示
    protected IEnumerator Print_Message() {
        //効果音の取得
        AudioSource sound = messagePanel.GetComponent<AudioSource>();
        //1行ずつ表示
        for (int i = start_ID; i <= end_ID; i++) {
            //名前とアイコン
            nameText.text = textWords[i, 1];
            //セリフ
            int lineLength = textWords[i, 3].Length;
            for(int j = 0; j < lineLength; j++) {
                if (textWords[i, 3][j] == '/') {
                    messageText.text += "\n";
                }
                else {
                    messageText.text += textWords[i, 3][j];
                    sound.Play();
                }
                for (float t = 0; t < textSpeed; t += 0.016f) { yield return null; }
            }
            //1行分表示後決定が押されるのを待つ
            yield return new WaitUntil(Wait_Input_Z);
            //終了
            if (i == end_ID)
                break;
            //次の行へ
            messageText.text = "";
        }

        //選択画面を表示する（選択時の処理は選択画面キャンバスに個別で入れること）
        if (is_Display_Selection_After_Message) {
            yield return new WaitForSeconds(0.2f);
            selection_Canvas.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            selection_Canvas.GetComponentInChildren<Button>().Select();
            yield return new WaitUntil(Wait_Input_Z);
            selection_Canvas.gameObject.SetActive(false);
        }

        //表示終了
        messagePanel.SetActive(false);
        endMessage = true;
    }


    //Zが入力されるのを待つ
    protected bool Wait_Input_Z() {
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            return true;
        }
        return false;
    }


    //メッセージ表示
    protected IEnumerator Print_Message_Auto(float waitingTime, float speed) {
        //効果音の取得
        AudioSource sound = messagePanel.GetComponent<AudioSource>();
        //1行ずつ表示
        for (int i = start_ID; i <= end_ID; i++) {
            //名前とアイコン
            nameText.text = textWords[i, 1];
            //セリフ
            int lineLength = textWords[i, 3].Length;
            for (int j = 0; j < lineLength; j++) {
                if (textWords[i, 3][j] == '/') {
                    messageText.text += "\n";
                }
                else {
                    messageText.text += textWords[i, 3][j];
                    sound.Play();
                }
                for (float t = 0; t < speed; t += 0.016f) { yield return null; }
            }
            //1行分表示後決定が押されるのを待つ
            for(float t = 0; t < waitingTime; t += 0.016f) { yield return null; }            
            //次の行へ
            messageText.text = "";
        }
        //表示終了
        messagePanel.SetActive(false);
        endMessage = true;
    }


    //メッセージ表示終了を他スクリプトで検知する用
    public bool End_Message() {
        if (endMessage) {
            endMessage = false;
            return true;
        }
        return false;
    }
    
}
