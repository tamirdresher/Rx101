using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
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
        SearchServiceClient _client = new SearchServiceClient();

        public MainWindow()
        {
            InitializeComponent();

            //Install-Package Rx-Main
            //Install-Package Rx-Xaml

            Observable.FromEventPattern(SearchBox, "TextChanged")
                .Select(_ => SearchBox.Text)
                .Where(txt => txt.Length >= 3)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .DistinctUntilChanged()
                .Select(txt => _client.SearchAsync(txt))
                .Switch()
                .ObserveOnDispatcher()
                .Subscribe(
                    results => SearchResults.ItemsSource = results,
                    err => { Debug.WriteLine(err); });

        }

        #region Backup

        void ReactiveSearch()
        {

            //1. Create Observable from TextChanged event of the SearchBox
            //2. Select the text that was entered
            //3. Keep only strings with >3 characters
            //4. Take only if the time passed > 0.5 sec
            //5. Take only if it's not the same string again
            //6. Asynchronically call the Search service
            //7. Discard results if another search was requested
            //8. Show the results inside the SearchResult listbox  

            var client = new SearchServiceClient();

            Observable.FromEventPattern(SearchBox, "TextChanged")
                .Select(_ => SearchBox.Text)
                .Where(txt => txt.Length > 3)
                .Throttle(TimeSpan.FromSeconds(0.5), DefaultScheduler.Instance)
                .DistinctUntilChanged()
                .Select(txt => client.SearchAsync(txt))
                .Switch()
                //.ObserveOn(Dispatcher.CurrentDispatcher)
                .ObserveOnDispatcher()
                .Subscribe(
                    results => SearchResults.ItemsSource = results,
                    err => { Debug.WriteLine(err); });


            //   .Subscribe(results => results.ForEach(res => Debug.WriteLine(res)));


        }
        #endregion
    }
}
