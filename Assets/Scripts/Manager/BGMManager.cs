using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BGM {
    public bool is_Folding = false; //Editor用
    public string name;
    public AudioClip clip;
    public float volume;

    public bool have_Intoro = false;
    public AudioClip intoro_Clip;

    public BGM(string name) {
        this.name = name;
    }
}

[System.Serializable]
public class BGMManager : MonoBehaviour{

    public List<BGM> BGM_List = new List<BGM>() {
        new BGM("Stage1"),
        new BGM("Stage1_Boss"),
        new BGM("Stage2"),
        new BGM("Stage2_Boss"),
        new BGM("Stage3"),
        new BGM("Stage3_Boss"),
        new BGM("Stage4"),
        new BGM("Stage4_Boss"),
        new BGM("Stage5"),
        new BGM("Stage5_Boss"),
        new BGM("Stage6"),
        new BGM("Stage6_Boss"),
        new BGM("Stage7_Boss"),
        new BGM("Title"),
    };

    private BGM now_BGM;
    private AudioSource audio_Source;   


    //シングルトン用
    public static BGMManager Instance;
    void Awake() {        
        //シングルトン
        if (Instance != null) {
            Destroy(this.gameObject);
        }
        else if (Instance == null) {
            Instance = this;
        }
        //シーンを遷移してもオブジェクトを消さない
        DontDestroyOnLoad(gameObject);
        //取得
        audio_Source = GetComponent<AudioSource>();
    }


    // Use this for initialization
    void Start () {        

        #if UNITY_EDITOR
        if (DebugModeManager.Instance.Is_Delete_BGM) 
            audio_Source.enabled = false;
        else
           audio_Source.enabled = true;
        #endif
    }
	

    //名前でリストからBGMを取得する
    private BGM Get_BGM(string name) {
        foreach(BGM b in BGM_List) {
            if (b.name == name)
                return b;
        }
        Debug.Log(name + " BGM is not Exist");
        return null;
    }


    /// <summary>
    /// BGMを変更する
    /// </summary>
    /// <param name="name">変更先のBGM名</param>
    public void Change_BGM(string name) {
        StopCoroutine("Fade_Out_Cor");

        BGM next_BGM = Get_BGM(name);        
        if (now_BGM == next_BGM) 
            return;        
        now_BGM = next_BGM;

        if (next_BGM.have_Intoro) {
            StartCoroutine("Play_BGM_Intoro_Cor", next_BGM);
        }
        else {            
            Play_BGM(next_BGM);
        }
    }

   
    /// <summary>
    /// BGMを止める
    /// </summary>
    public void Stop_BGM() {
        audio_Source.Stop();
        now_BGM = null;
    }


    /// <summary>
    /// 一時中断
    /// </summary>
    public void Pause_BGM() {
        audio_Source.Pause();        
    }

    /// <summary>
    /// 再開
    /// </summary>
    public void Resume_BGM() {        
        audio_Source.UnPause();
    }


    /// <summary>
    /// BGMのフェードアウト
    /// </summary>
    public void Fade_Out() {
        StartCoroutine("Fade_Out_Cor");
    }

    private IEnumerator Fade_Out_Cor() {
        while(audio_Source.volume > 0) {
            audio_Source.volume -= 0.01f;
            yield return null;
        }
        audio_Source.Stop();
    }
    

    //BGMを再生する
    private void Play_BGM(BGM next_BGM) {        
        audio_Source.volume = next_BGM.volume;  //音量     
        audio_Source.clip = next_BGM.clip;      //クリップ    
        audio_Source.loop = true;               //ループ再生
        audio_Source.Play();        
    }


    //イントロを流す、終了後本体をループ再生する
    private IEnumerator Play_BGM_Intoro_Cor(BGM next_BGM) {
        //イントロ流す
        audio_Source.volume = next_BGM.volume;
        audio_Source.clip = next_BGM.intoro_Clip;
        audio_Source.loop = false;
        audio_Source.Play();

        //イントロ終了まで待つ
        while (audio_Source.isPlaying) {            
            yield return null;
            if (now_BGM != next_BGM) {       //BGMが切り替わったとき終了する
                yield break;
            }
        }

        //本体をループ再生
        Play_BGM(next_BGM);
    }


    //===================== Editor用 =========================
    public void Add_BGM() {
        BGM_List.Add(new BGM("name"));
    }
    
    public void Remove(int index) {
        BGM_List.RemoveAt(index);
    }
}
