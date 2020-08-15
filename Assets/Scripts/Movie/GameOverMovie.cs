using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMovie : MonoBehaviour {


    public void Start_Movie(bool with_Larva) {
        if (with_Larva) {
            StartCoroutine("Game_Over_Movie_Cor");
        }
        else {
            StartCoroutine("Back_Title_Cor");
        }
    }
    

    private IEnumerator Game_Over_Movie_Cor() {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("GameOverScene");
    }


    private IEnumerator Back_Title_Cor() {
        yield return new WaitForSeconds(1.0f);
        PlayerPrefs.SetInt("STOCK", 3);
        SceneManager.LoadScene("TitleScene");
    }

    
}
