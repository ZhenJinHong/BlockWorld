using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.Magics
{
    public interface IMagicWandDefinition
    {
        Texture2D Icon { get; }
        string Name { get; }
    }
    [CreateAssetMenu(fileName = "New MagicWandDefinition", menuName = "ECSVoxelWorld/MagicWandDefinition")]
    public class MagicWandDefinition : ScriptableObject, IMagicWandDefinition
    {
        [SerializeField] Texture2D icon;
        public Texture2D Icon => icon;
        public string Name => name;
    }
}
