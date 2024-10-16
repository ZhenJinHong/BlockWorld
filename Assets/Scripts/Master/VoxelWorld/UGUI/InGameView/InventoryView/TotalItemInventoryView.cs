//using CatFramework.UiMiao;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace VoxelWorld.UGUICTR
//{
//    //public class TotalItemListView : MonoBehaviour, IViewModule
//    //{
//    //    public bool IsDestroy => this == null;

//    //    public void Close()
//    //    {
//    //    }

//    //    public void Open()
//    //    {

//    //    }

//    //    public bool Show(object content)
//    //    {
//    //        throw new System.NotImplementedException();
//    //    }
//    //}
//    //public class InventoryToolStripContent : IToolStripContent
//    //{
//    //    public string Label { get; set; }
//    //    public IList<ToolStripContent> Childrens { get; set; }
//    //    public void Click()
//    //    {
//    //    }
//    //}
//    public class TotalItemInventoryView : InGameView
//    {
//        [SerializeField] ToolStripMiao classifys;
//        [SerializeField] UInventoryViewTransfer uInventoryViewUMCtr;
//        void OpenVoxelInven(ToolStripContent c) { OpenInventory(dataCollection?.TotalVoxelItemInventory); }
//        void OpenShapeInven(ToolStripContent c) { OpenInventory(dataCollection?.ShapeInventory); }
//        void OpenBixelInven(ToolStripContent c) { OpenInventory(null); }
//        void OpenMagicWandInven(ToolStripContent c) { OpenInventory(dataCollection?.MagicWandInventory); }
//        List<IToolStripContent> inventorys;
//        protected override void Start()
//        {
//            base.Start();
//            inventorys = new List<IToolStripContent>();
//            inventorys.Add(new ToolStripContent()
//            {
//                Label = "体素",
//                action = OpenVoxelInven,
//            });
//            inventorys.Add(new ToolStripContent()
//            {
//                Label = "形状",
//                action = OpenShapeInven,
//            });
//            inventorys.Add(new ToolStripContent()
//            {
//                Label = "乙素",
//                action = OpenBixelInven,
//            });
//            inventorys.Add(new ToolStripContent()
//            {
//                Label = "行为",
//                action = OpenMagicWandInven,
//            });
//            classifys.Contents = inventorys;
//        }
//        public void OpenInventory(IInventory callBackInventory)
//        {
//            uInventoryViewUMCtr.CallBackInventory = callBackInventory;
//            uInventoryViewUMCtr.FlipPageListViewCtr.RecalculatePageNum(1, true);
//        }
//        GameCassette.IDataCollection dataCollection;
//        protected override void Enter(GameCassette.IDataCollection data)
//        {
//            base.Enter(data);
//            dataCollection = data;
//            uInventoryViewUMCtr.CallBackInventory = data.TotalVoxelItemInventory;
//            uInventoryViewUMCtr.FlipPageListViewCtr.RecalculatePageNum(1, true);
//        }
//        protected override void Exit(GameCassette.IDataCollection data)
//        {
//            base.Exit(data);
//            dataCollection = null;
//            uInventoryViewUMCtr.CallBackInventory = null;
//        }
//    }
//}