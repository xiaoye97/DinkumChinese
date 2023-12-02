using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LocJsonUpgradeTools;

namespace I2LocPatch
{
    [Serializable]
    public class TextLocData
    {
        public string Ori;
        public string Loc;

        public TextLocData()
        { }

        public TextLocData(string ori)
        {
            Ori = ori;
        }

        public TextLocData(string ori, string loc)
        {
            Ori = ori;
            Loc = loc;
        }

        public static List<TextLocData> LoadFromTxtFile(string path)
        {
            List<TextLocData> result = new List<TextLocData>();
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
                File.WriteAllText(path, "Key,Chinese");
            }
            else
            {
                var lines = File.ReadAllLines(path);
                if (lines.Length > 1)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i];
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }
                        string[] args = line.Split(new char[] { ',' }, 2);
                        if (args.Length == 2)
                        {
                            result.Add(new TextLocData() { Ori = args[0].I2StrToStr(), Loc = args[1].I2StrToStr() });
                        }
                    }
                }
            }
            return result;
        }

        public static List<TextLocData> LoadFromJsonFile(string path)
        {
            List<TextLocData> result = new List<TextLocData>();
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
                result.Add(new TextLocData() { Ori = "TextLocDataOri", Loc = "TextLocDataLoc" });
                File.WriteAllText(path, MainWindow.Json.ToJson(result, true));
            }
            else
            {
                var json = File.ReadAllText(path);
                try
                {
                    result = MainWindow.Json.FromJson<List<TextLocData>>(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                for (int i = 0; i < result.Count; i++)
                {
                    result[i].Loc = result[i].Loc.I2StrToStr();
                }
            }
            return result;
        }

        public static string GetLoc(List<TextLocData> list, string ori)
        {
            foreach (var loc in list)
            {
                if (loc.Ori == ori)
                {
                    return loc.Loc;
                }
            }
            return ori;
        }
    }
}