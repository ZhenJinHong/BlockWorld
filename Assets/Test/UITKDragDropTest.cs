//using CatFramework;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UIElements;

//namespace Assets.Test
//{
//    public class UITKDragDropTest : SmallPanel, ILatticeViewCallBack
//    {
//        [SerializeField] Texture2D Texture2D;
//        [SerializeField] Texture2D[] Items;
//        public int ItemCount => latticeItems.Length;

//        public LatticeItem GetLatticeItem(int index)
//        {
//            return latticeItems[index];
//        }
//        LatticeItem[] latticeItems;
//        LatticeItem[] latticeBarItems;
//        protected override void Start()
//        {
//            base.Start();
//            latticeItems = new LatticeItem[Items?.Length ?? 0];
            
//            for (int i = 0; i < latticeItems.Length; i++)
//            {
//                latticeItems[i] = new LatticeItem(i) { Icon = Items[i] };
//            }
//            latticeBarItems = new LatticeItem[9];
          
//            for (int i = 0; i < latticeBarItems.Length; i++)
//            {
//                latticeBarItems[i] = new LatticeItem(i);
//            }
//            //VisualElement visualElement = new VisualElement();
//            //visualElement.AddToClassList(VisualElementControl.latticeViewClass);
//            //Image image = new Image();
//            //image.image = Texture2D;
//            //visualElement.Add(image);
//            //Root.Add(visualElement);
//            LatticeView latticeView = new LatticeView(4, 5, this/*, Texture2D*/);
//            LatticeBar latticeBar = new LatticeBar(latticeView, latticeBarItems);

//            Root.Add(latticeView);
//            Root.Add(latticeBar);
//            Root.Add(latticeView.GetShareLatticeImage());
//            //DragAndDropManipulator dragAndDropManipulator = new DragAndDropManipulator(Root.Q<VisualElement>("target"));
//        }

//        public bool SwapLatticeItem(LatticeItem x, LatticeItem y)
//        {
//            if(x == null || y == null)
//            {
//                if (x == null) Debug.Log("X空");
//                if (y == null) Debug.Log("Y空");
//                return false;
//            }
//            Texture2D t = x.Icon;
//            x.Icon = y.Icon;
//            y.Icon = t;
//            return true;
//        }
//    }
//}