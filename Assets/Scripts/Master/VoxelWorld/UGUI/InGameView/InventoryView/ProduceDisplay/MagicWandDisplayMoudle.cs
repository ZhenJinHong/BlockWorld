using CatFramework;
using CatFramework.Magics;
using CatFramework.UiMiao;
using CatFramework.EventsMiao;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelWorld.UGUICTR
{
    public class MagicWandDisplayMoudle : ProductDisplayModule<MagicWandStorage>
    {
        [SerializeField] StringInputField nameField;
        IReadonlyItemStorageCollection<IUlatticeItemStorage> targetInventory;
        private void Start()
        {
            nameField.OnSubmit += ChangeName;
        }

        private void ChangeName(TInputField<string> field)
        {
            IMagicWand magicWand = Item?.GetMagicWand();
            if (magicWand != null)
                magicWand.Name = field.GetValue();
            targetInventory?.NotifyChange();
        }

        protected override void InternalSet(DataInteractionModuleCenter dataInteractionModuleCenter, MagicWandStorage magicWandStorage)
        {
            if (ProductCommonDisplay != null)
            {
                ProductCommonDisplay.SetItemIcon(magicWandStorage.ItemImage);
            }
            nameField.SetValueWithoutNotify(magicWandStorage.Name);
            targetInventory = dataInteractionModuleCenter.LocatedInventory;
            if (targetInventory == null) ConsoleCat.LogWarning("目标库存空");
        }
        protected override void ClearReference()
        {
            base.ClearReference();
            targetInventory = null;
        }
    }
}