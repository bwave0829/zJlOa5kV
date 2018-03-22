using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

//
//  プロセス名とウィンドウタイトルが一致したアプリにメッセージを送る
//

//
//  参照で「Microsoft.VisualBasic」を追加すること
//  （Microsoft.VisualBasic.Interaction.AppActivateを使用するため）
//

namespace FindWindow
{

    class Program
    {
        public static ArrayList confs = new ArrayList();


        /// <summary>正規表現による文字列置換</summary>
        /// <param name="rex">置換条件（正規表現：文字列）</param>
        /// <param name="dst">置換後（文字列）</param>
        /// <param name="target">置換対象（文字列）</param>
        /// <returns>置換された後の文字列全体</returns>
        public static string MyReplace(string rex, string dst, string target) {
            string ret_value = "";
            try {
                ret_value = String.Copy(target);

                Regex r = new Regex(rex);
                ret_value = r.Replace(ret_value, dst);
                return (ret_value);
            } catch (Exception ex) {
                Debug.Print("{0}", ex.Message);
                return (ret_value);
            }
        }


        //  プロセス名とウィンドウタイトルが一致したアプリにメッセージを送る
        //  ・プロセス名：完全一致
        //  ・ウィンドウタイトル：部分一致
        static void Winactivate(String procname,String wintitle, String msg, int waitsec)
        {
            Process[] ps =　Process.GetProcessesByName(procname);
            for( int winpos = 0; winpos < ps.Length; winpos++)
            {
                Debug.WriteLine("Title", ps[winpos].MainWindowTitle);
                if (0 <= ps[winpos].MainWindowTitle.IndexOf(wintitle))
                {
                    //ウィンドウをアクティブにする
                    Microsoft.VisualBasic.Interaction.AppActivate(ps[winpos].Id);

                    //  キーを送る
                    SendKeys.SendWait(msg);
                    System.Threading.Thread.Sleep(waitsec * 1000);
                }
            }
        }


        /// <summary>設定ファイルを読み込む</summary>
        /// <param name="confname">設定ファイル名</param>
        /// <returns>無し</returns>
        static void ReadConf(String confname, String[] rexs)
        { 
            String line = "";
            try { 
                using (StreamReader sr = new StreamReader(confname, Encoding.GetEncoding("Shift_JIS"))) {
                    while ((line = sr.ReadLine()) != null) {
                        for (int cnt = 0 ; cnt < rexs.Length; cnt++) {
                            line = MyReplace(String.Format("@{0}", cnt + 1), rexs[cnt], line);
                        }
                        Program.confs.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Main(string[] args)
        {
            if( args.Length < 3)
            {
                Console.WriteLine("conf_file wait_sec [rex1 rex2 .. rexN]");
                Environment.Exit(0);
            }
            String[] rexs = new String[args.Length-2];

            for (int cnt = 2; cnt < args.Length; cnt++) {
                rexs[cnt - 2] = args[cnt];
            }
            ReadConf(args[0], rexs);
            for (int i = 0; i < Program.confs.Count; i++) {
                Console.WriteLine("{0}", Program.confs[i]);
            }
            Environment.Exit(0);

            for (int i = 0; i < Program.confs.Count; i++)
            {
                if (Program.confs[i].ToString().Substring(0, 1).Equals("#")) continue;

                var mem = Program.confs[i].ToString().Split(new char[] { ',' });
                if(mem.Length == 3) { 
                    Winactivate(mem[0], mem[1], mem[2], int.Parse(args[1]));
                }
            }
        }
    }
}
