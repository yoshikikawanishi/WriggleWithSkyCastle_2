using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 選択の確認用キャンバスにアタッチする。
/// 自分の表示と非表示を切り替える関数を持つ。
/// </summary>
public class ConfirmCanvas : MonoBehaviour {

    [SerializeField] private List<CanvasGroup> other_Canvas;
    [SerializeField] private Button select_Button_In_Display_Canvas;
    [SerializeField] private Button select_Button_In_Delete_Canvas;
   

    public void Display_Confirm_Canvas() {
        gameObject.SetActive(true);
        foreach(var canvas in other_Canvas) {
            canvas.interactable = false;
        }
        EventSystem.current.SetSelectedGameObject(null);
        select_Button_In_Display_Canvas.Select();
    }


    public void Delete_Confirm_Canvas() {
        foreach (var canvas in other_Canvas) {
            canvas.interactable = true;
        }
        EventSystem.current.SetSelectedGameObject(null);
        select_Button_In_Delete_Canvas.Select();
        gameObject.SetActive(false);        
    }


}
