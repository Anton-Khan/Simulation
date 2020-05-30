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
using LiveCharts;
using LiveCharts.Wpf;

namespace Practice_1
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
        private Table table;
        private Graphic graphic;

        public ObservableCollection<Data> data = new ObservableCollection<Data>();
        private ChartValues<double> consumers = new ChartValues<double>();
        private ChartValues<double> potential = new ChartValues<double>();
        private int iteration_count = 0;
        private int con = 0;
        private int pot = 0;

        private bool[,] agents;
        private List<SpoilData> agents_has_product = new List<SpoilData>();

        private double draw_scale = 0;
        private double population_count = 10000;
        private double resolution = 200;
        private double population_at_line = 0;

        private int night = 500;

        private double buy_prob = 0.05;
        private double notify_prob = 0.01;

        private byte spoil_time = 5;

        private Random r = new Random();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid.Background = Brushes.Red;
            DataGrid.Height = resolution;
            DataGrid.Width = resolution;

            table = new Table(data);
            graphic = new Graphic(consumers, potential);


            population_at_line = Math.Sqrt(population_count);
            population_at_line = Math.Ceiling(population_at_line);
            draw_scale = resolution / population_at_line;
            agents = new bool[(int)population_at_line, (int)population_at_line];
            pot = (int)population_count;

            ClearAgents();




        }

        private void ClearAgents()
        {
            for (int x = 0; x < population_at_line; x++)
            {
                for (int y = 0; y < population_at_line; y++)
                {
                    agents[x, y] = false;
                }
            }
        }

        private void Recalculation()
        {
            population_at_line = Math.Sqrt(population_count);
            population_at_line = Math.Ceiling(population_at_line);
            draw_scale = resolution / population_at_line;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid.Children.Clear();
                double temp = Int32.Parse(((sender as ComboBox).SelectedItem as TextBlock).Text);
                DataGrid.Height = temp;
                DataGrid.Width = temp;
                resolution = temp;
                draw_scale = resolution / population_at_line;
                Draw();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Table_Button_Click(object sender, RoutedEventArgs e)
        {
            Data_Content.Content = table;
        }

        private void Graphic_Button_Click(object sender, RoutedEventArgs e)
        {
            Data_Content.Content = graphic; 
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            ClearAgents();
            data.Clear();
            consumers.Clear();
            potential.Clear();
            con = 0;
            agents_has_product.Clear();

            

            

            try
            {
                iteration_count = Int32.Parse(Iteration.Text);
                population_count = Int32.Parse(Pop_count.Text);
                night = Int32.Parse(Night.Text);
                buy_prob = Double.Parse(Buy_prob.Text.Replace('.', ','));
                notify_prob = Double.Parse(Notify_prob.Text.Replace('.', ','));
                spoil_time = Byte.Parse(Spoiling.Text);
                Recalculation();
                pot = (int)population_count;
                Draw();
                ScaleChange.IsEnabled = false;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            Task.Run(() =>
            {

                var r = new Random();

                for (int i = 0; i < iteration_count; i++)
                {
                    Thread.Sleep(night);
                    
                    
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        int buy_today = Buying();
                        Notificate(buy_today);
                        int returned = Spoil();
                        pot = pot - buy_today + returned;
                        con = con + buy_today - returned;
                        consumers.Add(con);
                        potential.Add(pot);
                        data.Add(new Data() { Day = i + 1, Consumer = con, Potential = pot });

                    });
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ScaleChange.IsEnabled = true;
                });
                
            });
        }


        private void Draw()
        {
            DataGrid.Children.Clear();
            for (int y = 0; y < population_at_line; y++)
            {
                for (int x = 0; x < population_at_line; x++)
                {
                    if (!agents[x, y])
                    {
                        Rectangle rec = new Rectangle();
                        rec.Stroke = Brushes.Black;
                        rec.Fill = Brushes.Yellow;
                        rec.Height = draw_scale;
                        rec.Width = draw_scale;
                        Canvas.SetLeft(rec, draw_scale * x);
                        Canvas.SetBottom(rec, draw_scale * y);
                        
                        DataGrid.Children.Add(rec);
                    }
                    else
                    {
                        Rectangle rec = new Rectangle();
                        rec.Stroke = Brushes.Black;
                        rec.Fill = Brushes.Red;
                        rec.Height = draw_scale;
                        rec.Width = draw_scale;
                        Canvas.SetLeft(rec, draw_scale * x);
                        Canvas.SetBottom(rec, draw_scale * y);

                        DataGrid.Children.Add(rec);
                    }

                }
            }
            

        }

        

        private int Buying()
        {
            int count = 0;
            for (int x = 0; x < population_at_line; x++)
            {
                for (int y = 0; y < population_at_line; y++)
                {
                    if (!agents[x, y] && r.Next(0, 100) > 100-buy_prob*100)
                    {
                        agents[x, y] = true;
                        int number = x + y * (int)population_at_line;
                        (DataGrid.Children[number] as Rectangle).Fill = Brushes.Red;
                        agents_has_product.Add(new SpoilData() { X=(short)x, Y=(short)y, Number= number, SpoilTime = spoil_time });
                        count++;
                    }
                }
            }
            return count;
        }

        private void Notificate(int consumer_today)
        {
            for (int i = 0; i < consumer_today; i++)
            {
                if (r.Next(0, 100) > 100 - notify_prob * 100)
                {
                    buy_prob += 0.001;
                    Title = buy_prob.ToString();
                }
            }
        }

        private int Spoil()
        {
            int count = 0;

            for (int i = 0; i < agents_has_product.Count; i++)
            {
                if (agents_has_product[i].SpoilTime <= 0) {
                    agents[agents_has_product[i].X, agents_has_product[i].Y] = false;
                    (DataGrid.Children[agents_has_product[i].Number] as Rectangle).Fill = Brushes.LightGreen;
                    agents_has_product.RemoveAt(i);
                    
                    count++;
                }

                agents_has_product[i].SpoilTime--;
            }

            return count;
        }

        


    }
}



/*
 
     
    
     
     
     
     
     
     
     
     
     
     */
