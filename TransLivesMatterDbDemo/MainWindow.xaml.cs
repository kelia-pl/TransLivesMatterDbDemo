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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TransLivesMatterDbDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        TransLivesMatterDbDemo.Contoller.DbTool DbTool;

        private void BtnTestConn_Click(object sender, RoutedEventArgs e)
        {
            DbTool = new(TxtDbName.Text, TxtDbLogin.Text, TxtDbPass.Password);

            bool hasError;
            string log = DbTool.TestConnection(out hasError);
            TxtStatusLog.Text = log;
            if (!hasError)
            {
                BtnLoadData.IsEnabled = true;
            }
        }

        private void BtnLoadData_Click(object sender, RoutedEventArgs e)
        {
            bool hasError;
            string log = TxtStatusLog.Text;
            DataTable dataTable = DbTool.GetTableColumns(out hasError, ref log);
            if (!hasError)
            {
                DtGrDataFromDb.AutoGenerateColumns = true;
                DtGrDataFromDb.DataContext= dataTable.DefaultView;
                TxtStatusLog.Text = log;
                DtGrDataFromDb.CanUserAddRows = false;
            }
            else
            {
                TxtStatusLog.Text = log;
            }
        }
    }
}
