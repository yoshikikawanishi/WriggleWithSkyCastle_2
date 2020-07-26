using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AyaMovie : MonoBehaviour {

    [SerializeField] private Aya aya;
    //画面エフェクト用
    [SerializeField] private AyaCameraFrame camera_Frame_Effect;

    //表示するメッセージのステータス
    //表示開始するx座標とファイルのメッセージ    
    private class Message {
        public float start_Message_Line;
        public Vector2Int id;
        public Message(float start_Message_Line, Vector2Int id) {
            this.start_Message_Line = start_Message_Line;
            this.id = id;
        }        
    }

    private List<Message> message_List_In_First_Time = new List<Message>() {
        new Message(3500f, new Vector2Int(1, 2)),
        new Message(4500f, new Vector2Int(4, 6)),
        new Message(5700f, new Vector2Int(8, 9)),
    };
    private List<Message> message_List_In_Second_Time = new List<Message>() {
        new Message(3500f, new Vector2Int(11, 12)),
        new Message(4500f, new Vector2Int(15, 16)),
        new Message(5700f, new Vector2Int(18, 19)),
    };
    private List<Message> message_List_In_Third_Time = new List<Message>() {
        new Message(3500f, new Vector2Int(21, 23)),
    };

    private GameObject main_Camera;
    private MessageDisplay _message;    

    //何回目のムービーか
    private int movie_Count = 0;
   
    
    private void Start() {
        _message = GetComponent<MessageDisplay>();
        _message.Set_Canvas_And_Panel_Name("AyaMessageCanvas", "AyaMessagePanel");
        main_Camera = GameObject.FindWithTag("MainCamera");

        if (movie_Count >= 3 && aya != null)
            Destroy(aya.gameObject);
    }


    public int Get_Movie_Count() {
        return movie_Count;
    }


    //ムービーを開始する
    public void Play_Aya_Movie() {
        //ムービーの回数を取得
        if (!PlayerPrefs.HasKey("Aya")) {
            PlayerPrefs.SetInt("Aya", 0);
        }
        movie_Count = PlayerPrefs.GetInt("Aya") + 1;
        PlayerPrefs.SetInt("Aya", movie_Count);
        //３回目以降は文が登場しない
        if (movie_Count >= 3 && aya != null)
            Destroy(aya.gameObject);
        //ムービー開始
        StartCoroutine("Aya_Movie_Cor");
    }


    //ムービー本体
    private IEnumerator Aya_Movie_Cor() {
        if (movie_Count > 3)
            yield break;                

        //カメラエフェクト
        camera_Frame_Effect.Appear();                                       
        
        List<Message> list;
        switch (movie_Count) {
            case 1: list = message_List_In_First_Time; break;
            case 2: list = message_List_In_Second_Time; break;
            default: list = message_List_In_Third_Time; break;
        }

        for(int i = 0; i < list.Count; i++) {
            //すでに過ぎたものは飛ばす
            if (list[i].start_Message_Line < main_Camera.transform.position.x)
                continue;
            //設定したラインに到達するまで待つ
            while (!Is_Reach_Line(list[i].start_Message_Line))
                yield return null;
            
            Display_Message(list[i].id);            
            yield return new WaitUntil(_message.End_Message);
            //3回目のムービー時ムービー終わる
            if (movie_Count == 3) {
                camera_Frame_Effect.Disappear();
                yield break;
            }                
            camera_Frame_Effect.Attack();
        }
    }   


    //メッセージ表示
    private void Display_Message(Vector2Int ID) {
        _message.Start_Display_Auto("AyaText", ID.x, ID.y, 1.5f, 0.05f, false);
    }


    //カメラが引数地点の近くにいるかどうか
    private bool Is_Reach_Line(float line) {
        if(Mathf.Abs(main_Camera.transform.position.x - line) < 32f) {
            return true;
        }
        return false;
    }
}
