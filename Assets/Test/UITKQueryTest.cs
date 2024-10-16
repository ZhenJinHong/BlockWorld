//using CatFramework;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.UIElements;

//namespace Assets.Test
//{
//    public class UITKQueryTest : MonoBehaviour
//    {
//        [SerializeField] UIDocument document;
//        public void Query()
//        {
//            var query = document.rootVisualElement.Query<TextElement>();
//            query.ForEach(F);
//        }
//        void F(TextElement textElement)
//        {
//            Debug.Log(textElement.text);
//        }
//    }
//}