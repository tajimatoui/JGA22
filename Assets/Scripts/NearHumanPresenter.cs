using UnityEngine;
using UnityEditor;

namespace Picking
{
    public class NearHumanPresenter : MonoBehaviour
    {
        [SerializeField]
        private NearHumanData data = null;
        [SerializeField]
        private NearHumanView view = null;

        void Start()
        {
            data.onPhazeChange += view.OnPhazeChange;
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
                        var instances = FindObjectsOfType<NearHumanPresenter>();
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
            if (data == null)
                Debug.LogWarning($"{gameObject.name} の Data が未設定です");
            if (view == null)
                Debug.LogWarning($"{gameObject.name} の View が未設定です");
        }
#endif // UNITY_EDITOR
    }
}
