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
        double speed0X = 0;
        double speed0Y = 0;
        double speedX = 0;
        double speedY = 0;
        public const double gravConst = 6.6720e-08;
        double omega = 0;
        double initialCoordX;
        double initialCoordY;
        double centrMass = 0;
        double accelX = 0; double accelY = 0;
        double[] particlePathX = new double[100];
        double[] particlePathY = new double[100];
        int arrayCounter = 0;
        double firstMassX = 0;
        double secondMassX = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            //double[] dataY = new double[] { 1, 4, 9, 16, 25 };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            formsPlot1.Reset();
            firstMass = Convert.ToDouble(textBox1.Text);
            secondMass = Convert.ToDouble(textBox2.Text);
            speed0X = Convert.ToDouble(textBox4.Text);
            speed0Y = Convert.ToDouble(textBox5.Text);
            distanceBetweenMasses = Convert.ToDouble(textBox3.Text);
            alpha = secondMass / (firstMass + secondMass);
            centrMass = (firstMass + secondMass * distanceBetweenMasses) / (firstMass + secondMass);

            firstMassX = -secondMass / (firstMass + secondMass) * distanceBetweenMasses;
            secondMassX = firstMass / (firstMass + secondMass) * distanceBetweenMasses;
            omega = Math.Sqrt(gravConst * (firstMass / (Math.Pow(firstMassX - centrMass, 2) + Math.Pow(initialCoordX, 2))) + secondMass / (Math.Pow(secondMassX - centrMass - initialCoordX, 2) + Math.Pow(initialCoordY, 2)));


            double L1 = distanceBetweenMasses * (1 - Math.Pow(alpha / 3.0, 1.0 / 3)); //сделать отдельной функцией, вынести в другой класс
            double L2 = distanceBetweenMasses * (1 + Math.Pow(alpha / 3.0, 1.0 / 3)); //сделать отдельной функцией, вынести в другой класс
            double L3 = -distanceBetweenMasses * (1 + 5.0 / 12 * alpha); //сделать отдельной функцией, вынести в другой класс
            double L4x = distanceBetweenMasses / 2 * (firstMass - secondMass) / (firstMass + secondMass); //сделать отдельной функцией, вынести в другой класс
            double L4y = Math.Sqrt(3) / 2 * distanceBetweenMasses; //сделать отдельной функцией, вынести в другой класс
            double L5x = distanceBetweenMasses / 2 * (firstMass - secondMass) / (firstMass + secondMass); //сделать отдельной функцией, вынести в другой класс
            double L5y = -Math.Sqrt(3) / 2 * distanceBetweenMasses; //сделать отдельной функцией, вынести в другой класс



            double[] dataX = new double[] { L1, L2, L3, L4x, L5x };
            double[] dataY = new double[] { 0, 0, 0, L4y, L5y };
            double[] dataXFirstMass = new double[] { -secondMass / (firstMass + secondMass) * distanceBetweenMasses };
            double[] dataYFirstMass = new double[] { 0 };
            double[] dataXSecondMass = new double[] { firstMass / (firstMass + secondMass) * distanceBetweenMasses };

            formsPlot1.plt.PlotScatter(dataXFirstMass, dataYFirstMass, markerSize: 20, markerShape: MarkerShape.filledCircle);
            formsPlot1.plt.PlotScatter(dataXSecondMass, dataYFirstMass, markerSize: 20 * (secondMass / firstMass), markerShape: MarkerShape.filledCircle);
            formsPlot1.plt.PlotScatter(dataX, dataY, lineWidth: 0);
            formsPlot1.Render();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (arrayCounter < particlePathX.Length)
            {
                particlePathX[arrayCounter] = initialCoordX + speed0X;
                particlePathY[arrayCounter] = initialCoordY + speed0Y;

                double omega2 = Math.Sqrt(gravConst * (firstMass / (Math.Pow(firstMassX - centrMass, 2) + Math.Pow(initialCoordX, 2))) + secondMass / (Math.Pow(secondMassX - centrMass - initialCoordX, 2) + Math.Pow(initialCoordY, 2)));


                double F1x = -gravConst * firstMass * initialCoordX / Math.Pow(firstMassX + (initialCoordX - centrMass), 3);
                double F2x = -gravConst * secondMass * initialCoordX / Math.Pow((firstMassX - centrMass) - initialCoordX, 3);
                double Fyx = Math.Pow(omega2, 2) * initialCoordX;
                double Fkorx = -2 * omega2 * speed0Y;

                double F1y = -gravConst * firstMass * initialCoordX / Math.Pow(initialCoordY, 3.0);
                double F2y = -gravConst * secondMass * initialCoordX / Math.Pow(initialCoordY, 3.0);
                double Fyy = Math.Pow(omega2, 2) * initialCoordY;
                double Fkory = 2 * omega2 * speed0X;

                accelX = F1x + F2x + Fyx + Fkorx;
                accelY = F1y + F2y + Fyy + Fkory;
                speed0X = speed0X + accelX;
                speed0Y = speed0Y + accelY;
                listBox1.Items.Add("Координата X" + arrayCounter + ": = " + initialCoordX + ". Координата Y" + arrayCounter + ": = " + initialCoordY);
                initialCoordX = particlePathX[arrayCounter];
                initialCoordY = particlePathY[arrayCounter];
            }
            else timer1.Stop();
            arrayCounter++;
            formsPlot1.plt.PlotScatter(particlePathX, particlePathY);
            formsPlot1.Render();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start();
            Array.Clear(particlePathX, 0, 100);
            Array.Clear(particlePathY, 0, 100);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (trackBar1.Value > 500) initialCoordX = trackBar1.Value - 500;
            else if (trackBar1.Value < 500) initialCoordX = trackBar1.Value - 500;
            double[] dataX1 = new double[] { initialCoordX };
            double[] dataY1 = new double[] { initialCoordY };

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (trackBar2.Value > 500) initialCoordY = trackBar2.Value - 500;
            else if (trackBar2.Value < 500) initialCoordY = trackBar2.Value - 500;
            double[] dataX2 = new double[] { initialCoordX };
            double[] dataY2 = new double[] { initialCoordY };

        }
    }
}
