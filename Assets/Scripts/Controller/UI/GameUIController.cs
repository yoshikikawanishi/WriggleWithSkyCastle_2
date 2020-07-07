using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour {

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

        //UI初期値
        Change_Player_UI(score_Text, 9, player_Manager.Get_Score(), score_Text_Value); //スコア
        Change_Player_UI(power_Text, 3, player_Manager.Get_Power(), power_Text_Value); //パワー
        Change_Stock_UI();          //ストック
        Change_Life_UI();           //ライフ
        Change_Beetle_Power_UI();   //カブトムシパワー
    }
	

	// Update is called once per frame
	void Update () {
        Change_Player_UI(score_Text, 9, player_Manager.Get_Score(), score_Text_Value); //スコア
        Change_Player_UI(power_Text, 3, player_Manager.Get_Power(), power_Text_Value); //パワー
        Change_Stock_UI();          //ストック
        Change_Life_UI();           //ライフ
        Change_Beetle_Power_UI();   //カブトムシパワー
        Change_Option_UI();         //オプション
    }


    //テキストUIの変更
    private void Change_Player_UI(Text text, int digit, int value, int text_Value) {
        if(value != text_Value) {
            text_Value = value;
            text.text = value.ToString("D" + digit.ToString());
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
        //増加時エフェクト
        if (beetle_Power_Slider_Value < beetle_Power_Manager.Get_Beetle_Power()) {            
            beetle_Power_Slider.GetComponent<ParticleSystem>().Play();
        }        
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
