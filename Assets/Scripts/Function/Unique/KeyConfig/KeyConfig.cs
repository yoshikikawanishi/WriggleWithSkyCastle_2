using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

/// <summary>
/// キーコンフィグ情報を取り扱う
/// </summary>
public class KeyConfig {
    private Dictionary<string, List<KeyCode>> config = new Dictionary<string, List<KeyCode>>();
    
    /// <summary>
    /// キーコンフィグを管理するクラスを生成する
    /// </summary>
    public KeyConfig() {
        
    }

    /// <summary>
    /// 指定したキーの入力状態をチェックする
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <param name="predicate">キーが入力されているかを判定する述語</param>
    /// <returns>入力状態</returns>
    private bool InputKeyCheck(string keyName, Func<KeyCode, bool> predicate) {
        bool ret = false;
        foreach (var keyCode in config[keyName])
            if (predicate(keyCode))
                return true;
        return ret;
    }

    /// <summary>
    /// 指定したキーが押下状態かどうかを返す
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <returns>入力状態</returns>
    public bool GetKey(string keyName) {
        return InputKeyCheck(keyName, Input.GetKey);
    }

    /// <summary>
    /// 指定したキーが入力されたかどうかを返す
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <returns>入力状態</returns>
    public bool GetKeyDown(string keyName) {
        return InputKeyCheck(keyName, Input.GetKeyDown);
    }

    /// <summary>
    /// 指定したキーが離されたかどうかを返す
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <returns>入力状態</returns>
    public bool GetKeyUp(string keyName) {
        return InputKeyCheck(keyName, Input.GetKeyUp);
    }

    /// <summary>
    /// 指定されたキーに割り付けられているキーコードを返す
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <returns>キーコード</returns>
    public List<KeyCode> GetKeyCode(string keyName) {
        if (config.ContainsKey(keyName))
            return new List<KeyCode>(config[keyName]);
        return new List<KeyCode>();
    }

    /// <summary>
    /// 名前文字列に対するキーコードを設定する
    /// </summary>
    /// <param name="keyName">キーに割り付ける名前</param>
    /// <param name="keyCode">キーコード</param>
    /// <returns>キーコードの設定が正常に完了したかどうか</returns>
    public bool SetKey(string keyName, List<KeyCode> keyCode) {
        if (string.IsNullOrEmpty(keyName) || keyCode.Count < 1)
            return false;
        config[keyName] = keyCode;
        return true;
    }

    /// <summary>
    /// コンフィグからキーコードを削除する
    /// </summary>
    /// <param name="keyName">キーに割り付けられている名前</param>
    /// <returns></returns>
    public bool RemoveKey(string keyName) {
        return config.Remove(keyName);
    }

    /// <summary>
    /// 設定されているキーコンフィグを確認用文字列として受け取る
    /// </summary>
    /// <returns>キーコンフィグを表す文字列</returns>
    public string CheckConfig() {
        StringBuilder sb = new StringBuilder();        
        foreach (var keyValuePair in config) {
            sb.AppendLine("Key : " + keyValuePair.Key);
            foreach (var value in keyValuePair.Value)
                sb.AppendLine("  |_ Value : " + value);
        }
        return sb.ToString();
    }


    /// <summary>
    ///　文字列からキーコードを取得する
    ///　</summary>
    public KeyCode GetCode(string keyCodeName) {
        foreach(KeyCode key in Enum.GetValues(typeof(KeyCode))) {
            if(key.ToString() == keyCodeName) {
                return key;
            }
        }
        return new KeyCode();
    }

    /// <summary>
    /// ファイルからキーコンフィグファイルをロードする
    /// </summary>
    public void LoadConfigFile() {

        string filePath = Application.dataPath + @"\StreamingAssets\KeyConfig.txt";
        TextFileReader text = new TextFileReader();
        text.Read_Text_File_Path(filePath);

        string keyname, key1, key2;
        for (int i = 1; i < text.rowLength; i++) {
            keyname = text.textWords[i, 0];
            key1 = text.textWords[i, 1];
            key2 = text.textWords[i, 2];
            List<KeyCode> keycode = new List<KeyCode>() { GetCode(key1), GetCode(key2) };
            SetKey(keyname, keycode);
        }

    }

    /// <summary>
    /// 現在のキーコンフィグをファイルにセーブする
    /// </summary>
    public void SaveConfigFile() {  
        
        string filePath = Application.dataPath + @"\StreamingAssets\KeyConfig.txt";

        StreamWriter sw_Clear = new StreamWriter(filePath, false);
        sw_Clear.Write("#KEYNAME" + "\t" + "#VALUE1" + "\t" + "VALUE2");
        foreach(var keyValuePair in config) {
            sw_Clear.Write("\n" + keyValuePair.Key + "\t" + keyValuePair.Value[0] + "\t" + keyValuePair.Value[1]);
        }
        sw_Clear.Flush();
        sw_Clear.Close();

    }

}