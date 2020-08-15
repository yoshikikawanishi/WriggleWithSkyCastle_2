using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureSlicer {

    public Sprite[] Slice_Sprite(Texture2D texture, Vector2Int tile_Size) {
        // ピボットは中心
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        // 左上からTILESIZEで切り取っていく（16*16）
        // 合計何枚スプライトが得られるか
        int arraySize = (texture.height / tile_Size.y) * (texture.width / tile_Size.x);        
        // 必要なだけ配列を確保
        Sprite[] sprites = new Sprite[arraySize];
        for (int y = 0; y < texture.height / tile_Size.y; y++) {
            for (int x = 0; x < texture.width / tile_Size.x; x++) {
                // スプライトの切り取る場所をRectで指定
                // 左下が(0,0)なことに注意
                Rect rect = new Rect(x * tile_Size.x, texture.height - (y + 1) * tile_Size.y, tile_Size.x, tile_Size.y);
                // 最後の引数はPixel Per Unit（たぶん）
                Sprite sprite = Sprite.Create(texture, rect, pivot, 1f);                
                // 確保しておいた配列に代入
                if(y * (texture.width / tile_Size.x) + x < sprites.Length)
                    sprites[y * (texture.width / tile_Size.x) + x] = sprite;
            }
        }
        return sprites;
    }


    /// <summary>
    /// 画像の特定の位置が透明かどうかを辺別する
    /// </summary>    
    /// <param name="judge_Rate">全体の透明度の割合がこれ以下ならtrueを返す</param>
    /// <returns></returns>
    public bool Is_Tranceparent(Texture2D texture, Vector2Int pos, float judge_Rate) {        
        if (texture.GetPixel(pos.x, pos.y).a < judge_Rate)
            return true;
        return false;
    }


    //https://qiita.com/Katumadeyaruhiko/items/c2b9b4ccdfe51df4ad4a
    // texture を　Radable　にする
    public Texture2D createReadableTexture2D(Texture2D texture2d) {
        RenderTexture renderTexture = RenderTexture.GetTemporary(
                    texture2d.width,
                    texture2d.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(texture2d, renderTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTexture;
        Texture2D readableTextur2D = new Texture2D(texture2d.width, texture2d.height);
        readableTextur2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        readableTextur2D.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);        
        return readableTextur2D;
    }
}
