using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diploma
{
    public partial class Form1 : Form
    {
        double distanceBetweenMasses = 0;
        double firstMass = 0;
        double secondMass = 0;
        double alpha = 0;
        double L1 = 0, L2 = 0, L3 = 0, L4x = 0, L4y = 0, L5x = 0, L5y = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            //double[] dataY = new double[] { 1, 4, 9, 16, 25 };
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            formsPlot1.Reset();
            firstMass = Convert.ToDouble(textBox1.Text);
            secondMass = Convert.ToDouble(textBox2.Text);
            distanceBetweenMasses = Convert.ToDouble(textBox3.Text);
            alpha = secondMass / (firstMass + secondMass);
            double L1 = distanceBetweenMasses * (1 - Math.Pow(alpha / 3, 1.0 / 3.0)); //сделать отдельной функцией, вынести в другой класс
            double L2 = distanceBetweenMasses * (1 + Math.Pow(alpha / 3, 1.0 / 3.0)); //сделать отдельной функцией, вынести в другой класс
            double L3 = -distanceBetweenMasses * (1 + 5 / 12 * alpha); //сделать отдельной функцией, вынести в другой класс
            double L4x = distanceBetweenMasses / 2 * (firstMass - secondMass) / (firstMass + secondMass); //сделать отдельной функцией, вынести в другой класс
            double L4y = Math.Sqrt(3) / 2 * distanceBetweenMasses; //сделать отдельной функцией, вынести в другой класс
            double L5x = distanceBetweenMasses / 2 * (firstMass - secondMass) / (firstMass + secondMass); //сделать отдельной функцией, вынести в другой класс
            double L5y = -Math.Sqrt(3) / 2 * distanceBetweenMasses; //сделать отдельной функцией, вынести в другой класс
            double[] dataX = new double[] { L1, L2, L3, L4x, L5x };
            double[] dataY = new double[] { 0, 0, 0, L4y, L5y };
            double[] dataXFirstMass = new double[] { 0 };
            double[] dataYFirstMass = new double[] { 0 };
            double[] dataXSecondMass = new double[] { distanceBetweenMasses };
            formsPlot1.plt.PlotScatter(dataXFirstMass, dataYFirstMass, markerSize: 15, markerShape: MarkerShape.filledCircle);
            formsPlot1.plt.PlotScatter(dataXSecondMass, dataYFirstMass, markerSize: 15 * firstMass / secondMass, markerShape: MarkerShape.filledCircle);
            formsPlot1.plt.PlotScatter(dataX, dataY, lineWidth: 0);
            formsPlot1.Render();
        }
    }
}
