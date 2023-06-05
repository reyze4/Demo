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
using Demo.Model;


namespace Demo.Pages
{
    /// <summary>
    /// Логика взаимодействия для EdutAddPage.xaml
    /// </summary>
    public partial class EdutAddPage : Page
    {
        Agent contextagent;
        public EdutAddPage(Agent agent)
        {
            InitializeComponent();
            contextagent = agent;  
            DataContext = contextagent;
        }

        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BSave_Click(object sender, RoutedEventArgs e)
        {
            App.DB.SaveChanges();
            NavigationService.GoBack();
        }
    }
}
