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
                sprites[y * texture.width / tile_Size.x + x] = sprite;
            }
        }
        return sprites;
    }
}
