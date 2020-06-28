using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoEndingScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("Demo_Ending_Cor");
	}
	

    private IEnumerator Demo_Ending_Cor() {
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
        yield return new WaitForSeconds(1.0f);
        GetComponent<MessageDisplay>().Start_Display("DemoEndingText", 1, 2);
    }
	
}
