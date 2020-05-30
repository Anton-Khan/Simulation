using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Practice_1
{
    /// <summary>
    /// Логика взаимодействия для Table.xaml
    /// </summary>
    public partial class Table : UserControl
    {
        private ObservableCollection<Data> datas;

        public Table()
        {
            InitializeComponent();
        }


        public Table(ObservableCollection<Data> data)
        {
            InitializeComponent();
            datas = data;
            TableGrid.ItemsSource = datas;
        }



    }
}
