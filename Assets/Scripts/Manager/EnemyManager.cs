using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OneLifeEnemy {
    public readonly GameObject obj;
    public readonly string name;
    private bool is_Exist = true;

    public OneLifeEnemy(GameObject obj) {
        this.obj = obj;
        this.name = obj.name;
    }

    public void Delete_Exist() {
        is_Exist = false;
    }

    public bool Is_Exist() {
        return is_Exist;
    }    
}


/// <summary>
/// 敵を一度倒されたら復活しないようにする
/// Enemyクラスのis_One_Lifeをtrueにした敵だけに反映させる
/// </summary>
public class EnemyManager : MonoBehaviour {
    
    [SerializeField] private string file_Name;
    private string file_Path;    

    [SerializeField] private readonly float rough_Update_Span = 1.5f;
    private float rough_Update_Time = 0;

    private List<OneLifeEnemy> enemies = new List<OneLifeEnemy>();


    void Awake() {
        SceneManager.sceneUnloaded += SceneUnloaded;
        file_Path = Application.dataPath + @"\StreamingAssets\" + file_Name + ".txt";
    }
    
    void Start () {        
        Find_Enemies();                 //シーン内の敵を探す       
        Sort_Enemies_In_Position();     //enemiesリストをソートする
        Write_Initial_Setting();        //初回時見つかった敵をテキストファイルに保存
        Load_Data_And_Delete_Enemy();   //ファイルの読み込みと一度倒された敵の消去
    }

    void Update() {
        //Updateのスパンを長くする
        if(rough_Update_Time < rough_Update_Span) {
            rough_Update_Time += Time.deltaTime;
        }
        else {
            RoughUpdate();
            rough_Update_Time = 0;
        }
    }
    void RoughUpdate() {
        Delete_Enemy();                 //倒されている敵のフラグを消す(次回以降その敵が出ない)        
    }
   
    void SceneUnloaded(Scene thisScene) {
        Save_Data();                    //倒された敵をテキストファイルに上書き保存
    }


    //シーン内にいる敵を取得する
    private void Find_Enemies() {
        enemies.Clear();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("EnemyTag");
        Enemy enemy;
        
        //EnemyTagのオブジェクトの中からEnemyクラスを持っていて、is_One_Lifeになっているオブジェクトを探す
        for(int i = 0; i < objs.Length; i++) {
            enemy = objs[i].GetComponent<Enemy>();
            if (enemy == null || !objs[i].activeSelf)
                continue;
            if (enemy.is_One_Life)
                enemies.Add(new OneLifeEnemy(objs[i]));
        }        
    }


    //敵をx座標の小さい順にソートする
    private void Sort_Enemies_In_Position() {
        var c = new Comparison<OneLifeEnemy>(Compare);
        enemies.Sort(c);        
    }

    static int Compare(OneLifeEnemy a, OneLifeEnemy b) {
        if (a.obj.transform.position.x > b.obj.transform.position.x)
            return 1;
        else if(a.obj.transform.position.x < b.obj.transform.position.x)
            return -1;
        return 0;
    }


    //初期のシーン内の敵をテキストファイルに書き出す
    private void Write_Initial_Setting() {
        if (!SceneManagement.Instance.Is_First_Visit()) 
            return;        

        StreamWriter sw_Clear = new StreamWriter(file_Path, false);

        sw_Clear.Write("Name" + "\t" + "IsExist");
        for(int i = 0; i < enemies.Count; i++) {
            sw_Clear.Write("\n" + enemies[i].name + "\t" + "True");
        }

        sw_Clear.Flush();
        sw_Clear.Close();
    }


    //データの読み込みと敵の消去
    private void Load_Data_And_Delete_Enemy() {
        TextFileReader text = new TextFileReader();
        text.Read_Text_File_Path(file_Path);

        for (int i = 1; i < text.rowLength; i++) {
            if (text.textWords[i, 1] == "False") {
                enemies[i - 1].Delete_Exist();
                enemies[i - 1].obj.SetActive(false);
            }
        }
    }


    //倒されている敵のフラグを変える
    //RoughUpdate内で呼ぶから注意    
    private void Delete_Enemy() {
        for(int i = 0; i < enemies.Count; i++) {            
            if (!enemies[i].Is_Exist())
                continue;
            if (!enemies[i].obj.activeSelf) {
                enemies[i].Delete_Exist();
            }
            else if (enemies[i].obj == null)
                enemies[i].Delete_Exist();
        }        
    }


    //データを上書き保存する
    private void Save_Data() {
        StreamWriter sw_Clear = new StreamWriter(file_Path, false);

        sw_Clear.Write("Name" + "\t" + "IsExist");
        for (int i = 0; i < enemies.Count; i++) {
            sw_Clear.Write("\n" + enemies[i].name + "\t" + enemies[i].Is_Exist().ToString());
        }

        sw_Clear.Flush();
        sw_Clear.Close();
    }
    
}
