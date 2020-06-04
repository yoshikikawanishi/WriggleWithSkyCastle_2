using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMMelody : MonoBehaviour {

    //BGMの曲調
    public enum Melody {
       intro,
       A,
       B,
       C,
       main,
       none,
    }
    private Melody now_Melody = Melody.intro;
    private Melody old_Melody = Melody.none;

    [System.Serializable]
    public class OneMelody {
        public Vector2 span;
        public Melody melody;
    }
    public List<OneMelody> melody_List = new List<OneMelody>();
    private int list_Count = 0;

    private float now_BGM_Time = 0;
    private float BGM_Launch_Time = -1;    


    void Update() {
        if (BGM_Launch_Time < 0)
            return;

        now_BGM_Time = (Time.unscaledTime - BGM_Launch_Time) % melody_List[melody_List.Count - 1].span.y;        

        if (melody_List[list_Count].span.x <= now_BGM_Time && now_BGM_Time < melody_List[list_Count].span.y) {            
            now_Melody = melody_List[list_Count].melody;            
        }
        else {
            list_Count = (list_Count + 1) % melody_List.Count;            
        }
        
    }


    //時間計測開始
    public void Start_Time_Count() {
        BGM_Launch_Time = Time.unscaledTime;
    }


    //メロディ取得
    public Melody Get_Now_Melody() {
        return now_Melody;
    }
   

    public float Get_Now_BGM_Time() {
        now_BGM_Time = (Time.unscaledTime - BGM_Launch_Time) % melody_List[melody_List.Count - 1].span.y;
        return now_BGM_Time;
    }


    //メロディ切り替え時そのメロディを返す
    //updateで呼ぶこと
    public Melody Switch_Melody_Trigger() {
        if(old_Melody != now_Melody) {
            old_Melody = now_Melody;
            return now_Melody;
        }
        return Melody.none;
    }

    //====================================Editor用======================================    
    public void Add_Melody() {
        melody_List.Add(new OneMelody());
    }

    public void Remove_Melody() {
        melody_List.RemoveAt(melody_List.Count - 1);
    }
}
