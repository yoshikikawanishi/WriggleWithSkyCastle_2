using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WitchFairyBattleMovie : SingletonMonoBehaviour<WitchFairyBattleMovie> {

    [SerializeField] private GameObject select_Panel;
    [SerializeField] private GameObject erase_Bullet_Bomb;

    private float camera_Initial_Pos = 1200f;    
    private Vector2 player_Initial_Pos = new Vector2(1300f, -51f);
    private float player_Initial_Direction = -1;
    private Vector2 enemy_Initial_Pos = new Vector2(1100f, 32f);
    private float enemy_Initial_Direction = -1;

    private Vector2 player_Return_Pos;
    private float camera_Return_Pos;
    private Vector3 enemy_Localscale;

    private WitchFairy battle_Enemy;
    private GameObject main_Camera;
    private GameObject player;    
    private MessageDisplay _message;


    new void Awake() {
        base.Awake();
        main_Camera = GameObject.FindWithTag("MainCamera");
        player = GameObject.FindWithTag("PlayerTag");
        _message = GetComponent<MessageDisplay>();
        _message.Set_Canvas_And_Panel_Name("BattleEventCanvas", "BattleMessagePanel");
    }


    /// <summary>
    /// 戦闘開始時のムービー
    /// </summary>
    /// <param name="battle_Enemy">戦闘を行う妖精</param>
    public void Play_Start_Battle_Movie(WitchFairy battle_Enemy) {
        if (battle_Enemy == null)
            return;
        if (player == null)
            return;

        this.battle_Enemy = battle_Enemy;
        enemy_Localscale = battle_Enemy.transform.localScale;

        EventSystem.current.SetSelectedGameObject(null);

        StartCoroutine("Play_Start_Battle_Movie_Cor");
    }


    private IEnumerator Play_Start_Battle_Movie_Cor() {
        //自機の動きを止める
        PlayerMovieFunction.Instance.Disable_Controlle_Player();
        //カメラ止める
        main_Camera.GetComponent<CameraController>().enabled = false;           

        //フェードアウト
        FadeInOut.Instance.Start_Rotate_Fade_Out();
        yield return new WaitForSeconds(1.0f);

        //自機の動きを止める
        PlayerMovieFunction.Instance.Disable_Controlle_Player();
        //戦闘開始前の位置を保存
        camera_Return_Pos = main_Camera.transform.position.x;
        player_Return_Pos = player.transform.position;
        //初期位置調整
        main_Camera.transform.position = new Vector3(camera_Initial_Pos, 0, -10);
        player.transform.position = player_Initial_Pos;
        player.transform.localScale = new Vector3(1 * player_Initial_Direction, 1, 1);
        battle_Enemy.transform.position = enemy_Initial_Pos;
        battle_Enemy.transform.localScale = new Vector3(enemy_Initial_Direction, 1, 1) * enemy_Localscale.y;

        //フェードイン
        FadeInOut.Instance.Delete_Fade_Out_Obj();

        Display_Message(1, 1);
        yield return new WaitUntil(_message.End_Message);
        Play_Battle_Movie();             
    }


    /// <summary>
    /// 戦闘中のムービー
    /// </summary>
    public void Play_Battle_Movie() {
        //自機の動きを止める
        PlayerMovieFunction.Instance.Disable_Controlle_Player();
        //選択肢表示
        select_Panel.SetActive(true);
        //カーソル
        select_Panel.GetComponentInChildren<Button>().Select();
    }
    


    /// <summary>
    /// 戦闘終了時のムービー
    /// </summary>
    public void Finish_Battle() {
        StartCoroutine("Finish_Battle_Cor");
    }
    
    private IEnumerator Finish_Battle_Cor() {
        //弾消し
        var bomb = Instantiate(erase_Bullet_Bomb, main_Camera.transform);
        bomb.transform.position = main_Camera.transform.position;
        Destroy(bomb, 1.5f);

        //メッセージ
        Display_Message(4, 5);
        yield return new WaitUntil(_message.End_Message);

        //フェードアウト
        FadeInOut.Instance.Start_Fade_Out(new Color(0, 0, 0), 0.02f);
        yield return new WaitForSeconds(1.5f);        
        FadeInOut.Instance.Delete_Fade_Out_Obj();                

        //操作可能化、位置を戻す        
        PlayerMovieFunction.Instance.Enable_Controlle_Player();       
        main_Camera.GetComponent<CameraController>().enabled = true;
        main_Camera.transform.position = new Vector3(camera_Return_Pos, 0, -10);
        if (player != null)
            player.transform.position = player_Return_Pos;
        _message.Quit_Message();
        select_Panel.SetActive(false);
    }


    /// <summary>
    /// メッセージ表示
    /// </summary>
    /// <returns></returns>
    public void Display_Message(int start_ID, int end_ID) {
        _message.Start_Display_Auto("WitchFairyText", start_ID, end_ID, 1.0f, 0.05f, true);
    }



    // ===================================== ボタン関数 ========================================

    public void Fight_Button() {
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            PlayerMovieFunction.Instance.Enable_Controlle_Player();
            Display_Message(2, 2);
            if (battle_Enemy != null) {
                battle_Enemy.StartCoroutine("Attack_Cor");
            }
            EventSystem.current.SetSelectedGameObject(null);
            select_Panel.SetActive(false);
        }
    }


    public void Escape_Button() {
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            PlayerMovieFunction.Instance.Enable_Controlle_Player();
            Display_Message(11, 11);
            if (battle_Enemy != null) {
                battle_Enemy.StartCoroutine("Attack_Cor");
            }
            EventSystem.current.SetSelectedGameObject(null);
            select_Panel.SetActive(false);
        }
    }
   
}
