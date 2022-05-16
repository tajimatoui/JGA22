using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Picking
{
    // 南京錠の開錠状態を保持します。
    public sealed class Padlock : LockBase
    {
        [System.Serializable]
        public class Data
        {
            public Circle left = new Circle();
            public Circle right = new Circle();
            public float time = 0.0f;
        }


        public override bool Unlocked => phaze >= datas.Count;

        public void Initialize()
        {
            phaze = 0;
            unlockTime = 0.0f;
        }

        public override void TryUnlock(Vector2 left, Vector2 right)
        {
            if (Unlocked)
                return;

            var leftHit = datas[phaze].left.IsHit(left);
            var rightHit = datas[phaze].right.IsHit(right);

            if (leftHit && rightHit)
            {
                Director.Left = Director.Right = vibrationHit;

                unlockTime += Time.deltaTime;
                if (unlockTime >= datas[phaze].time)
                {
                    Director.Left = Director.Right = 0.0f;
                    Director.ScheduleLeft(vibrationUnlock, 0.1f);
                    Director.ScheduleRight(vibrationUnlock, 0.1f);
                    unlockTime = 0.0f;
                    phaze++;
                }
            }
            else
            {
                Director.Left = Director.Right = 0.0f;

                if (leftHit)
                    Director.Left = vibrationNear;
                else if (rightHit)
                    Director.Right = vibrationNear;

                unlockTime = Mathf.Max(unlockTime - Time.deltaTime, 0.0f);
            }
        }


        [SerializeField][Header("各フェーズの開錠条件リスト")]
        private List<Data> datas = new List<Data>();
        [SerializeField][Header("振動強度設定")]
        private float vibrationNear = 0.2f;
        [SerializeField]
        private float vibrationHit = 0.4f;
        [SerializeField]
        private float vibrationUnlock = 1.0f;

        [Header("確認用")]
        [SerializeField]
        private int phaze;
        [SerializeField]
        private float unlockTime;


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
                        var instances = FindObjectsOfType<Padlock>();
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
            if (datas.Count <= 0)
                Debug.LogWarning($"{gameObject.name} の Datas が未設定です");
        }
#endif // UNITY_EDITOR
    }
}
