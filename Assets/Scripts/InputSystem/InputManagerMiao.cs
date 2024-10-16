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

        #region ���
        public static event Action<CursorLockMode> OnCursorLockEvent
        {
            add { events.OnCursorLockEvent += value; }
            remove { events.OnCursorLockEvent -= value; }
        }
        // ��ֱ��ʹ��ͨ�����������,��Ϊ���״̬��ĳЩ��������
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
            return Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());// ͬһ֡��Ҫ����������ж��?
        }
        #endregion
        static RebindingOperation RebindingOperation;
        static Events events;
        // ֻ��C#�ʲ����ɵ���Ҫ��������(ʵ����Unity�Զ�����)
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
            // �ɽű����ɵ�Ӧ�ôݻ�,�����б���ֱ������
            for (int i = 0; i < cshapeInputAssets.Count; i++)
            {
                cshapeInputAssets[i].Dispose();// һ����Ϊ�ʲ��ᱻ������ʹ���ߵ�����,����OnDestroy��ȡ��ע������;��OnDestroy��ʱ��Ҳ�رչ�����,��ʱ�ͷ��ʲ�,��Destroy����ΪDestroy��������,����ȡ��ע�����벻����?
            }
            cshapeInputAssets.Clear();// �رյ�ʱ��Unity��������Щ
            inputAssets.Clear();
        }
        public static T GetShapeInputAsset<T>() where T : class, ICShapeInputAsset, new()
        {
            if (isShutDown) { ConsoleCat.LogWarning("�������Ѿ��ػ�������ʹ��"); return null; }
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
        #region ������������
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
            //��ȡ��������ID����󶨵ļ�
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
                asset.LoadBindingOverridesFromJson(rebinds);// �÷����Ѿ��Լ��п�
        }

        //Ϊ�˱����Ҵ��������Ҷ��������Ҷ�Ӧ����
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
                ConsoleCat.LogWarning("���󶨵���ΪΪ���ϰ�ť");
            }
#endif

            CancelRebindingOperation();

            inputActionMap.Disable();
            //��������Using
            RebindingOperation = inputAction.PerformInteractiveRebinding(0);
            RebindingOperation.OnComplete(OnCompleteBinding);
            RebindingOperation.OnCancel(OnCancelBinding);
            //�󶨲�����ȡ����ʽ//�ᵼ��EҲ���ų�
            RebindingOperation.WithCancelingThrough("<Keyboard>/escape");
            //�󶨲����ų����
            RebindingOperation.WithControlsExcluding("Mouse");
            //��ʼ�󶨲���
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
                    ConsoleCat.Log("ȡ���˰�");
                inputActionMap.Enable();
                OnCancel?.Invoke(rebindingOperation.action.bindings[0].ToDisplayString());
                rebindingOperation.Dispose();
            }
        }
        #endregion
    }
}