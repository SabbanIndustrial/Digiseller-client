using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;

namespace DigiSeller
{
    /// <summary>
    /// Реализует методы api для получения статистики
    /// </summary>
    public class Statistics
    {



        public static string Sells(string token, string content)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Post("https://api.digiseller.ru/api/seller-sells", content, "application/json");
            string responceString = response.ToString();
            return responceString;
        }
        public static string SaleStatistics(string token, int period, Currency currency)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Get($"https://api.digiseller.ru/api/salesstatistics/{token}/days/{period}/{currency}");
            string responceString = response.ToString();
            return responceString;
        }
        public static string ViewsStatistics(string token, int period)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Get($"https://api.digiseller.ru/api/viewsstatistics/{token}/days/{period}");
            string responceString = response.ToString();
            return responceString;
        }

    }
}
