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
using System.Text.RegularExpressions;
using winForms = System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Obfuscation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 _.,!?:;><=+-*";

        Random r = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }
      
        static string Encrypt(string text, int key)
        {
            string Text = text;
            int Key = key;
            StringBuilder enc_Text = new StringBuilder(Text);

            for (int i = 0; i < Text.Length; i++)
            {
                enc_Text[i] = Alphabet[(Alphabet.IndexOf(Text[i]) + Key) % Alphabet.Length];
            }
            return enc_Text.ToString();
        }
        

        string Permutation(string Alphabet)//пепрестановка заданного состава
        {
            StringBuilder name = new StringBuilder(Alphabet);

            for (int j = Alphabet.Length - 1; j > 0; j--)
            {                
                int n = r.Next(j);
                char num = name[n];
                name[n] = name[j];
                name[j] = num;
            }

            return name.ToString();
        }

        string ReplaceIdentifiers(string code)//замена идентификаторов
        {
            string result = code;
            //сперва защитим от изменений строки
            List<string> Strings = new List<string>();
            string pat = @"[""][\s\S]+?[""]";
            Regex r = new Regex(pat);
            MatchCollection mats = r.Matches(code);
            int count = 0;

            foreach (Match m in mats)
            {
                Strings.Add(m.Value);
                string pat1 = m.Value;
                Regex r1 = new Regex(pat1);
                MatchCollection mats1 = r1.Matches(code);
                result = r1.Replace(result,"_"+count.ToString()+"_");
                count++;
            }

            List<string> KeyWords = new List<string>() { "int", "char", "double", "float", "bool", "void", "class", "struct", "long", "enum", "short","\\[\\]" };            

            string Alphabet = "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            //string result = code;

            Random rand = new Random();

            for (int i = 0;i<KeyWords.Count;i++)
            {
                string pattern = @"(?<=" + KeyWords[i] + " )" + "[a-z][\\w]*";
                RegexOptions options = RegexOptions.IgnoreCase;
                Regex reg = new Regex(pattern,options);
                MatchCollection matches = reg.Matches(code);
                foreach(Match m in matches)
                {
                    if (m.Value != "main")
                    {
                        string name = Permutation(Alphabet);

                        while ("0123456789".IndexOf(name[0]) != -1)
                        {
                            name = Permutation(Alphabet);
                        }

                        int q = rand.Next(1, Alphabet.Length + 1);
                        string identitfier = name.ToString().Substring(0, q);

                        string pattern1 = @"(?<!\w)" + m.Value + "(?!\\w)";
                        Regex reg1 = new Regex(pattern1);
                        MatchCollection matches1 = reg1.Matches(result);
                        result = reg1.Replace(result, identitfier);
                    }
                }                
            }

            for(int i = 0;i<Strings.Count;i++)
            {
                result = result.Replace("_" + i+"_", Strings[i]);
            }

            return result;           
        }

        string RemoveComments(string code)//удаление комментариев
        {
            string pattern = @"//[\S\s]+?\n";
            Regex reg = new Regex(pattern);
            MatchCollection m = reg.Matches(code);
            string result = reg.Replace(code, String.Empty);
            return result;
        }

        string RemoveEntires(string code)//удаение пробелов, переносов
        {
            string pattern = @"(?<=[;{})])\s+?(?=\S)";
            Regex reg = new Regex(pattern);
            MatchCollection m = reg.Matches(code);
            string result = reg.Replace(code, String.Empty);
            return result;
        }

        string EncryptStrings(string code)
        {
            string result = code;

            string pattern = @"[""][\s\S]+?[""]";
            Regex reg = new Regex(pattern);
            MatchCollection matches = reg.Matches(code);
            
            foreach(Match m in matches)
            {
                if (m.Value != "\"pause\"")
                {
                    string s = m.Value.Remove(0, 1);
                    s = s.Remove(s.Length - 1, 1);
                    s = Encrypt(s, 17);

                    string pattern1 = @"(?<!\\w)" + m.Value + "(?!\\w)";
                    Regex reg1 = new Regex(pattern1);
                    MatchCollection matches1 = reg1.Matches(code);
                    result = reg1.Replace(result, "Decrypt(" + '\u0022' + s + '\u0022' + ",17)");
                }
            }

            Regex r2 = new Regex(@"#include <iostream>");
            MatchCollection m2 = r2.Matches(result);
            result = r2.Replace(result, "#include <iostream>#include <string>");

            Regex r1 = new Regex(@"using namespace std;");
            MatchCollection m1 = r1.Matches(result);
            result = r1.Replace(result, "using namespace std;string Decrypt(string text, int key){string result = text;string Alphabet = "+'\u0022'+"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 _.,!?:;><=+-*"+'\u0022'+"; for (int i = 0; i < text.length(); i++){result[i] = Alphabet[(Alphabet.length() + (Alphabet.find(text[i]) - key)) % Alphabet.length()];}return result;}");

            return result;
        }
        private void btn_AddOriginalCode_Click(object sender, RoutedEventArgs e)
        {
            winForms.OpenFileDialog opn = new winForms.OpenFileDialog();
            opn.Filter = "Text Files(*.cpp) | *.cpp";
            if (opn.ShowDialog() == winForms.DialogResult.OK)
            {
                txb_OriginalCode.Text = File.ReadAllText(opn.FileName, Encoding.Default);
            }
        }

        private void btn_Obfuscate_Click(object sender, RoutedEventArgs e)
        {
            string originalCode = txb_OriginalCode.Text;
            string resultCode = RemoveComments(originalCode);
            resultCode = RemoveEntires(resultCode);
            resultCode = ReplaceIdentifiers(resultCode);
            txb_ObfuscatedCode.Text = EncryptStrings(resultCode);
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            winForms.SaveFileDialog save = new winForms.SaveFileDialog();
            save.DefaultExt = ".cpp";

            if (save.ShowDialog() == winForms.DialogResult.OK)
            {
                if((sender as Button).Name=="btn_Save1")
                {
                    File.WriteAllText(save.FileName, txb_OriginalCode.Text, Encoding.Default);
                    txb_OriginalPath.Text = save.FileName;
                }
                else
                {
                    File.WriteAllText(save.FileName, txb_ObfuscatedCode.Text, Encoding.Default);
                    txb_ObfuscatedPath.Text = save.FileName;
                }
                
            }
        }

        private void btn_Run_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Name == "btn_Run1")
            {
                string OriginalPathExe = System.IO.Path.GetDirectoryName(txb_OriginalPath.Text) + "\\" + System.IO.Path.GetFileNameWithoutExtension(txb_OriginalPath.Text) + ".exe";
                Process p = new Process();
                p.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(txb_OriginalPath.Text);
                p.StartInfo.FileName = @"C:\MinGW\bin\g++.exe";                
                p.StartInfo.Arguments = System.IO.Path.GetFileName(txb_OriginalPath.Text)+" -o "+ System.IO.Path.GetFileNameWithoutExtension(txb_OriginalPath.Text)+".exe";
                p.Start();
                p.WaitForExit();
                Process.Start(OriginalPathExe);
            }
            else
            {
                string ObfuscatedPathExe = System.IO.Path.GetDirectoryName(txb_ObfuscatedPath.Text) + "\\" + System.IO.Path.GetFileNameWithoutExtension(txb_ObfuscatedPath.Text) + ".exe";
                Process p = new Process();
                p.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(txb_ObfuscatedPath.Text);
                p.StartInfo.FileName = @"C:\MinGW\bin\g++.exe";
                p.StartInfo.Arguments = System.IO.Path.GetFileName(txb_ObfuscatedPath.Text) + " -o " + System.IO.Path.GetFileNameWithoutExtension(txb_ObfuscatedPath.Text) + ".exe";
                p.Start();
                p.WaitForExit();
                Process.Start(ObfuscatedPathExe);
            }
        }

    }
}
