using MBLDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 入力を管理する
/// </summary>
internal class InputManager : SingletonMonoBehaviour<InputManager> {
    #region InnerClass

    /// <summary>
    /// キーコンフィグ設定の変更や保存・ロードを管理する
    /// </summary>
    internal class KeyConfigSetting {
        private Array keyCodeValues;
        private static InputManager inputManager;
        private static KeyConfigSetting instance;

        public static KeyConfigSetting Instance {
            get {
                if (inputManager == null)
                    inputManager = InputManager.Instance;
                return instance = instance != null ? instance : new KeyConfigSetting();
            }
        }

        private KeyConfigSetting() {
        }

        /// <summary>
        /// 呼び出されたフレームで押下状態のKeyCodeリストを返す
        /// </summary>
        /// <returns>押下状態のKeyCodeリスト</returns>
        private List<KeyCode> GetCurrentInputKeyCode() {
            List<KeyCode> ret = new List<KeyCode>();
            if (keyCodeValues == null)
                keyCodeValues = Enum.GetValues(typeof(KeyCode));
            foreach (var code in keyCodeValues)
                if (Input.GetKey((KeyCode)(int)code))
                    ret.Add((KeyCode)(int)code);
            return ret;
        }

        /// <summary>
        /// コンフィグにキーをセットする
        /// </summary>
        /// <param name="key">キーを表す識別子</param>
        /// <param name="keyCode">割り当てるキーコード</param>
        /// <returns>割り当てが正常に終了したかどうか</returns>
        public bool SetKey(Key key, List<KeyCode> keyCode) {
            return inputManager.keyConfig.SetKey(key.String, keyCode);
        }

        /// <summary>
        /// コンフィグから値を消去
        /// </summary>
        /// <param name="key">キーを表す識別子</param>
        /// <returns>値の消去が正常に終了したかどうか</returns>
        public bool RemoveKey(Key key) {
            return inputManager.keyConfig.RemoveKey(key.String);
        }

        /// <summary>
        /// 押されているキーを名前文字列に対するキーとして設定する
        /// </summary>
        /// <param name="key">キーに割り付ける名前</param>
        /// <returns>キーコードの設定が正常に完了したかどうか</returns>
        public bool SetCurrentKey(Key key) {
            //HACK:マウス入力も受け付けるようにするべきなので今後改善
            //マウス{0~6}の入力を弾く
            var currentInput = GetCurrentInputKeyCode().Where(c => c < KeyCode.Mouse0 || KeyCode.Mouse6 < c).ToList();

            if (currentInput == null || currentInput.Count < 1)
                return false;
            var code = inputManager.keyConfig.GetKeyCode(key.String);
            //既に設定されているキーと一部でも同じキーが押されている場合
            if (code.Count > currentInput.Count && currentInput.All(k => code.Contains(k)))
                return false;
            RemoveKey(key);
            return SetKey(key, currentInput);
        }

        /// <summary>
        /// デフォルトのキー設定を適用する
        /// </summary>
        public void SetDefaultKeyConfig() {
            foreach (var key in Key.AllKeyData)
                SetKey(key, key.DefaultKeyCode);
        }

        public List<KeyCode> GetKeyCode(Key keyName) {
            return inputManager.keyConfig.GetKeyCode(keyName.String);
        }

        public List<KeyCode> GetKeyCode(string keyName) {
            return inputManager.keyConfig.GetKeyCode(keyName);
        }

        public void LoadSetting() {
            InputManager.Instance.keyConfig.LoadConfigFile();
        }

        public void SaveSetting() {
            InputManager.Instance.keyConfig.SaveConfigFile();
        }
    }

    #endregion InnerClass

    /// <summary>
    /// 使用するキーコンフィグ
    /// </summary>
    public readonly KeyConfig keyConfig = new KeyConfig();

    public new void Awake() {
        Debug.Log("Load key-config file.");

        //最初はデフォルトの設定をコンフィグに格納
        KeyConfigSetting.Instance.SetDefaultKeyConfig();        

        //コンフィグファイルがあれば読み出す
        try {
            InputManager.Instance.keyConfig.LoadConfigFile();
        }
        catch (IOException e) {
            Debug.Log(e.Message);
            InputManager.Instance.keyConfig.SaveConfigFile();
        }
        

    }

    /// <summary>
    /// 指定したキーが押下状態かどうかを返す
    /// </summary>
    /// <returns>入力状態</returns>
    internal bool GetKey(Key key) {
        return keyConfig.GetKey(key.String);
    }

    /// <summary>
    /// 指定したキーが入力されたかどうかを返す
    /// </summary>
    /// <returns>入力状態</returns>
    internal bool GetKeyDown(Key key) {
        return keyConfig.GetKeyDown(key.String);
    }

    /// <summary>
    /// 指定したキーが離されたかどうかを返す
    /// </summary>
    /// <returns>入力状態</returns>
    internal bool GetKeyUp(Key key) {
        return keyConfig.GetKeyUp(key.String);
    }

    /// <summary>
    /// 軸入力に対する値を返す
    /// </summary>
    /// <returns>入力値</returns>
    internal float GetAxes(Axes axes) {
        return Input.GetAxis(axes.String);
    }

    /// <summary>
    /// 軸入力に対する値を返す
    /// </summary>
    /// <returns>平滑化フィルターが適用されていない入力値</returns>
    internal float GetAxesRaw(Axes axes) {
        return Input.GetAxisRaw(axes.String);
    }
}
