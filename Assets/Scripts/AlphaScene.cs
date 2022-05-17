using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEditor;
using Picking.Locks;

namespace Picking
{
    // Alphaシーンの管理を行います。
    public sealed class AlphaScene : MonoBehaviour
    {
        [SerializeField][Header("手（データ）")]
        private HandsData handsData = null;
        [SerializeField][Header("鍵（データ）")]
        private LockBase lockComponent = null;
        [SerializeField][Header("演出")]
        private Director director = null;
        [SerializeField][Header("カメラ")]
        private CameraController cameraComponent = null;
        [SerializeField][Header("ドア")]
        private Door door = null;
        [SerializeField][Header("近くの人（データ）")]
        private NearHumanData nearHumanData = null;

        private void Start()
        {
            handsData.AdjustLeft = lockComponent.AdjustLeft;
            handsData.AdjustRight = lockComponent.AdjustRight;

            lockComponent.DirectorSetter = director;

            director.onVibrate += (left, right) => Gamepad.current?.SetMotorSpeeds(left, right);
            director.onVibrate += cameraComponent.SetVibrate;

            if (nearHumanData != null)
                nearHumanData.OnFound = GameOver;

            door.StartClose();
        }

        private void Update()
        {
            lockComponent.TryUnlock(handsData.Left, handsData.Right);
            if (handsData.Left.sqrMagnitude > 0.0f || handsData.Right.sqrMagnitude > 0.0f)
                nearHumanData?.CheckAtAct();
        }

        private void GameOver()
        {
            Debug.Log("GameOver");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
                        var instances = FindObjectsOfType<AlphaScene>();
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
            if (handsData == null)
                Debug.LogWarning($"{gameObject.name} の Hands Data が未設定です");
            if (lockComponent == null)
                Debug.LogWarning($"{gameObject.name} の Lock Component が未設定です");
            if (director == null)
                Debug.LogWarning($"{gameObject.name} の Director が未設定です");
            if (cameraComponent == null)
                Debug.LogWarning($"{gameObject.name} の Camera Component が未設定です");
            if (door == null)
                Debug.LogWarning($"{gameObject.name} の Door が未設定です");
            if (nearHumanData == null)
                Debug.LogWarning($"{gameObject.name} の Near Human Data が未設定です");
        }

#endif // UNITY_EDITOR
    }
}
