using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using System.Threading;
using System.Net;
using System.IO;

namespace BitcoinAddressSearch
{
    class Program
    {
        public static string location = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\";
        public static string Get(String url)
        {

            try
            {
                Console.WriteLine(url);
                String r = "";
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
                httpWebRequest.Method = "GET";
                var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var responseStream = httpWebResponse.GetResponseStream();
                if (responseStream != null)
                {
                    var streamReader = new StreamReader(responseStream);
                    r = streamReader.ReadToEnd();
                }
                if (responseStream != null) responseStream.Close();
                //Console.WriteLine(r);
                Console.WriteLine("Ret " + r);
                return r;
            }
            catch (WebException ex)
            {
                return null;
            }
        }

        static int delay = 1;
        static void Main(string[] args)
        {

            Console.WriteLine("## Brute force 0.0.0.1 bitcoin ##");
            Console.WriteLine("Enter delay ms(1): ");
            delay = int.Parse( Console.ReadLine());
            SearchBitcoinAddress();
            Console.ReadLine();
        }



        static void newKey()
        {
            try
            {
                Key k = new Key();
                BitcoinSecret secretKey = k.GetBitcoinSecret(Network.Main);
                BitcoinAddress addressKey = secretKey.PubKey.GetAddress(Network.Main);

                String ret = Get("https://blockchain.info/q/addressbalance/" + addressKey.ToString());
                if (ret != "0")
                {
                    Console.WriteLine("************ Check this out     Secret :" + secretKey + "         Key :" + addressKey.ToString());
                    string s = secretKey + " ----- " + addressKey.ToString();
                    System.IO.File.WriteAllText(location + "BitcoinAddres-" + addressKey.ToString() + ".txt", s);
                }
            }
            catch

            { 
            }
        }

        public static void SearchBitcoinAddress()
        {
            
            while (true)
            {

                Thread t = new Thread(newKey);
                t.Start();

                Thread.Sleep(delay);
            }
        }

    }
}