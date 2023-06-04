using System;
using System.Data;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace math
{
    class Global
    {
        public static string version = "1.8.01";
        public static string testkey;
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

            Console.WriteLine("操作を選択");
            Console.WriteLine("1 : 文字を含まない計算(小数を含まない)");
            Console.WriteLine("2 : 文字を含まない計算(小数を含む)");
            Console.WriteLine("3 : 文字を含む(1次式)");
            Console.WriteLine("4 : 文字を含む(2次式)");
            Console.WriteLine("5 : 不定積分");
            Console.WriteLine("6 : 定積分");
            Console.WriteLine("------------------------------");
            Console.WriteLine("0 : 終了");
            Console.WriteLine();
            Console.Write("Input:");
            int SelectedMode = int.Parse(Console.ReadLine());
            Console.Clear();
            CheckMode(SelectedMode);
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
                //通常計算double
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
            

             //s
             if (Mode == 0)
             {
                Exit();
             }
             else if (Mode == 1000)
            {
                Test();
            }
            
        }

        static void Exit()
        {
            //exit
        }

        static void Test()
        {
            if (Global.testkey == "38jdr328g7hdu")
            {
                Console.WriteLine("∫");
            }
            else
            {
                ModeSelect("");
            }
            
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
                    Console.WriteLine("アップデートが利用可能です:" + version);
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("アップデートデータの取得に失敗:" + httpResponse.StatusCode);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Math v" + Global.version);

            if (args.Length > 0)
            {
                Global.testkey = args[0];
            }

            if (NetworkInterface.GetIsNetworkAvailable() == true)
            {
                Task task = GetUpdate();
                task.Wait();
            }

            ModeSelect("");
        }
    }

    public class Calc
    {
        public static void Mode1()
        {
            Console.WriteLine("Selected Mode:1");
            Console.WriteLine("数式を入力");
            Console.Write("Input:");
            string Input = Console.ReadLine();
            DataTable dataTable = new DataTable();
            var Result = dataTable.Compute(Input, "");
            Program.ModeSelect(Convert.ToString(Result));
        }

        public static void Mode2()
        {
            Console.WriteLine("Selected Mode:2");
            Console.WriteLine("数式を入力");
            Console.Write("Input:");
            string Input = Console.ReadLine();
            DataTable dataTable = new DataTable();
            var Result = dataTable.Compute(Input, "");
            Program.ModeSelect(Convert.ToString(Result));
        }

        public static void Mode3()
        {
            Console.WriteLine("Selected Mode:3");
            Console.WriteLine("ax + b = c");
            Console.Write("a:");
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

        public static void Mode4()
        {
            Console.WriteLine("Selected Mode:4");
            Console.WriteLine("ax^2 + bx + c = 0");
            Console.Write("a:");
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

        public static void Mode5(bool Tei)
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
                    Calc.Teiseki(1,a + " / " + ap, ap,"",null,"",null,"",null,"",null);
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
    }
}
