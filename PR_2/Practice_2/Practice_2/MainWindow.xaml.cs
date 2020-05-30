using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Practice_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Graphic graphic;
        public ObservableCollection<Data> data = new ObservableCollection<Data>();
        private ChartValues<double> healthy = new ChartValues<double>();
        private ChartValues<double> latent = new ChartValues<double>();
        private ChartValues<double> ill = new ChartValues<double>();
        private ChartValues<double> recovered = new ChartValues<double>();

        private Data actual_data = new Data();
        private int population = 0;
        private double infectivity = 0;
        private double contact = 0;
        private double period = 0;
        private double duration = 0;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            graphic =  new Graphic(healthy, latent, ill, recovered);
            Content_control.Content = graphic;


            DataGrid.ItemsSource = data;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {



            try
            {
                population = Int32.Parse(Population_tb.Text);
                infectivity = Double.Parse(DataFlow_tb.Text.Replace('.',','));
                contact = Double.Parse(Contact_tb.Text.Replace('.', ','));
                period = Int32.Parse(Period_tb.Text);
                duration = Int32.Parse(Duration_tb.Text);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


            actual_data.Healthy = population - 1;
            actual_data.Latent = 0;
            actual_data.Ill = 1;
            actual_data.Recovered = 0;

            Task.Run(() =>
            {
                
                while(Math.Ceiling(actual_data.Recovered) != population)
                {
                    Thread.Sleep(200);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        double exp = (ExposedRate());
                        double i = (InfectionRate());
                        double r = (RecoveredRate());

                        Calculate(exp, i, r);

                        Translate(exp, i, r);

                        TableView();

                        GraphicView();

                        


                    });
                }
            });

        }

        private double ExposedRate()
        {
            return actual_data.Ill * contact *infectivity * actual_data.Healthy / population;
        }

        private double InfectionRate()
        {
            return actual_data.Latent / period;
        }

        private double RecoveredRate()
        {
            return actual_data.Ill / duration;
        }

        private void Calculate(double e, double i, double r)
        {
            actual_data.Healthy -= e;
            actual_data.Latent += e;
            actual_data.Latent -= i;
            actual_data.Ill += i;
            actual_data.Ill -= r;
            actual_data.Recovered += r;
        }
        

        private void Translate(double e, double i, double r)
        {
            Healthy_lb.Text = ((int)actual_data.Healthy).ToString();
            Latent_lb.Text = string.Format("{0:0.0000}", actual_data.Latent);
            Ill_lb.Text = string.Format("{0:0.0000}", actual_data.Ill);
            Recovery_lb.Text = string.Format("{0:0.0000}", actual_data.Recovered);
            ExposedRate_lb.Text = string.Format("{0:0.0000}", e);
            InfectionRate_lb.Text = string.Format("{0:0.0000}", i);
            RecoveredRate_lb.Text = string.Format("{0:0.0000}", r);

        }

        private void TableView()
        {
            data.Add(actual_data.Copy());
            DataGrid.ScrollIntoView(data[data.Count - 1]);
        }

        private void GraphicView()
        {
            Data step_data = actual_data.Copy();
            healthy.Add(step_data.Healthy);
            latent.Add(step_data.Latent);
            ill.Add(step_data.Ill);
            recovered.Add(step_data.Recovered);
        }

    }
}
