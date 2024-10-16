using CatFramework;
using CatFramework.Localized;
using CatFramework.UiMiao;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelWorld.UGUICTR
{
    public class LanguageSelectedView : DropdownMiao
    {
        protected override void Start()
        {
            LocalizedManagerMiao.LanguageCollection.GetAllLanguageNames(optionsData);
            OnSubmit += Selected;
            base.Start();
            SetValueWithoutNotify(0);
        }

        private void Selected(TInputField<int> field)
        {
            int index = field.GetValue();
            LocalizedManagerMiao.SwitchLanguage(optionsData[index]);
            if (ConsoleCat.Enable)
                ConsoleCat.Log($"选择语言:{optionsData[index]}");
        }
    }
}