using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;

namespace DigiSeller
{
    public class Helper
    {
        public static string GetTimestamp()
        {
            long unixTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            return unixTime.ToString();
        }

        /// <summary>
        /// Хеширует строку UTF8 алгоритмом SHA256
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetHashSha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }

        /// <summary>
        /// Подгружает строки из файла и возвращает список
        /// </summary>
        /// <returns></returns>
        public static List<string> LoadBaseFromFile()
        {
            List<string> toReturn = new List<string>();
            using (OpenFileDialog OFD = new OpenFileDialog())
            {
                OFD.Multiselect = true;
                OFD.Filter = "Текстовые файлы | *.txt";
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    string[] filesAccs = OFD.FileNames;
                    foreach (string filePath in filesAccs)
                    {
                        using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8, false))
                        {
                            while (!streamReader.EndOfStream)
                            {
                                string input = streamReader.ReadLine();
                                toReturn.Add(input);
                            }
                        }
                    }

                }
            }
            return toReturn;
        }


        /// <summary>
        /// Вспомагательные методы api, бесполезны.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetCategories(string id)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Get($"https://api.digiseller.ru/api/dictionary/categories/{id}/{0}");
            string responceString = response.ToString();
            return responceString;
        }

        public static string SellerGoods(string id)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            dynamic json = new ExpandoObject();
            json.id_seller = long.Parse(id);
            json.rows = 100;
            json.currency = "UAH";
            string content = JsonConvert.SerializeObject(json);
            HttpResponse response = request.Post($"https://api.digiseller.ru/api/seller-goods", content, "application/json");
            string responceString = response.ToString();
            return responceString;
        }

        public static string GetID(string token)
        {
            string ret2 = Communication.Messages(token, CorrType.admin, "0", "0");

            return "";

        }

        //public static void LabelDelay(this Control control)
        //{
        //    Action worker = delegate ()
        //    {

        //    };
        //    Action dg = delegate ()
        //    {

        //    };

        //    AsyncCallback cb = delegate (IAsyncResult ar)
        //    {
        //        control.Invoke(dg);
        //        worker.EndInvoke(ar);
        //    };
        //    worker.BeginInvoke(cb, null);
        //}


    }
}
