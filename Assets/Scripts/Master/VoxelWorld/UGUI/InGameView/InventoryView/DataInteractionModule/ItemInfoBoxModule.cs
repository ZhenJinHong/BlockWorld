//using CatFramework.UiMiao;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.InputSystem;

//namespace VoxelWorld.UGUICTR
//{
//    public class ItemInfoBoxModule : IDataInteractionModule
//    {
//        bool isEnable;
//        public bool IsEnable
//        {
//            get => isEnable && itemInfoBox != null;
//            set => isEnable = value;
//        }
//        ItemInfoBox itemInfoBox;
//        public ItemInfoBoxModule(ItemInfoBox itemInfoBox)
//        {
//            this.itemInfoBox = itemInfoBox;
//        }
//        public bool PointerClick(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData)
//        {
//            return false;
//            //return itemInfoBox != null && itemInfoBox.PointerClick(dataInteractionModuleCenter, pointerEventData);
//        }

//        public bool PointerEnter(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData)
//        {
//            return itemInfoBox.PointerEnter(dataInteractionModuleCenter, pointerEventData);
//        }

//        public void PointerExit(DataInteractionModuleCenter dataInteractionModuleCenter)
//        {
//            itemInfoBox.PointerExit(dataInteractionModuleCenter);
//        }

//        public bool PointerBeginDrag(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData)
//        {
//            return false;
//            //return itemInfoBox != null && itemInfoBox.PointerBeginDrag(dataInteractionModuleCenter, pointerEventData);
//        }

//        public void PointerEndDrag(DataInteractionModuleCenter dataInteractionModuleCenter)
//        {
//            //if (itemInfoBox != null) itemInfoBox.PointerEndDrag(dataInteractionModuleCenter);
//        }
//    }
//}