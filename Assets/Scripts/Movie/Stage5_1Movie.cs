using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage5_1Movie : MonoBehaviour {

	public void Fade_In() {
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.01f);
    }
}
