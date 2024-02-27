using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using lvl4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace lvl4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); 
            update();
            cbSort.Items.Add("По убыванию цены");
            cbSort.Items.Add("По возрастанию цены");
            cbSort.Items.Add("По алфавиту(А-Я)");
            cbSort.Items.Add("По алфавиту(Я-А)");
            cbSort.SelectedIndex = 2;

            List<ProductType> productTypes = EfModel.Init().ProductTypes.ToList();
            productTypes.Insert(0, new ProductType { TypeName = "Все" });
            cbFiltr.ItemsSource = productTypes;
            cbFiltr.SelectedIndex = 0;

        }

        private void update()
        {
            IEnumerable<Product> products = EfModel.Init().Products
                .Include(p => p.TypeProductNavigation)
                .Where(p => p.NameProduct.Contains(SearchTextBox.Text)
                || p.TypeProductNavigation.TypeName.Contains(SearchTextBox.Text) 
                || p.Article.Contains(SearchTextBox.Text));

            switch(cbSort.SelectedIndex)
            {
                case 0:
                    products = products.OrderBy(p => p.Price);
                    break;
                case 1:
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case 2:
                    products = products.OrderBy(p => p.NameProduct);
                    break;
                case 3:
                    products = products.OrderByDescending(p => p.NameProduct);
                    break;

            }

            if(cbFiltr.SelectedIndex > 0) {
                products = products.Where(p => p.TypeProduct == (cbFiltr.SelectedItem as ProductType).IdProductTypes);
            }


            DataGridContext.ItemsSource = products.ToList();

        }


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            update();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            update();
        }

        private void cbSort_FiltrChanged(object sender, SelectionChangedEventArgs e)
        {
            update();
        }
    }
}