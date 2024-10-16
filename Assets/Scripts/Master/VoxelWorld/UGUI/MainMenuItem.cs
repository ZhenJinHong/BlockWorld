using CatFramework;
using CatFramework.EventsMiao;
using CatFramework.UiMiao;
using System;
using System.Collections;
using UnityEngine;

namespace VoxelWorld.UGUICTR
{
    public enum GameState
    {
        All = 0,
        HomePage = 1,
        Ingame = 2,
    }
    public class MainMenuItem : ButtonMiao
    {
        [SerializeField] GameState showWhen;
        void Flush(bool ingame)
        {
            if (showWhen == GameState.All)
            {
                gameObject.SetActive(true);
            }
            else
            {
                if (ingame)
                {
                    gameObject.SetActive(showWhen == GameState.Ingame);
                }
                else
                {
                    gameObject.SetActive(showWhen == GameState.HomePage);
                }
            }

        }
        public static void Flush(RectTransform parent, bool ingame)
        {
            if (Assert.IsNull(parent)) return;
            var mainMenuItems = parent.GetComponentsInChildren<MainMenuItem>(true);
            if (mainMenuItems != null)
            {
                for (int i = 0; i < mainMenuItems.Length; i++)
                {
                    mainMenuItems[i].Flush(ingame);
                }
            }
        }
    }
}