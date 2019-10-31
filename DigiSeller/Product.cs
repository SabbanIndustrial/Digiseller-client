using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using Newtonsoft.Json;

namespace DigiSeller
{
    public enum ContentType
    {
        Text,
        File,
        Url,
        DigisellerCode,
        Form
    }

    public enum Currency
    {
        RUB,
        USD,
        EUR,
        UAH,
    }

    /// <summary>
    /// Не используется
    /// </summary>
    public class Product
    {
        public string content_type;

        public List<dynamic> name = new List<dynamic>();

        public dynamic price;

        public int comission_partner;
        public List<dynamic> categories = new List<dynamic>();

        public dynamic bonus;
        public dynamic guarantee;

        public List<dynamic> description = new List<dynamic>();

        public List<dynamic> add_info = new List<dynamic>();

        public bool address_required;

        public dynamic verify_code;

        public dynamic preorder;

        public dynamic instruction;

        //public int present_product_id;
    }

    public class Creator
    {

        public static void Create()
        {
            Product p = new Product();
            p.content_type = ContentType.Text.ToString();

            dynamic nameru = new ExpandoObject();
            nameru.locale = "ru-RU";
            nameru.value = "Тест продукт";
            p.name.Add(nameru);

            dynamic price = new ExpandoObject();
            price.price = 725;
            price.currency = Currency.RUB.ToString();
            p.price = price;

            p.comission_partner = 15;

            dynamic categories1 = new ExpandoObject();
            categories1.owner = 0;
            categories1.category_id = 7074;
            p.categories.Add(categories1);

            dynamic bonus = new ExpandoObject();
            bonus.enabled = true;
            bonus.percent = 5;
            p.bonus = bonus;

            dynamic guarantee = new ExpandoObject();
            guarantee.enabled = true;
            guarantee.value = 5;
            p.guarantee = guarantee;

            dynamic descriptionru = new ExpandoObject();
            descriptionru.locale = "ru-RU";
            descriptionru.value = "Описание";
            p.description.Add(descriptionru);

            dynamic addinforu = new ExpandoObject();
            addinforu.locale = "ru-RU";
            addinforu.value = "Тестовая дополнительная информация";
            p.add_info.Add(addinforu);

            p.address_required = false;

            dynamic verifycode = new ExpandoObject();
            verifycode.auto_verify = true;
            verifycode.verify_url = "http://example.com/verify";
            p.verify_code = verifycode;

            dynamic preorder = new ExpandoObject();
            preorder.enabled = true;
            preorder.delivery_date = DateTime.Now.ToString("yyyy-MM-dd");
            p.preorder = preorder;

            dynamic instructionru = new ExpandoObject();
            instructionru.locale = "ru-RU";
            instructionru.value = "Русская инструкция";

            List<dynamic> instlocales = new List<dynamic>();
            instlocales.Add(instructionru);


            dynamic instruction = new ExpandoObject();
            instruction.type = "Text";
            instruction.locales = instlocales;

            p.instruction = instruction;

           // p.present_product_id = 123321;


            string content = JsonConvert.SerializeObject(p);
            string stop = "";
        }




    }


}
