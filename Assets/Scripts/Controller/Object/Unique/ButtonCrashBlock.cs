using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBLDefine;


public class ButtonCrashBlock : MonoBehaviour {

    //ボタン種類
    private enum ButtonKind {
        jump = 0,
        attack = 1,
        fly = 2,
    }
    private const int BUTTONNUM = 3;
    
    //アイコンの色
    private readonly Color dark_Color = new Color(0.5f, 0.5f, 0.5f);
    private readonly Color light_Color = new Color(1, 1, 1);
    
    //アイコンスプライト
    [SerializeField] private Sprite icon_Jump;
    [SerializeField] private Sprite icon_Attack;
    [SerializeField] private Sprite icon_Fly;
    [SerializeField] private SpriteRenderer icon_Sprite;
    [Space]
    //要求ボタンの設定用
    [SerializeField] private int require_Button_Count = 3;
    private List<ButtonKind> require_Button_List = new List<ButtonKind>();
    
    //ボタンアイコン配列化用
    private Sprite[] icon_Textures = new Sprite[BUTTONNUM];
    
    //その他クラス
    private PlayerController player_Controller;
    private ChildColliderTrigger detection;

    private bool is_Player_Nearly = true;


    void Awake() {
        //初期設定
        icon_Textures[(int)ButtonKind.jump] = icon_Jump;
        icon_Textures[(int)ButtonKind.attack] = icon_Attack;
        icon_Textures[(int)ButtonKind.fly] = icon_Fly;
        for(int i = 0; i < require_Button_Count; i++) {
            switch(Random.Range(0, 3)) {
                case 0: require_Button_List.Add(ButtonKind.attack); break;
                case 1: require_Button_List.Add(ButtonKind.jump); break;
                case 2: require_Button_List.Add(ButtonKind.fly); break;
            }
        }

        StartCoroutine(Display_Required_Button_Sprite(dark_Color));
    }


    void Start() {
        //取得
        player_Controller = GameObject.FindWithTag("PlayerTag").GetComponent<PlayerController>();
        detection = GetComponentInChildren<ChildColliderTrigger>();
    }
    
    
    void Update() {
        //自機が近くにいるとき
        if (detection.Hit_Trigger()) {
            Accept_Input();
            //近づいた瞬間
            if (!is_Player_Nearly) {
                is_Player_Nearly = true;
                Do_Process_Approach_Player();
            }
        }
        //自機が離れているとき
        else {
            //離れた瞬間
            if (is_Player_Nearly) {
                is_Player_Nearly = false;
                Do_Process_Depart_Player();
            }
        }
    }    


    //自機が近付いた瞬間の処理
    private void Do_Process_Approach_Player() {
        icon_Sprite.color = light_Color;            //色
        player_Controller.Set_Can_Action(false);    //自機のアクション無効化
    }


    //自機が離れた瞬間の処理
    private void Do_Process_Depart_Player() {
        icon_Sprite.color = dark_Color;             //色
        player_Controller.Set_Can_Action(true);     //自機のアクション無効化
    }


    //入力受付
    private void Accept_Input() {
        if (require_Button_List.Count == 0)
            return;

        Key require_Key = Get_Key(require_Button_List[0]);
        if (InputManager.Instance.GetKeyDown(require_Key)) {
            Action_In_Input();
        }
    }


    //要求ボタンを入力されたときの処理
    private void Action_In_Input() {
        require_Button_List.RemoveAt(0);
        GetComponent<AudioSource>().Play();
        //ボタン変更
        if (require_Button_List.Count > 0) {
            StartCoroutine(Display_Required_Button_Sprite(new Color(1, 1, 1, 1)));
            GetComponent<ObjectShake>().Shake(0.2f, new Vector2(1, 0), true);
        }
        //消滅
        else {
            UsualSoundManager.Instance.Play_Shoot_Sound();
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().SetTrigger("CrashTrigger");
            Invoke("Do_Process_Depart_Player", 0.1f);
            Destroy(gameObject, 0.6f);
        }
    }



    //ButtonKindを入力ボタンに変換する
    private Key Get_Key(ButtonKind kind) {
        if (kind == ButtonKind.jump)
            return Key.Jump;
        if (kind == ButtonKind.attack)
            return Key.Attack;
        if (kind == ButtonKind.fly)
            return Key.Fly;
        return null;
    }


    //入力待ちのボタンを表示する
    private IEnumerator Display_Required_Button_Sprite(Color next_Color) {
        for (int i = 0; i < BUTTONNUM; i++) {            
            if (i == (int)require_Button_List[0]) {
                icon_Sprite.sprite = icon_Textures[i];
            }
        }
        icon_Sprite.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.1f);
        icon_Sprite.color = next_Color;
    }

}
