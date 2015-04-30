using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebRole1
{
    public partial class TestWebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            QueueClient Client = QueueClient.CreateFromConnectionString(connectionString, "testqueue");
            Client.Send(new BrokeredMessage());

            // Create message, passing a string message for the body
            BrokeredMessage message = new BrokeredMessage(TextBox1.Text);
            Client.Send(message);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Label1.Text = "";
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            QueueClient Client = QueueClient.CreateFromConnectionString(connectionString, "testqueue");
            BrokeredMessage message = Client.Receive();
            if (message != null)
            {
                Stream fstream = message.GetBody<Stream>();
                byte[] buffer = new byte[fstream.Length];
                fstream.Read(buffer, 0, (int)fstream.Length);
                string decodedString = Encoding.UTF8.GetString(buffer);

                Label1.Text = decodedString;
                message.Complete();
            }
            //// Configure the callback options
            //OnMessageOptions options = new OnMessageOptions();
            //options.AutoComplete = false;
            //options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            //// Callback to handle received messages
            //Client.OnMessage((message) =>
            //{
            //    try
            //    {
            //        // Process message from queue
            //        //Console.WriteLine("Body: " + message.GetBody<string>());
            //        //Console.WriteLine("MessageID: " + message.MessageId);
            //        //Console.WriteLine("Test Property: " +
            //        //message.Properties["TestProperty"]);

            //        Label1.Text = message.GetBody<string>();
            //        // Remove message from queue
            //        message.Complete();
            //    }
            //    catch (Exception)
            //    {
            //        // Indicates a problem, unlock message in queue
            //        message.Abandon();
            //    }
            //});
        }
 
    }
}