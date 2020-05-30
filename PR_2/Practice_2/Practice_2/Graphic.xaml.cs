using LiveCharts;
using LiveCharts.Wpf;
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

namespace Practice_2
{
    /// <summary>
    /// Логика взаимодействия для Graphic.xaml
    /// </summary>
    public partial class Graphic : UserControl
    {
        

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public Graphic()
        {
            InitializeComponent();
        }
        
        public Graphic(ChartValues<double> h, ChartValues<double> l, ChartValues<double> i, ChartValues<double> r)
        {
            InitializeComponent();
            

            CreateGraphic();

            SeriesCollection[0].Values = h;
            SeriesCollection[1].Values = l;
            SeriesCollection[2].Values = i;
            SeriesCollection[3].Values = r;


        }

        private void CreateGraphic()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Не болющих",
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Латентно зараженных",
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Заболевших",
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Выздоровевшие",
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                }
            };



            YFormatter = value => value + "";
            
            DataContext = this;
        }


    }
}
