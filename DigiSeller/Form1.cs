using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigiSeller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.DataSource = Enum.GetValues(typeof(ContentType));
            comboBox2.DataSource = Enum.GetValues(typeof(Currency));
            comboBox3.SelectedIndex = 0;
            comboBox4.DataSource = Enum.GetValues(typeof(TransferCurrency));
            comboBox5.DataSource = Enum.GetValues(typeof(ReviewType));
            textBox1.Text = Properties.Settings.Default.login;
            textBox2.Text = Properties.Settings.Default.password;
            textBox3.Text = Properties.Settings.Default.guid;


        }

        /// <summary>
        /// Сбрасывает сохранённые данные для входа.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            // richTextBox11.Text = Communication.GetReviews("180900", "1860620", ReviewType.all, 1, 10);
            //string stop = "";
            Properties.Settings.Default.Reset();
            //string dcp = Helper.GetCategories("797278");
            //string dcp2 = Helper.SellerGoods("797278");
            //string stop = "";
        }


        /// <summary>
        /// Сериализует в json описание нового товара.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateUniqueFixed(object sender, EventArgs e)
        {
            if (token.Length < 2)
            {
                MessageBox.Show("Сначала войдите в аккаунт.");
                return;
            }

            dynamic product = new ExpandoObject();

            product.content_type = comboBox1.SelectedValue.ToString();


            List<dynamic> categories = new List<dynamic>();

            dynamic category = new ExpandoObject();
            category.owner = 0;
            category.category_id = (int)numericUpDown3.Value;

            categories.Add(category);
            product.categories = categories;


            List<dynamic> names = new List<dynamic>();

            dynamic nameRU = new ExpandoObject();
            nameRU.locale = "ru-RU";
            nameRU.value = textBox4.Text;

            dynamic nameEN = new ExpandoObject();
            nameEN.locale = "en-US";
            nameEN.value = textBox5.Text;

            names.Add(nameRU);
            names.Add(nameEN);

            product.name = names;

            List<dynamic> descriptions = new List<dynamic>();

            dynamic descriptionRU = new ExpandoObject();
            descriptionRU.locale = "ru-RU";
            descriptionRU.value = richTextBox1.Text;


            dynamic descriptionEN = new ExpandoObject();
            descriptionEN.locale = "en-US";
            descriptionEN.value = richTextBox2.Text;

            descriptions.Add(descriptionRU);
            descriptions.Add(descriptionEN);

            product.description = descriptions;

            dynamic price = new ExpandoObject();
            price.price = (double)numericUpDown1.Value;
            price.currency = comboBox2.SelectedValue.ToString();
            product.price = price;


            List<dynamic> additionalInfos = new List<dynamic>();

            dynamic addinfoRU = new ExpandoObject();
            addinfoRU.locale = "ru-RU";
            addinfoRU.value = richTextBox3.Text;

            dynamic addinfoEN = new ExpandoObject();
            addinfoEN.locale = "en-US";
            addinfoEN.value = richTextBox4.Text;

            additionalInfos.Add(addinfoRU);
            additionalInfos.Add(addinfoEN);

            product.add_info = additionalInfos;

            product.comission_partner = (int)numericUpDown2.Value;

            if ((int)numericUpDown4.Value > 0)
            {
                dynamic bonus = new ExpandoObject();
                bonus.enabled = true;
                bonus.percent = (int)numericUpDown4.Value;
                product.bonus = bonus;
            }

            if ((int)numericUpDown5.Value > 0)
            {
                dynamic guarantee = new ExpandoObject();
                guarantee.enabled = true;
                guarantee.value = (int)numericUpDown5.Value;
                product.guarantee = guarantee;
            }

            if (comboBox3.SelectedIndex == 0)
            {
                product.address_required = false;
            }
            else
            {
                product.address_required = true;
            }

            List<dynamic> instructions = new List<dynamic>();

            dynamic instructionRU = new ExpandoObject();
            instructionRU.locale = "ru-RU";
            instructionRU.value = richTextBox5.Text;

            dynamic instructionEN = new ExpandoObject();
            instructionEN.locale = "en-US";
            instructionEN.value = richTextBox6.Text;

            instructions.Add(instructionRU);
            instructions.Add(instructionEN);


            dynamic instruction = new ExpandoObject();
            instruction.type = "Text";
            instruction.locales = instructions;

            product.instruction = instruction;


            string content = JsonConvert.SerializeObject(product);


            string responce = ProductManager.CreateUniqueFixed(token, content);
            if (responce.Contains("500 Internal Server Error"))
            {
                label8.ForeColor = Color.Red;
                label8.Text = "Ошибка: отсутствуют категории для добавления.";
                return;
            }
            dynamic resp = JsonConvert.DeserializeObject(responce);

            if (resp.retval == 0)
            {
                label8.ForeColor = Color.Green;
                label8.Text = "Товар добавлен. ID: " + resp.content.product_id;
                GetData(null, null);

            }
            if (resp.retval == 1)
            {
                label8.ForeColor = Color.Red;
                label8.Text = "Ошибка: " + resp.errors[0].message[1].value;
            }
            //dynamic verifycode = new ExpandoObject();
            //verifycode.auto_verify = false;
            //verifycode.verify_url = "";
            //product.verify_code = verifycode;
            //dynamic preorder = new ExpandoObject();
            //preorder.enabled = false;
            //preorder.delivery_date = DateTime.Now.ToString("yyyy-MM-dd");
            //product.preorder = preorder;
            //product.enabled = true;
            string stop = "";
        }

        string sellerID = "";
        string token = "";
        string login = "";
        string password = "";
        string apiguid = "";



        /// <summary>
        /// Обработчик нажатия на кнопку логина.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginClick(object sender, EventArgs e)
        {
            Action worker = delegate ()
            {
                login = textBox1.Text;
                password = textBox2.Text;
                apiguid = textBox3.Text;
                string temp = Api.GetApiToken(login, password, apiguid);

                dynamic responceJson = JsonConvert.DeserializeObject(temp);

                if (responceJson.token != null)
                {
                    token = responceJson.token;
                }
                if (responceJson.seller_id != null)
                {
                    sellerID = responceJson.seller_id;
                }
                string stop = "";
            };
            Action dg = delegate ()
            {
                if (token.Length < 2)
                {
                    label22.ForeColor = Color.Red;
                    label22.Text = "Ошибка.";
                }
                else
                {
                    UpdateNotifications(null, null);
                    CheckChat(null, null);
                    Properties.Settings.Default.login = login;
                    Properties.Settings.Default.password = password;
                    Properties.Settings.Default.guid = apiguid;
                    Properties.Settings.Default.Save();
                    label22.ForeColor = Color.Green;
                    label22.Text = "Токен получен.";
                    timer2.Enabled = true;
                    numericUpDown7.Value = long.Parse(sellerID);
                    GetData(null, null);
                }
            };

            AsyncCallback cb = delegate (IAsyncResult ar)
            {
                label22.Invoke(dg);
                worker.EndInvoke(ar);
            };
            worker.BeginInvoke(cb, null);

        }


        /// <summary>
        /// Заливает строковый товар.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTextProduct(object sender, EventArgs e)
        {
            if (token.Length < 2)
            {
                MessageBox.Show("Сначала войдите в аккаунт.");
                return;
            }
            dynamic d = new ExpandoObject();
            List<string> result = richTextBox7.Text.Split('\n').ToList();
            toAdd.AddRange(result);
            if (toAdd.Count == 1 && toAdd[0] == "")
            {
                label23.Text = $"Загружено 0 строк.";
                return;
            }
            label23.Text = $"Загружено {toAdd.Count} строк.";
            d.product_id = (int)numericUpDown6.Value;
            List<dynamic> content = new List<dynamic>();
            if (toAdd.Count == 0)
            {
                MessageBox.Show("Нечего добавлять.");
                return;
            }
            for (int i = 0; i < toAdd.Count; i++)
            {
                dynamic temp = new ExpandoObject();
                temp.value = toAdd[i];
                content.Add(temp);
            }
            d.content = content;

            string json = JsonConvert.SerializeObject(d);

            string ret = ProductManager.AddTextProduct(token, json);
            toAdd.Clear();
        }

        List<string> toAdd = new List<string>();

        /// <summary>
        /// Подгружает строки из файла для последующей заливки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFromFile(object sender, EventArgs e)
        {
            toAdd.AddRange(Helper.LoadBaseFromFile());
            int show = 10;
            if (toAdd.Count < 10)
            {
                show = toAdd.Count;
            }
            for (int i = 0; i < show; i++)
            {
                richTextBox7.Text += toAdd[i] + "\r\n";
            }
            label23.Text = $"Загружено {toAdd.Count} строк.";
        }
        /// <summary>
        /// Очищает список строк.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearToAddList(object sender, EventArgs e)
        {
            toAdd.Clear();
            label23.Text = $"Загружено {toAdd.Count} строк.";

        }


        /// <summary>
        /// Получает статистику покупок и выводит её.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetPurchaseStatistics(object sender, EventArgs e)
        {
            if (token.Length < 2)
            {
                MessageBox.Show("Сначала войдите в аккаунт.");
                return;
            }
            int period = (DateTime.Now - dateTimePicker1.Value).Days;
            if (period < 1) { period = 1; }
            string resp = Statistics.SaleStatistics(token, period, Currency.RUB);
            listBox1.Items.Clear();
            dynamic responceJson = JsonConvert.DeserializeObject(resp);
            foreach (var item in responceJson.statistics)
            {
                listBox1.Items.Add($"{item.Period} Сумма: {item.Amount.ToString()} WMR Количество: {item.Count}");
            }
        }
        /// <summary>
        /// Получает статистику просмотров и выводит её.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetViewsStatistics(object sender, EventArgs e)
        {
            if (token.Length < 2)
            {
                MessageBox.Show("Сначала войдите в аккаунт.");
                return;
            }
            int period = (DateTime.Now - dateTimePicker1.Value).Days;
            if (period < 1) { period = 1; }
            string resp = Statistics.ViewsStatistics(token, period);
            listBox1.Items.Clear();
            dynamic responceJson = JsonConvert.DeserializeObject(resp);
            foreach (var item in responceJson.statistics)
            {
                listBox1.Items.Add($"{item.Period} Количество: {item.Count}");
            }
        }










        /// <summary>
        /// Получает и выводит балансы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetBals(object sender, EventArgs e)
        {
            if (token.Length < 2)
            {
                MessageBox.Show("Сначала войдите в аккаунт.");
                return;
            }
            listBox2.Items.Clear();
            string ret = Money.GetBalance(token);
            dynamic responceJson = JsonConvert.DeserializeObject(ret);
            listBox2.Items.Add("WMZ доступно: " + responceJson.Zfree);
            listBox2.Items.Add("WMZ заблокировано: " + responceJson.Zlock);
            listBox2.Items.Add("WMR доступно: " + responceJson.Rfree);
            listBox2.Items.Add("WMR заблокировано: " + responceJson.Rlock);
            listBox2.Items.Add("WME доступно: " + responceJson.Efree);
            listBox2.Items.Add("WME заблокировано: " + responceJson.Elock);
            listBox2.Items.Add("WMU доступно: " + responceJson.Ufree);
            listBox2.Items.Add("WMU заблокировано: " + responceJson.Ulock);
            listBox2.Items.Add("WMX доступно: " + responceJson.Xfree);
            listBox2.Items.Add("WMX заблокировано: " + responceJson.Xlock);
        }
        /// <summary>
        /// Отправляет команду на вывод денег.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WithdrawMoney(object sender, EventArgs e)
        {
            if (token.Length < 2)
            {
                MessageBox.Show("Сначала войдите в аккаунт.");
                return;
            }
            string ret = Money.TransferMoney(token, (TransferCurrency)comboBox4.SelectedValue);
            dynamic responceJson = JsonConvert.DeserializeObject(ret);
            label25.Text = "Будет выведено: " + responceJson.amount + comboBox4.SelectedValue;
        }

        /// <summary>
        /// Получает и выводит список категорий и список товаров для id.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetData(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();

            string dcp = Helper.GetCategories(((int)numericUpDown7.Value).ToString());

            dynamic categories = JsonConvert.DeserializeObject(dcp);
            try
            {

                foreach (var item in categories.content)
                {
                    string name = item.name[0].value;
                    listBox4.Items.Add((string)item.id + ": " + (string)name);
                    listBox6.Items.Add((string)item.id + ": " + (string)name);
                }

                string dcp2 = Helper.SellerGoods(((int)numericUpDown7.Value).ToString());

                dynamic goods = JsonConvert.DeserializeObject(dcp2);


                foreach (var item in goods.rows)
                {
                    listBox3.Items.Add((string)item.id_goods + ": " + (string)item.name_goods);
                    listBox5.Items.Add((string)item.id_goods + ": " + (string)item.name_goods);
                    listBox8.Items.Add((string)item.id_goods + ": " + (string)item.name_goods);
                }
            }
            catch { }

            string stop = "";
        }

        private void PushGoodId(object sender, EventArgs e)
        {
            if (listBox5.SelectedItem != null)
            {
                numericUpDown6.Value = long.Parse((listBox5.SelectedItem).ToString().Split(':')[0]);
            }
        }

        private void PushCategoryId(object sender, EventArgs e)
        {
            if (listBox6.SelectedItem != null)
            {
                numericUpDown3.Value = long.Parse((listBox6.SelectedItem).ToString().Split(':')[0]);
            }
        }

        private void UpdateNotifications(object sender, EventArgs e)
        {
            if (token.Length < 2)
            {
                return;
            }
            string temp = Communication.GetNotifications(token);
            dynamic notifications = JsonConvert.DeserializeObject(temp);
            richTextBox10.Text = "";
            foreach (var item in notifications.notifications)
            {
                richTextBox10.Text += item.Subj + ": " + item.Text + "\r\n";
            }

            string stop = "";


        }

        private void OpenReviews(object sender, EventArgs e)
        {
            if (token.Length < 2)
            {
                MessageBox.Show("Сначала войдите в аккаунт.");
                return;
            }
            if (listBox8.SelectedItem != null)
            {
                richTextBox11.Text = "";
                string productID = (listBox8.SelectedItem).ToString().Split(':')[0];
                //string dcp = Communication.GetReviews("180900", "1860620", ReviewType.all, 1, 10).Replace("#", "").Replace("-", "").Replace("digiseller.response", "digisellerresponse");
                string dcp = Communication.GetReviews(sellerID, productID, (ReviewType)comboBox5.SelectedValue, (int)numericUpDown8.Value, (int)numericUpDown9.Value).Replace("#", "").Replace("-", "").Replace("digiseller.response", "digisellerresponse").Replace("@cnt", "cnt");
                dynamic reviews = JsonConvert.DeserializeObject(dcp);
                label41.Text = "Количество отзывов: " + reviews.digisellerresponse.reviews.cnt;
                try
                {
                    foreach (var item in reviews.digisellerresponse.reviews.review)
                    {
                        richTextBox11.Text += $"[{item.date}] {item.info.cdatasection}\r\n";
                    }
                }
                catch
                {

                }
            }
        }
    }
}
