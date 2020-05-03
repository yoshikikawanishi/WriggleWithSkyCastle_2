using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoBGMTimeKeeper  {

    //BGMの曲調
    public enum Melody {
        A,
        B,
        main
    }
    private Melody now_Melody = Melody.A;

    //曲のタイミング                            A   B    A  ｻﾋﾞ前   ｻﾋﾞ    ﾙｰﾌﾟ
    public readonly float[] BGM_Time_Keeper = { 0, 20f, 42f, 48f, 56.14f, 79.0f };

    private float BGM_Launch_Time = 0;
    private float now_BGM_Time = 0;
 

    //時間計測開始
    public void Start_Time_Count() {
        BGM_Launch_Time = Time.unscaledTime;
    }
    

    //メロディ取得
    public Melody Get_Now_Melody() {
        now_BGM_Time = (Time.unscaledTime - BGM_Launch_Time) % BGM_Time_Keeper[5];

        if (now_BGM_Time < BGM_Time_Keeper[1]) {
            if (now_Melody != Melody.A)
                now_Melody = Melody.A;
        }
        else if (now_BGM_Time < BGM_Time_Keeper[2]) {
            if (now_Melody != Melody.B)
                now_Melody = Melody.B;
        }
        else if (now_BGM_Time < BGM_Time_Keeper[3]) {
            if (now_Melody != Melody.A)
                now_Melody = Melody.A;
        }
        else if (now_BGM_Time < BGM_Time_Keeper[5]) {
            if (now_Melody != Melody.main)
                now_Melody = Melody.main;
        }
        return now_Melody;
    }


    public float Get_BGM_Launch_Time() {
        return BGM_Launch_Time;
    }
}
