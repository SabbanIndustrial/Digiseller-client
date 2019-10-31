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
    /// Реализует методы api для взаимодействия с товарами.
    /// </summary>
    public class ProductManager
    {

        public static string CreateUniqueFixed(string token, string product)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Post($"https://api.digiseller.ru/api/product/create/uniquefixed?token={token}", product, "application/json");
            string responceString = response.ToString();
            return responceString;
        }



        public static string AddTextProduct(string token, string product)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Post($"https://api.digiseller.ru/api/product/content/add/text?token={token}", product, "application/json");
            string responceString = response.ToString();
            return responceString;
        }





    }
}
