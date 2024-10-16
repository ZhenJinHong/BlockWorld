using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework
{
    public struct ConsoleMessage
    {
        public int Level;
        public string Message;
        public Color32 Color;
    }
    public static class ConsoleCat
    {
        class Events
        {
            public Action ContentIsChange;
            public Action<ConsoleMessage> ShootText;
        }

        static readonly List<ConsoleMessage> MessageList;
        static readonly Dictionary<string, ConsoleCommand> CommandDic;
        static Events events;

        public static event Action ContentIsChange
        {
            add => events.ContentIsChange += value;
            remove => events.ContentIsChange -= value;
        }
        public static event Action<ConsoleMessage> ShootText
        {
            add => events.ShootText += value;
            remove => events.ShootText -= value;
        }

        public static bool IsDebug { get; private set; }
        public static bool Enable { get; set; }
        public static ushort HistoryLimit = 256;

        public static int MessageCount
            => MessageList.Count;
        public static ConsoleMessage GetConsoleMessage(int index)
            => MessageList[index];
        public static IList<ConsoleMessage> Messages => MessageList;
        static ConsoleCat()
        {
            MessageList = new List<ConsoleMessage>();
            CommandDic = new Dictionary<string, ConsoleCommand>
            {
                ["Help"] = new ConsoleCommand(null, ShowHelp)
            };
            events = new Events();
//#if UNITY_EDITOR
            EnableDebug(true);
            Enable = true;
//#endif
        }
        public static void Start()
        {

        }
        public static void ShutDown()
        {
            CommandDic.Clear();
            MessageList.Clear();
            events = new Events();
        }

        public static void ShowHelp(string nu)
        {
            MessageList.Add(new ConsoleMessage() { Message = nu, Color = new Color32(0, 0, 255, 255) });
            foreach (var s in CommandDic.Values)
            {
                if (s.Help != null)
                    MessageList.Add(new ConsoleMessage() { Message = s.Help, Color = new Color32(0, 0, 255, 255) });
            }
            events.ContentIsChange?.Invoke();
        }
        static void AddText(ConsoleMessage message)
        {
            if (MessageList.Count > HistoryLimit)
            {
                MessageList.RemoveRange(0, HistoryLimit / 2);
#if UNITY_EDITOR
                Debug.Log($"控制台进行了一次清理，当前日志数量{MessageList.Count}");
#endif
            }
            MessageList.Add(message);
            events.ShootText?.Invoke(MessageList[^1]);
            events.ContentIsChange?.Invoke();
        }

        public static void RegisterCommand(string handle, string help, Action<string> callBack)
        {
            if (handle == null || callBack == null) return;
            CommandDic[handle] = new ConsoleCommand(help, callBack);
        }
        public static void UnRegisterCommand(string handle)
        {
            if (handle == null) return;
            CommandDic.Remove(handle);
        }
        public static void EnableDebug(bool debug)
        {
            IsDebug = debug;
            DebugInfo("EnableDebug:" + debug);
        }
        public static void Input(string input)
        {
            if (input == null) return;
            if (IsDebug)
            {
                string[] strings = input.Split('@', 2);
                if (strings[1] != null)
                {
                    if (CommandDic.TryGetValue(strings[0], out ConsoleCommand consoleCommand))
                    {
                        consoleCommand.Input(strings[1]);
                    }
                }
                DebugInfo(input);
            }
            else
            {
                Log(input);
            }
        }
        public static void GameError(string message)
        {
            LogError("错误！！！请尝试重启游戏或处理以下出现的错误");
            LogError(message);
        }
        public static void NullError()
            => LogError("空引用错误");
        public static void NullError(string name)
            => LogError(name + "为空");
        public static void NullWarning()
            => LogWarning("空引用错误");
        public static void BusyWarning()
            => BusyWarning("无信息");
        public static void BusyWarning(object message)
            => BusyWarning(message.ToString());
        public static void BusyWarning(string name)
            => LogWarning("尝试在工作期间访问改对象：" + name);

        public static void Log(object message)
            => Log(message.ToString());
        
        public static void LogError()
            => LogError("null");
        public static void LogError(object message)
            => LogError(message.ToString());
        public static void LogWarning()
            => LogWarning("null");
        public static void LogWarning(object message)
            => LogWarning(message.ToString());
        public static void Log(string s)
        {
            AddText(new ConsoleMessage() { Level = 0, Message = s, Color = new Color32(0, 255, 255, 255) });
#if UNITY_EDITOR
            Debug.Log(s);
#endif
        }
        public static void LogWarning(string s)
        {
            AddText(new ConsoleMessage() { Level = 1, Message = s, Color = new Color32(255, 215, 0, 255) });
#if UNITY_EDITOR
            Debug.LogWarning(s);
#endif
        }
        public static void LogError(string s)
        {
            AddText(new ConsoleMessage() { Level = 2, Message = s, Color = new Color32(220, 20, 60, 255) });
#if UNITY_EDITOR
            Debug.LogError(s);
#endif
        }
        
        public static void DebugInfo(object message)
            => DebugInfo(message.ToString());
        public static void DebugInfo(string s)
        {
            AddText(new ConsoleMessage() { Level = 4, Message = s, Color = new Color32(255, 255, 255, 255) });
#if UNITY_EDITOR
            Debug.Log(s);
#endif
        }
        public static void DebugWarning(string s)
        {
            AddText(new ConsoleMessage() { Level = 5, Message = s, Color = new Color32(255, 255, 0, 255) });
#if UNITY_EDITOR
            Debug.LogWarning(s);
#endif
        }
        public static void DebugError(string s)
        {
            AddText(new ConsoleMessage() { Level = 6, Message = s, Color = new Color32(255, 0, 0, 255) });
#if UNITY_EDITOR
            Debug.LogError(s);
#endif
        }
    }
}
