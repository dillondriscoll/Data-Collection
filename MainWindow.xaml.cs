using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DataCollection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        IMeasuringDevice newMeasureDevice = new MeasureLengthDevice();                               //Initialize the measuring device with a high scope so other functions can access it

        public MainWindow()
        {
            
            InitializeComponent();
            


        }

        private void metricButton_Click(object sender, RoutedEventArgs e)                             //Change measurement to Metric units
        {
            newMeasureDevice.setMetricUnits();
            
        }

        private void imperialButton_Click(object sender, RoutedEventArgs e)                            //Change measurement to Imperial Units
        {
            newMeasureDevice.setImperialUnits();
        }

        private void startCollectingButton_Click(object sender, RoutedEventArgs e)                     //Start the collection timer
        {
            newMeasureDevice.StartCollecting();
        }

        private void stopCollectingButton_Click(object sender, RoutedEventArgs e)                      //Stop the collection timer
        {
            newMeasureDevice.StopCollecting();
        }

        private void mostRecentMeasurementTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void displayCapturesButton_Click(object sender, RoutedEventArgs e)                   //Upon clicking this collects all the history arrays and displays them in the list box formatted with | seperators
        {
            capturedDataListBox.Items.Clear();                                                       //This line may be redundant                
            double[] temp = newMeasureDevice.GetRawData();
            String[] timeTemp = newMeasureDevice.getTimeStamps();
            String[] unitTemp = newMeasureDevice.getUnits();

            for (int i = 0; i < temp.Length; i++)
            {
                if(temp[i] != 0)
                    capturedDataListBox.Items.Add(String.Format("{0} | {1} | {2}", timeTemp[i], unitTemp[i], temp[i].ToString()));
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            mostRecentMeasurementTextBox.Text = newMeasureDevice.convertToMetricRecent().ToString() ;
            unitLabel.Content = "cm";

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            mostRecentMeasurementTextBox.Text = newMeasureDevice.convertToImperialRecent().ToString();
            unitLabel.Content = "in";
        }
    }
}
