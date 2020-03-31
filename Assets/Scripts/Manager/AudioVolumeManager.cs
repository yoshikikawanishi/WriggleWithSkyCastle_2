using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class AudioVolumeManager : SingletonMonoBehaviour<AudioVolumeManager> {

    private string FILEPATH;

    [SerializeField] private AudioMixer audio_Mixer;

    private float BGM_Volume = 0;
    private float SE_Volume = 0;

    public enum AudioGroup {
        BGM,
        SE,
    }

    private new void Awake() {
        base.Awake();
        FILEPATH = Application.dataPath + @"\StreamingAssets\AudioSetting.txt";               
    }

    private void Start() {
        //初めにデータを読み込む
        Load_Volume_Setting();
    }

    //ボリュームを上げる
    public void Increase_Volume(AudioGroup group) {
        if (group == AudioGroup.BGM) {
            if (0 < BGM_Volume || BGM_Volume < 20) {
                BGM_Volume += 2;
                audio_Mixer.SetFloat("BGMVol", BGM_Volume);
            }
        }
        else {
            if (0 < SE_Volume || SE_Volume < 20) {
                SE_Volume += 2;
                audio_Mixer.SetFloat("SEVol", SE_Volume);
            }
        }
    }

    //ボリュームを下げる
    public void Decrease_Volume(AudioGroup group) {
        if (group == AudioGroup.BGM) {
            if (0 < BGM_Volume || BGM_Volume < 20) {
                BGM_Volume -= 2;
                audio_Mixer.SetFloat("BGMVol", BGM_Volume);
            }
        }
        else {
            if (0 < SE_Volume || SE_Volume < 20) {
                SE_Volume -= 2;
                audio_Mixer.SetFloat("SEVol", SE_Volume);
            }
        }
    }

    //Getter
    public float Get_Volume(AudioGroup group) {
        if(group == AudioGroup.BGM) {
            return BGM_Volume + 80;
        }
        else {
            return SE_Volume + 80;
        }
    }


    //データの保存
    public void Save_Volume_Setting() {
        StreamWriter sw_Clear = new StreamWriter(FILEPATH, false);

        sw_Clear.Write("#AudioGroup\t#Volume\n");
        sw_Clear.Write("BGM\t" + BGM_Volume.ToString() + "\n");
        sw_Clear.Write("SE\t" + SE_Volume.ToString());

        sw_Clear.Flush();
        sw_Clear.Close();
    }

    //データの読み込み
    private void Load_Volume_Setting() {
        TextFileReader text = new TextFileReader();
        text.Read_Text_File_Path(FILEPATH);        

        BGM_Volume = float.Parse(text.textWords[1, 1]);
        SE_Volume = float.Parse(text.textWords[2, 1]);

        audio_Mixer.SetFloat("BGMVol", BGM_Volume);
        audio_Mixer.SetFloat("SEVol", SE_Volume);
    }

    //初期化
    public void Initialize_Volume_Setting() {
        BGM_Volume = 0;
        SE_Volume = 0;

        audio_Mixer.SetFloat("BGMVol", 0);
        audio_Mixer.SetFloat("SEVol", 0);

        Save_Volume_Setting();
    }
}
