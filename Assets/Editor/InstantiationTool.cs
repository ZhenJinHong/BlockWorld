using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CatFramework.EditorTool
{
    public class InstantiationTool : EditorWindow
    {
        [MenuItem("CatFramework/VoxelWorld/InstantiationTool")]
        public static void ShowWindow()
        {
            GetWindow<InstantiationTool>();
        }
        ObjectField parentField;
        ObjectField prefabField;
        Vector3Field countField;
        Vector3Field originalField;
        FloatField spacingField;
        private void CreateGUI()
        {
            parentField = new ObjectField("父级");
            parentField.objectType = typeof(GameObject);
            rootVisualElement.Add(parentField);
            prefabField = new ObjectField("预制体");
            prefabField.objectType = typeof(GameObject);
            rootVisualElement.Add(prefabField);
            countField = new Vector3Field("阵列数量");
            rootVisualElement.Add(countField);
            originalField = new Vector3Field("起点");
            rootVisualElement.Add(originalField);
            spacingField = new FloatField("阵列宽度");
            rootVisualElement.Add(spacingField);
            Button instantiateBtn = new Button();
            instantiateBtn.text = "实例化";
            instantiateBtn.clicked += Instantiate;
            rootVisualElement.Add(instantiateBtn);
        }
        void Instantiate()
        {
            GameObject parent = parentField.value as GameObject;
            GameObject prefab = prefabField.value as GameObject;
            if (parent != null && prefab != null)
            {
                Vector3 vector3 = countField.value;
                float spacing = spacingField.value;
                if (Mathf.Abs(spacing) < 0.1f) spacing = 0.1f;
                for (int y = 0; y < vector3.y; y++)
                {
                    for (int x = 0; x < vector3.x; x++)
                    {
                        for (int z = 0; z < vector3.z; z++)
                        {
                            GameObject gameObject = UnityEngine.Object.Instantiate(prefab, parent.transform);
                            gameObject.transform.position = new Vector3(x, y, z) * spacing + originalField.value;
                            gameObject.name = prefab.name;
                        }
                    }
                }
            }
        }
    }
}
