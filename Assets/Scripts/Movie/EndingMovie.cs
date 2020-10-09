using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMovie : MonoBehaviour {

    [SerializeField] private MovieSystem ending_Movie;
    [SerializeField] private SpriteRenderer back_Ground;
    [SerializeField] private GameObject staff_Roll_Text;
    [SerializeField] private Marisa marisa;

    private float scroll_Speed = 0.5f;
    private float scroll_Height = 1400;

    void Start() {
        BGMManager.Instance.Stop_BGM();
        ending_Movie.Start_Movie();
    }
    

    public void Start_Staff_Roll() {        
        StartCoroutine("Staff_Roll_Cor");
    }


    private IEnumerator Staff_Roll_Cor() {
        while(back_Ground.color.r > 0.5f) {
            yield return null;
            back_Ground.color -= new Color(0.02f, 0.02f, 0.02f, 0);
        }
        back_Ground.color = new Color(0.5f, 0.5f, 0.5f);
        back_Ground.sortingOrder = -10;
        BGMManager.Instance.Change_BGM("Ending");
        marisa.Start_Battle();

        int loop_Count = (int)(scroll_Height / scroll_Speed);
        for(int i = 0; i < loop_Count; i++) {
            staff_Roll_Text.transform.position += new Vector3(0, scroll_Speed);
            yield return null;
        }
        FadeInOut.Instance.Start_Fade_Out(new Color(0, 0, 0), 0.02f);
        BGMManager.Instance.Fade_Out();
        yield return new WaitForSeconds(1.5f);
        Change_Scene_To_Title();
    }


    private void Change_Scene_To_Title() {
        SceneManager.LoadScene("TitleScene");
    }
	
}
