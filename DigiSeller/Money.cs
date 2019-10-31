using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;

namespace DigiSeller
{
    public enum TransferCurrency
    {
        WMZ,
        WMR,
        WME,
        WMU,
        WMX
    }


    public class Money
    {
        /// <summary>
        /// Реализует метод api, получающий баланс
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetBalance(string token)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Get($"https://api.digiseller.ru/api/getbalance/{token}");
            string responceString = response.ToString();
            return responceString;
        }
        /// <summary>
        /// Реализует метод api, отправляющий запрос на вывод
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string TransferMoney(string token, TransferCurrency currency)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Get($"https://api.digiseller.ru/api/transfermoney/{token}/{currency}");
            string responceString = response.ToString();
            return responceString;
        }


    }
}
