using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MovieSystem : MonoBehaviour {

    [System.Serializable]
    public class Event {
        public string name;
        public enum Type {
            message,
            motion,
            genObj,
            function,
            wait,
        }
        public Type type = Type.message;

        public Message message = new Message();
        public Motion motion = new Motion();
        public GenObject gen_Obj = new GenObject();
        public Function function = new Function();
        public Wait wait = new Wait();
    }

    [System.Serializable]
    public class Message {
        public string file_Name;
        public bool dual_Panel = true;
        public int start_ID;
        public int end_ID;
        public bool is_Auto;        
    }

    [System.Serializable]
    public class Motion {
        public GameObject obj;
        public string anim_Parameter;
        public AnimationCurve x_Move = AnimationCurve.Constant(0, 1, 0);
        public AnimationCurve y_Move = AnimationCurve.Constant(0, 1, 0);
    }

    [System.Serializable]
    public class GenObject {
        public GameObject obj;
        public GameObject parent;
        public Vector2 position;
    }

    [System.Serializable]
    public class Function {
        public MonoBehaviour component;
        public string function;
    }

    [System.Serializable]
    public class Wait {
        public bool wait_Message;
        public float wait_Time;
    }

    public bool is_Disable_Controlle = true;
    public List<Event> list = new List<Event>();

    private MessageDisplay _message;
    private MessageDisplayCustom _message_Custom;    
    private bool is_End_Movie;


    void Awake() {
        _message = GetComponent<MessageDisplay>();
        _message_Custom = GetComponent<MessageDisplayCustom>();
        if(_message == null)
            _message = gameObject.AddComponent<MessageDisplay>();
        if (_message_Custom == null)
            _message_Custom = gameObject.AddComponent<MessageDisplayCustom>();
    }

    
    //ムービーを開始する
    public void Start_Movie() {
        StartCoroutine("Movie_Cor");
    }

    private IEnumerator Movie_Cor() {
        //初期設定
        if (is_Disable_Controlle) {
            PlayerMovieFunction.Instance.Disable_Controlle_Player();            
        }
        PauseManager.Instance.Set_Is_Pausable(false);

        //イベントをリスト順にこなす
        for (int i = 0; i < list.Count; i++) {
            Event e = list[i];
            switch (e.type) {
                case Event.Type.message:
                    Start_Message(e);
                    break;
                case Event.Type.motion:
                    StartCoroutine("Object_Move_Cor", e);
                    Play_Animation(e);
                    break;
                case Event.Type.genObj:
                    Generate_Object(e);
                    break;
                case Event.Type.function:
                    e.function.component.Invoke(e.function.function, 0);
                    break;
                case Event.Type.wait:
                    if (e.wait.wait_Message) {                        
                        yield return new WaitUntil(Is_End_Message);
                    }
                    yield return new WaitForSeconds(e.wait.wait_Time);
                    break;
            }
        }

        //終了設定
        if (is_Disable_Controlle) {
            PlayerMovieFunction.Instance.Enable_Controlle_Player();            
        }
        PauseManager.Instance.Set_Is_Pausable(true);
        is_End_Movie = true;
    }

    //ムービー終了
    public bool End_Movie() {
        if (is_End_Movie) {
            is_End_Movie = false;
            return true;
        }
        return false;
    }


    //メッセージ表示
    private void Start_Message(Event e) {
        if (e.message.is_Auto) {
            if(e.message.dual_Panel)
                _message_Custom.Start_Display_Auto(e.message.file_Name, e.message.start_ID, e.message.end_ID, 1.0f, 0.02f);
            else
                _message.Start_Display_Auto(e.message.file_Name, e.message.start_ID, e.message.end_ID, 1.0f, 0.02f);
        }
        else {
            if(e.message.dual_Panel)
                _message_Custom.Start_Display(e.message.file_Name, e.message.start_ID, e.message.end_ID);
            else
                _message.Start_Display(e.message.file_Name, e.message.start_ID, e.message.end_ID);
        }        
    }


    //メッセージ表示終了検知用
    private bool Is_End_Message() {
        if (_message.End_Message() || _message_Custom.End_Message())
            return true;
        return false;
    }


    //オブジェクトの移動を行う
    private IEnumerator Object_Move_Cor(Event e) {
        GameObject obj = e.motion.obj;
        AnimationCurve x_Curve = e.motion.x_Move;
        AnimationCurve y_Curve = e.motion.y_Move;
        if (x_Curve == null || y_Curve == null)
            yield break;

        Vector2 root_Pos = obj.transform.position;
        float xt = x_Curve.keys[x_Curve.keys.Length - 1].time;
        float yt = y_Curve.keys[y_Curve.keys.Length - 1].time;
        float end_Time = Mathf.Max(xt, yt);

        for (float t = 0; t < end_Time; t += 0.016f + Time.deltaTime) {
            obj.transform.position = root_Pos + new Vector2(x_Curve.Evaluate(t), y_Curve.Evaluate(t));
            yield return new WaitForSeconds(0.016f);
        }
    }

    //オブジェクトのアニメーション変更
    //Bool型パラメータはBool, Trigger型パラメータはTriggerが付いていること限定
    private void Play_Animation(Event e) {
        if (e.motion.anim_Parameter == "")
            return;
        Animator anim = e.motion.obj.GetComponent<Animator>();
        if (anim == null)
            return;
        
        string parameter = e.motion.anim_Parameter;        
        for (int i = 0; i < anim.parameters.Length; i++) {
            string param = anim.parameters[i].name;            
            if (param.Contains("Bool")) {
                anim.SetBool(param, false);                
            }
        }
        if (parameter.Contains("Bool")) {
            anim.SetBool(parameter, true);
        }
    }


    //オブジェクトの生成
    private void Generate_Object(Event e) {
        GameObject obj = Instantiate(e.gen_Obj.obj);
        obj.transform.SetParent(e.gen_Obj.parent.transform);
        obj.transform.localPosition = e.gen_Obj.position;
    }


    #region EditorGUI用
    public void Add_Event(int index) {
        list.Insert(index, new Event());
    }

    public void Remove_Event(int index) {
        list.RemoveAt(index);
    }
    #endregion
   
}
