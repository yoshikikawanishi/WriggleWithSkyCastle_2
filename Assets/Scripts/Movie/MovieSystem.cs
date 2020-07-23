using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MovieSystem : MonoBehaviour {

    /// <summary>
    ///　ムービーの構成単位
    ///　Typeで種類を選択
    /// </summary>
    [System.Serializable]
    public class Event {
        public string name;
        public enum Type {
            message,
            motion,
            animation,
            localScale,
            genObj,
            function,
            wait,
        }
        public Type type = Type.message;

        //それぞれのパラメータの箱を全部用意してるのはよくないけど、いい方法思いつかん
        public Message message = new Message();
        public Motion motion = new Motion();
        public Animation animation = new Animation();
        public LocalScale localScale = new LocalScale();
        public GenObject gen_Obj = new GenObject();
        public Function function = new Function();
        public Wait wait = new Wait();

        public bool is_Open_In_Inspector = true;

    }

    /// <summary>
    /// メッセージ表示のパラメータ
    /// </summary>
    [System.Serializable]
    public class Message {
        public string file_Name;
        public bool dual_Panel = true;
        public Vector2Int id;
        public bool is_Auto;        
    }

    /// <summary>
    /// オブジェクトの移動用パラメータ
    /// </summary>
    [System.Serializable]
    public class Motion {
        public Transform obj;
        public bool apply_Root_Position;
        public AnimationCurve x_Move = AnimationCurve.Constant(0, 1, 0);
        public AnimationCurve y_Move = AnimationCurve.Constant(0, 1, 0);
    }

    /// <summary>
    /// アニメーション変更のパラメータ
    /// </summary>
    [System.Serializable]
    public class Animation {
        public Animator animator;
        public enum ParameterKind {
            boolean,
            trigger,
        }
        public ParameterKind kind;
        public string parameter;
        public bool boolean;
    }
   
    /// <summary>
    /// ローカルスケールの変更用パラメータ
    /// </summary>
    [System.Serializable]
    public class LocalScale {
        public Transform obj;
        public enum Kind {
            direct,
            curve
        }
        public Kind kind;
        public Vector2 scale;
        public AnimationCurve x_Curve = AnimationCurve.Constant(0, 1, 1);
        public AnimationCurve y_Curve = AnimationCurve.Constant(0, 1, 1);
    }

    /// <summary>
    /// オブジェクト生成のパラメータ
    /// </summary>
    [System.Serializable]
    public class GenObject {
        public GameObject obj;
        public GameObject parent;
        public Vector2 position;
    }

    /// <summary>
    /// 関数呼び出しのパラメータ
    /// </summary>
    [System.Serializable]
    public class Function {
        public GameObject obj;
        public MonoBehaviour component;
        public string function_Name;
    }

    /// <summary>
    /// 待機のパラメータ
    /// </summary>
    [System.Serializable]
    public class Wait {
        public bool wait_Message;
        public float wait_Time;
    }

    //ムービー開始時に自機の操作を無効にするかどうか
    public bool is_Disable_Controlle = true;
    //ムービーの構成単位をリストで並べて順次実行する
    public List<Event> list = new List<Event>();

    //コンポーネント
    private MessageDisplay _message;    //１パネルのメッセージ表示コンポーネント
    private MessageDisplayCustom _message_Custom;      //２パネルのメッセージ表示用コンポーネント

    //ムービー全体の終了を検知する用
    private bool is_End_Movie;


    void Awake() {
        //取得
        _message = GetComponent<MessageDisplay>();
        _message_Custom = GetComponent<MessageDisplayCustom>();
        if(_message == null)
            _message = gameObject.AddComponent<MessageDisplay>();
        if (_message_Custom == null)
            _message_Custom = gameObject.AddComponent<MessageDisplayCustom>();
    }

    
    /// <summary>
    /// ムービー開始
    /// </summary>
    public void Start_Movie() {
        StartCoroutine("Movie_Cor");
    }


    //ムービー本体
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
                    break;
                case Event.Type.animation:
                    Change_Animation_Parameter(e);
                    break;
                case Event.Type.localScale:
                    StartCoroutine("Change_Local_Scale_Cor");
                    break;
                case Event.Type.genObj:
                    Generate_Object(e);
                    break;
                case Event.Type.function:
                    Invoke_Function(e);
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


    //外からムービー終了検知する用
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
                _message_Custom.Start_Display_Auto(e.message.file_Name, e.message.id.x, e.message.id.y, 1.0f, 0.02f);
            else
                _message.Start_Display_Auto(e.message.file_Name, e.message.id.x, e.message.id.y, 1.0f, 0.02f);
        }
        else {
            if(e.message.dual_Panel)
                _message_Custom.Start_Display(e.message.file_Name, e.message.id.x, e.message.id.y);
            else
                _message.Start_Display(e.message.file_Name, e.message.id.x, e.message.id.y);
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
        Transform obj = e.motion.obj;
        AnimationCurve x_Curve = e.motion.x_Move;
        AnimationCurve y_Curve = e.motion.y_Move;
        if (x_Curve == null || y_Curve == null)
            yield break;

        Vector2 root_Pos = Vector2.zero;
        if(e.motion.apply_Root_Position)
            root_Pos = obj.position;
        float xt = x_Curve.keys[x_Curve.keys.Length - 1].time;
        float yt = y_Curve.keys[y_Curve.keys.Length - 1].time;
        float end_Time = Mathf.Max(xt, yt);

        for (float t = 0; t < end_Time; t += Time.deltaTime) {
            obj.position = root_Pos + new Vector2(x_Curve.Evaluate(t), y_Curve.Evaluate(t));
            yield return null;
        }
    }    


    //アニメーションパラメータの変更
    private void Change_Animation_Parameter(Event e) {
        Animator anim = e.animation.animator;
        if(e.animation.kind == Animation.ParameterKind.boolean) {
            anim.SetBool(e.animation.parameter, e.animation.boolean);
        }
        else {
            anim.SetTrigger(e.animation.parameter);
        }        
    }


    //ローカルスケール変更用
    private IEnumerator Change_Local_Scale_Cor(Event e) {
        Transform obj = e.localScale.obj;        
        if(e.localScale.kind == LocalScale.Kind.direct) {
            Vector2 scale = e.localScale.scale;
            obj.localScale = new Vector3(scale.x, scale.y, 1);
        }
        else {
            AnimationCurve x_Curve = e.localScale.x_Curve;
            AnimationCurve y_Curve = e.localScale.y_Curve;
            float xt = x_Curve.keys[x_Curve.keys.Length - 1].time;
            float yt = y_Curve.keys[y_Curve.keys.Length - 1].time;
            float end_Time = Mathf.Max(xt, yt);

            for (float t = 0; t < end_Time; t += Time.deltaTime) {
                obj.localScale = new Vector3(x_Curve.Evaluate(t), y_Curve.Evaluate(t), 1);
                yield return null;
            }
        }
    }


    //オブジェクトの生成
    private void Generate_Object(Event e) {
        GameObject obj = Instantiate(e.gen_Obj.obj);
        obj.transform.SetParent(e.gen_Obj.parent.transform);
        obj.transform.localPosition = e.gen_Obj.position;
    }


    //関数呼び出し
    private void Invoke_Function(Event e) {
        e.function.component.Invoke(e.function.function_Name, 0);
    }

    #region EditorGUI用
    public void Add_Event(int index) {
        list.Insert(index, new Event());
    }    

    public void Remove_Event(int index) {
        list.RemoveAt(index);
    }

    public void Swap_Event(int index_A, int index_B) {
        if (index_A < 0 || index_B < 0)
            return;
        if (index_A > list.Count || index_B > list.Count)
            return;
        Event event_Tmp = list[index_A];
        list[index_A] = list[index_B];
        list[index_B] = event_Tmp;
    }

    #endregion
   
}
