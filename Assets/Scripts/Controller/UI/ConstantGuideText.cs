using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantGuideText : MonoBehaviour {

    public enum Kind {
        kick,
        quit_Fly,
        fly,
    }
    [SerializeField] private Kind kind;


	// Use this for initialization
	void Start () {
        if (kind == Kind.kick) {
            List<KeyCode> keys = InputManager.KeyConfigSetting.Instance.GetKeyCode(MBLDefine.Key.Attack);
            GetComponent<TextMesh>().text
                = "Kick\n"
                + "↓ + ("
                + keys[0].ToString() + " / "
                + keys[1].ToString().Replace("Joystick", "")
                + ")";
        }
        else if (kind == Kind.quit_Fly) {
            List<KeyCode> keys = InputManager.KeyConfigSetting.Instance.GetKeyCode(MBLDefine.Key.Fly);
            GetComponent<TextMesh>().text
                = "Quit Fly\n("
                + keys[0].ToString() + " / "
                + keys[1].ToString().Replace("Joystick", "")
                + ")";
        }
        else if (kind == Kind.fly) {
            List<KeyCode> keys = InputManager.KeyConfigSetting.Instance.GetKeyCode(MBLDefine.Key.Fly);
            GetComponent<TextMesh>().text
                = "Fly\n("
                + keys[0].ToString() + " / "
                + keys[1].ToString().Replace("Joystick", "")
                + ")";
        }
    }
	
}
