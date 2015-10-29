using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
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
using System.Windows.Threading;

namespace ReactiveSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var client = new SearchServiceClient();


            Observable.FromEventPattern(SearchBox, "TextChanged")
                .Select(_ => SearchBox.Text)
                .Where(txt => txt.Length > 3)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .DistinctUntilChanged()
                .Select(txt => client.SearchAsync(txt))
                .Switch()
                //.ObserveOn(Dispatcher.CurrentDispatcher)
                .ObserveOnDispatcher()
                .Subscribe(
                    results => SearchResults.ItemsSource = results,
                    err => { Debug.WriteLine(err); });
        }
    }
}
