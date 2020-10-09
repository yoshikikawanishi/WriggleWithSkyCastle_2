using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MBLDefine;

public class AudioConfigButton : MonoBehaviour {

    [SerializeField] private Button BGM_Button;
    [SerializeField] private Button SE_Button;

    //設定するときの効果音
    [SerializeField] private AudioSource cirsol_Sound;

    private bool can_Select = true;    

	
	void Start () {
        //初期設定
        AudioVolumeManager _manager = AudioVolumeManager.Instance;
        Text text = BGM_Button.GetComponentInChildren<Text>();
        text.text = ((int)_manager.Get_Volume(AudioVolumeManager.AudioGroup.BGM)).ToString();
        text = SE_Button.GetComponentInChildren<Text>();
        text.text = ((int)_manager.Get_Volume(AudioVolumeManager.AudioGroup.SE)).ToString();
    }
	

    //BGMボタン選択
    public void Select_BGM_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump) && can_Select) {
            StartCoroutine(Do_Volume_Setting_Cor(BGM_Button, AudioVolumeManager.AudioGroup.BGM));
        }
    }

    //SEボタン選択
    public void Select_SE_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump) && can_Select) {
            StartCoroutine(Do_Volume_Setting_Cor(SE_Button, AudioVolumeManager.AudioGroup.SE));
        }
    }


    /// <summary>
    /// ボリューム設定ボタン押下時に呼ばれる。ボリュームの設定を開始する。
    /// </summary>
    /// <param name="button"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    private IEnumerator Do_Volume_Setting_Cor(Button button, AudioVolumeManager.AudioGroup group) {
        AudioVolumeManager _manager = AudioVolumeManager.Instance;
        Text button_Text = button.GetComponentInChildren<Text>();

        yield return null;      //ボタン選択と同じフレームで呼び出すと、Key.Jump入力が入ってしまうため待つ
        
        //色を変える
        button.transform.Find("HorizonArrow").GetComponent<Image>().color = new Color(1, 1, 1, 1);
        //ボタン無効化
        can_Select = false;
        EventSystem.current.SetSelectedGameObject(null);

        while (true) {
            //ボリュームアップ
            if (Input.GetAxisRaw("Horizontal") > 0) {
                _manager.Increase_Volume(group);
                button_Text.text = ((int)_manager.Get_Volume(group)).ToString();
                cirsol_Sound.Play();
                yield return new WaitForSecondsRealtime(0.1f);
            }
            //ボリュームダウン
            if (Input.GetAxisRaw("Horizontal") < 0) {
                _manager.Decrease_Volume(group);
                button_Text.text = ((int)_manager.Get_Volume(group)).ToString();
                cirsol_Sound.Play();
                yield return new WaitForSecondsRealtime(0.1f);
            }

            //決定ボタンで戻る
            if (InputManager.Instance.GetKeyDown(Key.Jump)) {
                break;
            }

            yield return null;
        }
        
        //終了設定
        can_Select = true;
        button.transform.Find("HorizonArrow").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        EventSystem.current.SetSelectedGameObject(button.gameObject);
        _manager.Save_Volume_Setting();
    }
}
