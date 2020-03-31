using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CollectionManager : SingletonMonoBehaviour<CollectionManager> {

    private Dictionary<string, bool> collections_Data = new Dictionary<string, bool>();
    private string FILEPATH;

    private new void Awake() {
        base.Awake();
        FILEPATH = Application.dataPath + @"\StreamingAssets\CollectionsData.txt";
        //セーブファイルの読み込み
        Load_Data();

        //データの消去
        #if UNITY_EDITOR
        if (DebugModeManager.Instance.Delete_Collection_Data) {                        
            Delete_Collections();
        }
        #endif
    }


    //セーブファイルの読み込み
    private void Load_Data() {        
        TextFileReader text = new TextFileReader();
        text.Read_Text_File_Path(FILEPATH);
        
        for(int i = 1; i < text.rowLength; i++) {
            bool is_Collected = false;
            if (text.textWords[i, 1] == "True")
                is_Collected = true;
            collections_Data.Add(text.textWords[i, 0], is_Collected);
        }

        Debug.Log("Load Collections Data");
    }


    //データの保存
    private void Save_Data() {        
        StreamWriter sw_Clear = new StreamWriter(FILEPATH, false);                

        sw_Clear.Write("#Key\t#IsCollected");
        foreach (string key in collections_Data.Keys) {
            sw_Clear.Write("\n" + key + "\t" + collections_Data[key].ToString());            
        }

        sw_Clear.Flush();
        sw_Clear.Close();
    }


    //データの消去
    public void Delete_Collections() {
        List<string> key_List = new List<string>(collections_Data.Keys);
        foreach(string key in key_List) {
            collections_Data[key] = false;
        }
        Save_Data();
        Debug.Log("<color=#ff0000ff>Delete Collection Data </color>");
    }


    //アイテムを獲得する
    public void Aquire_Collection(string collection_Name) {
        collections_Data[collection_Name] = true;
        Save_Data();
    }


    //アイテムが獲得済みかどうか
    public bool Is_Collected(string collection_Name) {
        if (collections_Data.ContainsKey(collection_Name)) {
            return collections_Data[collection_Name];
        }

        //Debug.Log(collection_Name + " Collection is not Exist");
        return false;
    }
    

    //Getter
    public Dictionary<string, bool> Get_Collections_Data() {
        if (collections_Data == null) {
            Debug.Log("There is no Collections_Data");
        }
        return collections_Data;
    }    

}
