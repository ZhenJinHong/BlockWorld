using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace CatTestDOTS.Assets.Scripts.CatFramework_TestDOTS.Test
{
    public class AsyncTaskTest : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }
        private void OnDestroy()
        {
            //new Task(() =>
            //{
            //    Debug.Log("当前" + Time.deltaTime);
            //    Debug.Log("完成");
            //    Debug.Log("完成" + Time.deltaTime);
            //}).Start();
            Debug.Log("主线程");
        }
        //async Task Lazy()
        //{

        //    await new Task(() => { Debug.Log("完成"); });
        //}
    }
}