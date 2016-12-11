using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollection
{
    interface IMeasuringDevice
    {
                                                   //This method will return a decimal that represents the metric value of the most recent measurement that was captured.
        decimal MetricValue();
                                                 //This method will return a decimal that represents the imperial value of the most recent measurement that was captured.
        decimal ImperialValue();
                                               //This method will start the device running. It will begin collecting measurements and record them.
        void StartCollecting();
                                             // This method will stop the device.It will cease collecting measurements.
        void StopCollecting();
        

                                          //This method will retrieve a copy of all of the recent data that the measuring device has captured. The data will be returned as an array of integer values.
        double[] GetRawData();           //Gets capture history
        
        void setImperialUnits();        //Sets measurement to Imperial
        void setMetricUnits();         //Sets Measurement to Metric
        String[] getTimeStamps();     //Gets timestamp History   
        String[] getUnits();         //returns the units history
        double convertToImperialRecent();
        double convertToMetricRecent();
    }
}
