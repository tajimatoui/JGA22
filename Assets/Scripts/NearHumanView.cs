using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Picking
{
    // 通りすがりの人の見た目を管理します。
    public class NearHumanView : MonoBehaviour
    {
        public void OnPhazeChange(int rank)
        {
            image.color = colors[Mathf.Clamp(rank, 0, colors.Count - 1)];
        }

        [SerializeField][Header("フェーズ画像")]
        private Image image = null;

        [SerializeField][Header("各フェーズでの画像の色")]
        private List<Color> colors = new List<Color>();

        private void Awake()
        {
            image.color = colors[0];
        }


#if UNITY_EDITOR // エディター時のみ有効になります。ビルドされたものには反映されません。
        [InitializeOnLoadMethod] // この関数はUnityエディタ上でロードされたタイミングで実行されます。
        private static void OnProjectLoadedInEditor()
        {
            EditorApplication.playModeStateChanged +=
                (state) =>
                {
                    // プレイモード開始前にフィールド未設定の警告を行います。
                    if (state == PlayModeStateChange.ExitingEditMode)
                    {
                        var instances = FindObjectsOfType<NearHumanView>();
                        foreach (var instance in instances)
                        {
                            instance.WarnInvalidField();
                        }
                    }
                };
        }

        // Serializeされたフィールドが不正な場合警告を行います。
        private void WarnInvalidField()
        {
            if (colors.Count <= 0)
                Debug.LogWarning($"{gameObject.name} の Colors が未設定です");
        }
#endif // UNITY_EDITOR
    }
}
