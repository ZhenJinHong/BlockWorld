//using System;
//namespace CatFramework
//{
//    public interface IForm
//    {
//        /// <summary>
//        /// 资源名称
//        /// </summary>
//        string AssetName { get; }
//        ///// <summary>
//        ///// 覆盖上个窗口，需要被覆盖将入栈
//        ///// </summary>
//        //bool Covered { get; }
//        /// <summary>
//        /// 是否处于活动状态
//        /// </summary>
//        bool IsActivated { get; }
//        /// <summary>
//        /// 层级/排序
//        /// </summary>
//        int Order { get; }
//        /// <summary>
//        /// 提供给菜单选项的资源ID，用以翻译或者可能查找图像
//        /// </summary>
//        string MenuItemID { get; }

//        event Action<Form> OpenFormevent;
//        event Action<Form> CloseFormevent;

//        void UpdateForm();
//        /// <summary>
//        /// 按照层级，在同级中排序
//        /// </summary>
//        void Sort(int order);
//        void Open();
//        void Close();
//        void EnableForm();
//        void DisableForm();
//        void Show();
//        void Hide();
//        //void SetParent(IForm form);
//    }
//}