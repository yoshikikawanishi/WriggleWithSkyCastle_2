using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using MBLDefine;

public class ConfigButton : MonoBehaviour {

    [SerializeField] private GameObject jump_Button;
    [SerializeField] private GameObject attack_Button;
    [SerializeField] private GameObject ride_Button;
    [SerializeField] private GameObject slow_Button;
    [SerializeField] private GameObject shoot_Button;
    [SerializeField] private GameObject pause_Button;       
    
    private bool wait_Input = false;


    private void Start() {
        //ボタンテキストの変更
        Change_Button_Text(jump_Button, Key.Jump);
        Change_Button_Text(attack_Button, Key.Attack);
        Change_Button_Text(ride_Button, Key.Fly);
        Change_Button_Text(slow_Button, Key.Slow);
        Change_Button_Text(shoot_Button, Key.Shoot);
        Change_Button_Text(pause_Button, Key.Pause);
    }


    //テキストの変更
    private void Change_Button_Text(GameObject button, Key key) {
        InputManager.KeyConfigSetting key_Setting = InputManager.KeyConfigSetting.Instance;
        button.GetComponentInChildren<Text>().text 
            = key_Setting.GetKeyCode(key)[0].ToString()
            + "  /  "
            + key_Setting.GetKeyCode(key)[1].ToString().Replace("Joystick", "");
    }
 

    //入力待ち、コンフィグ変更
    private IEnumerator Change_Key_Config(Key changed_Key, GameObject button) {        
        InputManager.KeyConfigSetting key_Setting = InputManager.KeyConfigSetting.Instance;
        //色の変更
        button.GetComponent<Image>().color = new Color(1, 0.4f, 0.4f);
        //テキストの変更
        button.GetComponentInChildren<Text>().text = "・・・";
        //入力待ち
        wait_Input = true;
        yield return null;
        while (true) {
            button.GetComponent<Button>().Select();
            if (Input.anyKeyDown) {
                //矢印キーは受け付けない
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                    yield return null;
                    continue;
                }
                //押されたキーコードの取得
                KeyCode put_Button = new KeyCode();
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
                    if (Input.GetKeyDown(code)) {
                        put_Button = code;
                        break;
                    }
                }
                //コンフィグの変更
                //ゲームパッドのボタンが入力されたとき
                bool is_GamePad = false;
                for (int i = 0; i < 16; i++) {
                    if (Input.GetKeyDown("joystick button " + i.ToString())) {
                        key_Setting.SetKey(changed_Key, new List<KeyCode> { key_Setting.GetKeyCode(changed_Key)[0], put_Button });                        
                        is_GamePad = true;
                        break;
                    }
                }
                //キーボードのボタンが入力されたとき
                if (!is_GamePad) {
                    key_Setting.SetKey(changed_Key, new List<KeyCode> { put_Button, key_Setting.GetKeyCode(changed_Key)[1] });                    
                }
                //テキストの変更
                Change_Button_Text(button, changed_Key);
                break;
            }
            yield return null;
        }
        wait_Input = false;
        //色を戻す
        button.GetComponent<Image>().color = new Color(1, 1, 1);
        //保存する
        InputManager.Instance.keyConfig.SaveConfigFile();
        yield return null;
    }


    /*==================================== 以下、ボタン押下時の関数 =========================================*/

    /*
     if (!wait_Input && InputManager.Instance.GetKeyDown(Key.Jump)) {
            StartCoroutine(Change_Key_Config(Key.Jump, jump_Button));
     }         
    */

    //ジャンプ、決定ボタン
    public void Jump_Button() {
        if (!wait_Input && InputManager.Instance.GetKeyDown(Key.Jump)) {
            StartCoroutine(Change_Key_Config(Key.Jump, jump_Button));
        }
    }

    //攻撃ボタン
    public void Attack_Button() {
        if (!wait_Input && InputManager.Instance.GetKeyDown(Key.Jump)) {
            StartCoroutine(Change_Key_Config(Key.Attack, attack_Button));
        }
    }

    //飛行ボタン
    public void Ride_Button() {
        if (!wait_Input && InputManager.Instance.GetKeyDown(Key.Jump)) {
            StartCoroutine(Change_Key_Config(Key.Fly, ride_Button));
        }
    }

    //低速ボタン
    public void Slow_Button() {
        if (!wait_Input && InputManager.Instance.GetKeyDown(Key.Jump)) {
            StartCoroutine(Change_Key_Config(Key.Slow, slow_Button));
        }
    }

    //ショットボタン
    public void Shoot_Button() {
        if (!wait_Input && InputManager.Instance.GetKeyDown(Key.Jump)) {
            StartCoroutine(Change_Key_Config(Key.Shoot, shoot_Button));
        }
    }

    //ポーズボタン
    public void Pause_Button() {
        if (!wait_Input && InputManager.Instance.GetKeyDown(Key.Jump)) {
            StartCoroutine(Change_Key_Config(Key.Pause, pause_Button));
        }
    }


    //タイトルに戻る
    public void Back_Title_Button() {
        if (!wait_Input && InputManager.Instance.GetKeyDown(Key.Jump)) {            
            SceneManager.LoadScene("TitleScene");
        }
    }

    
}
