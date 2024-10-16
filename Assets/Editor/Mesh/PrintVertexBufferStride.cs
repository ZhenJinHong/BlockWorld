using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CatFramework.EditorTool
{
    public class PrintVertexBufferStride : EditorWindow
    {
        [MenuItem("CatFramework/Mesh/PrintVertexBufferStride")]
        public static void ShowWindow()
        {
            GetWindow<PrintVertexBufferStride>();
        }
        private void CreateGUI()
        {
            ObjectField gameObject = new ObjectField("游戏对象")
            {
                objectType = typeof(GameObject)
            };
            gameObject.RegisterValueChangedCallback(Print);
            rootVisualElement.Add(gameObject);
        }

        private void Print(ChangeEvent<UnityEngine.Object> evt)
        {
            if (evt.newValue is GameObject gameObject && gameObject.TryGetComponent<MeshFilter>(out var meshFilter))
            {
                var mesh = meshFilter.sharedMesh;
                // Prints 2 (two vertex streams)
                Debug.Log($"Vertex stream count: {mesh.vertexBufferCount}");
                // Next two lines print: 24 (12 bytes position + 12 bytes normal), 4 (4 bytes color)
                Debug.Log($"Steam 0 stride {mesh.GetVertexBufferStride(0)}");
            }
        }
    }
}
