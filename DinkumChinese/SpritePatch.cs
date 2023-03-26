using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace DinkumChinese
{
    public class SpritePatch
    {
        /// <summary>
        /// 尝试替换Image图片
        /// </summary>
        /// <param name="objPath"></param>
        /// <param name="imagePath"></param>
        public static void TryReplaceImageSprite(string objPath, string imagePath)
        {
            var go = GameObject.Find(objPath);
            if (go != null)
            {
                var image = go.GetComponent<Image>();
                if (image != null)
                {
                    Sprite sprite = FileHelper.LoadSprite(imagePath);
                    if (sprite != null)
                    {
                        image.sprite = sprite;
                    }
                }
            }
        }
    }
}