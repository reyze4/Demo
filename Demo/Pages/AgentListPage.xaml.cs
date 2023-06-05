using Demo.Model;
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

namespace Demo.Pages
{
    /// <summary>
    /// Логика взаимодействия для AgentListPage.xaml
    /// </summary>
    public partial class AgentListPage : Page
    {
        int maxPage = 0;
        public AgentListPage()
        {
            InitializeComponent();
            var agentType = App.DB.AgentType.ToList();
            agentType.Insert(0, new Model.AgentType{ Title = "Все типы" });
            Filtr.ItemsSource = agentType;
            Sortir.SelectedIndex = 0;
        }

        int numberPage = 0;
        int count = 10;

        int fakePage = 1;
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }
        private void LeftBtn_Click(object sender, RoutedEventArgs e)
        {
            numberPage--;
            fakePage--;
            if (numberPage < 0)
                numberPage = 0;
            if (fakePage < 1)
                fakePage = 1;
            Refresh();
            GeneratePageNumbers();
        }

        private void RightBtn_Click(object sender, RoutedEventArgs e)
        {
            numberPage++;
            fakePage++;
            if (numberPage == maxPage)
            {
                numberPage = maxPage - 1;
                fakePage--;
            }

            if (fakePage < maxPage + 1)
            {
                Refresh();
            }
            GeneratePageNumbers();
        }
        private void Refresh()
        {
            IEnumerable<Agent> services = App.DB.Agent;
            if (Sortir.SelectedIndex == 1)
                services = services.OrderBy(x => x.Priority);
            else if (Sortir.SelectedIndex == 2)
                services = services.OrderByDescending(x => x.Priority);

            if (Filtr.SelectedIndex != 0)
                services = services.Where(x => x.AgentType == Filtr.SelectedItem);

            if (TbSearch.Text.Length > 0)
            {
                services = services.Where(x => x.Title.ToLower().Contains(TbSearch.Text.ToLower()));
            }

            if (services.Count() > count)
            {
                if (services.Count() % count > 0)
                {
                    maxPage = (services.Count() / count) + 1;
                }
                else
                {
                    maxPage = services.Count() / count;
                }
            }
            else
            {
                maxPage = 1;
            }
            if (fakePage > maxPage)
            {
                fakePage = maxPage;
            }
            services = services.Skip(count * numberPage).Take(count).ToList();

            LVProduct.ItemsSource = services.ToList();
            GeneratePageNumbers();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
                maxPage = App.DB.Agent.Count();
                Refresh();
            
        }
        private void GeneratePageNumbers()
        {
            SPanelPages.Children.Clear();
            for (int i = 1; i <= maxPage; i++)
            {
                RadioButton btn = new RadioButton();
                btn.Content = i;
                btn.Margin = new Thickness(0, 0, 0, 0);
                btn.Click += PageButton_Click;
                Style style = this.FindResource("RadioPage") as Style;
                btn.Style = style;
                SPanelPages.Children.Add(btn);

                if (int.Parse(btn.Content.ToString()) == fakePage)
                {
                    btn.IsChecked = true;
                }
            }
        }
        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as RadioButton;
            string c = b.Content.ToString();
            int a = int.Parse(c) - 1;
            numberPage = a;
            fakePage = int.Parse(c);
            Refresh();



        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selService = (sender as Button).DataContext as Agent;
            NavigationService.Navigate(new EdutAddPage(selService));
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EdutAddPage(new Agent()));
            Refresh();
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            var selService = (sender as Button).DataContext as Agent;
            if (MessageBox.Show("Вы действительно хотите удалить эту запись?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                App.DB.Agent.Remove(selService);
                App.DB.SaveChanges();
                Refresh();
            }
        }
    }
}
