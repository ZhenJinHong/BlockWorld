//using CatFramework.Tools;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace Assets.Test
//{
//    public class TimerTest : MonoBehaviour, IPointerClickHandler
//    {
//        [SerializeField] float cd;
//        [SerializeField] Image Image;
//        Timer timer;

//        public void OnPointerClick(PointerEventData eventData)
//        {
//            if (timer.Ready(cd))
//            {
//                Debug.Log("重置");
//            }
//        }

//        // Update is called once per frame
//        void Update()
//        {
//            Debug.Log("百分比" + timer.Percent(cd));
//            Debug.Log("逆百分比" + timer.InversePercent(cd));
//            Image.fillAmount = timer.Percent(cd);
//        }
//    }
//}