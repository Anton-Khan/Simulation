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
using LiveCharts;
using LiveCharts.Wpf;

namespace Practice_1
{
    /// <summary>
    /// Логика взаимодействия для Graphic.xaml
    /// </summary>
    public partial class Graphic : UserControl
    {
        public List<Data> datas;

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        
        public Graphic()
        {
            InitializeComponent();
        }

        

        public Graphic(ChartValues<double> cons, ChartValues<double> poten)
        {
            InitializeComponent();

            
            

            CreateGraphic();

            SeriesCollection[0].Values = cons;
            SeriesCollection[1].Values = poten;


        }

        private void CreateGraphic()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Consumers",
                    Values = new ChartValues<double>() 
                },
                new LineSeries
                {
                    Title = "Potential",
                    Values = new ChartValues<double>() ,
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 10
                }
            };



            YFormatter = value => value + "";


            

            DataContext = this;
        }

      
    }
}
