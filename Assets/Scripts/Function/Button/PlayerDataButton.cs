using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MBLDefine;

public class PlayerDataButton : MonoBehaviour {

    public Button first_Select_Button;
    public Text item_Comment_Text;


	// Use this for initialization
	void Start () {
        first_Select_Button.Select();	
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
        { "Nitori", "のびーるアーム\n攻撃範囲が広がる" },
        { "Momizi", "文果真報の袋とじ\nよかった。" },
        { "Aya", "\nキックの速度が上昇する" }

    };

}
