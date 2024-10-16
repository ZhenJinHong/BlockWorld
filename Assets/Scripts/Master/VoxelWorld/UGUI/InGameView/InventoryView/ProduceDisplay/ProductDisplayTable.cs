using CatFramework.Magics;
using CatFramework.UiMiao;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VoxelWorld.UGUICTR
{
    public class ProductDisplayTable : MonoBehaviour, IPointerClickHanderModule<DataInteractionModuleCenter>
    {
        [SerializeField] ProductCommonDisplay productCommonDisplay;
        [SerializeField] ProductDisplayModule[] modules;

        bool isUsable;
        public bool IsUsable
        {
            get => isUsable && this != null;
            set => isUsable = value;
        }
        private void Start()
        {
            for (int i = 0; i < modules.Length; i++)
            {
                var module = modules[i];
                module.SetCommonDisplay(productCommonDisplay);
            }
        }
        public bool PointerClick(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Left && pointerEventData.clickCount == 1)
            {
                bool success = false;
                for (int i = 0; i < modules.Length; i++)
                {
                    var module = modules[i];
                    if (success)
                    {
                        if (module != null) module.Hide();
                    }
                    else if (module != null && module.Set(dataInteractionModuleCenter, pointerEventData))
                    {
                        success = true;
                    }
                }
                if (!success) { productCommonDisplay.ClearDisplay(); }
                return success;
            }
            return false;
        }
    }
    public abstract class ProductDisplayModule : MonoBehaviour
    {
        ProductCommonDisplay productCommonDisplay;
        protected ProductCommonDisplay ProductCommonDisplay => productCommonDisplay;
        public void SetCommonDisplay(ProductCommonDisplay productCommonDisplay)
        {
            this.productCommonDisplay = productCommonDisplay;
        }
        public abstract bool Set(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData);
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
    public abstract class ProductDisplayModule<T> : ProductDisplayModule where T : class, IUlatticeItemStorage
    {
        protected T Item { get; private set; }
        public override bool Set(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData)
        {
            IUlatticeItemStorage ulatticeItem = dataInteractionModuleCenter.LocatedLattice.LatticeItem;
            if (ulatticeItem is T item)
            {
                Item = item;
                InternalSet(dataInteractionModuleCenter, item);
                gameObject.SetActive(true);
                return true;
            }
            else
            {
                Item = null;
                ClearReference();
                gameObject.SetActive(false);
                return false;
            }
        }
        protected abstract void InternalSet(DataInteractionModuleCenter dataInteractionModuleCenter, T ulatticeItem);
        protected virtual void ClearReference() { }
    }
    public class MagicWandBuilder
    {
        public int Length = 1;
        public IMagicWandDefinition MagicWandDefinition;
        public MagicWand Create(string name)
        {
            return new MagicWand(Length, MagicWandDefinition, name);
        }
    }
    public class VoxelBluePrint
    {

    }
}