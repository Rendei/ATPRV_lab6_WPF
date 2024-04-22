using ATPRV_Lab6;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ATPRV_lab6_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> _filterWords = new List<string>() { "любовь", "любить", "бы", "цветок", "я", "ты", "вы", 
            "мы", "то", "наполеон", "война", "мир", "божий"};

        private Crawler _crawler;
        private TextFileReader _reader;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async void StartCrawlButton_Click(object sender, RoutedEventArgs e)
        {
            string rootUrl = URLTextBox.Text;
            int maxDepth = int.Parse(DepthTextBox.Text);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            _crawler = new Crawler(maxDepth);
            await _crawler.CrawlAsync(rootUrl);

            stopwatch.Stop();
            TimeLabel.Content = $"Время работы: {stopwatch.ElapsedMilliseconds} мс";
            TreeInfoLabel.Content = _crawler.DisplayTreeInfo();
            //crawler.DisplayResults();
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        { 
            _reader = new TextFileReader();
            Parallel.ForEach(_reader.Texts, text =>
            {
                _reader.FindWordsCount(text, _filterWords);
            });
            
            string result = string.Empty;
            
            foreach (var text in _reader.TextWords)
            {
                result += text.ToString();
            }
            WordResultButton.Content = result;
        }
    }
}