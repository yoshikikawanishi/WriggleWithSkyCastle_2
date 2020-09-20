using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyManager : MonoBehaviour {

    //BGMの曲調
    public enum Melody {
       intro,
       A1,
       A2,
       B1,
       B2,
       pre_Chorus,
       chorus1,
       chorus2,
       bridge,
       C,
       none,
    }
    private Melody now_Melody = Melody.none;
    
    //インスペクタで編集する用
    [System.Serializable]
    public class OneMelody {
        public Vector2 span;
        public Melody melody;
    }
    public List<OneMelody> melody_List = new List<OneMelody>();
    private int list_Count = 0;

    //時間計測用
    private float now_BGM_Time = 0;
    private float BGM_Launch_Time = -1;        


    void Update() {
        if (BGM_Launch_Time < 0)
            return;

        //時間計測        
        now_BGM_Time = (Time.unscaledTime - BGM_Launch_Time) % melody_List[melody_List.Count - 1].span.y;
        if (list_Count < melody_List.Count - 1) {
            if (now_BGM_Time > melody_List[list_Count].span.y) {
                list_Count = (list_Count + 1) % melody_List.Count;                
            }
        }
        else {
            if(now_BGM_Time < melody_List[list_Count].span.x) {
                list_Count = 0;                
            }
        }

        
        //現在のメロディを代入
        if (now_Melody != melody_List[list_Count].melody) {
            now_Melody = melody_List[list_Count].melody;
        }
    }   


    //時間計測開始
    public void Start_Time_Count() {
        BGM_Launch_Time = Time.unscaledTime;
        now_Melody = melody_List[0].melody;        
    }


    //メロディ取得
    public Melody Get_Now_Melody() {
        return now_Melody;
    }       


    //====================================Editor用======================================    
    public void Add_Melody() {
        melody_List.Add(new OneMelody());
    }

    public void Remove_Melody() {
        melody_List.RemoveAt(melody_List.Count - 1);
    }
}
