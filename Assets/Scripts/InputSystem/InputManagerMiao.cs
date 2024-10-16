using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;
using CatFramework.SLMiao;

namespace CatFramework.InputMiao
{
    public static class InputManagerMiao
    {
        class Events
        {
            public Action<CursorLockMode> OnCursorLockEvent;
        }

        public const string OverrideFileFormat = ".ipao";

        #region 光标
        public static event Action<CursorLockMode> OnCursorLockEvent
        {
            add { events.OnCursorLockEvent += value; }
            remove { events.OnCursorLockEvent -= value; }
        }
        // 不直接使用通用组件来控制,因为这个状态在某些情况不许改
        public static CursorLockMode CursorState
        {
            get => Cursor.lockState;
            set
            {
                if (Cursor.lockState != value)
                {
                    events.OnCursorLockEvent?.Invoke(value);
                    Cursor.lockState = value;
                }
            }
        }
        public static void SetCursorStateWithoutNotify(CursorLockMode cursorLockMode)
        {
            Cursor.lockState = cursorLockMode;
        }
        public static bool CursorVisible
        {
            get => Cursor.visible;
            set => Cursor.visible = value;
        }
        public static void SetCurror(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
            => Cursor.SetCursor(texture, hotspot, cursorMode);
        //static Ray ray;
        //static Vector2 mousePos;
        public static Ray GetMainCameraRay()
        {
            //var newMousePos = Mouse.current.position.ReadValue();
            //if (mousePos != newMousePos)
            //{
            //    mousePos = newMousePos;
            //    ray = Camera.main.ScreenPointToRay(newMousePos);
            //}
            //return ray;
            return Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());// 同一帧需要这个可能性有多大?
        }
        #endregion
        static RebindingOperation RebindingOperation;
        static Events events;
        // 只有C#资产生成的需要主动销毁(实际上Unity自动销毁)
        static List<ICShapeInputAsset> cshapeInputAssets;
        public static IReadOnlyList<InputActionAsset> Assets => inputAssets;
        static List<InputActionAsset> inputAssets;
        static bool isShutDown;
        static InputManagerMiao()
        {
            events = new Events();
            cshapeInputAssets = new List<ICShapeInputAsset>();
            inputAssets = new List<InputActionAsset>();
        }
        public static void Start()
        {
            //for (int i = 0; i < cshapeInputAssets.Count; i++)
            //{
            //    RegisterInputAsset(cshapeInputAssets[i].asset);
            //}
            isShutDown = false;
        }
        public static void ShutDown()
        {
            isShutDown = true;
            CancelRebindingOperation();
            events = new Events();
            // 由脚本生成的应该摧毁,否则列表不该直接清理
            for (int i = 0; i < cshapeInputAssets.Count; i++)
            {
                cshapeInputAssets[i].Dispose();// 一般行为资产会被缓存在使用者的类中,并在OnDestroy中取消注册输入;而OnDestroy的时候也关闭管理器,此时释放资产,是Destroy但因为Destroy不是立刻,所以取消注册输入不报错?
            }
            cshapeInputAssets.Clear();// 关闭的时候Unity销毁了这些
            inputAssets.Clear();
        }
        public static T GetShapeInputAsset<T>() where T : class, ICShapeInputAsset, new()
        {
            if (isShutDown) { ConsoleCat.LogWarning("管理器已经关机不该再使用"); return null; }
            for (int i = 0; i < cshapeInputAssets.Count; i++)
            {
                if (cshapeInputAssets[i] is T ca)
                {
                    return ca;
                }
            }
            T v = new T();
            cshapeInputAssets.Add(v);
            RegisterInputAsset(v.asset);
            return v;
        }
        public static bool Contains(InputActionAsset asset)
            => inputAssets.Contains(asset);
        public static void RegisterInputAsset(InputActionAsset asset)
        {
            if (inputAssets.Contains(asset)) return;
            inputAssets.Add(asset);
        }
        public static bool UnregisterInputAsset(InputActionAsset asset)
            => inputAssets.Remove(asset);
        #region 按键绑定面板管理
        public static void CancelRebindingOperation()
        {
            if (RebindingOperation != null)
            {
                if (!RebindingOperation.completed)
                    RebindingOperation.Cancel();
                RebindingOperation.Dispose();
                RebindingOperation = null;
            }
        }
        public static void GetMapNames(this ICShapeInputAsset asset, List<string> names)
            => asset.asset.GetMapNames(names);
        public static void GetMapNames(this InputActionAsset asset, List<string> names)
        {
            CancelRebindingOperation();
            names.Clear();
            var maps = asset.actionMaps;
            for (int i = 0; i < maps.Count; i++)
            {
                names.Add(maps[i].name);
            }
        }
        public static void GetMapNames(this ICShapeInputAsset asset, out string[] names)
            => asset.asset.GetMapNames(out names);
        public static void GetMapNames(this InputActionAsset asset, out string[] names)
        {
            CancelRebindingOperation();
            var maps = asset.actionMaps;
            names = new string[maps.Count];
            for (int i = 0; i < maps.Count; i++)
            {
                names[i] = maps[i].name;
            }
        }

        public static void ForeachActions(this ICShapeInputAsset asset, string mapName, Action<string, string> returnNameAndBinding)
            => asset.asset.FindActionMap(mapName).ForeachActions(returnNameAndBinding);
        public static void ForeachActions(this InputActionAsset asset, string mapName, Action<string, string> returnNameAndBinding)
            => asset.FindActionMap(mapName).ForeachActions(returnNameAndBinding);
        public static void ForeachActions(this InputActionMap map, Action<string, string> returnNameAndBinding)
        {
            CancelRebindingOperation();
            if (map == null || returnNameAndBinding == null) return;
            var actions = map.actions;
            //获取动作名（ID）与绑定的键
            foreach (InputAction inputAction in actions)
            {
                if (CheckActionIsValid(inputAction))
                    returnNameAndBinding(inputAction.name, inputAction.bindings[0].ToDisplayString());
            }
        }

        public static void ResetBinding(this ICShapeInputAsset asset)
            => asset.asset.ResetBinding();
        public static void ResetBinding(this InputActionAsset asset)
        {
            CancelRebindingOperation();
            asset.RemoveAllBindingOverrides();
        }

        public static bool CheckActionIsValid(InputAction inputAction)
        {
            return inputAction.type == InputActionType.Button && (!inputAction.bindings[0].isComposite);
        }

        public static void SaveBinding(this ICShapeInputAsset asset, string fileName = null)
            => asset.asset.SaveBinding(fileName);
        public static void SaveBinding(this InputActionAsset asset, string fileName = null)
        {
            CancelRebindingOperation();
            string rebinds = asset.SaveBindingOverridesAsJson();
            fileName ??= asset.name;
            SaveAndLoad.SaveAllText(fileName + OverrideFileFormat, rebinds, Paths.ArchivePath);
        }

        public static void LoadBinding(this ICShapeInputAsset asset, string fileName = null)
            => asset.asset.LoadBinding(fileName);
        public static void LoadBinding(this InputActionAsset asset, string fileName = null)
        {
            CancelRebindingOperation();
            fileName ??= asset.name;
            if (SaveAndLoad.LoadAllText(out string rebinds, Paths.ArchivePath, fileName + OverrideFileFormat))
                asset.LoadBindingOverridesFromJson(rebinds);// 该方法已经自己判空
        }

        //为了避免找错动作表，先找动作表，再找对应动作
        public static void BindingAction(this ICShapeInputAsset asset, string mapName, string actionID, Action<string> OnComplete, Action<string> OnCancel)
            => asset.asset.FindActionMap(mapName).BindingAction(actionID, OnComplete, OnCancel);
        public static void BindingAction(this InputActionAsset asset, string mapName, string actionID, Action<string> OnComplete, Action<string> OnCancel)
            => asset.FindActionMap(mapName).BindingAction(actionID, OnComplete, OnCancel);

        public static void BindingAction(this InputActionMap inputActionMap, string actionID, Action<string> OnComplete, Action<string> OnCancel)
        {
            if (inputActionMap == null) return;
            InputAction inputAction = inputActionMap.FindAction(actionID);
            if (inputAction == null || inputAction.bindings.Count < 1)
            {
                return;
            }
#if UNITY_EDITOR
            if (inputAction.bindings[0].isComposite)
            {
                ConsoleCat.LogWarning("将绑定的行为为复合按钮");
            }
#endif

            CancelRebindingOperation();

            inputActionMap.Disable();
            //不可以用Using
            RebindingOperation = inputAction.PerformInteractiveRebinding(0);
            RebindingOperation.OnComplete(OnCompleteBinding);
            RebindingOperation.OnCancel(OnCancelBinding);
            //绑定操作的取消方式//会导致E也被排除
            RebindingOperation.WithCancelingThrough("<Keyboard>/escape");
            //绑定操作排除鼠标
            RebindingOperation.WithControlsExcluding("Mouse");
            //开始绑定操作
            RebindingOperation.Start();

            void OnCompleteBinding(RebindingOperation rebindingOperation)
            {
                rebindingOperation.Dispose();
                inputActionMap.Enable();
                OnComplete?.Invoke(rebindingOperation.action.bindings[0].ToDisplayString());
            }
            void OnCancelBinding(RebindingOperation rebindingOperation)
            {
                if (ConsoleCat.Enable)
                    ConsoleCat.Log("取消了绑定");
                inputActionMap.Enable();
                OnCancel?.Invoke(rebindingOperation.action.bindings[0].ToDisplayString());
                rebindingOperation.Dispose();
            }
        }
        #endregion
    }
}