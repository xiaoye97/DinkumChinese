using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksum;

namespace DinkumChinesePublishTool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            // 建立打包文件夹
            DirectoryInfo buildDir = new DirectoryInfo("DinkumChinesePublish");
            if (buildDir.Exists)
            {
                buildDir.Delete(true);
            }
            buildDir.Create();
            DirectoryInfo buildDir1 = new DirectoryInfo("DinkumChinesePublish/DinkumChinese");
            if (buildDir1.Exists)
            {
                buildDir1.Delete(true);
            }
            buildDir1.Create();
            DirectoryInfo buildDir2 = new DirectoryInfo("DinkumChinesePublish/DinkumChinese_WithBepInEx");
            if (buildDir2.Exists)
            {
                buildDir2.Delete(true);
            }
            buildDir2.Create();
            // 不带Bep的版本
            Console.WriteLine("开始打包不带Bep的版本...");
            // 复制配置文件
            CopyFile("BepInEx/config/xiaoye97.I2LocPatch.cfg", "DinkumChinesePublish/DinkumChinese/BepInEx/config/xiaoye97.I2LocPatch.cfg");
            CopyFile("BepInEx/config/xiaoye97.I18NFont4UnityGame.cfg", "DinkumChinesePublish/DinkumChinese/BepInEx/config/xiaoye97.I18NFont4UnityGame.cfg");
            // 复制插件
            CopyFile("BepInEx/plugins/DinkumChinese.dll", "DinkumChinesePublish/DinkumChinese/BepInEx/plugins/DinkumChinese.dll");
            CopyFile("BepInEx/plugins/I2LocPatch.dll", "DinkumChinesePublish/DinkumChinese/BepInEx/plugins/I2LocPatch.dll");
            CopyFile("BepInEx/plugins/I18NFont4UnityGame.dll", "DinkumChinesePublish/DinkumChinese/BepInEx/plugins/I18NFont4UnityGame.dll");
            CopyFile("BepInEx/plugins/XYModLib.dll", "DinkumChinesePublish/DinkumChinese/BepInEx/plugins/XYModLib.dll");
            CopyFile("BepInEx/plugins/Newtonsoft.Json.dll", "DinkumChinesePublish/DinkumChinese/BepInEx/plugins/Newtonsoft.Json.dll");
            // 复制字体和文本
            CopyDirectory("BepInEx/plugins/I2LocPatch", "DinkumChinesePublish/DinkumChinese/BepInEx/plugins/I2LocPatch");
            CopyDirectory("BepInEx/plugins/I18NFont4UnityGame", "DinkumChinesePublish/DinkumChinese/BepInEx/plugins/I18NFont4UnityGame");
            // 文件夹压缩
            Console.WriteLine("开始压缩不带Bep的版本...");
            ZipFile("DinkumChinesePublish/DinkumChinese", "DinkumChinese_V1_X_0.zip");

            // 带Bep的版本
            Console.WriteLine("开始打包带Bep的版本...");
            // 复制BepInEx
            CopyDirectory("BepInEx/core", "DinkumChinesePublish/DinkumChinese_WithBepInEx/BepInEx/core");
            CopyFile("doorstop_config.ini", "DinkumChinesePublish/DinkumChinese_WithBepInEx/doorstop_config.ini");
            CopyFile("winhttp.dll", "DinkumChinesePublish/DinkumChinese_WithBepInEx/winhttp.dll");
            // 复制配置文件
            CopyFile("BepInEx/config/xiaoye97.I2LocPatch.cfg", "DinkumChinesePublish/DinkumChinese_WithBepInEx/BepInEx/config/xiaoye97.I2LocPatch.cfg");
            CopyFile("BepInEx/config/xiaoye97.I18NFont4UnityGame.cfg", "DinkumChinesePublish/DinkumChinese_WithBepInEx/BepInEx/config/xiaoye97.I18NFont4UnityGame.cfg");
            // 复制插件
            CopyFile("BepInEx/plugins/DinkumChinese.dll", "DinkumChinesePublish/DinkumChinese_WithBepInEx/BepInEx/plugins/DinkumChinese.dll");
            CopyFile("BepInEx/plugins/I2LocPatch.dll", "DinkumChinesePublish/DinkumChinese_WithBepInEx/BepInEx/plugins/I2LocPatch.dll");
            CopyFile("BepInEx/plugins/I18NFont4UnityGame.dll", "DinkumChinesePublish/DinkumChinese_WithBepInEx/BepInEx/plugins/I18NFont4UnityGame.dll");
            CopyFile("BepInEx/plugins/XYModLib.dll", "DinkumChinesePublish/DinkumChinese_WithBepInEx/BepInEx/plugins/XYModLib.dll");
            CopyFile("BepInEx/plugins/Newtonsoft.Json.dll", "DinkumChinesePublish/DinkumChinese_WithBepInEx/BepInEx/plugins/Newtonsoft.Json.dll");
            // 复制字体和文本
            CopyDirectory("BepInEx/plugins/I2LocPatch", "DinkumChinesePublish/DinkumChinese_WithBepInEx/BepInEx/plugins/I2LocPatch");
            CopyDirectory("BepInEx/plugins/I18NFont4UnityGame", "DinkumChinesePublish/DinkumChinese_WithBepInEx/BepInEx/plugins/I18NFont4UnityGame");
            // 文件夹压缩
            Console.WriteLine("开始压缩带Bep的版本...");
            ZipFile("DinkumChinesePublish/DinkumChinese_WithBepInEx", "DinkumChinese_V1_X_0_WithBepInEx.zip");
            sw.Stop();
            Console.WriteLine($"执行完毕，共耗时{sw.ElapsedMilliseconds}ms");
            Console.ReadLine();
        }

        public static void CopyDirectory(string srcPath, string destPath, List<string> ignoreDirs = null, List<string> ignoreFiles = null)
        {
            try
            {
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                if (ignoreDirs == null) ignoreDirs = new List<string>();
                if (ignoreFiles == null) ignoreFiles = new List<string>();
                DirectoryInfo dir = new DirectoryInfo(srcPath); FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    // 判断是否文件夹
                    if (i is DirectoryInfo)
                    {
                        // 检查是否需要忽略，是的话就跳过
                        if (ignoreDirs.Contains(i.Name))
                        {
                            continue;
                        }
                        if (!Directory.Exists(destPath + "\\" + i.Name))
                        {
                            // 目标目录下不存在此文件夹即创建子文件夹
                            Directory.CreateDirectory(destPath + "\\" + i.Name);
                        }
                        // 递归调用复制子文件夹
                        CopyDirectory(i.FullName, destPath + "\\" + i.Name, ignoreDirs, ignoreFiles);
                    }
                    else
                    {
                        // 检查是否需要忽略，是的话就跳过
                        if (ignoreFiles.Contains(i.Name))
                        {
                            continue;
                        }
                        // 不是文件夹即复制文件，true表示可以覆盖同名文件
                        File.Copy(i.FullName, destPath + "\\" + i.Name, true);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void CopyFilesWithSearch(string srcPath, string destPath, List<string> fileContainsName = null)
        {
            try
            {
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                if (fileContainsName == null) fileContainsName = new List<string>();
                DirectoryInfo dir = new DirectoryInfo(srcPath); FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is FileInfo)
                    {
                        bool canCopy = false;
                        // 检查是否需要复制
                        foreach (var name in fileContainsName)
                        {
                            if (i.Name.Contains(name))
                            {
                                canCopy = true;
                                break;
                            }
                        }
                        if (canCopy)
                        {
                            // 不是文件夹即复制文件，true表示可以覆盖同名文件
                            File.Copy(i.FullName, destPath + "\\" + i.Name, true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void CopyFile(string srcPath, string destPath)
        {
            FileInfo src = new FileInfo(srcPath);
            if (src.Exists)
            {
                FileInfo dest = new FileInfo(destPath);
                if (!dest.Directory.Exists)
                {
                    dest.Directory.Create();
                }
                File.Copy(src.FullName, destPath, true);
            }
        }

        #region 文件压缩

        /// <summary>
        /// 将文件夹压缩
        /// </summary>
        /// <param name="srcFiles">文件夹路径</param>
        /// <param name="strZip">压缩之后的名称</param>
        public static void ZipFile(string srcFiles, string strZip)
        {
            ZipOutputStream zipStream = null;
            try
            {
                var len = srcFiles.Length;
                var strlen = srcFiles[len - 1];
                if (srcFiles[srcFiles.Length - 1] != Path.DirectorySeparatorChar)
                {
                    srcFiles += Path.DirectorySeparatorChar;
                }
                zipStream = new ZipOutputStream(File.Create(strZip));
                zipStream.SetLevel(6);
                zip(srcFiles, zipStream, srcFiles);
                zipStream.Finish();
                zipStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Clear Resource
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
            }
        }

        /// <summary>
        /// 将文件夹压缩
        /// </summary>
        /// <param name="srcFiles">文件夹路径</param>
        /// <param name="outstream">压缩包流</param>
        /// <param name="strZip">压缩之后的名称</param>
        public static void zip(string srcFiles, ZipOutputStream outstream, string staticFile)
        {
            if (srcFiles[srcFiles.Length - 1] != Path.DirectorySeparatorChar)
            {
                srcFiles += Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32();
            //获取指定目录下所有文件和子目录文件名称
            string[] filenames = Directory.GetFileSystemEntries(srcFiles);
            //遍历文件
            foreach (string file in filenames)
            {
                if (Directory.Exists(file))
                {
                    zip(file, outstream, staticFile);
                }
                //否则，直接压缩文件
                else
                {
                    //打开文件
                    FileStream fs = File.OpenRead(file);
                    //定义缓存区对象
                    byte[] buffer = new byte[fs.Length];
                    //通过字符流，读取文件
                    fs.Read(buffer, 0, buffer.Length);
                    //得到目录下的文件（比如:D:\Debug1\test）,test
                    string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempfile);
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    outstream.PutNextEntry(entry);
                    //写文件
                    outstream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        #endregion 文件压缩
    }
}