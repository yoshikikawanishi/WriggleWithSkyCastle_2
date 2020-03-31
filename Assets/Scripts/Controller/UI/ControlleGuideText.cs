using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MBLDefine;

public class ControlleGuideText : MonoBehaviour {

    private Animator _anim;
    private Text _text;

    private bool is_Wait = true;
    private bool is_End_Guide = false;
    private int index = 0;

    [SerializeField] List<string> guide_Key_List = new List<string> {
        "Jump"
    };


    private void Awake() {
        _anim = GetComponent<Animator>();
        _text = GetComponent<Text>();       
    }


    private void Start() {
        StartCoroutine(Change_Guide_Cor(guide_Key_List[0]));
    }


    // Update is called once per frame
    void Update () {

        if (index >= guide_Key_List.Count || !is_Wait) {
            if(index == guide_Key_List.Count) {
                is_End_Guide = true;
                index++;
            }
            return;
        }

        if (InputManager.Instance.GetKeyDown(Get_Key(guide_Key_List[index]))) {
            index++;
            if (index < guide_Key_List.Count)
                StartCoroutine(Change_Guide_Cor(guide_Key_List[index]));
            else
                StartCoroutine(Change_Guide_Cor(null));
        }
	}


    //テキストを変更する
    private IEnumerator Change_Guide_Cor(string next_Key) {
        InputManager.KeyConfigSetting key_Setting = InputManager.KeyConfigSetting.Instance;

        is_Wait = false;
        _anim.SetTrigger("OutTrigger");

        yield return new WaitForSeconds(1.0f);

        if (next_Key == null)
            yield break;
        _text.text  = next_Key.ToString() + "\n"
                    + key_Setting.GetKeyCode(next_Key)[0].ToString()
                    + "  /  "
                    + key_Setting.GetKeyCode(next_Key)[1].ToString().Replace("Joystick", "");

        _anim.SetTrigger("InTrigger");

        yield return new WaitForSeconds(0.2f);
        is_Wait = true;
    
    }


    /// <summary>
    /// ガイドが終了した時を検知するよう
    /// </summary>
    /// <returns></returns>
    public bool End_Guide_Trigger() {
        if (is_End_Guide) {
            is_End_Guide = false;
            return true;
        }
        return false;
    }


    //文字列からKeyを取得する
    private Key Get_Key(string keyName) {
        foreach(Key key in Key.AllKeyData) {
            if (key.ToString() == keyName)
                return key;
        }
        return null;
    }


}
