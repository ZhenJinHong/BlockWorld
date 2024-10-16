using CatFramework;
using CatDOTS;
using System.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using CatFramework.DataMiao;

namespace Master
{
    public class BaseGameManager : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
        private IEnumerator Start()
        {
            // 后续修改以玩家点击屏幕调用主菜单开启
            yield return null;
            //do
            //{
            //    world = World.DefaultGameObjectInjectionWorld;// 使用等待，在第二播放时会提示不能引用释放的数据
            //    yield return null;
            //    ConsoleCat.Log("世界已创建");
            //}
            //while (world == null || !world.IsCreated || !world.Unmanaged.IsCreated);// 不大可能是这个判断的原因//因为判断还未执行就报错
           
            Initialize();

            DataManagerMiao.NotifyApplyAllSetting();
        }
        protected virtual void Initialize() { }
        protected virtual void OnDestroy()
        {
        }
    }
}