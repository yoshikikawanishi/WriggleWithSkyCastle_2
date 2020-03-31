using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AyaMovie : MonoBehaviour {

    [SerializeField] private Aya aya;
    //画面エフェクト用
    [SerializeField] private AyaCameraFrame camera_Frame_Effect;

    //セリフのID
    [SerializeField] private Vector2Int[]   start_Message_ID        = new Vector2Int[3];
    [Space]
    [SerializeField] private float          on_The_Way_Message_Line1;
    [SerializeField] private Vector2Int[]   on_The_Way_Message_ID1  = new Vector2Int[2];
    [Space]
    [SerializeField] private float          on_The_Way_Message_Line2;
    [SerializeField] private Vector2Int[]   on_The_Way_Message_ID2  = new Vector2Int[2];
    [Space]
    [SerializeField] private Vector2Int[]   damaged_Message_ID      = new Vector2Int[3];

    private GameObject main_Camera;
    private MessageDisplay _message;

    //何回目のムービーか
    private int movie_Count = 0;
    //自機のライフ確認用
    private int player_Life;
    private int damaged_Count = 0;        

    
    void Awake () {        
        _message = GetComponent<MessageDisplay>();        
	}

    private void Start() {
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
        if (movie_Count > start_Message_ID.Length)
            yield break;
        
        _message.Set_Canvas_And_Panel_Name("AyaMessageCanvas", "AyaMessagePanel");        

        //開始セリフ
        Display_Message(start_Message_ID[movie_Count - 1]);        
        yield return new WaitUntil(_message.End_Message);        

        //カメラエフェクト
        camera_Frame_Effect.Appear();

        //一定x座標を越えた時のセリフ
        StartCoroutine("On_The_Way_Message1_Cor");
        StartCoroutine("On_The_Way_Message2_Cor");
        //自機被弾時セリフ
        StartCoroutine("Player_Damaged_Movie_Cor");        
    }


    //一定のx座標を超えた時のセリフ
    private IEnumerator On_The_Way_Message1_Cor() {        
        //待つ
        while(main_Camera.transform.position.x < on_The_Way_Message_Line1) {
            yield return null;
        }
        //セリフ
        Display_Message(on_The_Way_Message_ID1[movie_Count - 1]);
        yield return new WaitUntil(_message.End_Message);
    }

    private IEnumerator On_The_Way_Message2_Cor() {
        //待つ
        while (main_Camera.transform.position.x < on_The_Way_Message_Line2) {
            yield return null;
        }
        //セリフ
        Display_Message(on_The_Way_Message_ID2[movie_Count - 1]);
        yield return new WaitUntil(_message.End_Message);
        camera_Frame_Effect.Disappear();
    }


    //自機被弾時セリフ    
    private IEnumerator Player_Damaged_Movie_Cor() {
        //初期設定
        damaged_Count = 0;
        player_Life = PlayerManager.Instance.Get_Life();
        //待つ
        while (true) {
            //自機が回復したとき
            if (PlayerManager.Instance.Get_Life() > player_Life)
                player_Life++;
            //被弾時
            if (Is_Player_Damaged()) {
                damaged_Count++;
                break;    
            }
            yield return null;
        }
        //セリフ
        Display_Message(damaged_Message_ID[damaged_Count - 1]);
        yield return new WaitUntil(_message.End_Message);
        //次の被弾時セリフを待つ        
        if(damaged_Count < damaged_Message_ID.Length)
            StartCoroutine("Player_Damaged_Movie_Cor");                
    }


    //自機のライフが減ったときにtrueを返す    
    private bool Is_Player_Damaged() {
        if(PlayerManager.Instance.Get_Life() < player_Life) {
            player_Life = PlayerManager.Instance.Get_Life();
            return true;
        }
        return false;
    }



    private void Display_Message(Vector2Int ID) {
        _message.Start_Display_Auto("AyaText", ID.x, ID.y, 1.5f, 0.05f, false);
    }
}
