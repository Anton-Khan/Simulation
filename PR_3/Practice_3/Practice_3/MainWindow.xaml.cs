using System;
using System.Collections.Generic;
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

namespace Practice_3
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

        List<Rectangle> people_at_start = new List<Rectangle>();
        List<Rectangle> people_at_finish = new List<Rectangle>();
        List<Rectangle> people_on_boat = new List<Rectangle>();



        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();

            Task.Run(() =>
            {
                int i = 0;
                int frames = 0;
                bool isRightDir = true;
                bool isStop = true;
                

                while (true)
                {

                    frames++;
                    Thread.Sleep(10);

                   

                    Application.Current.Dispatcher.Invoke(() =>
                    {


                        if (isRightDir==true)
                        {
                            if (people_at_start.Count<4 && frames%100 == 0 && isStop)
                            {                                
                                    AddPeople(!isRightDir, 100-people_at_start.Count*25);
                            }
                            else if(people_on_boat.Count >= 4) // пока людей нет в лодке false
                            {
                                isStop = false;
                                if (i > 599 - 200)
                                    isRightDir = false;
                                i += 2;
                                Canvas.SetLeft(Boat, 150 + i);
                                for (int w = 0; w < people_on_boat.Count; w++)
                                {
                                    Canvas.SetLeft(people_on_boat[w], w*25+140 + i);
                                }
                                

                            }

                           
                            if (frames % 200 == 0 && people_at_start.Count>2)
                            {
                                if ((4 - people_on_boat.Count) > 1) { 

                                    var temp = r.Next(1, (4 - people_on_boat.Count));
                                    Title = temp + "";
                                    for (int q = 0; q <temp; q++)
                                    {
                                        Replace_to_Boat(people_on_boat.Count);
                                    }
                                }
                                else{
                                    Replace_to_Boat(1);
                                }
                            }
                        }
                        else if(isRightDir == false)
                        {
                            if (frames % 100 == 0)
                            {
                                Replace_from_Boat();
                               
                            }
                            if (people_on_boat.Count < 1) //  пока людей нет в лодке false
                            {
                                if (i < 1)
                                {
                                    isRightDir = true;
                                    isStop = true;
                                }
                                i-=2;
                                Canvas.SetLeft(Boat, 150 + i);
                            }
                        }

                        
                        if (frames % 200 == 0 && people_at_finish.Count > 0)
                        {
                            int t = r.Next(0, (people_at_finish.Count - 1));
                            canvas.Children.Remove(people_at_finish[t]);
                            people_at_finish.RemoveAt(t);

                            
                        }

                    });

                    if (frames > 1000)
                        frames = 0;
                }
            });

        }
        

        private void AddPeople(bool isRight, double horizont)
        {
            Random r = new Random();
            BitmapImage theImage;
            if (r.Next(0, 2) > 0)
            {
                theImage = new BitmapImage(new Uri(@"C:\Users\Khan\Desktop\Имитационное моделирование\Practice_1\Simulation\PR_3\Practice_3\Practice_3\Asset\man.png", UriKind.Relative));
            }
            else
            {
                theImage = new BitmapImage(new Uri(@"C:\Users\Khan\Desktop\Имитационное моделирование\Practice_1\Simulation\PR_3\Practice_3\Practice_3\Asset\woman.png", UriKind.Relative));
            }
            ImageBrush myImageBrush = new ImageBrush(theImage);
            Rectangle a = new Rectangle();
            a.Height = 60;
            a.Width = 40;
            a.Fill = myImageBrush;
            Canvas.SetBottom(a, 210);

            Canvas.SetLeft(a, horizont);
            people_at_start.Add(a);

            canvas.Children.Add(a);

        }


        private void Replace_to_Boat(int index)
        {
            
            if (people_on_boat.Count < 4)
            {
                
                Canvas.SetLeft(people_at_start[people_at_start.Count - 1], index*25 + 140);
                Canvas.SetBottom(people_at_start[people_at_start.Count - 1], 180);
                people_on_boat.Add(people_at_start[people_at_start.Count - 1]);
                people_at_start.RemoveAt(people_at_start.Count - 1);

            }
        }

        private void Replace_from_Boat()
        {
            if (people_on_boat.Count != 0)
            {
                
                Canvas.SetLeft(people_on_boat[people_on_boat.Count - 1], (people_on_boat.Count - 1) * 25 + 650 );
                Canvas.SetBottom(people_on_boat[people_on_boat.Count - 1], 210);
                people_at_finish.Add(people_on_boat[people_on_boat.Count - 1]);
                people_on_boat.RemoveAt(people_on_boat.Count - 1);

            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddPeople(true, new Random().Next(0, 100));

        }
    }
}
