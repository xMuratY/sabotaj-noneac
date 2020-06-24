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
        public static string CreateMD5Hash(string input)
        {
            // Step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }
        static void Main(string[] args)
        {
            Console.Write("Username:");
            var username = Console.ReadLine();
            Console.Write("Password:");
            var password = Console.ReadLine();

            HttpRequest req = new HttpRequest();

            req.AddHeader("X-API-KEY", "51160e2a7adfbceab4ba4b96cec18f4d");
            req.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            req.UserAgent = "xd";

            var reqParams = new RequestParams();
            reqParams["u"] = username;
            reqParams["p"] = CreateMD5Hash(password);

            var result = req.Post("https://api.sabotajoyun.com/login?", reqParams);

            SabotajAuth auth = JsonConvert.DeserializeObject<SabotajAuth>(result.ToString());
            
            if(auth.result == true)
            {
                Console.WriteLine("Login OK!");
                ProcessStartInfo pinfo = new ProcessStartInfo();
                pinfo.FileName = "C:\\Program Files (x86)\\TyperGames\\Sabotaj\\Sabotaj\\Sabotaj\\Binaries\\Win64\\Sabotaj.exe";
                //pinfo.WorkingDirectory = "C:\\Program Files (x86)\\TyperGames\\Sabotaj\\Sabotaj\\Sabotaj\\Binaries\\Win64";
                pinfo.Arguments = "-sessionID " + auth.sId + " -lang tr -FeatureLevelES31";
                Process.Start(pinfo);
            }
            else
            {
                Console.WriteLine("ERROR: " + auth.errortext);
            }

            Console.Read();
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