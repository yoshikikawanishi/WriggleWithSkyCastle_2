using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 勢いで作ったから汚くてごめんね！
/// オープニングイベントのこと全部ここにあるよ
/// 強制スクロール、タイルの破壊、ザコ妖精生成等々
/// </summary>
public class OpeningScene : MonoBehaviour {

    [SerializeField] private MovieSystem first_Opening_Movie;
    [Space]
    [SerializeField] private float right_Side;
    [SerializeField] private float scroll_Speed;
    [Space]
    [SerializeField] private Tilemap ground_Tile;
    [SerializeField] private GameObject ground_Crash_Effect;
    [Space]
    [SerializeField] private GameObject blue_Fairy;
    [SerializeField] private float blue_Fairy_Width;

    private GameObject main_Camera;
    private CameraController camera_Controller;


    void Start() {
        //取得
        main_Camera = GameObject.FindWithTag("MainCamera");
        camera_Controller = main_Camera.GetComponent<CameraController>();

        //エフェクトのオブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(ground_Crash_Effect, 20);

        //オープニング開始
        Start_Opening();        
    }



    public void Start_Opening() {
        StartCoroutine("Opening_Cor");
    }


    private IEnumerator Opening_Cor() {
        camera_Controller.enabled = false;

        //地面破壊開始
        StartCoroutine("Crash_Ground_Cor");

        first_Opening_Movie.Start_Movie();
        yield return new WaitUntil(first_Opening_Movie.End_Movie);
        
        //強制スクロール開始
        StartCoroutine("Scroll_Camera_Cor");        
    }


    //画面揺らし、ムービー用
    public void Shake_Camera() {
        CameraShake shake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        shake.Shake(0.3f, new Vector2(3, 3), true);
    }


    //フェードイン、ムービー用
    public void Fade_In() {
        FadeInOut.Instance.Start_Fade_In(new Color(1, 1, 1), 0.01f);
    }


    //強制スクロール、イベント開始時呼ばれる
    private IEnumerator Scroll_Camera_Cor() {
        while(main_Camera.transform.position.x < right_Side) {
            main_Camera.transform.position += new Vector3(scroll_Speed, 0, 0);
            yield return new WaitForSeconds(0.016f);
        }
    }


    //巨大青妖精に合わせて地面タイルを破壊していく
    private IEnumerator Crash_Ground_Cor() {        
        while (blue_Fairy.transform.position.y < -220) {
            yield return null;
        }

        //破壊したい地面タイルの縦ラインインデックス
        int crash_Ground_Line = Cal_Crash_Ground_Line();

        while (true) {
            int line = Cal_Crash_Ground_Line();
            if(crash_Ground_Line != line) {
                crash_Ground_Line = line;
                Delete_Ground_Tile_Vertical_Row(line);
            }
            yield return null;
        }
    }


    //タイルマップの番号から座標を取得
    private Vector2 Get_Position_By_Index(Vector2Int index) {
        return index * 32 + new Vector2Int(16, 16);
    }
    

    //座標からタイルマップの番号を取得
    private Vector2Int Get_Index_By_Position(Vector2 position) {
        Vector2Int index = new Vector2Int((int)(position.x / 32), (int)(position.y / 32));
        if (position.x < 0)
            index += new Vector2Int(-1, 0);
        if (position.y < 0)
            index += new Vector2Int(0, -1);
        return index;
    }


    //青妖精の現在位置から壊すタイルマップのインデックスを取得
    private int Cal_Crash_Ground_Line() {
        return Get_Index_By_Position(blue_Fairy.transform.position + new Vector3(blue_Fairy_Width, 0)).x;
    }


    //タイルの指定した番号の縦一列を消してエフェクトを出す
    private void Delete_Ground_Tile_Vertical_Row(int x_Line) {
        for(int i = -6; i < 6; i++) {
            if (ground_Tile.GetTile(new Vector3Int(x_Line, i, 0)) == null)
                continue;
            ground_Tile.SetTile(new Vector3Int(x_Line, i, 0), null);
            var effect = ObjectPoolManager.Instance.Get_Pool(ground_Crash_Effect).GetObject();
            effect.transform.position = Get_Position_By_Index(new Vector2Int(x_Line, i));
        }
    }
    
}
