using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using HtmlAgilityPack;

namespace Main;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        ResultsList.Items.Clear();

        string query = SearchBox.Text;

        if (string.IsNullOrWhiteSpace(query))
            return;

        if (DuckCheck.IsChecked == true)
        {
            if (ImageCheck.IsChecked == true)
                await SearchDuckImages(query);
            else
                await SearchDuckText(query);
        }

        if (BingCheck.IsChecked == true)
        {
            if (ImageCheck.IsChecked == true)
                await SearchBingImages(query);
            else
                await SearchBingText(query);
        }
    }

    private async Task<string> GetHtml(string url)
    {
        HttpClient client = new();

        client.DefaultRequestHeaders.Add(
            "User-Agent",
            "Mozilla/5.0"
        );

        return await client.GetStringAsync(url);
    }

    private async Task SearchDuckText(string query)
    {
        ResultsList.Items.Add("===== DuckDuckGo =====");

        string url =
            $"https://html.duckduckgo.com/html/?q={WebUtility.UrlEncode(query)}";

        string html = await GetHtml(url);

        HtmlDocument doc = new();
        doc.LoadHtml(html);

        var links =
            doc.DocumentNode.SelectNodes("//a[contains(@class,'result__a')]");

        if (links == null)
            return;

        foreach (var item in links.Take(10))
        {
            ResultsList.Items.Add(item.InnerText.Trim());
            ResultsList.Items.Add(item.GetAttributeValue("href", ""));
            ResultsList.Items.Add("");
        }
    }

    private async Task SearchBingText(string query)
    {
        ResultsList.Items.Add("===== Bing =====");

        string url =
            $"https://www.bing.com/search?q={WebUtility.UrlEncode(query)}";

        string html = await GetHtml(url);

        HtmlDocument doc = new();
        doc.LoadHtml(html);

        var results =
            doc.DocumentNode.SelectNodes("//li[contains(@class,'b_algo')]");

        if (results == null)
            return;

        foreach (var item in results.Take(10))
        {
            var title = item.SelectSingleNode(".//h2/a");

            if (title != null)
            {
                ResultsList.Items.Add(title.InnerText.Trim());
                ResultsList.Items.Add(
                    title.GetAttributeValue("href", "")
                );
                ResultsList.Items.Add("");
            }
        }
    }

    private async Task SearchDuckImages(string query)
    {
        ResultsList.Items.Add("===== DuckDuckGo Images =====");

        string url =
            $"https://duckduckgo.com/?q={WebUtility.UrlEncode(query)}&iax=images&ia=images";

        ResultsList.Items.Add(url);
    }

    private async Task SearchBingImages(string query)
    {
        ResultsList.Items.Add("===== Bing Images =====");

        string url =
            $"https://www.bing.com/images/search?q={WebUtility.UrlEncode(query)}";

        string html = await GetHtml(url);

        HtmlDocument doc = new();
        doc.LoadHtml(html);

        var images = doc.DocumentNode.SelectNodes("//img");

        if (images == null)
            return;

        int count = 0;

        foreach (var image in images)
        {
            string src = image.GetAttributeValue("src", "");

            if (!string.IsNullOrEmpty(src))
            {
                ResultsList.Items.Add(src);

                count++;

                if (count >= 10)
                    break;
            }
        }
    }
}