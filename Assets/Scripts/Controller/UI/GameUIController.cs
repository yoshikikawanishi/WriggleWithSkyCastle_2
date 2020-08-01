using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャンバスにアタッチすること
/// </summary>
public class GameUIController : MonoBehaviour {

    private enum AppearState {
        both,
        normal,
        flying
    }

    [SerializeField] private AppearState appear_State;
    [Space]
    [SerializeField] private Text score_Text;
    [SerializeField] private Text power_Text;
    [SerializeField] private Text stock_Text;
    [SerializeField] private GameObject life_Images_Parent;
    [SerializeField] private Slider beetle_Power_Slider;
    [SerializeField] private GameObject save_Text;
    [SerializeField] private Text option_Text;
    [Space]
    [SerializeField] private AnimationCurve beetle_Power_Converter = AnimationCurve.Linear(0, 0, 100, 100);

    private GameObject[] life_Images = new GameObject[9];

    private PlayerManager player_Manager;
    private BeetlePowerManager beetle_Power_Manager;

    private PlayerController player_Controller;

    private bool is_Appear = false;
    private int score_Text_Value = 0;
    private int power_Text_Value = 0;
    private int stock_Text_Value = 32;
    private int life_Image_Number = 0;
    private int beetle_Power_Slider_Value = 0;
    private PlayerManager.Option now_Option;

    private Image beetle_Power_Slider_Image;


	// Use this for initialization
	void Start () {
        //取得
        player_Manager = PlayerManager.Instance;
        beetle_Power_Manager = BeetlePowerManager.Instance;
        for (int i = 0; i < 9; i++) {
            life_Images[i] = life_Images_Parent.transform.GetChild(i).gameObject;
        }
        beetle_Power_Slider_Image = beetle_Power_Slider.transform.Find("Fill Area").GetComponentInChildren<Image>();

        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            player_Controller = player.GetComponent<PlayerController>();
        }


        //UI初期値
        if (!Is_Appear()) {
            Switch_Appearance(false);
        }
        Change_Power_UI();          //パワー
        Change_Score_UI();          //スコア
        Change_Stock_UI();          //ストック
        Change_Life_UI();           //ライフ
        Change_Beetle_Power_UI();   //カブトムシパワー
    }
	

	// Update is called once per frame
	void Update () {        
        //表示切替
        if(is_Appear != Is_Appear()) {
            is_Appear = Is_Appear();
            Switch_Appearance(is_Appear);
        }

        //UI更新
        if (Is_Appear()) {
            Change_Power_UI();          //パワー
            Change_Score_UI();          //スコア
            Change_Stock_UI();          //ストック
            Change_Life_UI();           //ライフ
            Change_Beetle_Power_UI();   //カブトムシパワー
            Change_Option_UI();         //オプション
        }
    }


    //表示するか否か
    private bool Is_Appear() {
        if (appear_State == AppearState.both)
            return true;
        if (player_Controller.Get_Is_Ride_Beetle() && appear_State == AppearState.flying)
            return true;
        else if (!player_Controller.Get_Is_Ride_Beetle() && appear_State == AppearState.normal)
            return true;

        return false;
    }


    //表示切替
    private void Switch_Appearance(bool is_Appear) {
        GetComponent<Canvas>().enabled = is_Appear;        
    }
  

    //パワー
    private void Change_Power_UI() {
        if(power_Text_Value != player_Manager.Get_Power()) {
            power_Text_Value = player_Manager.Get_Power();
            power_Text.text = (power_Text_Value * 0.01f).ToString("0.00") + "/4.00";
        }
    }


    //スコア
    private void Change_Score_UI() {
        if(score_Text_Value != player_Manager.Get_Score()) {
            score_Text_Value = player_Manager.Get_Score();
            score_Text.text = score_Text_Value.ToString("D" + 9.ToString());
        }
    }


    //ストックUIの変更
    private void Change_Stock_UI() {
        if(stock_Text_Value != player_Manager.Get_Stock()) {            
            stock_Text_Value = player_Manager.Get_Stock();
            stock_Text.text = "× " + stock_Text_Value.ToString();
        }
    }
    

    //ライフUI変更
    private void Change_Life_UI() {
        if(life_Image_Number == player_Manager.Get_Life()) {
            return;
        }
        life_Image_Number = player_Manager.Get_Life();
        for(int i = 0; i < life_Image_Number; i++) {
            life_Images[i].SetActive(true);
        }
        for(int i = life_Image_Number; i < 9; i++) {
            life_Images[i].SetActive(false);
        }
    }


    //カブトムシパワーUI変更
    private void Change_Beetle_Power_UI() {          
        //値の変更
        if (beetle_Power_Slider_Value != beetle_Power_Manager.Get_Beetle_Power()) {
            beetle_Power_Slider_Value = beetle_Power_Manager.Get_Beetle_Power();
            beetle_Power_Slider.value = beetle_Power_Converter.Evaluate(beetle_Power_Slider_Value);
            //色
            beetle_Power_Slider_Image.color = new Color(1, 1, 1, 0.8f);
            if (beetle_Power_Slider_Value >= 90)
                beetle_Power_Slider_Image.color = new Color(1, 1, 1, 1.0f);
            if (beetle_Power_Slider_Value <= 30f)
                beetle_Power_Slider_Image.color = new Color(1, 0.5f, 0.5f, 0.8f);
        }    
    }


    //オプションUI変更
    private void Change_Option_UI() {
        if (now_Option == player_Manager.Get_Option()) {
            return;            
        }
        now_Option = player_Manager.Get_Option();        

        switch (now_Option) {
            case PlayerManager.Option.none:         Set_Option_Text("none",     new Color(1.0f, 1.0f, 1.0f, 0.0f)); break;
            case PlayerManager.Option.bee:          Set_Option_Text("Bee",      new Color(0.9f, 0.3f, 0.0f, 0.5f)); break;
            case PlayerManager.Option.butterfly:    Set_Option_Text("Butterfly",new Color(0.8f, 1.0f, 0.0f, 0.5f)); break;
            case PlayerManager.Option.mantis:       Set_Option_Text("Mantis",   new Color(0.0f, 1.0f, 0.0f, 0.5f)); break;
            case PlayerManager.Option.spider:       Set_Option_Text("Spider",   new Color(0.0f, 0.0f, 1.0f, 0.5f)); break;
        }
    }
    
    private void Set_Option_Text(string text, Color outline_Color) {
        option_Text.text = "Option " + text;
        option_Text.GetComponent<Outline>().effectColor = outline_Color;
    }


    //セーブUIの点滅
    public void Display_Save_Text() {
        StartCoroutine("Blink_Save_Text_Cor");
    }

    private IEnumerator Blink_Save_Text_Cor() {
        for(int i = 0; i < 3; i++) {
            save_Text.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            save_Text.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }
    }

}
