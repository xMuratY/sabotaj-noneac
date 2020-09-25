using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Cryptography;

namespace SabotajLauncher
{
    class Program
    {
        //paste md5
        public static string CreateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }
        
        static void Launch(string username, string password)
        {
            HttpRequest req = new HttpRequest();

            //24092020-_-TFHES fucking stupids
            var dtnow = DateTime.Now;
            req.AddHeader("X-API-KEY", CreateMD5Hash($"{dtnow.Day}{dtnow.Month}{dtnow.Year}-_-TFHES"));
            req.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            req.UserAgent = "xd";

            var reqParams = new RequestParams();
            reqParams["u"] = username;
            reqParams["p"] = CreateMD5Hash(password);

            var result = req.Post("https://api.sabotajoyun.com/login?", reqParams);

            SabotajAuth auth = JsonConvert.DeserializeObject<SabotajAuth>(result.ToString());

            if (auth.result == true)
            {
                Console.WriteLine("Login OK!");
                ProcessStartInfo pinfo = new ProcessStartInfo();
                pinfo.FileName = "C:\\Program Files (x86)\\TyperGames\\Sabotaj\\Sabotaj\\Sabotaj\\Binaries\\Win64\\Sabotaj.exe";
                pinfo.Arguments = "-sessionID " + auth.sId + " -lang tr -FeatureLevelES31";
                Process.Start(pinfo);
            }
            else
            {
                Console.WriteLine("ERROR: " + auth.errortext);
            }
        }

        static void Main(string[] args)
        {
            Console.Write("Username:");
            var username = Console.ReadLine();
            Console.Write("Password:");
            var password = Console.ReadLine();
            
        begin:
                Launch(username, password);

            Console.WriteLine("Press y for relaunch, n for exit.");
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                Console.WriteLine("=");
                goto begin ;
            }
        }
    }
}

public class SabotajAuth
{
    public int error { get; set; }
    public string errortext { get; set; }
    public bool result { get; set; }
    public string sId { get; set; }
}
