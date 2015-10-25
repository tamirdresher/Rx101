using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
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

            var baseAddress = "http://localhost.fiddler:2458/api/Search?searchTerm=";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            Observable.FromEventPattern(SearchBox, "TextChanged")
                .Select(_ => SearchBox.Text)
                .Where(txt => txt.Length > 3)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .DistinctUntilChanged()
                .Select(txt => client.GetAsync(baseAddress + txt))
                .Switch()
                .SelectMany(response => response.Content.ReadAsAsync<IEnumerable<string>>())
                .ObserveOn(Dispatcher.CurrentDispatcher)
                .Subscribe(
                    results => SearchResults.ItemsSource = results,
                    err => { Debug.WriteLine(err); });
        }
    }
}
