using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MBLDefine;

public class PauseManager : SingletonMonoBehaviour<PauseManager> {

    private GameObject pause_Canvas_Prefab;
    private GameObject pause_Canvas;
    private GameObject setting_Canvas_Prefab;
    private GameObject setting_Canvas;

    public enum STATE {
        normal,
        pause,
    }
    private STATE state = STATE.normal;

    private float time_Scale_Before_Pause = 1.0f;

    private bool is_Pausable = true;


	// Use this for initialization
	void Start () {        
        //シーン読み込みのデリケート
        SceneManager.sceneLoaded += OnSceneLoaded;
        //取得
        pause_Canvas_Prefab = Resources.Load("UI/PauseCanvas") as GameObject;
        setting_Canvas_Prefab = Resources.Load("UI/SettingCanvas") as GameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (!is_Pausable || !SceneManagement.Instance.Is_Game_Scene()) {
            return;
        }
        if (InputManager.Instance.GetKeyDown(Key.Pause)) {            
            if(state == STATE.normal) {
                Pause_Game();
            }
            else if(state == STATE.pause) {
                Release_Pause_Game();
            }
        }
	}


    //シーン読み込み時に状態を戻す
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {        
        state = STATE.normal;
        Time.timeScale = 1;
        Set_Is_Pausable(true);
    }

    
    /// <summary>
    /// ポーズ、時間の停止、ポーズ画面の生成
    /// </summary>
    private void Pause_Game() {
        if(Time.timeScale == 0) {
            Debug.Log("Can't_Pause");
            return;
        }

        state = STATE.pause;
        time_Scale_Before_Pause = Time.timeScale;
        Time.timeScale = 0;

        //自機の操作無効化
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if(player != null)
            player.GetComponent<PlayerController>().Set_Is_Playable(false);        
        //ポーズキャンバスの生成
        if(pause_Canvas == null) {
            pause_Canvas = Instantiate(pause_Canvas_Prefab);
        }
        pause_Canvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);       
        pause_Canvas.GetComponentsInChildren<Button>()[0].Select();
        //効果音
        UsualSoundManager.Instance.Play_Pause_In_Sound();
    }

    
    /// <summary>
    /// ポーズ解除、時間再開、ポーズキャンバスをなくす
    /// </summary>
    public void Release_Pause_Game() {
        StartCoroutine("Release_Pause_Game_Cor");
    }

    private IEnumerator Release_Pause_Game_Cor() {
        yield return null;

        state = STATE.normal;
        Time.timeScale = time_Scale_Before_Pause;

        //自機の操作有効化
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if(player != null) 
            player.GetComponent<PlayerController>().Set_Is_Playable(true);        
        //ポーズキャンバスを消す
        if (pause_Canvas != null)
            pause_Canvas.SetActive(false);
        if (setting_Canvas != null)
            setting_Canvas.SetActive(false);
        //効果音
        UsualSoundManager.Instance.Play_Pause_Out_Sound();
    }


    /// <summary>
    /// 設定画面を出す、ポーズ画面を隠す
    /// </summary>
    public void Display_Setting_Canvas() {
        //ポーズ画面を隠す
        if (pause_Canvas != null)
            pause_Canvas.SetActive(false);
        
        if(setting_Canvas == null) {
            setting_Canvas = Instantiate(setting_Canvas_Prefab);
        }
        setting_Canvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        setting_Canvas.GetComponentsInChildren<Button>()[0].Select();
    }


    /// <summary>
    /// 設定画面を終わる、ポーズ画面を出す
    /// </summary>
    public void Hide_Setting_Canvas() {
        if (setting_Canvas != null)
            setting_Canvas.SetActive(false);

        //ポーズキャンバスの生成
        if (pause_Canvas == null) {
            pause_Canvas = Instantiate(pause_Canvas_Prefab);
        }
        pause_Canvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        pause_Canvas.GetComponentsInChildren<Button>()[0].Select();
    }


    //Setter
    public void Set_Is_Pausable(bool is_Pausable) {
        this.is_Pausable = is_Pausable;
    }
}
