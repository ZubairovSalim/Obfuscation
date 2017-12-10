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
                result = r1.Replace(result,"_"+count.ToString());
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
                result = result.Replace("_" + i, Strings[i]);
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
                string s = m.Value.Remove(0, 1);
                s = s.Remove(s.Length - 1, 1);
                s = Encrypt(s, 17);

                string pattern1 = @"(?<!\\w)" + m.Value + "(?!\\w)";
                Regex reg1 = new Regex(pattern1);
                MatchCollection matches1 = reg1.Matches(code);
                result = reg1.Replace(result, "Decrypt("+'\u0022'+ s+'\u0022'+")");
            }

            return result;
        }

        private void btn_Obfuscate_Click(object sender, RoutedEventArgs e)
        {
            string originalCode = txb_OriginalCode.Text;
            string resultCode = RemoveComments(originalCode);
            resultCode = RemoveEntires(resultCode);
            resultCode = ReplaceIdentifiers(resultCode);
            txb_ObfuscatedCode.Text = EncryptStrings(resultCode);
        }
    }
}

//string Decr = "char[] Decrypt(char[] text, int key){char[] Alphabet = "+'\u0022'+"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 _.,!?:;><=+-*"+'\u0022'+";char[] result = new char[sizeof(text) / sizeof(int)];for (int i = 0; i < text.Length; i++){result[i] = Alphabet[(strlen(Alphabet) + (Alphabet.find(text[i]) - key)) % strlen(Alphabet)];}return result;}";