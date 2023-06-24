using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MathNet.Numerics;

namespace math
{
    class Global
    {
        public static string version = "2.2.21";
        public static string testkey = Guid.NewGuid().ToString("N");
        public static bool update = false;
        public static bool devmode = false;
        //public static int a;
    }

    class Program
    {
        public static void ModeSelect(string Answer)
        {
            Console.WriteLine();

            if (Answer != "")
            {
                Console.WriteLine("前回の計算の答えは " + Answer);
                Console.WriteLine();
                Console.WriteLine();
            }

            if (Global.devmode == true)
            {
                Console.WriteLine("開発者モード有効");
                Console.WriteLine();

            }

            Console.WriteLine("操作を選択");
            Console.WriteLine("1 : 文字を含まない計算");
            Console.WriteLine("2 : 微分");
            //Console.WriteLine("2 : 文字を含まない計算(小数を含む)");
            Console.WriteLine("3 : 1次式の解");
            Console.WriteLine("4 : 2次式の解");
            Console.WriteLine("5 : 不定積分");
            Console.WriteLine("6 : 定積分");
            Console.WriteLine("7 : 計算練習");
            Console.WriteLine("8 : 辞書モード");
            Console.WriteLine("9 : 定積分(MathNet.Numerics.Integrate.GaussKronrod使用),(開発中)");
            Console.WriteLine("10 : 三角比の値");
            Console.WriteLine("11 : 平方根の値");
            Console.WriteLine("------------------------------");
            if (Global.update == true)
            {
                Console.WriteLine("100 : アップデートリリースページを開く");
            }

            if (Global.devmode == true)
            {
                Console.WriteLine("1001 : 設定用キーの取得");
                Console.WriteLine("1002 : Google Classroomを開く");
                Console.WriteLine("1003 : チャートを開く");
                //Console.WriteLine("1004 : 辞書モード");
                Console.WriteLine();
            }

            Console.WriteLine("0 : 終了");
            Console.WriteLine();
            Console.Write("Input:");
            try
            {
                string mode = Console.ReadLine();
                if (mode == Global.testkey)
                {
                    Console.Clear();
                    Test();
                }
                else
                {
                    int SelectedMode = int.Parse(mode);
                    Console.Clear();
                    CheckMode(SelectedMode);
                }
            }
            catch(OverflowException)
            {
                ExceptionHandler("System.OverflowException", "値がintの規定値を超えています");
            }
            catch (FormatException)
            {
                ExceptionHandler("System.FormatException", "int型以外の値が入力されました");
            }
        }

        public static void CheckMode(int Mode)
        {
            //int CalcMode = ModeSelect("");

            //Math
            if (Mode == 1)
            {
                //通常計算
                Calc.Mode1();
            }
            else if (Mode == 2)
            {
                //微分
                Calc.Mode2();
            }
            else if (Mode == 3)
            {
                //1次式計算
                Calc.Mode3();
            }
            else if (Mode == 4)
            {
                //2次式計算
                Calc.Mode4();
            }
            else if (Mode == 5)
            {
                //不定積分
                Calc.Mode5(false);
            }
            else if (Mode == 6)
            {
                //定積分
                Calc.Mode5(true);
            }
            else if (Mode == 7)
            {
                //計算練習
                try
                {
                    Calc.Mode7();
                }
                catch (OverflowException)
                {
                    ExceptionHandler("System.OverflowException", "値がintの規定値を超えています");
                }
                catch (FormatException)
                {
                    ExceptionHandler("System.FormatException", "int型以外の値が入力されました");
                }
            }
            else if (Mode == 9)
            {
                Calc.Mode9();
            }
            else if (Mode == 10)
            {
                //三角比の値
                Calc.Mode10();
            }
            else if (Mode == 11)
            {
                //平方根の値
                Calc.Mode11();
            }
            
            

             //s
             else if (Mode == 0)
            {
                Exit();
            }
             else if (Mode == 100)
            {
                if (Global.update == true)
                {
                    //Process.Start("msedge.exe", "https://github.com/nfmcpwr/Math/releases/");
                    OpenUrl("https://github.com/nfmcpwr/Math/releases/");
                }
                else
                {
                    ModeSelect("");
                }
            }
             else if (Mode == 1001 && Global.devmode == true)
            {
                Console.WriteLine(Global.testkey);
                ModeSelect("");
            }
            else if (Mode == 1002 && Global.devmode == true)
            {
                //System.Diagnostics.Process.Start("explorer.exe", "msedge https://classroom.google.com/c/NjAzNDU1NDUxMTU4");
                OpenUrl("https://classroom.google.com/c/NjAzNDU1NDUxMTU4");
            }
            else if (Mode == 1003 && Global.devmode == true)
            {
                string name = DataManager.Activity.File.ReadFileActivity("math","chartlogin.id");
                string key = DataManager.Activity.File.ReadFileActivity("math", "chartlogin.key");
                OpenUrl("https://sviewer.jp/books/index.html?name=" + name + "&password=" + key);
            }
            else if (Mode == 8)
            {
                //辞書モード
                DicMode();
            }
            else
            {
                ModeSelect("");
            }
            
        }

        static void Exit()
        {
            //exit
        }

        static void Test()
        {
            Console.WriteLine("チャートログイン情報の設定");
            //DataManager.Activity.CheckDirectoryActivity("math");
            Console.Write("ID:");
            string id = Console.ReadLine();
            //DataManager.Activity.CheckFileActivity("math", "chartlogin.id");
            DataManager.Activity.File.WriteFileActivity("math", "chartlogin.id",id);
            Console.WriteLine();
            Console.Write("パスワード:");
            string pw = Console.ReadLine();
            //DataManager.Activity.CheckFileActivity("math", "chartlogin.key");
            DataManager.Activity.File.WriteFileActivity("math", "chartlogin.key", pw);

        }

        static void DicMode()
        {
            Console.Clear();
            Console.WriteLine("辞書モード(exitで終了)");
            Console.Write("検索する単語を入力:");
            string w = Console.ReadLine();
            if (w == "exit")
            {
                ModeSelect("");
            }
            else
            {
                try
                {
                    StreamReader streamReader = new StreamReader(".\\dicdata\\" + w, Encoding.UTF8);
                    string b6 = streamReader.ReadToEnd();
                    Console.WriteLine();
                    Console.Write("意味:");
                    Console.WriteLine(Encoding.UTF8.GetString(Convert.FromBase64String(b6)));
                    Console.ReadLine();
                    DicMode();
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine();
                    Console.WriteLine("単語が辞書に登録されていません");
                    Console.Write("辞書データを追加しますか?(y/n):");
                    string t = Console.ReadLine();
                    if (t == "y")
                    {
                        ProcessStartInfo processStartInfo = new ProcessStartInfo()
                        {
                            FileName = "adddicdata.bat",
                            UseShellExecute = true,
                            //Arguments = "adddicdata.bat"
                            //WorkingDirectory = ".\\dicdata"
                        };

                        Process.Start(processStartInfo).WaitForExit();
                        
                        
                        //Process.Start(".\\dicdata\\newdicdata.exe").WaitForExit();
                        DicMode();
                    }
                    else
                    {
                        DicMode();
                    }
                    
                }
            }

        }

        static Process OpenUrl(string url)
        {
            ProcessStartInfo pi = new ProcessStartInfo()
            {
                FileName = url,
                UseShellExecute = true,
            };

            return Process.Start(pi);
        }

        static async Task GetUpdate()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("math", Global.version));
            HttpResponseMessage httpResponse = await httpClient.GetAsync("https://api.github.com/repos/nfmcpwr/Math/releases/latest");
            if (httpResponse.IsSuccessStatusCode == true)
            {
                string json = await httpResponse.Content.ReadAsStringAsync();
                //Console.WriteLine(json);
                JsonDocument jsonDocument = JsonDocument.Parse(json);
                JsonElement element = jsonDocument.RootElement;
                element.TryGetProperty("tag_name", out JsonElement value);
                string version = value.GetString();
                //Console.WriteLine(version);
                if (version != "v" + Global.version)
                {
                    Console.WriteLine();
                    Console.WriteLine("アップデートが利用可能:" + version);
                    Console.WriteLine();
                    Global.update = true;
                }
            }
            else
            {
                Console.WriteLine("アップデートデータの取得に失敗:" + httpResponse.StatusCode);
            }
            ModeSelect("");
        }

        static void ConnectionCheck()
        {
            Ping ping = new Ping();
            //int c = 1;
            try
            {
                PingReply reply = ping.Send("google.com");
                //Console.WriteLine(reply.Status);
                Task task = GetUpdate();
                task.Wait();
                
            }
            catch (PingException)
            {
                ModeSelect("");
            }
            
            
        }

        public static void ExceptionHandler(string Exception,string d)
        {
            Console.Clear();
            Console.WriteLine("ExceptionHandler");
            Console.WriteLine("version: " + Global.version);
            Console.WriteLine("内容: " + Exception);
            Console.WriteLine();
            Console.WriteLine(d);
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            DataManager.Activity.Root.CheckRootDirActivity();
            DataManager.Activity.CheckDirectoryActivity("math");
            DataManager.Activity.CheckFileActivity("math", "chartlogin.id");
            DataManager.Activity.CheckFileActivity("math", "chartlogin.key");

            if (File.Exists(Path.Combine(DataManager.GlobalVariables.DataPath,"math","dev")) == false)
            {
                DataManager.Activity.CheckFileActivity("math", "dev");
                Exit();
            }

            Console.WriteLine("Math v" + Global.version);
            if (0 < args.Length && args[0] == "BFC73BBA0F6D4883A3EDC5B905455ED1")
            {
                Global.devmode = true;
            }
            ConnectionCheck();

            /*if (NetworkInterface.GetIsNetworkAvailable() == true)
            {
                Task task = GetUpdate();
                task.Wait();
                ModeSelect("");
            }
            else
            {
                ModeSelect("");
            }*/
            //ModeSelect("");
        }
    }

    public class Calc
    {
        public static void Mode1()
        {
            Console.WriteLine("Selected Mode:1");
            Console.WriteLine("数式を入力");
            Console.Write("Input:");
            try
            {
                string Input = Console.ReadLine();
                DataTable dataTable = new DataTable();
                var Result = dataTable.Compute(Input, "");
                Program.ModeSelect(Convert.ToString(Result));
            }
            catch (EvaluateException)
            {
                Program.ExceptionHandler("System.Data.EvaluateException","値に文字が含まれています");
            }
        }

        /*public static void Mode2()
        {
            Console.WriteLine("Selected Mode:2");
            Console.WriteLine("数式を入力");
            Console.Write("Input:");
            string Input = Console.ReadLine();
            DataTable dataTable = new DataTable();
            var Result = dataTable.Compute(Input, "");
            Program.ModeSelect(Convert.ToString(Result));
        }*/

        public static void Mode3()
        {
            Console.WriteLine("Selected Mode:3");
            Console.WriteLine("ax + b = c");
            Console.Write("a:");
            try
            {
                int a = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("b:");
                int b = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("c:");
                int c = int.Parse(Console.ReadLine());
                Console.WriteLine();
                double x = (c - b) / a;

                Program.ModeSelect(Convert.ToString(x));
            }
            catch(OverflowException)
            {
                Program.ExceptionHandler("System.OverflowException", "値がintの規定値を超えています");
            }
            catch (FormatException)
            {
                Program.ExceptionHandler("System.FormatException", "int型でない値が入力されました");
            }

        }

        public static void Mode4()
        {
            Console.WriteLine("Selected Mode:4");
            Console.WriteLine("ax^2 + bx + c = 0");
            Console.Write("a:");
            try
            {
                int a = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("b:");
                int b = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("c:");
                int c = int.Parse(Console.ReadLine());

                Console.WriteLine();
                double x = (-1 * b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                double x2 = (-1 * b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                string Result = Convert.ToString(x) + "," + Convert.ToString(x2);

                if (x == x2)
                {
                    Result = x + ",重解";
                }

                Program.ModeSelect(Result);
            }
            catch(OverflowException)
            {
                Program.ExceptionHandler("System.OverflowException", "値がintの規定値を超えています");
            }
            catch (FormatException)
            {
                Program.ExceptionHandler("System.FormatException", "int型でない値が入力されました");
            }
        }

        public static void Mode5(bool Tei)
        {
            try
            {

                if (Tei == true)
            {
                Console.WriteLine("Selected Mode:6");
            }
            else
            {
                Console.WriteLine("Selected Mode:5");
            }

            Console.Write("項数を入力:");
            int kousuu = int.Parse(Console.ReadLine());
            Console.WriteLine();
            if (kousuu == 1)
            {
                Console.WriteLine("∫ax^p dx");
                Console.Write("a:");
                int a = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("p:");
                int p = int.Parse(Console.ReadLine());
                Console.WriteLine();

                int ap = p + 1;
                string ans = "(" + Convert.ToString(a) + " / " + Convert.ToString(ap) + ") x^" + ap + " + C";

                if (Tei == true)
                {
                    Calc.Teiseki(1, a + " / " + ap, ap, "", null, "", null, "", null, "", null);
                }
                else
                {
                    Program.ModeSelect(ans);
                }
            }
            else if (kousuu == 2)
            {
                Console.WriteLine("∫ax^p + bx^q dx");
                Console.Write("a:");
                int a = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("p:");
                int p = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("b:");
                int b = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("q:");
                int q = int.Parse(Console.ReadLine());
                Console.WriteLine();

                int ap = p + 1;
                int bq = q + 1;
                string ans1 = "(" + Convert.ToString(a) + " / " + Convert.ToString(ap) + ") x^" + ap;
                string ans2 = "(" + Convert.ToString(b) + " / " + Convert.ToString(bq) + ") x^" + bq;
                if (Tei == true)
                {
                    Calc.Teiseki(2, a + " / " + ap, ap, b + " / " + bq, bq, "", null, "", null, "", null);
                }
                else
                {
                    Program.ModeSelect(ans1 + " + " + ans2 + " + C");
                }
            }
            else if (kousuu == 3)
            {
                Console.WriteLine("∫ax^p + bx^q + cx^r dx");
                Console.Write("a:");
                int a = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("p:");
                int p = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("b:");
                int b = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("q:");
                int q = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("c:");
                int c = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("r:");
                int r = int.Parse(Console.ReadLine());
                Console.WriteLine();

                int ap = p + 1;
                int bq = q + 1;
                int cr = r + 1;
                string ans1 = "(" + Convert.ToString(a) + " / " + Convert.ToString(ap) + ") x^" + ap;
                string ans2 = "(" + Convert.ToString(b) + " / " + Convert.ToString(bq) + ") x^" + bq;
                string ans3 = "(" + Convert.ToString(c) + " / " + Convert.ToString(cr) + ") x^" + cr;
                if (Tei == true)
                {
                    Calc.Teiseki(3, a + " / " + ap, ap, b + " / " + bq, bq, c + " / " + cr, cr, "", null, "", null);
                }
                else
                {
                    Program.ModeSelect(ans1 + " + " + ans2 + " + " + ans3 + " + C");
                }
            }
            else if (kousuu == 4)
            {
                Console.WriteLine("∫ax^p + bx^q + cx^r + dx^s dx");
                Console.Write("a:");
                int a = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("p:");
                int p = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("b:");
                int b = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("q:");
                int q = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("c:");
                int c = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("r:");
                int r = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("d:");
                int d = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("s:");
                int s = int.Parse(Console.ReadLine());
                Console.WriteLine();

                int ap = p + 1;
                int bq = q + 1;
                int cr = r + 1;
                int ds = s + 1;
                string ans1 = "(" + Convert.ToString(a) + " / " + Convert.ToString(ap) + ") x^" + ap;
                string ans2 = "(" + Convert.ToString(b) + " / " + Convert.ToString(bq) + ") x^" + bq;
                string ans3 = "(" + Convert.ToString(c) + " / " + Convert.ToString(cr) + ") x^" + cr;
                string ans4 = "(" + Convert.ToString(d) + " / " + Convert.ToString(ds) + ") x^" + ds;
                if (Tei == true)
                {
                    Calc.Teiseki(4, a + " / " + ap, ap, b + " / " + bq, bq, c + " / " + cr, cr, d + " / " + ds, ds, "", null);
                }
                else
                {
                    Program.ModeSelect(ans1 + " + " + ans2 + " + " + ans3 + " + " + ans4 + " + C");
                }
            }
            else if (kousuu == 5)
            {
                Console.WriteLine("∫ax^p + bx^q + cx^r + dx^s + ex^t dx");
                Console.Write("a:");
                int a = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("p:");
                int p = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("b:");
                int b = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("q:");
                int q = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("c:");
                int c = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("r:");
                int r = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("d:");
                int d = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("s:");
                int s = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("e:");
                int e = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("t:");
                int t = int.Parse(Console.ReadLine());
                Console.WriteLine();

                int ap = p + 1;
                int bq = q + 1;
                int cr = r + 1;
                int ds = s + 1;
                int et = t + 1;
                string ans1 = "(" + Convert.ToString(a) + " / " + Convert.ToString(ap) + ") x^" + ap;
                string ans2 = "(" + Convert.ToString(b) + " / " + Convert.ToString(bq) + ") x^" + bq;
                string ans3 = "(" + Convert.ToString(c) + " / " + Convert.ToString(cr) + ") x^" + cr;
                string ans4 = "(" + Convert.ToString(d) + " / " + Convert.ToString(ds) + ") x^" + ds;
                string ans5 = "(" + Convert.ToString(e) + " / " + Convert.ToString(et) + ") x^" + et;
                if (Tei == true)
                {
                    Calc.Teiseki(5, a + " / " + ap, ap, b + " / " + bq, bq, c + " / " + cr, cr, d + " / " + ds, ds, e + " / " + et, et);
                }
                else
                {
                    Program.ModeSelect(ans1 + " + " + ans2 + " + " + ans3 + " + " + ans4 + " + " + ans5 + " + C");
                }
            }
        }
            catch (FormatException)
            {
                Program.ExceptionHandler("System.FormatException", "int型でない値が入力されました");
            }
            catch(OverflowException)
            {
                Program.ExceptionHandler("System.OverflowException", "値がintの規定値を超えています");
            }
        }

        public static void Teiseki(int kousuu,string F1_1,int? F1_2,string F2_1,int? F2_2,string F3_1,int? F3_2,string F4_1,int? F4_2,string F5_1,int? F5_2) //ax^p + bx^q = a,p,b,q
        {
            Console.WriteLine("積分区間の入力");
            Console.WriteLine("b ");
            Console.WriteLine("∫f(x) dx");
            Console.WriteLine("a ");
            Console.WriteLine();
            Console.Write("b:");
            int b = int.Parse(Console.ReadLine());
            Console.WriteLine();
            Console.Write("a:");
            int a = int.Parse(Console.ReadLine());
            Console.WriteLine();

            string answer;

            if (kousuu == 1)
            {
                answer = F1_1 + " * " + Math.Pow(b, (double)F1_2) + " - " + F1_1 + " * " + Math.Pow(a, (double)F1_2);
            }
            else if (kousuu == 2)
            {
                answer = "(" + F1_1 + " * " + Math.Pow(b, (double)F1_2) + F2_1 + " * " + Math.Pow(b, (double)F2_2) + ") - (" + F1_1 + " * " + Math.Pow(a, (double)F1_2) + F2_1 + " * " + Math.Pow(a, (double)F2_2) + ")";
            }
            else if (kousuu == 3)
            {
                answer = "(" + F1_1 + " * " + Math.Pow(b, (double)F1_2) + F2_1 + " * " + Math.Pow(b, (double)F2_2) + F3_1 + " * " + Math.Pow(b, (double)F3_2) + ") - (" + F1_1 + " * " + Math.Pow(a, (double)F1_2) + F2_1 + " * " + Math.Pow(a, (double)F2_2) + F3_1 + " * " + Math.Pow(a, (double)F3_2) + ")";
            }
            else if (kousuu == 4)
            {
                answer = "(" + F1_1 + " * " + Math.Pow(b, (double)F1_2) + F2_1 + " * " + Math.Pow(b, (double)F2_2) + F3_1 + " * " + Math.Pow(b, (double)F3_2) + F4_1 + " * " + Math.Pow(b, (double)F4_2) + ") - (" + F1_1 + " * " + Math.Pow(a, (double)F1_2) + F2_1 + " * " + Math.Pow(a, (double)F2_2) + F3_1 + " * " + Math.Pow(a, (double)F3_2) + F4_1 + " * " + Math.Pow(a, (double)F4_2) + ")";
            }
            else if (kousuu == 5)
            {
                answer = "(" + F1_1 + " * " + Math.Pow(b, (double)F1_2) + F2_1 + " * " + Math.Pow(b, (double)F2_2) + F3_1 + " * " + Math.Pow(b, (double)F3_2) + F4_1 + " * " + Math.Pow(b, (double)F4_2) + F5_1 + " * " + Math.Pow(b, (double)F5_2) + ") - (" + F1_1 + " * " + Math.Pow(a, (double)F1_2) + F2_1 + " * " + Math.Pow(a, (double)F2_2) + F3_1 + " * " + Math.Pow(a, (double)F3_2) + F4_1 + " * " + Math.Pow(a, (double)F4_2) + F5_1 + " * " + Math.Pow(a, (double)F5_2) + ")";
            }
            else
            {
                answer = "";
            }

            Console.WriteLine("操作を選択");
            Console.WriteLine("------------------------------");
            Console.WriteLine("0 : 式で出力");
            Console.WriteLine("1 : 計算して出力(分数処理不可)");
            Console.WriteLine();
            Console.Write("Input:");
            int keisan = int.Parse(Console.ReadLine());

            if (keisan == 0)
            {
                Program.ModeSelect(answer);
            }
            else if(keisan == 1)
            {
                DataTable dataTable = new DataTable();
                var Result = dataTable.Compute(answer, "");
                Program.ModeSelect(Convert.ToString(Result));
            }
        }

        public static void Mode2() //微分
        {
            Console.WriteLine("Selected Mode:2");
            try
            {


                Console.Write("項数を入力:");
                int kousuu = int.Parse(Console.ReadLine());
                Console.WriteLine();
                if (kousuu == 1)
                {
                    Console.WriteLine("ax^p");
                    Console.Write("a:");
                    int a = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    int p = int.Parse(Console.ReadLine());
                    Console.WriteLine();

                    int ap = p - 1;
                    string ans = (a * p) + "x^" + ap;
                    if (p != 0)
                    {
                        Program.ModeSelect(ans);
                    }
                    else
                    {
                        Program.ModeSelect(" ");
                    }


                }
                else if (kousuu == 2)
                {
                    Console.WriteLine("ax^p + bx^q");
                    Console.Write("a:");
                    int a = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    int p = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("b:");
                    int b = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("q:");
                    int q = int.Parse(Console.ReadLine());
                    Console.WriteLine();

                    int ap = p - 1;
                    int bq = q - 1;

                    string ans1 = (a * p) + "x^" + ap;
                    string ans2 = (b * q) + "x^" + bq;

                    StringBuilder stringBuilder = new StringBuilder();
                    bool p0 = false;
                    //bool q0 = false;

                    if (p != 0)
                    {
                        stringBuilder.Append(ans1);
                        p0 = true;
                    }

                    if (q != 0)
                    {
                        if (p0 == true)
                        {
                            stringBuilder.Append(" + ");
                        }

                        stringBuilder.Append(ans2);
                        //q0 = true;
                    }

                    Program.ModeSelect(stringBuilder.ToString());

                }
                else if (kousuu == 3)
                {
                    Console.WriteLine("ax^p + bx^q + cx^r");
                    Console.Write("a:");
                    int a = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    int p = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("b:");
                    int b = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("q:");
                    int q = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("c:");
                    int c = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("r:");
                    int r = int.Parse(Console.ReadLine());
                    Console.WriteLine();

                    int ap = p - 1;
                    int bq = q - 1;
                    int cr = r - 1;
                    string ans1 = (a * p) + "x^" + ap;
                    string ans2 = (b * q) + "x^" + bq;
                    string ans3 = (c * r) + "x^" + cr;

                    StringBuilder stringBuilder = new StringBuilder();
                    bool p0 = false;
                    bool q0 = false;
                    //bool r0 = false;

                    if (p != 0)
                    {
                        stringBuilder.Append(ans1);
                        p0 = true;
                    }

                    if (q != 0)
                    {
                        if (p0 == true)
                        {
                            stringBuilder.Append(" + ");
                        }

                        stringBuilder.Append(ans2);
                        q0 = true;
                    }

                    if (r != 0)
                    {
                        if (q0 == true)
                        {
                            stringBuilder.Append(" + ");
                        }

                        stringBuilder.Append(ans3);
                        //r0 = true;
                    }

                    Program.ModeSelect(stringBuilder.ToString());

                }
                else if (kousuu == 4)
                {
                    Console.WriteLine("ax^p + bx^q + cx^r + dx^s");
                    Console.Write("a:");
                    int a = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    int p = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("b:");
                    int b = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("q:");
                    int q = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("c:");
                    int c = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("r:");
                    int r = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("d:");
                    int d = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("s:");
                    int s = int.Parse(Console.ReadLine());
                    Console.WriteLine();

                    int ap = p - 1;
                    int bq = q - 1;
                    int cr = r - 1;
                    int ds = s - 1;
                    string ans1 = (a * p) + "x^" + ap;
                    string ans2 = (b * q) + "x^" + bq;
                    string ans3 = (c * r) + "x^" + cr;
                    string ans4 = (d * s) + "x^" + ds;

                    StringBuilder stringBuilder = new StringBuilder();
                    bool p0 = false;
                    bool q0 = false;
                    bool r0 = false;
                    //bool s0 = false;

                    if (p != 0)
                    {
                        stringBuilder.Append(ans1);
                        p0 = true;
                    }

                    if (q != 0)
                    {
                        if (p0 == true)
                        {
                            stringBuilder.Append(" + ");
                        }

                        stringBuilder.Append(ans2);
                        q0 = true;
                    }

                    if (r != 0)
                    {
                        if (q0 == true)
                        {
                            stringBuilder.Append(" + ");
                        }

                        stringBuilder.Append(ans3);
                        r0 = true;
                    }

                    if (s != 0)
                    {
                        if (r0 == true)
                        {
                            stringBuilder.Append(" + ");
                        }

                        stringBuilder.Append(ans4);
                        //s0 = true;
                    }

                    Program.ModeSelect(stringBuilder.ToString());

                }
                else if (kousuu == 5)
                {
                    Console.WriteLine("ax^p + bx^q + cx^r + dx^s + ex^t");
                    Console.Write("a:");
                    int a = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    int p = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("b:");
                    int b = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("q:");
                    int q = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("c:");
                    int c = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("r:");
                    int r = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("d:");
                    int d = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("s:");
                    int s = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("e:");
                    int e = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("t:");
                    int t = int.Parse(Console.ReadLine());
                    Console.WriteLine();

                    int ap = p - 1;
                    int bq = q - 1;
                    int cr = r - 1;
                    int ds = s - 1;
                    int et = t - 1;
                    string ans1 = (a * p) + "x^" + ap;
                    string ans2 = (b * q) + "x^" + bq;
                    string ans3 = (c * r) + "x^" + cr;
                    string ans4 = (d * s) + "x^" + ds;
                    string ans5 = (e * t) + "x^" + et;

                    StringBuilder stringBuilder = new StringBuilder();
                    bool p0 = false;
                    bool q0 = false;
                    bool r0 = false;
                    bool s0 = false;

                    if (p != 0)
                    {
                        stringBuilder.Append(ans1);
                        p0 = true;
                    }

                    if (q != 0)
                    {
                        if (p0 == true)
                        {
                            stringBuilder.Append(" + ");
                        }

                        stringBuilder.Append(ans2);
                        q0 = true;
                    }

                    if (r != 0)
                    {
                        if (q0 == true)
                        {
                            stringBuilder.Append(" + ");
                        }

                        stringBuilder.Append(ans3);
                        r0 = true;
                    }

                    if (s != 0)
                    {
                        if (r0 == true)
                        {
                            stringBuilder.Append(" + ");
                        }

                        stringBuilder.Append(ans4);
                        s0 = true;
                    }

                    if (t != 0)
                    {
                        if (s0 == true)
                        {
                            stringBuilder.Append(" + ");
                        }

                        stringBuilder.Append(ans5);
                        //t0 = true;
                    }

                    Program.ModeSelect(stringBuilder.ToString());

                }

            } 
            catch (OverflowException)
            {
                Program.ExceptionHandler("System.OverflowException", "値がintの規定値を超えています");
            }
            catch (FormatException)
            {
                Program.ExceptionHandler("System.FormatException", "int型でない値が入力されました");
            }
        }

        public static void Mode7()
        {
            Console.WriteLine("計算トレーニングモード");
            Console.WriteLine("含む計算を選択");
            Console.WriteLine("1 : +");
            Console.WriteLine("2 : -");
            Console.WriteLine("3 : *");
            //Console.WriteLine("4 : /");

            Console.Write("Input:");
            int mode = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("レベル選択");
            Console.WriteLine("1 : 4桁まで,-符号なし");
            Console.WriteLine("2 : +-4桁");
            Console.Write("Input:");
            int level = int.Parse(Console.ReadLine());
            

            if (level == 1)
            {
                Random random = new Random();
                int[] number = { 106, 112, 115, 122, 130, 214, 215, 217, 222, 228, 229, 302, 305, 307, 312, 314, 315, 321, 322, 402, 404, 422, 507, 515, 518, 520, 523, 525, 606, 610, 611, 617, 618, 620, 701, 702, 706, 715, 722, 730, 804, 808, 810, 820, 830, 908, 909, 1001, 1005, 1010, 1013, 1028, 1031, 1107, 1115, 1124, 1201, 1208, 1210, 1212, 1213 };
                Stopwatch stopwatch = new Stopwatch();
                bool[] result = new bool[10];
                stopwatch.Start();
                result[0] = Q(number[random.Next(0, number.Length)], number[random.Next(0, number.Length)], mode);
                result[1] = Q(number[random.Next(0, number.Length)], number[random.Next(0, number.Length)], mode);
                result[2] = Q(number[random.Next(0, number.Length)], number[random.Next(0, number.Length)], mode);
                result[3] = Q(number[random.Next(0, number.Length)], number[random.Next(0, number.Length)], mode);
                result[4] = Q(number[random.Next(0, number.Length)], number[random.Next(0, number.Length)], mode);
                result[5] = Q(number[random.Next(0, number.Length)], number[random.Next(0, number.Length)], mode);
                result[6] = Q(number[random.Next(0, number.Length)], number[random.Next(0, number.Length)], mode);
                result[7] = Q(number[random.Next(0, number.Length)], number[random.Next(0, number.Length)], mode);
                result[8] = Q(number[random.Next(0, number.Length)], number[random.Next(0, number.Length)], mode);
                result[9] = Q(number[random.Next(0, number.Length)], number[random.Next(0, number.Length)], mode);
                stopwatch.Stop();
                Console.Clear();
                Console.WriteLine("結果");
                int count = result.Count(x => x == true);
                TimeSpan timeSpan = stopwatch.Elapsed;
                int score = (int)((count * 100) - Math.Round(timeSpan.TotalSeconds,MidpointRounding.AwayFromZero));
                Console.WriteLine("正解数: " + count + "/10");
                Console.WriteLine("タイム: " + timeSpan.TotalSeconds + " 秒");
                Console.WriteLine("スコア: " + score);
                Console.WriteLine();
                Console.Write("Enterで終了");
                Console.ReadLine();
                Console.Clear();
                Program.ModeSelect("");
            }
            else
            {
                Random random = new Random();
                Stopwatch stopwatch = new Stopwatch();
                bool[] result = new bool[10];
                stopwatch.Start();
                result[0] = Q(random.Next(-9999,9999), random.Next(-9999,9999), mode);
                result[1] = Q(random.Next(-9999,9999), random.Next(-9999,9999), mode);
                result[2] = Q(random.Next(-9999,9999), random.Next(-9999,9999), mode);
                result[3] = Q(random.Next(-9999,9999), random.Next(-9999,9999), mode);
                result[4] = Q(random.Next(-9999,9999), random.Next(-9999,9999), mode);
                result[5] = Q(random.Next(-9999,9999), random.Next(-9999,9999), mode);
                result[6] = Q(random.Next(-9999,9999), random.Next(-9999,9999), mode);
                result[7] = Q(random.Next(-9999,9999), random.Next(-9999,9999), mode);
                result[8] = Q(random.Next(-9999,9999), random.Next(-9999,9999), mode);
                result[9] = Q(random.Next(-9999,9999), random.Next(-9999,9999), mode);
                stopwatch.Stop();
                Console.Clear();
                Console.WriteLine("結果");
                int count = result.Count(x => x == true);
                TimeSpan timeSpan = stopwatch.Elapsed;
                int score = (int)((count * 100) - Math.Round(timeSpan.TotalSeconds, MidpointRounding.AwayFromZero));
                Console.WriteLine("正解数: " + count + "/10");
                Console.WriteLine("タイム: " + timeSpan.TotalSeconds + " 秒");
                Console.WriteLine("スコア: " + score);
                Console.WriteLine();
                Console.Write("Enterで終了");
                Console.ReadLine();
                Console.Clear();
                Program.ModeSelect("");
            }

           static bool Q(int a,int b,int mode)
            {
                Console.WriteLine();
                int answer;
                if (mode == 1)
                {
                    answer = a + b;
                    Console.Write(a + " + " + b + " = ");
                    int input = int.Parse(Console.ReadLine());
                    if (answer == input)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (mode == 2)
                {
                    answer = a - b;
                    Console.Write(a + " - " + b + " = ");
                    int input = int.Parse(Console.ReadLine());
                    if (answer == input)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (mode == 3)
                {
                    answer = a * b;
                    Console.Write(a + " * " + b + " = ");
                    int input = int.Parse(Console.ReadLine());
                    if (answer == input)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false; //エラー回避用
                }
            } 
        }

        public static void Mode9() //testing
        {
            int a = 1;
            int b = 3;
            double result = Integrate.GaussKronrod(x => (x * x) + 2 * x + 1, a, b);
            Program.ModeSelect(Convert.ToString(result));
        }

        public static void Mode10()
        {
            Console.WriteLine("Selected Mode:10");
            Console.WriteLine("操作を選択");
            Console.WriteLine("s : Sin");
            Console.WriteLine("c : Cos");
            Console.WriteLine("t : Tan");
            Console.WriteLine();
            Console.Write("Input:");
            string mode = Console.ReadLine();
            Console.WriteLine();
            Console.Write("角度を入力:");
            int kaku = int.Parse(Console.ReadLine());

            double ans;

            if (mode == "s")
            {
                double rad = (kaku / 180) * Math.PI;
                ans = Math.Sin(rad);
            }
            else if (mode == "c")
            {
                double rad = (kaku / 180) * Math.PI;
                ans = Math.Cos(rad);
            }
            else if (mode == "t")
            {
                double rad = (kaku / 180) * Math.PI;
                ans = Math.Tan(rad);
            }
            else
            {
                ans = 0; //エラー回避
            }
            

            Program.ModeSelect(Convert.ToString(ans));
        }

        public static void Mode11()
        {
            Console.WriteLine("Selected Mode:11");
            Console.Write("ルートの中の値:");
            int val = int.Parse(Console.ReadLine());
            double ans = Math.Sqrt(val);
            Program.ModeSelect(Convert.ToString(ans));
        }
    }
}
