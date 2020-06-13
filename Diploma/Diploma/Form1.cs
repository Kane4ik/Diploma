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
        double directionCoordX = 0;
        double directionCoordY = 0;
        bool plotClicked = true;
        double centrMass = 0;
        double accelX = 0; double accelY = 0;
        double[] particlePathX = new double[100];
        double[] particlePathY = new double[100];
        int arrayCounter = 0;
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
            omega = gravConst * (firstMass + secondMass) / Math.Pow(distanceBetweenMasses, 2);
            centrMass = (firstMass + secondMass * distanceBetweenMasses) / (firstMass + secondMass);

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

            initialCoordX = (firstMass + secondMass * distanceBetweenMasses) / (firstMass + secondMass) - 75;
            initialCoordY = -32;

            formsPlot1.plt.PlotScatter(dataXFirstMass, dataYFirstMass, markerSize: 10, markerShape: MarkerShape.filledCircle);
            formsPlot1.plt.PlotScatter(dataXSecondMass, dataYFirstMass, markerSize: 10 * (secondMass / firstMass), markerShape: MarkerShape.filledCircle);
            formsPlot1.plt.PlotScatter(dataX, dataY, lineWidth: 0);
            formsPlot1.Render();

            plotClicked = false;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (arrayCounter < particlePathX.Length)
            {
                particlePathX[arrayCounter] = speed0X;
                particlePathY[arrayCounter] = speed0Y;

                accelX = Math.Round(-gravConst * firstMass * initialCoordX / Math.Pow((Math.Pow(initialCoordX, 2) + Math.Pow((-secondMass / (firstMass + secondMass) * distanceBetweenMasses), 2)), 3.0 / 2.0) - gravConst * secondMass * initialCoordX / Math.Pow((Math.Pow(initialCoordX, 2) + Math.Pow(firstMass / (firstMass + secondMass) * distanceBetweenMasses, 2)), 3.0 / 2.0) + Math.Pow(omega, 2) * initialCoordX + 2 * omega * speed0Y, 15);
                accelY = Math.Round(-gravConst * firstMass * ((-secondMass / (firstMass + secondMass) * distanceBetweenMasses) - initialCoordY) / Math.Pow(Math.Pow(-secondMass / (firstMass + secondMass) * distanceBetweenMasses - initialCoordY, 2), 3.0 / 2.0) + gravConst * secondMass * (firstMass / (firstMass + secondMass) * distanceBetweenMasses - initialCoordY) / Math.Pow(Math.Pow(firstMass / (firstMass + secondMass) * distanceBetweenMasses - initialCoordY, 2), 3.0 / 2.0) + Math.Pow(omega, 2) * initialCoordY + 2 * omega * speed0X, 15);
                speed0X = initialCoordX + accelX;
                speed0Y = initialCoordY + accelY;
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
        }

        /*private void formsPlot1_MouseClicked(object sender, MouseEventArgs e)
        {
            if (plotClicked == false)
            {
                initialCoordX = Cursor.Position.X;
                initialCoordY = Cursor.Position.Y;
                
                double[] dataXClick = new double[] { Cursor.Position.X };
                double[] dataYClick = new double[] { Cursor.Position.Y };
                formsPlot1.plt.PlotScatter(dataXClick, dataYClick, markerSize: 15, markerShape: MarkerShape.filledCircle);
                formsPlot1.Render();
            }
            plotClicked = true;
            
        }
        */
    }
}
