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
    }
    private Melody now_Melody = Melody.A;

    [System.Serializable]
    public class OneMelody {
        public Vector2 span;
        public Melody melody;
    }
    public List<OneMelody> melody_List = new List<OneMelody>();

    private float now_BGM_Time = 0;
    private float BGM_Launch_Time = 0;    


    //時間計測開始
    public void Start_Time_Count() {
        BGM_Launch_Time = Time.unscaledTime;
    }


    //メロディ取得
    public Melody Get_Now_Melody() {
        now_BGM_Time = (Time.unscaledTime - BGM_Launch_Time) % melody_List[melody_List.Count - 1].span.y;

        for (int i = 0; i < melody_List.Count; i++) {
            if (melody_List[i].span.x < now_BGM_Time && now_BGM_Time < melody_List[i].span.y) {
                now_Melody = melody_List[i].melody;
            }
        }
        return now_Melody;
    }
   

    public float Get_Now_BGM_Time() {
        now_BGM_Time = (Time.unscaledTime - BGM_Launch_Time) % melody_List[melody_List.Count - 1].span.y;
        return now_BGM_Time;
    }


    //====================================Editor用======================================    
    public void Add_Melody() {
        melody_List.Add(new OneMelody());
    }

    public void Remove_Melody() {
        melody_List.RemoveAt(melody_List.Count - 1);
    }
}
