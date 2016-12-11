using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataCollection
{
    class MeasureLengthDevice : IMeasuringDevice
    {
        private Units unitsToUse;                                            //Declares all important variables
        private double[] dataCaptured;
        private double mostRecentMeasure;
        private Device newDevice;
        private System.Windows.Threading.DispatcherTimer timer2;
        private String[] timeStamps;
        private String[] unitsHistory;
        

        public MeasureLengthDevice()
        {
                                                                              //Constructor for MeasureLengthDevice, starts virtual device timer , initializes history arrays, and most recent measure variable
            this.newDevice = new Device();
            this.timeStamps = new string[5];
            this.unitsHistory = new string[5];
            this.unitsToUse = Units.Metric;
            this.dataCaptured = new double[5];
            this.mostRecentMeasure = newDevice.GetMeasurement();

        }
        
       

        double[] IMeasuringDevice.GetRawData()
        {
                                                                             //getter for capture history
            return dataCaptured;
            
        }

        decimal IMeasuringDevice.ImperialValue()
        {
                                                                            //Conversion logic
            if(unitsToUse == Units.Metric)
            {
                decimal temp = (decimal)mostRecentMeasure * (decimal)2.54;
                return temp;
            }
            else
            {
                return (decimal)mostRecentMeasure;
            }
        }

        decimal IMeasuringDevice.MetricValue()
        {
                                                                            //Conversion logic
            if(unitsToUse == Units.Metric)
            {
                return (decimal)mostRecentMeasure;
            }
            else
            {
                decimal temp = (decimal)mostRecentMeasure / (decimal)2.54;
                return temp;
                
            }
        }
        
        void IMeasuringDevice.StartCollecting()
        {
            {
                                                                         //Timer declaration for collecting device data
                timer2 = new System.Windows.Threading.DispatcherTimer();
                timer2.Interval = TimeSpan.FromSeconds(15);
                timer2.Tick += new EventHandler(timer_Tick);
                timer2.Start();
            }

         
        }
        public String GetTimestamp(DateTime value)                     //Timestamp logic
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }


        private void timer_Tick(object sender, EventArgs e)
        {
                                                                      // code here  updates your mostRecentMeasure from your virtual device’s GetMeasurement method. Makes sure the measurement is in the correct type Imperial vs Metric
            bool emptyElement = false;
            String timeStamp = GetTimestamp(DateTime.Now);
            if (unitsToUse == Units.Metric)
            {
                                                                      //Get most recent measure and convert to Metric Value
                mostRecentMeasure = newDevice.GetMeasurement();
                mostRecentMeasure = (double)((IMeasuringDevice)this).MetricValue();

                for (int i = 0; i < dataCaptured.Length; i++)
                {
                    if (dataCaptured[i] == 0)
                    {
                        emptyElement = true;
                        dataCaptured[i] = mostRecentMeasure;
                        unitsHistory[i] = unitsToUse.ToString();
                        timeStamps[i] = timeStamp;
                        break;
                    }
                    else
                    {
                        emptyElement = false;
                    }
                }

                    if (emptyElement == false)                      //If array is full push all elements down and add new measurement/timestamp/units to the end of their respective arrays
                    {

                        dataCaptured = queuer(dataCaptured);
                        timeStamps = stampQueuer(timeStamps);
                        unitsHistory = unitsQueuer(unitsHistory);
                    }
                }
            

            else
            {
                                                                  //Get most recent measure and convert to Imperial
                mostRecentMeasure = newDevice.GetMeasurement();
                mostRecentMeasure = (double)((IMeasuringDevice)this).ImperialValue();
                for (int i = 0; i < dataCaptured.Length; i++)
                {
                    if (dataCaptured[i] == 0)
                    {
                        emptyElement = true;
                        timeStamps[i] = timeStamp;
                        unitsHistory[i] = unitsToUse.ToString();
                        dataCaptured[i] = mostRecentMeasure;
                        
                        break;
                    }
                    else
                    {
                        emptyElement = false;
                    }
                }


                if (emptyElement == false)
                {

                    dataCaptured = queuer(dataCaptured);
                    timeStamps = stampQueuer(timeStamps);
                    unitsHistory = unitsQueuer(unitsHistory);
                }


            }

            foreach (System.Windows.Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                                                               //update mostRecentMeasurementTextbox with the name of your textbox.
                    (window as MainWindow).mostRecentMeasurementTextBox.Text = mostRecentMeasure.ToString();
                }
            }
        }
        
        public double[] queuer(double[] a)                   //Logic for pushing down elements and adding element to end of measurement array
        {
            Queue<double> holder = new Queue<double>(a);
            holder.Dequeue();
            holder.Enqueue(mostRecentMeasure);
            double[] temp = holder.ToArray();
            return temp;
            
        }
        public string[] stampQueuer(String[] a)              //Logic for pushing down elements and adding element to end of timestamp array
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            Queue<String> holder = new Queue<String>(a);
            holder.Dequeue();
            holder.Enqueue(timeStamp);
            String[] temp = holder.ToArray();
            return temp;
        }
        public String[] unitsQueuer(String[] a)             //Logic for pushing down elements and adding element to end of unit of measurement array
        {

            String thisUnit = unitsToUse.ToString();
            
            Queue<String> holder = new Queue<String>(a);
            holder.Dequeue();
            holder.Enqueue(thisUnit);
            String[] temp = holder.ToArray();
            return temp;
        }

        void IMeasuringDevice.StopCollecting()
        {
                                                          //Stop the collection of data by stopping the timer2 object
            timer2.Stop();
        }
        void IMeasuringDevice.setImperialUnits()
        {
                                                        //Set unitsToUse to Imperial units
            unitsToUse = Units.Imperial;
        }
        void IMeasuringDevice.setMetricUnits()  
        {
                                                        //Set units to Metric
            unitsToUse = Units.Metric;
        }
        String[] IMeasuringDevice.getTimeStamps()      //getter for timestamp history
        {
            return timeStamps;
        }
        String[] IMeasuringDevice.getUnits()          //getter for unit of measurement history
        {
            return unitsHistory;
        }
        double[] ConvertToMetric(double[] a)         //convert history to other measurement unit
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = a[i] / 2.54;
                unitsHistory[i] = Units.Metric.ToString();
            }

            return a;
        }
        double[] ConvertToImperial(double[] a)         //convert history to other measurement unit
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = a[i] * 2.54;
                unitsHistory[i] = Units.Imperial.ToString();
            }

            return a;
        }
        double IMeasuringDevice.convertToImperialRecent() 
        {
            if(unitsToUse == Units.Imperial)
            {
                return mostRecentMeasure;
            }
            else
            {
                return mostRecentMeasure * 2.54;
            }
        }
        double IMeasuringDevice.convertToMetricRecent()
        {
            if (unitsToUse == Units.Metric)
            {
                return mostRecentMeasure;
            }
            else
            {
                return mostRecentMeasure / 2.54;
            }
        }
    }

}