using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MBLDefine;

public class ControlleGuideText : MonoBehaviour {

    private enum Action {
        Jump,
        Attack,
        Kick,
        Pause,
        Fly,
        Shoot,
        Slow,
    }

    //ガイドに表示するためのクラス
    private class Guide {
        public Action action;
        public Key key;
        public string comment;

        public Guide(Action action, Key key, string comment) {
            this.action = action;
            this.key = key;
            this.comment = comment;
        }
        
        public override string ToString() {
            string s = "・"
                + action.ToString()
                + "\n"
                + "[ "
                + key.DefaultKeyCode[0]
                + " / "
                + key.DefaultKeyCode[1].ToString().Substring(9)                
                + comment
                + " ]";                
            return s;
        }
    }

    private Animator _anim;
    private Text _text;

    private bool is_Wait = true;
    private bool is_End_Guide = false;
    private int index = 0;

    [SerializeField] private List<Action> action_List;
    private List<Guide> guide_List = new List<Guide>();
    

    void Awake() {
        //取得
        _anim = GetComponent<Animator>();
        _text = GetComponent<Text>();

        //guide_Listにaction_Listに設定された値を代入
        Set_Guide_List();
    }


    void Start() {
        StartCoroutine(Change_Guide_Cor(guide_List[0]));
    }


    void Update () {
        //最後まで行ったら終了
        if (index >= guide_List.Count || !is_Wait) {
            if(index == guide_List.Count) {
                is_End_Guide = true;
                index++;
            }
            return;
        }
        //次のガイドを表示
        if (InputManager.Instance.GetKeyDown(guide_List[index].key)) {
            index++;
            if (index < guide_List.Count)
                StartCoroutine(Change_Guide_Cor(guide_List[index]));
            else
                StartCoroutine(Change_Guide_Cor(null));
        }
	}


    //テキストを変更する
    private IEnumerator Change_Guide_Cor(Guide guide) {
        InputManager.KeyConfigSetting key_Setting = InputManager.KeyConfigSetting.Instance;

        is_Wait = false;
        _anim.SetTrigger("OutTrigger");

        yield return new WaitForSeconds(1.0f);

        if (guide == null)
            yield break;
        _text.text = guide.ToString();

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


    //ActionからGuideを設定
    private void Set_Guide_List() {
        foreach(Action a in action_List) {
            switch (a) {
                case Action.Jump:   guide_List.Add(new Guide(a, Key.Jump, ""));         break;
                case Action.Attack: guide_List.Add(new Guide(a, Key.Attack, ""));       break;
                case Action.Kick:   guide_List.Add(new Guide(a, Key.Attack, " + ↓"));  break;
                case Action.Fly:    guide_List.Add(new Guide(a, Key.Fly, ""));          break;
                case Action.Shoot:  guide_List.Add(new Guide(a, Key.Shoot, ""));        break;
                case Action.Slow:   guide_List.Add(new Guide(a, Key.Slow, ""));         break;
                case Action.Pause:  guide_List.Add(new Guide(a, Key.Pause, ""));        break;
            }
        }
    }

}
