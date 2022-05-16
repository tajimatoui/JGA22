using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picking
{
    public sealed class HandsPresenter : MonoBehaviour
    {

        [SerializeField][Header("手のデータ")]
        private HandsData handsData = null;
        [SerializeField][Header("手の見た目")]
        private HandsView handsView = null;
        [SerializeField][Header("手の入力")]
        private HandsInput handsController = null;

        private void Awake()
        {
            handsController.LeftSetter = (value) => { handsData.Left = value; };
            handsController.RightSetter = (value) => { handsData.Right = value; };
            handsView.LeftPosition = () => { return handsData.Left; };
            handsView.RightPosition = () => { return handsData.Right; };
        }
    }
}
