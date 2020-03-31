using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextFileReader  {

    //テキストの加工前の一行を入れる変数
    private string[] textMessage;
    //テキストの複数列を入れる2次元配列
    public string[,] textWords;
    //テキスト内の行数を取得する変数
    public int rowLength;
    //テキスト内の列数を取得する変数
    public int columnLength;

    
    //コンストラクタ
    public TextFileReader() {

    }

    public TextFileReader(string file_Name) {
        this.Read_Text_File(file_Name);
    }


    //テキストファイルの読み込み(Resources)
    public void Read_Text_File(string file_Name) {
        TextAsset textasset = new TextAsset();
        textasset = Resources.Load("Text/" + file_Name, typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        if(textasset == null) {
            Debug.Log(file_Name + " is not Exist");
        }
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる

        //Splitで一行づつを代入した1次配列を作成
        textMessage = TextLines.Split('\n'); //

        //行数と列数を取得
        columnLength = textMessage[0].Split('\t').Length;
        rowLength = textMessage.Length;

        //2次配列を定義
        textWords = new string[rowLength, columnLength];

        for (int i = 0; i < rowLength; i++) {

            string[] tempWords = textMessage[i].Split('\t'); //textMessageをカンマごとに分けたものを一時的にtempWordsに代入

            for (int n = 0; n < columnLength; n++) {
                textWords[i, n] = tempWords[n]; //2次配列textWordsにカンマごとに分けたtempWordsを代入していく
            }
        }
    }


    //テキストファイル読み込み(ファイルパス)
    public void Read_Text_File_Path(string file_Path) {
        string TextLines = File.ReadAllText(file_Path); //テキスト全体をstring型で入れる変数を用意して入れる

        //Splitで一行づつを代入した1次配列を作成
        textMessage = TextLines.Split('\n'); //

        //行数と列数を取得
        columnLength = textMessage[0].Split('\t').Length;
        rowLength = textMessage.Length;

        //2次配列を定義
        textWords = new string[rowLength, columnLength];

        for (int i = 0; i < rowLength; i++) {

            string[] tempWords = textMessage[i].Split('\t'); //textMessageをカンマごとに分けたものを一時的にtempWordsに代入

            for (int n = 0; n < columnLength; n++) {
                textWords[i, n] = tempWords[n]; //2次配列textWordsにカンマごとに分けたtempWordsを代入していく
            }
        }
    }

}
