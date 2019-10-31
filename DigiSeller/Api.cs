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
    class Api
    {   
        /// <summary>
        /// Получает токен api для последующего использования
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="api_guid"></param>
        /// <returns></returns>
        public static string GetApiToken(string login, string password, string api_guid)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            string timestamp = Helper.GetTimestamp();
            string sign = Helper.GetHashSha256(password + api_guid + timestamp);

            dynamic json = new ExpandoObject();
            json.login = login;
            json.timestamp = long.Parse(timestamp);
            json.sign = sign;

            string content = JsonConvert.SerializeObject(json);

            HttpResponse response = request.Post("https://api.digiseller.ru/api/apilogin", content, "application/json");
            string responceString = response.ToString();
            return responceString;
        }
    }
}
