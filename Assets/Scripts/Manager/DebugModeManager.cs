using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//デバッグ用にデータ等を変更できるようにする
[System.Serializable]
public class DebugModeManager : SingletonMonoBehaviour<DebugModeManager> {
    
    
    //収集アイテムのデータを消す       
    public bool Delete_Collection_Data;

    //訪問済みシーンの消去    
    public bool Delete_Visited_Scene_Date;

    //テストプレイ時のシーンを初回訪問にするかどうか    
    public bool Is_First_Visit_Scene_In_Testplay;

    //BGM鳴らすかどうか
    public bool Is_Delete_BGM;

    //自機のデータを消す        
    public bool Delete_Player_Data;
    //自機の初期データ    
    public int Player_Life = 3;    
    public int Player_Power = 0;
    public PlayerManager.Option Player_Option = PlayerManager.Option.none;

    //幽香戦のデータを消す            
    public bool Delete_Yuka_Data;
    //ルーミアのデータ消す
    public bool Delete_Rumia_Data;
    //文のデータを消す
    public bool Delete_Aya_Data;
    //ゲームオーバーのデータを消す
    public bool Delete_Has_Game_Over;
    
}
