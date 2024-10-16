//using System.Collections;
//using UnityEngine;
//using UnityEngine.EventSystems;

//namespace Assets.Test
//{
//    public class ClickTest : MonoBehaviour, IPointerClickHandler
//    {
//        public void OnPointerClick(PointerEventData eventData)
//        {
//            Debug.Log(eventData.button);
//            Debug.Log(eventData.pointerId);
//            Debug.Log(eventData);
//        }

//        // Use this for initialization
//        void Start()
//        {

//        }

//        // Update is called once per frame
//        void Update()
//        {
//            // 除了1false,其他都为true
//            Debug.Log("默认" + EventSystem.current.IsPointerOverGameObject());
//            Debug.Log("0" + EventSystem.current.IsPointerOverGameObject(0));
//            Debug.Log("1" + EventSystem.current.IsPointerOverGameObject(1));
//            Debug.Log("2" + EventSystem.current.IsPointerOverGameObject(2));
//            Debug.Log("-1" + EventSystem.current.IsPointerOverGameObject(-1));
//            Debug.Log("-2" + EventSystem.current.IsPointerOverGameObject(-2));
//            Debug.Log("-3" + EventSystem.current.IsPointerOverGameObject(-3));
//        }
//    }
//}