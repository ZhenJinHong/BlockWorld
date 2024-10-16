using CatFramework.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.Magics
{
    public interface IMagicEnergy
    {

    }
    public interface IMagicWandMaterialData
    {
        Texture2D Icon { get; }
    }
    public interface IMagicWandCoreData
    {
        Texture2D Icon { get; }
    }
    public interface IMagicWandMaterial
    {
        int Length { get; }
        IMagicWandMaterialData MagicWandMaterialData { get; }

        IMagic GetMagic(int index);
        void SkipToMagic(int index);
    }
    public interface IMagicWandCore
    {
        IMagicWandCoreData MagicWandCoreData { get; }

        void AppendDelay(float v);
    }
    public interface IMagicWand : IMagicWandMaterial, IMagicWandCore
    {
        Texture2D Icon { get; }
        string Name { get; set; }
        IMagicWandHolder User { get; }
        IMagicEnergy MagicEnergy { get; }

        void Hold(IMagicWandHolder user);
        void Release();

        bool Fire();
        IMagic this[int index] { get; set; }
    }
    // 魔杖的存在是为了连续触发,如果只有一个魔法实际上就和直接执行魔法是差不多
    public class MagicWand : IMagicWand
    {
        readonly IMagic[] magics;
        public int Length => magics.Length;
        public Texture2D Icon => MagicWandDefinition?.Icon;
        public string Name { get; set; }
        public IMagicWandDefinition MagicWandDefinition { get; private set; }
        public IMagicWandHolder User { get; private set; }
        public IMagicEnergy MagicEnergy { get; private set; }
        public IMagicWandMaterialData MagicWandMaterialData { get; private set; }
        public IMagicWandCoreData MagicWandCoreData { get; private set; }
        public int Current { get; private set; }
        Timer timer;
        public MagicWand(int length)
        {
            timer = new Timer();
            magics = new IMagic[length < 1 ? 1 : length];
        }
        public MagicWand(int length, IMagicWandDefinition magicWandDefinition, string name) : this(length)
        {
            MagicWandDefinition = magicWandDefinition;
            Name = name ?? "MagicWand";
        }
        public void Hold(IMagicWandHolder user)
            => User = user;
        public void Release()
            => User = null;
        public void SetMagicEnergy(IMagicEnergy magicEnergy)
            => MagicEnergy = magicEnergy;
        public void SetMagicWandMaterial(IMagicWandMaterialData magicWandMaterialData)
        {
            MagicWandMaterialData = magicWandMaterialData;
        }
        public void SetMagicWandCore(IMagicWandCoreData magicWandCoreData)
        {
            MagicWandCoreData = magicWandCoreData;
        }
        public IMagic this[int index]
        {
            get => magics[index];
            set
            {
                magics[index]?.Linked(null);
                value?.Linked(this);
                magics[index] = value;
            }
        }
        public IMagic GetMagic(int index)
            => magics[index];
        public virtual bool Fire()
        {
            bool isFire = false;
            if (timer.Ready())
            {
                do
                {
                    if (magics[Current].Fire())
                    {
                        isFire = true;
                    }
                    else
                    {
                        break;
                    }
                }
                while (timer.Ready() && Current < Length);
                if (Current == Length)
                    Current = 0;
            }
            return isFire;
        }
        #region
        public void SkipToMagic(int index)
        {

        }
        public void AppendDelay(float v)
        {
            timer.AppendDelay(v);
        }
        #endregion
    }
    public class LinkageMagicWand : MagicWand
    {
        public LinkageMagicWand(int primaryLength) : base(primaryLength)
        {
        }
    }
}
