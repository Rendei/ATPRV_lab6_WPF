using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ATPRV_Lab6
{
    class Crawler
    {
        private readonly int _maxDepth;
        private readonly HttpClient _httpClient;
        private HashSet<string> _visitedUrls;
        private List<(string, string)> _images;
        private int _nodeCount;
        private int _leafCount;
        private int _maxHeight;
        private int _currentDepth;

        public Crawler(int maxDepth)
        {
            _maxDepth = maxDepth;
            _httpClient = new HttpClient();
            _visitedUrls = new HashSet<string>();
            _images = new List<(string, string)>();
            _nodeCount = 0;
            _leafCount = 0;
            _maxHeight = 0;
            _currentDepth = 0;
        }

        public async Task CrawlAsync(string url, int depth = 0)
        {
            if (depth > _maxDepth) return;

            if (_visitedUrls.Contains(url)) return;

            _visitedUrls.Add(url);

            _nodeCount++;
            _currentDepth = Math.Max(_currentDepth, depth);
            _maxHeight = Math.Max(_maxHeight, _currentDepth);
            try
            {
                var response = await _httpClient.GetAsync(url);

                // Проверяем, было ли перенаправление
                if (response.StatusCode == System.Net.HttpStatusCode.MovedPermanently)
                {
                    var newUrl = response.Headers.Location.AbsoluteUri;
                    await CrawlAsync(newUrl, depth);
                    return;
                }

                var html = await response.Content.ReadAsStringAsync();
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                // Получаем все изображения на странице
                var imageNodes = htmlDocument.DocumentNode.SelectNodes("//img[@src]");
                if (imageNodes != null)
                {
                    foreach (var imageNode in imageNodes)
                    {
                        string imageUrl = imageNode.Attributes["src"].Value;
                        _images.Add((url, imageUrl));
                    }
                }

                // Получаем ссылки на другие страницы и рекурсивно обходим их
                var linkNodes = htmlDocument.DocumentNode.SelectNodes("//a[@href]");
                if (linkNodes != null)
                {
                    var tasks = new List<Task>();
                    foreach (var linkNode in linkNodes)
                    {
                        string nextUrl = linkNode.Attributes["href"].Value;
                        if (Uri.TryCreate(new Uri(url), nextUrl, out Uri resultUri) &&
                            (resultUri.Scheme == Uri.UriSchemeHttp || resultUri.Scheme == Uri.UriSchemeHttps))
                        {
                            tasks.Add(CrawlAsync(resultUri.ToString(), depth + 1));
                        }
                    }
                    await Task.WhenAll(tasks);
                }

                if (depth == _maxDepth)
                {
                    _leafCount++;
                    DisplayTreeInfo();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public string DisplayTreeInfo()
        {
            return $"Высота дерева: {_maxHeight}\n" +
                $"Количество узлов: {_nodeCount}\n" +
                $"Количество листьев: {_leafCount}";
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
        }

        public void DisplayResults()
        {
            Console.WriteLine("Собранные изображения:");
            foreach (var image in _images)
            {
                Console.WriteLine($"Страница: {image.Item1}, Изображение: {image.Item2}");
            }
        }

    }
}
