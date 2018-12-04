using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BerthaPiSensorReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            double _temperature;
            double _pressure;
            double _humidity;
            //int _userId;
            int number = 0;

            //Creates a UdpClient for reading incoming data.
            UdpClient udpReceiver = new UdpClient(7890);
            // This IPEndPoint will allow you to read datagrams sent from any ip-source on port 9000


            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 7890);
            //udpReceiver.Connect(RemoteIpEndPoint);

            // Blocks until a message returns on this socket from a remote host.
            Console.WriteLine("Receiver is blocked");
            try
            {
                while (true)
                {
                    byte[] receiveBytes = udpReceiver.Receive(ref RemoteIpEndPoint);

                    string receivedData = Encoding.ASCII.GetString(receiveBytes);

                    if (receivedData.Equals("STOP.Secret")) throw new Exception("Receiver stopped");

                    Console.WriteLine("Sender: " + receivedData.ToString());
                    //Console.WriteLine("This message was sent from " +
                    //                            RemoteIpEndPoint.Address.ToString() +
                    //                            " on their port number " +
                    //                            RemoteIpEndPoint.Port.ToString());
                    //if (receivedData.Equals("STOP")) throw new Exception("Receiver stopped");

                    // string[] textLines = receivedData.Split(' '); is possible but a little more difficult 
                    //best to split by '\n' or \r\ and then split by ' ' or  ':'
                    string[] textLines = receivedData.Split('\n');

                    for (int index = 0; index < textLines.Length; index++)
                        Console.Write(textLines[index]);

                    string[] list1 = textLines[0].Split(':');
                    string text1 = list1[1];
                    string[] list2 = textLines[1].Split(':');
                    string text2 = list2[1];
                    string[] list3 = textLines[2].Split(':');
                    string text3 = list3[1];
                    //string[] list4 = textLines[3].Split(':');
                    //string text4 = list4[1];


                    _temperature = double.Parse(text1);
                    _pressure = double.Parse(text2);
                    _humidity = double.Parse(text3);
                    //_userId = int.Parse(text4);

                    //Console.WriteLine("Numerical values");
                    Console.WriteLine("Temperature" + _temperature);
                    Console.WriteLine("Pressure" + _pressure);
                    Console.WriteLine("Humidity" + _humidity);
                    //Console.WriteLine("UserId: " + _userId);

                    RaspberryRecord raspberry = new RaspberryRecord(_temperature, _pressure, _humidity, 1);

                    PostRaspberryRecordAsync(raspberry);
                    Thread.Sleep(1000);
                    number++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

                static async Task PostRaspberryRecordAsync(RaspberryRecord raspberry)
                {
                    HttpClient client = new HttpClient();

                    client.BaseAddress = new Uri("https://berthapibeta20181204125106.azurewebsites.net");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/raspberryrecords", raspberry);
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine("" + response);
                }
    }
}
