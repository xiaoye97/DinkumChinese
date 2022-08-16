using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace DinkumChinese
{
    public static class FileHelper
    {
        public static Dictionary<string, Sprite> SpriteCache = new Dictionary<string, Sprite>();

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Sprite LoadSprite(string path)
        {
            if (SpriteCache.ContainsKey(path))
            {
                return SpriteCache[path];
            }
            try
            {
                if (File.Exists(path))
                {
                    var byets = File.ReadAllBytes(path);
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(byets);
                    Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    SpriteCache[path] = sprite;
                    return sprite;
                }
            }
            catch
            {
            }
            return null;
        }
    }
}