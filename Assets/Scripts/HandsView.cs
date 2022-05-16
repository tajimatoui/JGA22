using System;
using UnityEngine;
using UnityEditor;

namespace Picking
{
    // 手の状態を見た目に反映します。
    public sealed class HandsView : MonoBehaviour
    {
        public Func<Vector2> LeftPosition { private get; set; }
        public Func<Vector2> RightPosition { private get; set; }


        [SerializeField][Header("左手回転")]
        private Transform leftRotation = null;
        [SerializeField][Header("右手回転")]
        private Transform rightRotation = null;

        [SerializeField][Header("手の傾き最大値")]
        private float rotationMax = 20.0f;

        // 回転の初期状態を保存
        private Quaternion defaultLeftRotation;
        private Quaternion defaultRightRotation;

        private void Start()
        {
            defaultLeftRotation = leftRotation.localRotation;
            defaultRightRotation = rightRotation.localRotation;
        }

        private void Update()
        {
            var left = (LeftPosition != null) ? LeftPosition() : new Vector2(0.0f, 0.0f);
            leftRotation.localRotation = defaultLeftRotation * Quaternion.Euler(left.y * rotationMax, -left.x * rotationMax, 0.0f);
            var right = (RightPosition != null) ? RightPosition() : new Vector2(0.0f, 0.0f);
            rightRotation.localRotation = defaultRightRotation * Quaternion.Euler(right.y * rotationMax, -right.x * rotationMax, 0.0f);
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
                        var instances = FindObjectsOfType<HandsView>();
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
            if (leftRotation == null)
                Debug.LogWarning($"{gameObject.name} の LeftRotation が未設定です");
            if (rightRotation == null)
                Debug.LogWarning($"{gameObject.name} の RightRotation が未設定です");
        }
#endif // UNITY_EDITOR
    }
}
