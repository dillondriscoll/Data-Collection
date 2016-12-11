using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollection
{
    public class Device
    {
        private Random rnd = new Random();
        private System.Windows.Threading.DispatcherTimer timer1;
        private double inches;
        public Device()
        {
            this.inches = 0;
            
            timer1 = new System.Windows.Threading.DispatcherTimer();
            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Tick += new EventHandler(timer_Tick);
            timer1.Start();
           
            
        }
    

        private void timer_Tick(object sender, EventArgs e)
        {
            inches = rnd.Next(0, 100) * .10;
            
        }
    
    
        public double GetMeasurement()
        {

            return inches;
            
        }
    }
}
