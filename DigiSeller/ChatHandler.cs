using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigiSeller
{


    /// <summary>
    /// В данной части отделена логика чата.
    /// </summary>
    public partial class Form1
    {

        ChatData currentTalk = new ChatData();
        Dictionary<string, string> chats = new Dictionary<string, string>();
        List<ChatData> chates = new List<ChatData>();

        /// <summary>
        /// Метод, вызываемый таймером, обновляет чаты.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckChat(object sender, EventArgs e)
        {
            if (token.Length < 2)
            {
                MessageBox.Show("Сначала войдите в аккаунт.");
                return;
            }
            List<ChatData> temp = new List<ChatData>();

            Action worker = delegate ()
            {
                temp.Clear();
                string ret = Communication.GetChatList(token, CorrType.admin);
                dynamic d = JsonConvert.DeserializeObject(ret);
                foreach (var item in d.chats)
                {
                    temp.Add(new ChatData((string)item.Name, (long)item.CorrID, CorrType.admin));
                }
                ret = Communication.GetChatList(token, CorrType.buyer);
                d = JsonConvert.DeserializeObject(ret);
                foreach (var item in d.chats)
                {
                    temp.Add(new ChatData((string)item.Name, (long)item.CorrID, CorrType.buyer));
                }
                ret = Communication.GetChatList(token, CorrType.user);
                d = JsonConvert.DeserializeObject(ret);
                foreach (var item in d.chats)
                {
                    temp.Add(new ChatData((string)item.Name, (long)item.CorrID, CorrType.user));
                }
            };
            Action dg = delegate ()
            {
                dynamic unreadJson = JsonConvert.DeserializeObject(Communication.GetUnreadMessagesCount(token));

                label30.Text = "гостей:" + unreadJson.count1;
                label31.Text = "покупателей:" + unreadJson.count2;
                label32.Text = "продавцов:" + unreadJson.count3;
                label33.Text = "администрации:" + unreadJson.count4;
                chates.Clear();
                chates.AddRange(temp);
                flowLayoutPanel2.Controls.Clear();
                foreach (var item in chates)
                {
                    Button b = new Button();
                    b.Size = new Size(130, 30);
                    b.Name = item.name;
                    b.Text = item.name;
                    b.Click += ChatsClick;
                    flowLayoutPanel2.Controls.Add(b);
                }
            };

            AsyncCallback cb = delegate (IAsyncResult ar)
            {
                flowLayoutPanel2.Invoke(dg);
                worker.EndInvoke(ar);
            };
            worker.BeginInvoke(cb, null);
            string stop = "";
        }




        private void ChatsClick(object sender, EventArgs e)
        {
            if (token.Length < 2)
            {
                MessageBox.Show("Сначала войдите в аккаунт.");
                return;
            }
            (sender as Button).ForeColor = Color.Green;
            label36.ForeColor = Color.Green;
            label36.Text = "Загрузка чата";
            dynamic responceJson = new ExpandoObject();

            Action worker = delegate ()
            {
                ChatData temp = new ChatData();
                foreach (var item in chates)
                {
                    if (item.name == ((Button)sender).Name)
                    {
                        temp = item;
                    }
                }
                if (temp.name == null)
                {
                    return;
                }

                string ret2 = Communication.Messages(token, temp.corrType, temp.corrID.ToString(), "0");
                responceJson = JsonConvert.DeserializeObject(ret2);
                currentTalk = temp;
            };
            Action dg = delegate ()
            {
                richTextBox8.Text = "";
                foreach (var item in responceJson.messages)
                {
                    if (item.IsAuthor == 0)
                    {
                        richTextBox8.Text += $"[{item.DateWrite}] Собеседник: {item.Text}\n";
                    }
                    else
                    {
                        richTextBox8.Text += $"[{item.DateWrite}] Вы: {item.Text}\n";
                    }
                }
                label36.Text = "чат:\r\n"+currentTalk.name;
            };
            AsyncCallback cb = delegate (IAsyncResult ar)
            {
                richTextBox8.Invoke(dg);
                worker.EndInvoke(ar);
            };
            worker.BeginInvoke(cb, null);
            string stop = "";
        }


        private void SendMessage(object sender, EventArgs e)
        {
            if (token.Length < 2)
            {
                MessageBox.Show("Сначала войдите в аккаунт.");
                return;
            }
            if (currentTalk.name == null)
            {
                return;
            }
            string ret = Communication.NewMessage(token, currentTalk.corrType, currentTalk.corrID, richTextBox9.Text);
            richTextBox9.Text = "";

            foreach (Control item in flowLayoutPanel2.Controls)
            {
                if (item is Button && (item as Button).Name == currentTalk.name)
                {
                    ChatsClick((item as Button), null);
                }
            }


        }




    }

    public struct ChatData
    {
        public ChatData(string Name, long CorrID, CorrType CType)
        {
            name = Name;
            corrID = CorrID;
            corrType = CType;
        }
        public string name;
        public long corrID;
        public CorrType corrType;
    }



}
