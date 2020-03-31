using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour {

    
    [SerializeField] private Button load_Button;
    [SerializeField] private Button play_Guide_Button;
    [Space]
    [SerializeField] private bool do_Delete_Data_Test;


    private void Awake() {
        //データ消去のテスト
        if (do_Delete_Data_Test) {
            Debug.Log("Delete Data Test");
            PlayerPrefs.DeleteAll();
            SceneManagement.Instance.Delete_Visit_Scene();
        }
    }


    // Use this for initialization
    void Start () {
        load_Button.Select();
        //セーブデータがないとき
        if (!PlayerPrefs.HasKey("SCENE")) {
            load_Button.interactable = false;
            load_Button.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 0.5f);
            play_Guide_Button.Select();
        }
        
	}
	
	
}
