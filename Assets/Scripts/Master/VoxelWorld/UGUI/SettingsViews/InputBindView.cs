using CatFramework.UiMiao;
using CatFramework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using CatFramework.InputMiao;

namespace VoxelWorld.UGUICTR
{
    public class InputBindView : MonoBehaviour
    {
        [SerializeField] StringFPLVTransfer assetOption;
        [SerializeField] StringFPLVTransfer actionMapOption;
        [SerializeField] BindingKeyView bindingKeyView;
        ItemStorageList<string> inputAssetNames = new ItemStorageList<string>();
        ItemStorageList<string> inputActionMapNames = new ItemStorageList<string>();
        InputActionAsset currentInputAsset;
        public InputActionAsset CurrentInputAsset
        {
            set
            {
                if (value != currentInputAsset)
                {
                    currentInputAsset = value;
                    if (value != null)
                    {
                        value.GetMapNames(inputActionMapNames);
                        actionMapOption.Items = inputActionMapNames;
                        actionMapOption.Open();
                    }
                }
            }
        }
        private void Start()
        {
            actionMapOption.OnSelectedItem += SwitchMaps;
            foreach (InputActionAsset inputActions in InputManagerMiao.Assets)
            {
                inputAssetNames.Add(inputActions.name);
            }
            assetOption.Items = inputAssetNames;
            assetOption.OnSelectedItem += SwitchAsset;
            assetOption.Open();
            SwitchAsset(0);
        }
        void SwitchAsset(int index)
        {
            if (index > -1 && index < InputManagerMiao.Assets.Count)
            {
                CurrentInputAsset = InputManagerMiao.Assets[index];
                SwitchMaps(0);
            }
        }
        void SwitchMaps(int index)
        {
            bindingKeyView.CurrentMap = currentInputAsset.FindActionMap(actionMapOption.Items[index]);
            bindingKeyView.Open();
        }
    }
}