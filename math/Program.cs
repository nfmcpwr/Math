using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Text.Json;
//using KeyControl;
using System.Threading;
using System.Threading.Tasks;
//using System.Drawing;
//using System.Windows.Forms;

namespace math
{
    class Flags
    {
        public bool Chart_enabled { get; set; }
        public bool Classroom_enabled { get; set; }
        public bool Dev { get; set; }
        public bool Settings_enabled { get; set; }
    }

    class Config
    {
        public string Chart_id { get; set; }
        public string Chart_pw { get; set; }
        public string Classroom_url { get; set; }
        public bool Flag_override { get; set; }
    }
    class Global
    {
        public static string version = "3.6";
        public static int vc = 5597;
        public static string testkey = Guid.NewGuid().ToString("N");
        public static bool update = false;
        public static string utext;
        public static Flags flags = new Flags();
        public static Config config = new Config();

        public static async void GetFlagsAndConfig()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Math", version));
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://raw.githubusercontent.com/nfmcpwr/Math/main/flag.json");
                string json = await response.Content.ReadAsStringAsync();
                flags = JsonConvert.DeserializeObject<Flags>(json);
            }
            catch (Exception)
            {
                StreamReader sr = new StreamReader(Path.Combine("data", "default.json"), Encoding.UTF8);
                flags = JsonConvert.DeserializeObject<Flags>(sr.ReadToEnd());
                sr.Close();
            }
            finally
            {
                StreamReader src = new StreamReader(Path.Combine("data", "config.json"), Encoding.UTF8);
                config = JsonConvert.DeserializeObject<Config>(src.ReadToEnd());
                src.Close();

                if (config.Flag_override == true)
                {
                    FlagOverride();
                }
            }
        }

        private static void FlagOverride()
        {
            StreamReader sro = new StreamReader(Path.Combine(Environment.CurrentDirectory, "data", "default.json"), Encoding.UTF8);
            Flags overridedFlag = JsonConvert.DeserializeObject<Flags>(sro.ReadToEnd());
            sro.Close();

            flags = overridedFlag;
        }
    }

    class Program
    {
        public static void ModeSelect(string Answer)
        {
            //Console.WriteLine();
            Console.Clear();

            if (Global.update == true)
            {
                Console.WriteLine(Global.utext);
                Console.WriteLine();
            }

            if (Answer != "")
            {
                Console.WriteLine("前回の計算の答えは " + Answer);
                Console.WriteLine();
                //Console.WriteLine();
            }

            if (Global.flags.Dev == true)
            {
                Console.WriteLine();

            }

            Console.WriteLine("操作を選択");
            Console.WriteLine();
            Console.WriteLine("1 : 文字を含まない計算");
            Console.WriteLine("2 : 微分");
            Console.WriteLine("3 : 1次式の解");
            Console.WriteLine("4 : 2次式の解");
            Console.WriteLine("5 : 不定積分");
            Console.WriteLine("6 : 定積分");
            Console.WriteLine("7 : 計算練習");
            Console.WriteLine("8 : 辞書モード");
            Console.WriteLine("9 : 定積分(MathNet使用)");
            Console.WriteLine("10 : 三角比の値");
            Console.WriteLine("11 : 平方根の値");
            Console.WriteLine("12 : 小数 -> 分数変換");
            Console.WriteLine("13 : 累乗");
            Console.WriteLine("14 : 最大公約数");
            Console.WriteLine("15 : 最小公倍数");
            Console.WriteLine("16 : データ");
            Console.WriteLine("17 : 関数グラフ");
            //Console.WriteLine("18 : 翻訳");
            Console.WriteLine("19 : ベクトル");
            Console.WriteLine("------------------------------");
            if (Global.update == true)
            {
                Console.WriteLine("100 : アップデートリリースページを開く");
            }

            if (Global.flags.Dev == true)
            {
                Console.WriteLine("1000 : Test");
            }

            if (Global.flags.Settings_enabled == true)
            {
                Console.WriteLine("1001 : 設定");
            }

            if (Global.flags.Classroom_enabled == true)
            {
                Console.WriteLine("1002 : Google Classroomを開く");
            }

            if (Global.flags.Chart_enabled == true)
            {
                Console.WriteLine("1003 : チャートを開く");
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
            catch (OverflowException e)
            {
                //Exception e = new OverflowException();
                ExceptionHandler(e.ToString(), e);
            }
            catch (FormatException e)
            {
                //Exception e = new FormatException();
                ExceptionHandler(e.ToString(), e);
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


                Calc.Mode7();

            }
            else if (Mode == 9)
            {
                //mathnet 定積分
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
            else if (Mode == 12)
            {
                //小数 -> 分数
                Console.WriteLine("Mode12");
                /*Console.WriteLine("モード選択");
                Console.WriteLine("1 : old(非推奨)");
                Console.WriteLine("2 : table");
                Console.WriteLine("3 : new");
                Console.Write(":3");*/
                string m = "3"; //Console.ReadLine();
                if (m == "1")
                {
                    Console.Write("値:");
                    double value = double.Parse(Console.ReadLine());
                    Calc.Flaction.Fraction fraction = Calc.Flaction.ConvertToFraction(value);
                    ModeSelect($"{fraction.Numerator} / {fraction.Denominator}");
                }
                else if (m == "2")
                {
                    Calc.Mode12_1();
                }
                else if (m == "3")
                {
                    Calc.Mode12_2();
                }
                else
                {
                    ModeSelect("");
                }
            }
            else if (Mode == 13)
            {
                //累乗
                Calc.Mode13();
            }
            else if (Mode == 14)
            {
                //最大公約数
                Calc.Mode14();
            }
            else if (Mode == 15)
            {
                //最小公倍数
                Calc.Mode15();
            }
            else if (Mode == 16)
            {
                //データ
                Calc.Mode16();
            }
            else if (Mode == 17)
            {
                //グラフ
                Calc.Mode17();
            }
            //else if (Mode == 18)
            //{
            //    Translation.TMode();
            //}
            else if (Mode == 19)
            {
                Calc.Mode19();
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
                    OpenUrl("https://github.com/nfmcpwr/Math/releases/");
                }
                else
                {
                    ModeSelect("");
                }
            }
            else if (Mode == 1000 && Global.flags.Dev == true)
            {
                Test();
            }
            else if (Mode == 1001 && Global.flags.Settings_enabled == true)
            {
                Settings();

            }
            else if (Mode == 1002 && Global.flags.Classroom_enabled == true)
            {
                OpenUrl(Global.config.Classroom_url);
            }
            else if (Mode == 1003 && Global.flags.Chart_enabled == true)
            {
                OpenUrl("https://sviewer.jp/books/index.html?name=" + Global.config.Chart_id + "&password=" + Global.config.Chart_pw);
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
            //Test
            double a = double.Parse(Console.ReadLine());
            double b = double.Parse(Console.ReadLine());
            double c = double.Parse(Console.ReadLine());

            for (int x = -100; x < 100; x++)
            {
                Console.WriteLine("x = " + x + ", " + (a * Math.Pow(x, 2) + b * x + c));
            }

            Console.ReadLine();
            ModeSelect("");
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
                            UseShellExecute = true
                        };

                        Process.Start(processStartInfo).WaitForExit();

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
            httpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("Math", Global.version));
            HttpResponseMessage httpResponse = await httpClient.GetAsync("https://api.github.com/repos/nfmcpwr/Math/releases/latest");
            if (httpResponse.IsSuccessStatusCode == true)
            {
                string json = await httpResponse.Content.ReadAsStringAsync();

                JsonDocument jsonDocument = JsonDocument.Parse(json);
                JsonElement element = jsonDocument.RootElement;
                element.TryGetProperty("tag_name", out JsonElement value);
                string version = value.GetString();

                if (version != "v" + Global.version)
                {
                    Global.update = true;
                    Global.utext = "アップデート利用可能: " + version;
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

            try
            {
                PingReply reply = ping.Send("google.com");

                Task task = GetUpdate();
                task.Wait();

            }
            catch (PingException)
            {
                ModeSelect("");
            }


        }

        public static void ExceptionHandler(string exception, Exception e)
        {
            Console.Clear();
            Console.WriteLine("ExceptionHandler");
            Console.WriteLine("version: " + Global.version);
            Console.WriteLine("内容: " + exception);
            Console.WriteLine();
            Console.WriteLine(e.Message);
            Console.WriteLine();
            Console.Write("Enterで終了");
            Console.ReadLine();
        }

        static void Settings()
        {
            Console.WriteLine("設定");
            Console.WriteLine("1 : チャート");
            Console.WriteLine("0 : 戻る");

            string m = Console.ReadLine();
            if (m == "1")
            {
                StreamWriter sw = new StreamWriter(Path.Combine("data", "config.json"), false, Encoding.UTF8);
                Console.Write("ID:");
                Global.config.Chart_id = Console.ReadLine();
                Console.Write("PW:");
                Global.config.Chart_pw = Console.ReadLine();

                sw.Write(JsonConvert.SerializeObject(Global.config));
                sw.Close();

                Settings();
            }
            else
            {
                ModeSelect("");
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "Math " + Global.version + "(" + Global.vc + ")";
            Global.GetFlagsAndConfig();

            if (File.Exists(Path.Combine(DataManager.GlobalVariables.DataPath, "math", "dev")) == false)
            {
                DataManager.Activity.CheckFileActivity("math", "dev");
                Exit();
            }

            Thread.Sleep(1000);
            ConnectionCheck();
        }
    }

    //public class Translation
    //{
    //    class InternalValue
    //    {
    //        public static readonly string key = l.I("JKiHPcVUxmnyNTSWZozjfeYlrAXasFEOvQuBbICw");
    //    }

    //    public static void TMode()
    //    {
    //        try
    //        {
    //            Console.WriteLine("翻訳");
    //            Console.WriteLine("1 : en -> ja");
    //            Console.WriteLine("2 : ja -> en");

    //            Console.WriteLine("0 : 終了");
    //            Console.WriteLine("-----------------------------------");
    //            string i = Console.ReadLine();

    //            if (i == "1")
    //            {
    //                Request("EN", "JA");
    //            }
    //            else if (i == "2")
    //            {
    //                Request("JA", "EN");
    //            }
    //            else
    //            {
    //                Program.ModeSelect("");
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Exception ex = new HttpRequestException(e.Message, new HttpRequestException());
    //            Program.ExceptionHandler(ex.ToString(), e);
    //        }
    //    }

    //    static void Request(string source, string target)
    //    {
    //        string src;
    //        string tgt;

    //        if (source == "EN")
    //        {
    //            src = "eng_Latn";
    //        }
    //        else if (source == "JA")
    //        {
    //            src = "jpn_Jpan";
    //        }
    //        else
    //        {
    //            throw new Exception();
    //        }

    //        if(target == "EN")
    //        {
    //            tgt = "eng_Latn";
    //        }
    //        else if (target == "JA")
    //        {
    //            tgt = "jpn_Jpan";
    //        }
    //        else
    //        {
    //            throw new Exception();
    //        }


    //        Console.Write("Text:");
    //        string txt = Console.ReadLine();

    //        HttpClient client = new HttpClient();
    //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", InternalValue.key);
    //        client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("Math", Global.version));

    //        HttpRequestMessage req = new HttpRequestMessage();
    //        req.RequestUri = new Uri("https://api.nlpcloud.io/v1/nllb-200-3-3b/translation");
    //        req.Method = HttpMethod.Post;
    //        req.Content = new StringContent(Input(src, tgt, txt), Encoding.UTF8, "application/json");

    //        Task task = Translate(client, req);
    //        task.Wait();

    //        //Console.ReadLine();
    //    }

    //    static string Input(string source, string target, string text)
    //    {
    //        Body reqbody = new Body();
    //        reqbody.source = source;
    //        reqbody.target = target;
    //        reqbody.text = text;

    //        string json = JsonConvert.SerializeObject(reqbody);
    //        return json;
    //    }


    //    static async Task Translate(HttpClient c, HttpRequestMessage r)
    //    {
    //        HttpResponseMessage response = await c.SendAsync(r);
    //        if (response.IsSuccessStatusCode != true)
    //        {
    //            Console.WriteLine(response.StatusCode);
    //        }
    //        else
    //        {
    //            string j = await response.Content.ReadAsStringAsync();
    //            Translatedtext ttext = JsonConvert.DeserializeObject<Translatedtext>(j);
    //            //Console.WriteLine(j);
    //            Console.WriteLine("TranslatedText: " + ttext.translation_text);
    //        }

    //        TMode();
    //    }

    //    public class Body
    //    {
    //        public string text { get; set; }
    //        public string source { get; set; }
    //        public string target { get; set; }
    //    }

    //    public class Translatedtext
    //    {
    //        public string translation_text { get; set; }
    //    }
    //}

    public class Calc
    {
        public static void Mode1()
        {
            Console.WriteLine("Mode1");
            Console.WriteLine("数式を入力");
            Console.Write("Input:");
            try
            {
                string Input = Console.ReadLine();
                DataTable dataTable = new DataTable();
                var Result = dataTable.Compute(Input, "");
                Program.ModeSelect(Convert.ToString(Result));
            }
            catch (Exception e)
            {
                //Exception e = new EvaluateException();
                Program.ExceptionHandler(e.ToString(), e);
            }
        }


        public static void Mode3()
        {
            Console.WriteLine("Mode3");
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
            catch (Exception e)
            {
                //Exception e = new OverflowException();
                Program.ExceptionHandler(e.ToString(), e);
            }
            //catch (FormatException e)
            //{
            //    //Exception e = new FormatException();
            //    Program.ExceptionHandler(e.ToString(), e);
            //}

        }

        public static void Mode4()
        {
            Console.WriteLine("Mode4");
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
            catch (Exception e)
            {
                //Exception e = new OverflowException();
                Program.ExceptionHandler(e.ToString(), e);
            }
            //catch (FormatException e)
            //{
            //    //Exception e = new FormatException();
            //    Program.ExceptionHandler(e.ToString(), e);
            //}
        }

        public static void Mode5(bool Tei)
        {
            try
            {

                if (Tei == true)
                {
                    Console.WriteLine("Mode6");
                }
                else
                {
                    Console.WriteLine("Mode5");
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
            catch (Exception e)
            {
                //Exception e = new FormatException();
                Program.ExceptionHandler(e.ToString(), e);
            }
            //catch(OverflowException e)
            //{
            //    //Exception e = new OverflowException();
            //    Program.ExceptionHandler(e.ToString(), e);
            //}
        }

        public static void Teiseki(int kousuu, string F1_1, int? F1_2, string F2_1, int? F2_2, string F3_1, int? F3_2, string F4_1, int? F4_2, string F5_1, int? F5_2) //ax^p + bx^q = a,p,b,q
        {
            try
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
                else if (keisan == 1)
                {
                    DataTable dataTable = new DataTable();
                    var Result = dataTable.Compute(answer, "");
                    Program.ModeSelect(Convert.ToString(Result));
                }
            }
            catch (Exception e)
            {
                Program.ExceptionHandler(e.ToString(), e);
            }
        }

        public static void Mode2() //微分
        {
            Console.WriteLine("Mode2");
            try
            {


                Console.Write("項数を入力:");
                int kousuu = int.Parse(Console.ReadLine());
                Console.WriteLine();
                if (kousuu == 1)
                {
                    Console.WriteLine("ax^p");
                    Console.Write("a:");
                    double a = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    double p = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    double ap = p - 1;
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
                    double a = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    double p = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("b:");
                    double b = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("q:");
                    double q = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    double ap = p - 1;
                    double bq = q - 1;

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
                    double a = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    double p = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("b:");
                    double b = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("q:");
                    double q = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("c:");
                    double c = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("r:");
                    double r = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    double ap = p - 1;
                    double bq = q - 1;
                    double cr = r - 1;
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
                    double a = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    double p = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("b:");
                    double b = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("q:");
                    double q = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("c:");
                    double c = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("r:");
                    double r = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("d:");
                    double d = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("s:");
                    double s = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    double ap = p - 1;
                    double bq = q - 1;
                    double cr = r - 1;
                    double ds = s - 1;
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
                    double a = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    double p = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("b:");
                    double b = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("q:");
                    double q = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("c:");
                    double c = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("r:");
                    double r = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("d:");
                    double d = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("s:");
                    double s = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("e:");
                    double e = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("t:");
                    double t = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    double ap = p - 1;
                    double bq = q - 1;
                    double cr = r - 1;
                    double ds = s - 1;
                    double et = t - 1;
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
            catch (Exception e)
            {
                //Exception e = new OverflowException();
                Program.ExceptionHandler(e.ToString(), e);
            }
            //catch (FormatException e)
            //{
            //    //Exception e = new FormatException();
            //    Program.ExceptionHandler(e.ToString(), e);


            //}
        }

        public static void Mode7()
        {
            try
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
                else
                {
                    Random random = new Random();
                    Stopwatch stopwatch = new Stopwatch();
                    bool[] result = new bool[10];
                    stopwatch.Start();
                    result[0] = Q(random.Next(-9999, 9999), random.Next(-9999, 9999), mode);
                    result[1] = Q(random.Next(-9999, 9999), random.Next(-9999, 9999), mode);
                    result[2] = Q(random.Next(-9999, 9999), random.Next(-9999, 9999), mode);
                    result[3] = Q(random.Next(-9999, 9999), random.Next(-9999, 9999), mode);
                    result[4] = Q(random.Next(-9999, 9999), random.Next(-9999, 9999), mode);
                    result[5] = Q(random.Next(-9999, 9999), random.Next(-9999, 9999), mode);
                    result[6] = Q(random.Next(-9999, 9999), random.Next(-9999, 9999), mode);
                    result[7] = Q(random.Next(-9999, 9999), random.Next(-9999, 9999), mode);
                    result[8] = Q(random.Next(-9999, 9999), random.Next(-9999, 9999), mode);
                    result[9] = Q(random.Next(-9999, 9999), random.Next(-9999, 9999), mode);
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
            }
            catch (Exception e)
            {
                Program.ExceptionHandler(e.ToString(), e);
            }

            static bool Q(int a, int b, int mode)
            {
                try
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
                catch (Exception e)
                {
                    Program.ExceptionHandler(e.ToString(), e);
                    return false;
                }
            }
        }

        public static void Mode9()
        {
            try
            {
                Console.WriteLine("Mode9");
                //int u = 1;
                //int y = 3;
                //double result = Integrate.GaussKronrod(x => Math.Pow(x,2) + 2 * x + 1, u, y);
                //Program.ModeSelect(Convert.ToString(result));

                Console.Write("項数を入力:");
                int kousuu = int.Parse(Console.ReadLine());
                Console.WriteLine();
                if (kousuu == 1)
                {
                    Console.WriteLine("ax^p");
                    Console.Write("a:");
                    double a = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    double p = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    //積分区間入力
                    Console.WriteLine("積分区間入力");
                    Console.WriteLine("b ");
                    Console.WriteLine("∫f(x) dx");
                    Console.WriteLine("a ");
                    Console.WriteLine();
                    Console.Write("b:");
                    double kukanb = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("a:");
                    double kukana = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    //計算
                    double result = Integrate.GaussKronrod(x => a * Math.Pow(x, p), kukana, kukanb);
                    Program.ModeSelect(Convert.ToString(result));


                }
                else if (kousuu == 2)
                {
                    Console.WriteLine("ax^p + bx^q");
                    Console.Write("a:");
                    double a = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    double p = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("b:");
                    double b = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("q:");
                    double q = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    //積分区間入力
                    Console.WriteLine("積分区間入力");
                    Console.WriteLine("b ");
                    Console.WriteLine("∫f(x) dx");
                    Console.WriteLine("a ");
                    Console.WriteLine();
                    Console.Write("b:");
                    double kukanb = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("a:");
                    double kukana = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    //計算
                    double result = Integrate.GaussKronrod(x => (a * Math.Pow(x, p)) + (b * Math.Pow(x, q)), kukana, kukanb);
                    Program.ModeSelect(Convert.ToString(result));



                }
                else if (kousuu == 3)
                {
                    Console.WriteLine("ax^p + bx^q + cx^r");
                    Console.Write("a:");
                    double a = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    double p = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("b:");
                    double b = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("q:");
                    double q = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("c:");
                    double c = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("r:");
                    double r = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    //積分区間入力
                    Console.WriteLine("積分区間入力");
                    Console.WriteLine("b ");
                    Console.WriteLine("∫f(x) dx");
                    Console.WriteLine("a ");
                    Console.WriteLine();
                    Console.Write("b:");
                    double kukanb = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("a:");
                    double kukana = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    //計算
                    double result = Integrate.GaussKronrod(x => (a * Math.Pow(x, p)) + (b * Math.Pow(x, q)) + (c * Math.Pow(x, r)), kukana, kukanb);
                    Program.ModeSelect(Convert.ToString(result));



                }
                else if (kousuu == 4)
                {
                    Console.WriteLine("ax^p + bx^q + cx^r + dx^s");
                    Console.Write("a:");
                    double a = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    double p = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("b:");
                    double b = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("q:");
                    double q = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("c:");
                    double c = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("r:");
                    double r = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("d:");
                    double d = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("s:");
                    double s = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    //積分区間入力
                    Console.WriteLine("積分区間入力");
                    Console.WriteLine("b ");
                    Console.WriteLine("∫f(x) dx");
                    Console.WriteLine("a ");
                    Console.WriteLine();
                    Console.Write("b:");
                    double kukanb = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("a:");
                    double kukana = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    //計算
                    double result = Integrate.GaussKronrod(x => (a * Math.Pow(x, p)) + (b * Math.Pow(x, q)) + (c * Math.Pow(x, r)) + (d * Math.Pow(x, s)), kukana, kukanb);
                    Program.ModeSelect(Convert.ToString(result));



                }
                else if (kousuu == 5)
                {
                    Console.WriteLine("ax^p + bx^q + cx^r + dx^s + ex^t");
                    Console.Write("a:");
                    double a = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("p:");
                    double p = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("b:");
                    double b = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("q:");
                    double q = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("c:");
                    double c = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("r:");
                    double r = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("d:");
                    double d = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("s:");
                    double s = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("e:");
                    double e = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("t:");
                    double t = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    //積分区間入力
                    Console.WriteLine("積分区間入力");
                    Console.WriteLine("b ");
                    Console.WriteLine("∫f(x) dx");
                    Console.WriteLine("a ");
                    Console.WriteLine();
                    Console.Write("b:");
                    double kukanb = double.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.Write("a:");
                    double kukana = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    //計算
                    double result = Integrate.GaussKronrod(x => (a * Math.Pow(x, p)) + (b * Math.Pow(x, q)) + (c * Math.Pow(x, r)) + (d * Math.Pow(x, s)) + (e * Math.Pow(x, t)), kukana, kukanb);
                    Program.ModeSelect(Convert.ToString(result));



                }
            }
            catch (Exception e)
            {
                Program.ExceptionHandler(e.ToString(), e);
            }
        }

        public static void Mode10()
        {
            try
            {
                Console.WriteLine("Mode10");
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
            catch (Exception e)
            {
                Program.ExceptionHandler(e.ToString(), e);
            }
        }

        public static void Mode11()
        {
            try
            {
                Console.WriteLine("Mode11");
                Console.Write("ルートの中の値:");
                double val = double.Parse(Console.ReadLine());
                double ans = Math.Sqrt(val);
                Program.ModeSelect(Convert.ToString(ans));
            }
            catch (Exception e)
            {
                Program.ExceptionHandler(e.ToString(), e);
            }
        }

        public class Flaction
        {
            public static Fraction ConvertToFraction(double decimalNumber)
            {
                const double tolerance = 1.0E-6;
                double numerator = 1;
                double denominator = 1;
                double fractionValue = numerator / denominator;

                while (Math.Abs(fractionValue - decimalNumber) > tolerance)
                {
                    if (fractionValue < decimalNumber)
                    {
                        numerator++;
                    }
                    else
                    {
                        denominator++;
                        numerator = decimalNumber * denominator;
                    }
                    fractionValue = numerator / denominator;
                }

                return new Fraction((int)numerator, (int)denominator);
            }


            public struct Fraction
            {
                public int Numerator { get; }
                public int Denominator { get; }

                public Fraction(int numerator, int denominator)
                {
                    Numerator = numerator;
                    Denominator = denominator;
                }
            }
        }

        public static void Mode12_1()
        {
            using (StreamReader reader = new StreamReader("data\\convrule.txt", Encoding.UTF8))
            {
                Console.Write("Input(小数点以下6桁):");
                string inp = Console.ReadLine();
                string[] input = inp.Split(".");

                string t = reader.ReadToEnd();
                string[] table = t.Split("-");

                string bun = "";

                for (int i = 0; i < table.Length / 2; i++)
                {
                    //Console.WriteLine(table[i]);
                    if (table[i] == input[1])
                    {
                        bun = table[i + 1];
                        break;
                    }
                    else
                    {
                        i += 1;
                    }
                }

                //Console.ReadLine();

                if (bun != "")
                {
                    string ans = FractionLib.Fraction.Add(input[0] + "/1", bun);
                    Program.ModeSelect(ans);
                }
                else
                {
                    Exception e = new Exception("リストに値が含まれていません");
                    Program.ExceptionHandler(e.ToString(), e);
                }
            }

        }

        public static void Mode12_2()
        {
            try
            {
                Console.Write("小数:");
                string inp = Console.ReadLine();
                string[] input = inp.Split(".");

                double sii = double.Parse(inp) * Math.Pow(10, input[1].Length);
                int bo = 1 * (int)Math.Pow(10, input[1].Length);
                int si = (int)sii;

                int gcd = 1;

                while (1 == 1)
                {
                    if (Euclid.GreatestCommonDivisor(si, bo) != 1)
                    {
                        gcd = (int)Euclid.GreatestCommonDivisor(si, bo);
                        si /= gcd;
                        bo /= gcd;
                    }
                    else
                    {
                        break;
                    }
                }

                Program.ModeSelect(si + "/" + bo);
            }
            catch (Exception e)
            {
                Program.ExceptionHandler(e.ToString(), e);
            }
        }

        public static void Mode13()
        {
            try
            {
                Console.WriteLine("Mode13");
                Console.WriteLine("a^n");
                Console.Write("a:");
                double a = double.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("n:");
                double n = double.Parse(Console.ReadLine());
                string ans = Convert.ToString(Math.Pow(a, n));
                Program.ModeSelect(ans);
            }
            catch (Exception e)
            {
                Program.ExceptionHandler(e.ToString(), e);
            }
        }

        public static void Mode14()
        {
            try
            {
                Console.WriteLine("Mode14");
                Console.Write("値の数:");
                int num = int.Parse(Console.ReadLine());
                Console.WriteLine();
                long[] value = new long[num];
                for (int i = 0; i <= num - 1; i++)
                {
                    Console.Write(i + "番目:");
                    value[i] = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                }

                long ans = Euclid.GreatestCommonDivisor(value);
                Program.ModeSelect(Convert.ToString(ans));
            }
            catch (Exception e)
            {
                Program.ExceptionHandler(e.ToString(), e);
            }
        }

        public static void Mode15()
        {
            try
            {
                Console.WriteLine("Mode15");
                Console.Write("値の数:");
                int num = int.Parse(Console.ReadLine());
                Console.WriteLine();
                long[] value = new long[num];
                for (int i = 0; i <= num - 1; i++)
                {
                    Console.Write(i + "番目:");
                    value[i] = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                }

                long ans = Euclid.LeastCommonMultiple(value);
                Program.ModeSelect(Convert.ToString(ans));
            }
            catch (Exception e)
            {
                Program.ExceptionHandler(e.ToString(), e);
            }
        }

        public static void Mode16()
        {
            try
            {
                Console.WriteLine("Mode16");
                Console.Write("値の数:");
                int num = int.Parse(Console.ReadLine());
                Console.WriteLine();
                double[] value = new double[num];
                for (int i = 0; i <= num - 1; i++)
                {
                    Console.Write(i + "番目:");
                    value[i] = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                }

                Console.WriteLine("平均値  : " + value.Average());
                Console.WriteLine("第1四分位数 : " + value.Percentile(25));
                Console.WriteLine("中央値 : " + value.Median());
                Console.WriteLine("第3四分位数 : " + value.Percentile(75));
                Console.WriteLine("分散 : " + value.PopulationVariance());
                Console.WriteLine("標準偏差 : " + value.PopulationStandardDeviation());
                Program.ModeSelect("");
                //Program.ModeSelect(Convert.ToString(avg));
            }
            catch (Exception e)
            {
                Program.ExceptionHandler(e.ToString(), e);
            }
        }

        public static void Mode17()
        {
            try
            {
                Console.WriteLine("Mode17");
                Process.Start("./graphwriter.exe").WaitForExit();
                Program.ModeSelect("");
            }
            catch (Exception e)
            {
                Program.ExceptionHandler(e.ToString(), e);
            }
        }

        public static void Mode19()
        {
            Console.WriteLine("1 : +");
            Console.WriteLine("2 : -");
            Console.WriteLine("3 : *");
            Console.WriteLine("4 : /");
            Console.WriteLine("-------------------------------------");
            string m = Console.ReadLine();

            Vector2 v1 = new Vector2();
            Vector2 v2 = new Vector2();
            Vector2 ans = new Vector2();

            if (m == "1")
            {
                Console.WriteLine("v1 + v2");
                Console.Write("v1.X:");
                v1.X = float.Parse(Console.ReadLine());
                Console.Write("v1.Y:");
                v1.Y = float.Parse(Console.ReadLine());
                Console.Write("v2.X:");
                v2.X = float.Parse(Console.ReadLine());
                Console.Write("v2.Y:");
                v2.Y = float.Parse(Console.ReadLine());

                ans = v1 + v2;
            }
            else if (m == "2")
            {
                Console.WriteLine("v1 - v2");
                v1.X = float.Parse(Console.ReadLine());
                Console.Write("v1.Y:");
                v1.Y = float.Parse(Console.ReadLine());
                Console.Write("v2.X:");
                v2.X = float.Parse(Console.ReadLine());
                Console.Write("v2.Y:");
                v2.Y = float.Parse(Console.ReadLine());

                ans = v1 - v2;
            }
            else if (m == "3")
            {
                Console.WriteLine("v1 * v2");
                v1.X = float.Parse(Console.ReadLine());
                Console.Write("v1.Y:");
                v1.Y = float.Parse(Console.ReadLine());
                Console.Write("v2.X:");
                v2.X = float.Parse(Console.ReadLine());
                Console.Write("v2.Y:");
                v2.Y = float.Parse(Console.ReadLine());

                ans = v1 * v2;
            }
            else if (m == "4")
            {
                Console.WriteLine("v1 / v2");
                v1.X = float.Parse(Console.ReadLine());
                Console.Write("v1.Y:");
                v1.Y = float.Parse(Console.ReadLine());
                Console.Write("v2.X:");
                v2.X = float.Parse(Console.ReadLine());
                Console.Write("v2.Y:");
                v2.Y = float.Parse(Console.ReadLine());

                ans = v1 / v2;
            }
            else
            {
                Program.ModeSelect("");
            }

            Program.ModeSelect(ans.ToString());
        }
    }
}
