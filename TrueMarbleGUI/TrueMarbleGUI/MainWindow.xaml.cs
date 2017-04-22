using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using TrueMarbleData;
using System.IO;
using System.Diagnostics;

namespace TrueMarbleGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ITMDataController m_tmData;
        int zoomValue;  //to store the value received from slider
        int x = 0;  //intialize both x and y values to 0
        int y = 0;

        bool load = true;   

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (load == true)
            {
                //MessageBox.Show("Load Button works! \n Slider value is " + zoomValue);
                //textBox.Text = " " + zoomValue + " ," + x + " ," + y;

                try
                {
                    byte[] buff = m_tmData.LoadTile(zoomValue, x, y);
                    MemoryStream m_Stream = new MemoryStream(buff);
                    JpegBitmapDecoder jbd = new JpegBitmapDecoder(m_Stream, BitmapCreateOptions.None, BitmapCacheOption.None);
                    imgTile.Source = jbd.Frames[0];

                    if (buff == null && buff.Length > 0)
                    {
                        Console.WriteLine("Empty title\n");
                    }
                }

                catch (IOException IOe)
                {
                    MessageBox.Show("Failed to load image\n Error : " + IOe);
                }
            }
            else
            {
                MessageBox.Show("Out of range, please check your values");
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChannelFactory<ITMDataController> ITMChannel;
            NetTcpBinding tcpBind = new NetTcpBinding();
            string sURL = "net.tcp://localhost:65000/TMData";   //same URL as server's
            tcpBind.MaxReceivedMessageSize = System.Int32.MaxValue;     //assigning the max amount of message size and array length
            tcpBind.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;
            ITMChannel = new ChannelFactory<ITMDataController>(tcpBind, sURL);  //binds the url with channel
            m_tmData = ITMChannel.CreateChannel();  //opens channel

            
        }


        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //zoomValue = (int)slider2.Value;

            zoomValue = Convert.ToInt32(slider2.Value); //gets the value of slider change, and converts it into integer and stores in zoomValue
            //refreshTiles();

        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            x++; //increases the x axis by 1 unit per click
            if (x < 0 || x == 0)
            {
                x = 0;
                load = true;
                refreshTiles();
            }

            else if (x < m_tmData.GetNumTilesAcross(zoomValue))
            {
                load = true;
                refreshTiles();
            }

            else
            {
                x = m_tmData.GetNumTilesAcross(zoomValue) - 1;
                load = true;
                refreshTiles();
            }
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
           x--; //decreases the x axis by 1 unit per click

            if (x < 0 || x == 0)
            {
                x = 0;
                load = true;
                refreshTiles();
            }

            else if (x < m_tmData.GetNumTilesAcross(zoomValue))
            {
                load = true;
                refreshTiles();
            }

            else
            {
                x = m_tmData.GetNumTilesAcross(zoomValue) - 1;
                load = true;
                refreshTiles();
            }
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            y++; //increases the x axis by 1 unit per click

            if (y < 0 || y == 0)
            {
                y = 0;
                load = true;
                refreshTiles();
            }

            else if (y < m_tmData.GetNumTilesDown(zoomValue))
            {
                load = true;
                refreshTiles();
            }

            else
            {
                y = m_tmData.GetNumTilesDown(zoomValue) - 1;
                load = true;
                refreshTiles();
            }


        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            y--;  //decreases the y axis by 1 unit per click

            if (y < 0 || y == 0)
            {
                y = 0;
                load = true;
                refreshTiles();
            }

            else if (y < m_tmData.GetNumTilesDown(zoomValue))
            {
                load = true;
                refreshTiles();
            }

            else
            {
                y = m_tmData.GetNumTilesDown(zoomValue) - 1;
                load = true;
                refreshTiles();
            }
        }

        public void refreshTiles()
        {
            try
            {
                //int zoomValue = 0;
                int zoomValNew = zoomValue;
                Debug.WriteLine("{0},{1},{2}", zoomValue, x, y);
                byte[] buff = m_tmData.LoadTile(zoomValNew, x, y);
                MemoryStream m_Stream = new MemoryStream(buff);
                JpegBitmapDecoder jbd = new JpegBitmapDecoder(m_Stream, BitmapCreateOptions.None, BitmapCacheOption.None);
                imgTile.Source = jbd.Frames[0];

                if (buff == null && buff.Length > 0)
                {
                    Console.WriteLine("Empty title\n");
                }
            }

            catch (IOException IOe)
            {
                MessageBox.Show("Failed to load image\n Error : " + IOe);
            }
        }

    }
}
