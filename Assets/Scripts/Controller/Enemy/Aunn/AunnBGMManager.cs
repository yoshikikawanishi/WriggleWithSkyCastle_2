﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AunnBGMManager{

    //BGMの曲調
    public enum Melody {
        A,
        B,
        C,
        main
    }
    private Melody now_Melody = Melody.A;

    //曲のタイミング                       A    B    A    C  ｻﾋﾞ前 ｻﾋﾞ  B   ﾙｰﾌﾟ
    public readonly float[] BGM_TIME = { 0, 13f, 26f, 35f, 40f, 46f, 72f, 81f};

    private float now_BGM_Time = 0;
    private float BGM_Launch_Time = 0;    


    //時間計測開始
    public void Start_Time_Count() {
        BGM_Launch_Time = Time.unscaledTime;
    }


    //メロディ取得
    public Melody Get_Now_Melody() {
        now_BGM_Time = (Time.unscaledTime - BGM_Launch_Time) % BGM_TIME[BGM_TIME.Length - 1];

        if (now_BGM_Time < BGM_TIME[1]) {
            if (now_Melody != Melody.A)
                now_Melody = Melody.A;
        }
        else if (now_BGM_Time < BGM_TIME[2]) {
            if (now_Melody != Melody.B)
                now_Melody = Melody.B;
        }
        else if (now_BGM_Time < BGM_TIME[3]) {
            if (now_Melody != Melody.A)
                now_Melody = Melody.A;
        }
        else if (now_BGM_Time < BGM_TIME[4]) {
            if (now_Melody != Melody.C)
                now_Melody = Melody.C;
        }
        else if (now_BGM_Time < BGM_TIME[6]) {
            if (now_Melody != Melody.main)
                now_Melody = Melody.main;
        }
        else if(now_BGM_Time < BGM_TIME[7]) {
            if (now_Melody != Melody.B)
                now_Melody = Melody.B;
        }
        return now_Melody;
    }


    public float Get_BGM_Launch_Time() {
        return BGM_Launch_Time;
    }

    public float Get_Now_BGM_Time() {
        return now_BGM_Time;
    }
}
