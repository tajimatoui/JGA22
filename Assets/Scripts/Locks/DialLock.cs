using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Picking.Locks
{
    // 南京錠の開錠状態を保持します。
    public class DialLock : LockBase
    {
        public override bool Unlocked => phaze >= angles.Count;

        public void Initialize()
        {
            phaze = 0;
            unlockTime = 0.0f;
        }

        public override void TryUnlock(Vector2 left, Vector2 right)
        {
            if (Unlocked)
                return;

            var angle = Mathf.Atan2(-right.x, -right.y) * Mathf.Rad2Deg + 180.0f;

            var distance = Mathf.Min(Mathf.Abs(angles[phaze] - angle), Mathf.Abs(360.0f - Mathf.Abs(angles[phaze] - angle)));

            if (distance <= hitDistance)
            {
                unlockTime += Time.deltaTime;
                if (unlockTime < time)
                {
                    DirectorSetter.Right = vibrationHit;
                }
                else
                {
                    DirectorSetter.Right = 0.0f;
                    DirectorSetter.ScheduleLeft(vibrationUnlock, 0.1f);
                    DirectorSetter.ScheduleRight(vibrationUnlock, 0.1f);
                    unlockTime = 0.0f;
                    phaze++;
                }
            }
            else
            {
                unlockTime = Mathf.Max(unlockTime - Time.deltaTime, 0.0f);
                var vibrationRate = (vibrationDistance - distance) / vibrationDistance;
                DirectorSetter.Right = (vibrationRate > 0.0f) ? vibrationRate * (vibrationNearMax - vibrationNearMin) + vibrationNearMin : 0.0f;
            }
        }


        [SerializeField][Header("各フェーズの開錠角度リスト（度数法）")]
        private List<float> angles = new List<float>();
        [SerializeField][Header("角度範囲")]
        private float hitDistance = 3.0f;
        [SerializeField][Header("ロック解除にかかる時間")]
        private float time = 1.0f;
        [SerializeField][Header("振動開始範囲")]
        private float vibrationDistance = 15.0f;
        [SerializeField][Header("振動強度設定")]
        private float vibrationNearMin = 0.05f;
        [SerializeField]
        private float vibrationNearMax = 0.15f;
        [SerializeField]
        private float vibrationHit = 0.3f;
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
                        var instances = FindObjectsOfType<DialLock>();
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
            if (angles.Count <= 0)
                Debug.LogWarning($"{gameObject.name} の Angles が未設定です");
        }
#endif // UNITY_EDITOR
    }
}
