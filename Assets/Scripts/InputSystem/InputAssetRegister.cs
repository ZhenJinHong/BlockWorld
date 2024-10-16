using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatFramework.InputMiao
{
    //public abstract class CShapeInputAssetRegister<T> where T : class, new()
    //{
    //    T CShapeInputAsset;
    //    private void Start()
    //    {
    //        CShapeInputAsset = InputManagerMiao.GetShapeInputAsset<T>();
    //        InputManagerMiao.RegisterInputAsset(InputActionAsset);
    //    }
    //    private void OnDestroy()
    //    {
    //        InputManagerMiao.UnregisterInputAsset(InputActionAsset);
    //    }
    //    public abstract InputActionAsset InputActionAsset { get; }
    //}
    public class InputAssetRegister : MonoBehaviour
    {
        [SerializeField] bool enableInStart;
        [SerializeField] InputActionAsset[] assets;
        private void Start()
        {
            foreach (var asset in assets)
            {
                InputManagerMiao.RegisterInputAsset(asset);
                asset.LoadBinding();
                if (enableInStart)
                    Enable();
            }
        }
        private void OnDestroy()
        {
            foreach (var asset in assets)
            {
                InputManagerMiao.UnregisterInputAsset(asset);
                if (enableInStart)
                    Disable();
            }
        }
        public void Enable()
        {
            foreach (var asset in assets)
            {
                if (asset != null)
                    asset.Enable();
            }
        }
        public void Disable()
        {
            foreach (var asset in assets)
            {
                if (asset != null)
                    asset.Disable();
            }
        }
    }
}