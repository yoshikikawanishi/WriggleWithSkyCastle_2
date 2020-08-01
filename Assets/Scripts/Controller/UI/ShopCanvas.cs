using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ショップキャンバス、Resources/UIに入れること、
/// Shopから生成される、ショップの内容も書いてる
/// </summary>
public class ShopCanvas : MonoBehaviour {

    private enum Goods {
        life = 0,
        stock = 1,
        bee = 2,
        butterfly = 3,
        mantis = 4,
        spider = 5,        
    }
    
    [SerializeField] private Button life_Up_Button;
    [SerializeField] private Text comment_Text;
    [SerializeField] private Button quit_Button;

    private PlayerController player_Controller;
    private PlayerManager player_Manager;

    //値段
    private int life_Up_Price;  //Adjust_Life_Up_Price()で調整
    private const int STOCK_UP_PRICE = 100;
    private const int OPTION_PRICE = 10;

    //コメント
    private const string WELLCOME_COMMENT = "■ よくきたね\n■ Pと　アイテムを　交換するよ";
    private const string THANKS_COMMENT = "■ まいど";
    private const string NOT_ENOUGH_COMMENT = "■ Pが　足りないよ";


    void Awake() {
        //変数取得        
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null)
            player_Controller = player.GetComponent<PlayerController>();
        player_Manager = PlayerManager.Instance;
    }


    //生成時
    void OnEnable() {
        Adjust_Life_Up_Price();
        StartCoroutine("Start_Shopping_Cor");
    }


    //ライフアップアイテムの値段を自機のライフに合わせて調整する
    public void Adjust_Life_Up_Price() {
        int player_Life = player_Manager.Get_Life();
        int price = 1;

        switch (player_Life) {
            case 1: price = 10; break;
            case 2: price = 20; break;
            case 3: price = 40; break;
            case 4: price = 80; break;
            case 5: price = 120; break;
            case 6: price = 160; break;
            default: price = 200; break;
        }

        life_Up_Button.GetComponentInChildren<Text>().text = (price * 0.01f).ToString("0.0");
        life_Up_Price = price;
    }


    //初期設定
    private IEnumerator Start_Shopping_Cor() {
        //即呼ばれるとボタン選択がおかしくなる
        yield return null;
        //ボタン選択
        EventSystem.current.SetSelectedGameObject(null);
        life_Up_Button.Select();
        //コメント変更
        Set_Shop_Comment(WELLCOME_COMMENT);
        //時間停止
        Time.timeScale = 0;
        PauseManager.Instance.Set_Is_Pausable(false);
        //自機操作禁止
        if (player_Controller != null)
            player_Controller.Set_Is_Playable(false);
    }


    //終了設定
    private void Quit_Shopping() {
        //時間再開
        Time.timeScale = 1;
        PauseManager.Instance.Set_Is_Pausable(true);
        //自機操作再開
        if (player_Controller != null)
            player_Controller.Set_Is_Playable(true);
        //キャンバス消す
        gameObject.SetActive(false);
    }


    //ショップコメント変更
    private void Set_Shop_Comment(string comment) {
        comment_Text.text = comment;
    }


    //アイテム購入、パワーの減少
    private bool Buy(Goods goods, int price) {
        if (player_Manager.Get_Power() < price) {
            Set_Shop_Comment(NOT_ENOUGH_COMMENT);
            return false;
        }

        switch (goods) {
            case Goods.life:    player_Manager.Add_Life(); Adjust_Life_Up_Price();      break;
            case Goods.stock:   player_Manager.Add_Stock();                             break;
            case Goods.bee:     player_Manager.Set_Option(PlayerManager.Option.bee);    break;
            case Goods.butterfly: player_Manager.Set_Option(PlayerManager.Option.butterfly); break;
            case Goods.mantis:  player_Manager.Set_Option(PlayerManager.Option.mantis); break;
            case Goods.spider:  player_Manager.Set_Option(PlayerManager.Option.spider); break;
        }

        player_Manager.Set_Power(player_Manager.Get_Power() - price);
        Set_Shop_Comment(THANKS_COMMENT);
        
        //効果音
        if (goods == Goods.stock)
            UsualSoundManager.Instance.Play_Stock_Up_Sound();
        else
            UsualSoundManager.Instance.Play_Life_Up_Sound();

        quit_Button.Select();

        return true;
    }

    // ==================================== ボタン関数 =========================================    

    public void Buy_Goods_Button(int goods_Number) {
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            Goods goods = (Goods)Enum.ToObject(typeof(Goods), goods_Number);

            int price = 0;
            if (goods == Goods.life)
                price = life_Up_Price;
            else if (goods == Goods.stock)
                price = STOCK_UP_PRICE;
            else
                price = OPTION_PRICE;

            Buy(goods, price);
        }
    }


    public void Quit_Button() {
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            Quit_Shopping();
        }
    }
}
