using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MovieSystem : MonoBehaviour {

    [System.Serializable]
    public class Event {
        public string name;
        public enum Type {
            message,
            motion,
            genObj,
            wait,
        }
        public Type type = Type.message;

        public Message message = new Message();
        public Motion motion = new Motion();
        public GenObject gen_Obj = new GenObject();
        public Wait wait = new Wait();
    }

    [System.Serializable]
    public class Message {
        public string file_Name;
        public int start_ID;
        public int end_ID;
        public bool is_Auto;        
    }

    [System.Serializable]
    public class Motion {
        public GameObject obj;
        public string anim_Parameter;
        public bool param_Bool;
        public AnimationCurve x_Move;
        public AnimationCurve y_Move;
    }

    [System.Serializable]
    public class GenObject {
        public GameObject obj;
        public GameObject parent;
        public Vector2 position;
    }

    [System.Serializable]
    public class Wait {
        public bool wait_Message;
        public float wait_Time;
    }

    
    public List<Event> list = new List<Event>();
    
    private MessageDisplay _message;


    void Awake() {
        _message = GetComponent<MessageDisplay>();
        if(_message == null)
            _message = gameObject.AddComponent<MessageDisplay>();        
    }

    
    //ムービーを開始する
    public void Start_Movie() {
        StartCoroutine("Movie_Cor");
    }

    private IEnumerator Movie_Cor() {
        for(int i = 0; i < list.Count; i++) {
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
                case Event.Type.wait:
                    if (e.wait.wait_Message) {
                        yield return new WaitUntil(_message.End_Message);
                    }
                    yield return new WaitForSeconds(e.wait.wait_Time);
                    break;
            }
        }
        yield return null;
    }


    //メッセージ表示
    private void Start_Message(Event e) {
        if (e.message.is_Auto) {
            _message.Start_Display_Auto(e.message.file_Name, e.message.start_ID, e.message.end_ID, 1.0f, 0.02f);
        }
        else {
            _message.Start_Display(e.message.file_Name, e.message.start_ID, e.message.end_ID);
        }        
    }

    //オブジェクトの移動を行う
    private IEnumerator Object_Move_Cor(Event e) {
        GameObject obj = e.motion.obj;
        AnimationCurve x_Curve = e.motion.x_Move;
        AnimationCurve y_Curve = e.motion.y_Move;
        if (x_Curve == null || y_Curve == null)
            yield break;

        float xt = x_Curve.keys[x_Curve.keys.Length - 1].time;
        float yt = y_Curve.keys[y_Curve.keys.Length - 1].time;
        float end_Time = Mathf.Max(xt, yt);

        for (float t = 0; t < end_Time; t += 0.016f + Time.deltaTime) {
            obj.transform.position = new Vector3(x_Curve.Evaluate(t), y_Curve.Evaluate(t));
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
