using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public class ConsoleViewCtr : PageListViewUMCtr<ButtonMiao, IReadOnlyList<string>>
    {
        public override int ItemCount => ConsoleCat.MessageCount;

        protected override void BindItem(int itemIndex, ButtonMiao visualItem)
        {
            visualItem.LabelNoTranslate = ConsoleCat.GetConsoleMessage(itemIndex).Message;
        }

        protected override void UnBindItem(int itemIndex, ButtonMiao visualItem)
        {
            visualItem.LabelNoTranslate = string.Empty;
        }
    }
    public class ConsoleView : PageListViewTransfer<ButtonMiao, IReadOnlyList<string>, ConsoleViewCtr>
    {
        [SerializeField] ViewUMCtrBase viewUMCtrBase;
        protected override void Start()
        {
            base.Start();
            viewUMCtrBase.OnOpen += Open;
            viewUMCtrBase.OnClose += Close;
            ConsoleCat.ShootText += Show;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            viewUMCtrBase.OnOpen -= Open;
            viewUMCtrBase.OnClose -= Close;
            ConsoleCat.ShootText -= Show;
        }
        void Open(ViewUMCtrBase viewUMCtrBase) => Open();
        void Close(ViewUMCtrBase viewUMCtrBase) => Close();
        void Show(ConsoleMessage consoleMessage)
        {
            //viewUMCtrBase.Open();
        }
    }
}