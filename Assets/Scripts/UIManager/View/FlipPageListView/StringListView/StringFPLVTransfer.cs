using System;
using System.Collections;
using System.Collections.Generic;

namespace CatFramework.UiMiao
{
    public class StringFPLVTransfer : PageListViewTransfer<ButtonMiao, IReadOnlyList<string>, StringFPLVUMCtr>
    {
        public event Action<int> OnSelectedItem
        {
            add => viewController.OnSelectedItem += value;
            remove => viewController.OnSelectedItem -= value;
        }
    }
}