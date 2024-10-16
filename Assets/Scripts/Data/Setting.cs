using CatFramework.SLMiao;
using System;
using System.Text;

namespace CatFramework.DataMiao
{
    public abstract class Setting : ISetting
    {
        public virtual string Name => GetType().Name;
        public Setting()
        {
            ReadDefaultValue();
            ResetSetting();
        }
        /// <summary>
        /// 用以拿去编辑器阶段的默认值，以便在重置设置里应用默认，再然后根据读取的文件会覆写
        /// </summary>
        protected virtual void ReadDefaultValue() { }
        public virtual void ApplySetting() { }
        /// <summary>
        /// 不做多余的操作,比如抛事件或保存,或应用
        /// </summary>
        public abstract void ResetSetting();
        public override string ToString()
        {
            return Serialization.ObjectFieldToString(this);
        }
    }
}
