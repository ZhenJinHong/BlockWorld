using CatFramework.Tools;
using CatFramework.UiMiao;
using CatFramework.UiTK;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace VoxelWorld.UGUICTR
{
    public class ItemInfoBox : MonoBehaviour, IPointerEnterHanderModule<DataInteractionModuleCenter>
    {
        [SerializeField] ViewUMCtrBase viewUMCtrBase;
        [SerializeField] ViewModule[] itemInfoViews;
        RectTransform rectTransform;
        bool isUsable;
        public bool IsUsable
        {
            get => this != null && isUsable;
            set => isUsable = value;
        }
        private void Start()
        {
            rectTransform = transform as RectTransform;
        }
        public bool PointerEnter(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData)
        {
            var ulattice = dataInteractionModuleCenter.LocatedLattice;
            if (ulattice == null || ulattice.LatticeItem == null)
            {
                if (viewUMCtrBase.IsVisual) viewUMCtrBase.Close();
            }
            else
            {
                for (int i = 0; i < itemInfoViews.Length; i++)
                {
                    if (itemInfoViews[i].Show(ulattice.LatticeItem))
                    {
                        if (!viewUMCtrBase.IsVisual)
                            viewUMCtrBase.Open();
                        if (rectTransform != null)
                        {
                            UGUIUtility.ClampFloatView(rectTransform, Mouse.current.position.value);
                        }
                        break;
                    }
                }
            }
            return true;
        }

        public void PointerExit(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData)
        {
            if (viewUMCtrBase.IsVisual) viewUMCtrBase.Close();
        }
    }
}