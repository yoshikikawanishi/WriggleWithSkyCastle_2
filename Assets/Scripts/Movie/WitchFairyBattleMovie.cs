﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchFairyBattleMovie : MonoBehaviour {

    [SerializeField] private float battle_Camera_Pos;
    [SerializeField] private float return_Camera_Pos;
    [SerializeField] private Vector2 player_Initial_Pos;
    [SerializeField] private Vector2 player_Return_Pos;
    [SerializeField] private Vector2 enemy_Initial_Pos;
    [SerializeField] private GameObject erase_Bullet_Bomb;

    private GameObject battle_Enemy;
    private GameObject main_Camera;
    private GameObject player;
    private List<GameObject> erased_Enemies = new List<GameObject>();
    private MessageDisplay _message;


    void Awake() {
        main_Camera = GameObject.FindWithTag("MainCamera");
        player = GameObject.FindWithTag("PlayerTag");
        _message = GetComponent<MessageDisplay>();
        _message.Set_Canvas_And_Panel_Name("BattleEventCanvas", "BattleMessagePanel");
    }


    /// <summary>
    /// 戦闘開始時のムービー
    /// </summary>
    /// <param name="battle_Enemy">戦闘を行う妖精</param>
    public void Start_Battle_Movie(GameObject battle_Enemy) {
        if (battle_Enemy == null || !battle_Enemy.activeSelf)
            return;
        if (player == null)
            return;

        this.battle_Enemy = battle_Enemy;
        StartCoroutine("Battle_Movie_Cor");
    }

    private IEnumerator Battle_Movie_Cor() {
        //自機の動きを止める
        PlayerMovieFunction.Instance.Disable_Controlle_Player();
        //カメラ止める
        main_Camera.GetComponent<CameraController>().enabled = false;
        //画面内のアクティブな敵を消す
        erased_Enemies = Erase_Visible_Enemy();        

        //フェードアウト
        FadeInOut.Instance.Start_Rotate_Fade_Out();
        yield return new WaitForSeconds(1.0f);

        //自機の動きを止める
        PlayerMovieFunction.Instance.Disable_Controlle_Player();
        //初期位置調整
        main_Camera.transform.position = new Vector3(battle_Camera_Pos, 0, -10);
        player.transform.position = player_Initial_Pos;
        battle_Enemy.transform.position = enemy_Initial_Pos;

        //フェードイン
        FadeInOut.Instance.Delete_Fade_Out_Obj();

        //メッセージ
        Display_Message(1, 1);
        yield return new WaitForSeconds(2.0f);

        //戦闘開始
        PlayerMovieFunction.Instance.Enable_Controlle_Player();          
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

        //フェードアウト
        FadeInOut.Instance.Start_Fade_Out(new Color(0, 0, 0), 0.02f);
        yield return new WaitForSeconds(1.5f);        
        FadeInOut.Instance.Delete_Fade_Out_Obj();
        
        //敵の復活
        for(int i = 0; i < erased_Enemies.Count; i++) {
            erased_Enemies[i].SetActive(true);
        }
        erased_Enemies.Clear();

        //操作可能化
        if (player != null)
            player.transform.position = player_Return_Pos;
        main_Camera.GetComponent<CameraController>().enabled = true;
        main_Camera.transform.position = new Vector3(return_Camera_Pos, 0, -10);
        _message.Quit_Message();

        Destroy(gameObject);
    }


    /// <summary>
    /// メッセージ表示
    /// </summary>
    /// <returns></returns>
    public void Display_Message(int start_ID, int end_ID) {
        _message.Start_Display_Auto("WitchFairyText", start_ID, end_ID, 1.0f, 0.05f);
    }


    //付近のアクティブな敵を消す, 戦闘する敵は除く
    //消した敵を返す
    private List<GameObject> Erase_Visible_Enemy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyTag");
        List<GameObject> active_Enemies = new List<GameObject>();
        float camera_Distance;

        foreach (GameObject enemy in enemies) {
            if (!enemy.activeSelf || enemy == battle_Enemy)
                continue;

            camera_Distance = battle_Camera_Pos - enemy.transform.position.x;           
            if (Mathf.Abs(camera_Distance) < 350f) {
                active_Enemies.Add(enemy);
                enemy.SetActive(false);
            }
        }

        return active_Enemies;
    }
}
