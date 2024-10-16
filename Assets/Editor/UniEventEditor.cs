//using CatFramework.EventsMiao;
//using CatFramework.Tools;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEditor;
//using UnityEditor.UIElements;
//using UnityEngine;
//using UnityEngine.UIElements;

//namespace CatFramework.EditorTool
//{
//    [CustomPropertyDrawer(typeof(UniEvent))]
//    public class UniEventEditor : PropertyDrawer
//    {
//        ObjectField objectField;
//        DropdownField componentsField;
//        DropdownField methodsField;
//        List<MonoBehaviour> components = new List<MonoBehaviour>();
//        public override VisualElement CreatePropertyGUI(SerializedProperty property)
//        {
//            VisualElement container = new VisualElement();
//            objectField = new ObjectField("Target");
//            objectField.bindingPath = "m_Target";
//            objectField.objectType = typeof(MonoBehaviour);
//            objectField.RegisterValueChangedCallback(ObjectChange);
//            componentsField = new DropdownField("组件");
//            componentsField.RegisterValueChangedCallback(ComponentChange);
//            methodsField = new DropdownField("方法");
//            methodsField.bindingPath = "m_MethodName";
//            container.Add(objectField);
//            container.Add(componentsField);
//            componentsField.Add(methodsField);
//            return container;
//        }
//        private void ComponentChange(ChangeEvent<string> evt)
//        {
//            if (componentsField.index > -1 && componentsField.index < components.Count)
//            {
//                MonoBehaviour monoBehaviour = components[componentsField.index];
//                objectField.SetValueWithoutNotify(monoBehaviour);
//                ObjectUtility.GetMethodNames(monoBehaviour.GetType(), methodsField.choices);
//            }
//        }
//        private void ObjectChange(ChangeEvent<UnityEngine.Object> evt)
//        {
//            if (evt.newValue is MonoBehaviour gameObject)
//            {
//                gameObject.GetComponents<MonoBehaviour>(components);
//                List<string> options = componentsField.choices;
//                options.Clear();
//                foreach (var component in components)
//                {
//                    options.Add(component.GetType().Name);
//                }
//            }
//        }
//    }
//}
