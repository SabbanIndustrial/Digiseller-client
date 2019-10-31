using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using xNet;

namespace DigiSeller
{
    public enum CorrType
    {
        support,
        admin,
        user,
        buyer,
        visitor,
        anonym
    }

    public enum ReviewType
    {
        good,
        bad,
        all,
    }


    /// <summary>
    /// Реализует методы api, для общения.
    /// </summary>
    public class Communication
    {

        public static string Messages(string token, CorrType corrType, string corrID, string lastID)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Get($"https://api.digiseller.ru/api/messages/{token}/{corrType}/{corrID}/{lastID}");
            string responceString = response.ToString();
            return responceString;
        }


        public static string NewMessage(string token, CorrType corrType, long corrID, string text)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            dynamic json = new ExpandoObject();
            json.CorrType = corrType.ToString();
            json.CorrID = corrID;
            json.Text = text;
            string content = JsonConvert.SerializeObject(json);
            HttpResponse response = request.Post($"https://api.digiseller.ru/api/newmessage/{token}", content, "application/json");
            string responceString = response.ToString();
            return responceString;
        }

        public static string GetChatList(string token, CorrType corrType)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Get($"https://api.digiseller.ru/api/chatlist/{token}/{corrType}");
            string responceString = response.ToString();
            return responceString;
        }

        public static string GetUnreadMessagesCount(string token)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Get($"https://api.digiseller.ru/api/unreadmessages/{token}");
            string responceString = response.ToString();
            return responceString;
        }

        public static string CheckNewMessages(string token, long corrID, CorrType corrType, long lastID)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Get($"https://api.digiseller.ru/api/checknewmessages/{token}/{corrID}/{corrType}/{lastID}");
            string responceString = response.ToString();
            return responceString;
        }

        public static string CheckNewChats(string token, long ID1, long ID2, long ID3, long ID4)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Get($"https://api.digiseller.ru/api/checknewchats/{token}/{ID1}/{ID2}/{ID3}/{ID4}");
            string responceString = response.ToString();
            return responceString;
        }
        public static string GetNewChats(string token, long ID1, long ID2, long ID3, long ID4)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Get($"https://api.digiseller.ru/api/getnewchats/{token}/{ID1}/{ID2}/{ID3}/{ID4}");
            string responceString = response.ToString();
            return responceString;
        }


        public static string GetNotifications(string token)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");
            HttpResponse response = request.Get($"https://api.digiseller.ru/api/getnotifications/{token}/{0}");
            string responceString = response.ToString();
            return responceString;
        }


        public static string GetReviews(string sellerID, string productID, ReviewType reviewType, int pageNum, int rowsPerPage)
        {
            HttpRequest request = new HttpRequest();
            request.IgnoreProtocolErrors = true;
            request.AddHeader(HttpHeader.Accept, "application/json");

            //dynamic digiseller = new ExpandoObject();
            //digiseller.seller.id = long.Parse(sellerID);
            //digiseller.product.id = long.Parse(productID);
            //digiseller.reviews.type = reviewType;
            //digiseller.pages.num = pageNum;
            //digiseller.pages.rows = rowsPerPage;


            string content = $"<digiseller.request><seller><id>{sellerID}</id></seller><product><id>{productID}</id></product><reviews><type>{reviewType}</type></reviews><pages><num>{pageNum}</num><rows>{rowsPerPage}</rows></pages></digiseller.request>";
            HttpResponse response = request.Post($"http://shop.digiseller.ru/xml/shop_reviews.asp", content, "text/xml");
            string responceString = response.ToString();
            if (responceString.Contains("500 Internal Server Error"))
            {
                return "";
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(responceString);
            
            string json = JsonConvert.SerializeXmlNode(doc);

            //string jsonResponce = JsonConvert.SerializeXmlNode(responceString);
            return json;
        }



    }
}
