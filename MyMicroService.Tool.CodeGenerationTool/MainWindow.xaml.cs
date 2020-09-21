using MyMicroService.Tool.CodeGenerationTool.Enums;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace MyMicroService.Tool.CodeGenerationTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.ComboBox_DbType.ItemsSource = new List<dynamic>()
            {
                new {ID=(int)EnumDbType.MySql,Name=EnumDbType.MySql.ToString() }
            };
        }

        private void ComboBox_DbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Convert.ToInt32(this.ComboBox_DbType.SelectedValue) == (int)EnumDbType.MySql)
            {
                this.TextBox_Port.Text = "3306";
            }
        }
    }
}
