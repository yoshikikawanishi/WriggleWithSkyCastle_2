using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MBLDefine;

public class PlayerDataButton : MonoBehaviour {

    public Button first_Select_Button;
    public Text item_Comment_Text;
    public GameObject high_Score;
    public GameObject completed_Mark;
	
	void Start () {
        first_Select_Button.Select();
        Display_High_Score();
        Display_Completed_Mark();
	}


    public void Back_Title_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            SceneManager.LoadScene("TitleScene");
        }
    }


    public void Display_Item_Comment(string collection_Name) {
        //取得済みのアイテムなら表示
        if (CollectionManager.Instance.Is_Collected(collection_Name)) {
            if (item_Dic.ContainsKey(collection_Name))
                item_Comment_Text.text = item_Dic[collection_Name];
            else
                Debug.Log("There is no item comment");
        }
    }    


    public void Display_High_Score() {
        if (PlayerPrefs.HasKey("HIGHSCORE")) {
            high_Score.SetActive(true);
            high_Score.GetComponent<Text>().text = "ハイスコア：" + PlayerPrefs.GetInt("HIGHSCORE");
        }
    }


    public void Display_Completed_Mark() {
        if (CollectionManager.Instance.Is_Completed()) {
            completed_Mark.SetActive(true);
        }
    }


    /*---------------------------------------アイテムとそのコメント----------------------------------------*/
    private Dictionary<string, string> item_Dic = new Dictionary<string, string>() {
        { "Rumia", "ルーミアのリボン" },
        { "Mystia", "鳥獣伎楽の楽譜\nファンに高値で売れる" },
        { "LightFairy", "東方三月精　～ Eastern and Little Nature Deity.\nねうち：6,000G" },
        { "Yuka", "蠢符「リトルバグストーム」\nチャージショットを撃てるようになる" },
        { "Medicine", "もたせると　どくを　かいふくする　きのみ\n毒攻撃が強くなる" },
        { "Shizuha", "秋姉妹万歳！秋姉妹万歳！" },        
        { "BigFrog", "大蝦蟇の加護\n復活時の初期ライフが増える" },
        { "Hina", "お札\n敵を倒した時お札が飛び散る" },
        { "Nitori", "河童の腕\n攻撃範囲が広がる" },
        { "Momizi", "文果真報の袋とじ\nよかった。" },
        { "Aya", "天狗の羽団扇\nキックの速度が上昇する" },
        { "Yamame", "ダーマ\n?" },
        { "Saki", "ワザのまきもの\nチャージキックを会得する" },
        { "Kisume", "ドスン\n???" },
        { "Hourai", "楽園の人形" },
    };

}
