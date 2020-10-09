using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMovie : MonoBehaviour {

    [SerializeField] private MovieSystem ending_Movie;
    [SerializeField] private SpriteRenderer back_Ground;
    [SerializeField] private GameObject staff_Roll_Text;

    private float scroll_Speed = 1f;
    private float scroll_Height = 1100;

    void Start() {
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

        int loop_Count = (int)(scroll_Height / scroll_Speed);
        for(int i = 0; i < loop_Count; i++) {
            staff_Roll_Text.transform.position += new Vector3(0, scroll_Speed);
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        Change_Scene_To_Title();
    }


    private void Change_Scene_To_Title() {
        SceneManager.LoadScene("TitleScene");
    }
	
}
