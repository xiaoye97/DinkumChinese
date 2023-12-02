using I2LocPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LocJsonUpgradeTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IJson Json
        {
            get
            {
                if (_json == null)
                {
                    _json = new LitJsonHelper();
                }
                return _json;
            }
        }

        private static IJson _json;
        public static IJson NTJson
        {
            get
            {
                if (_ntjson == null)
                {
                    _ntjson = new NewtonHelper();
                }
                return _ntjson;
            }
        }

        private static IJson _ntjson;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void mergeButton_Click(object sender, RoutedEventArgs e)
        {
            string oldJson = oldJsonTextBox.Text;
            string newJson = newJsonTextBox.Text;
            var oldList = Json.FromJson<List<TextLocData>>(oldJson);
            var newList = Json.FromJson<List<TextLocData>>(newJson);
            // 先转换所有原文为I2Str
            for (int i = 0; i < oldList.Count; i++)
            {
                var item = oldList[i];
                item.Ori = item.Ori.StrToI2Str();
                item.Loc = item.Loc.StrToI2Str();
            }
            for (int i = 0; i < newList.Count; i++)
            {
                var item = newList[i];
                item.Ori = item.Ori.StrToI2Str();
                item.Loc = item.Loc.StrToI2Str();
            }
            for (int i = 0; i < newList.Count; i++)
            {
                var item = newList[i];
                foreach (var old in oldList)
                {
                    if (old.Ori == item.Ori)
                    {
                        item.Loc = old.Loc;
                        break;
                    }
                }
            }
            string output = NTJson.ToJson(newList, true);
            outputJsonTextBox.Text = output;
        }
    }

    public static class TextEx
    {
        public static string newLineChar = "þ";

        public static string StrToI2Str(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            return str.Replace("\n", newLineChar).Replace("\r", newLineChar);
        }

        public static string I2StrToStr(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            return str.Replace(newLineChar, "\n");
        }
    }
}