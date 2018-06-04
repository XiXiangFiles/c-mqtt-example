using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTTtest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MqttClient client;
        String str = "";
        String topic;
        TextBox t;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Connect_btn_Click(object sender, RoutedEventArgs e)
        {
            String ip=IP_t.Text;
            Console.WriteLine(ip);
            client = new MqttClient(IPAddress.Parse(ip));
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
            t = megs;
            
        }

        

      
        private void subscribe_btn_Click(object sender, RoutedEventArgs e)
        {
            topic=topic_t.Text;
            Console.WriteLine(topic);
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            //megs.Text = str;
            System.Threading.Thread newThread =new System.Threading.Thread(AMethod);
            newThread.Start();

        }
        void AMethod()
        {
            while (true) {
                Dispatcher.Invoke(delegate {              // we need this construction because the receiving code in the library and the UI with textbox run on different threads
                    megs.Text = str;
                });
            }
            
        }

        private void publish_btn_Click(object sender, RoutedEventArgs e)
        {
            String msg=mes_push.Text;
            Console.WriteLine(msg);
            client.Publish(topic, Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
          
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received 
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            Console.WriteLine(ReceivedMessage);
            str = ReceivedMessage;

        }

    }

}
