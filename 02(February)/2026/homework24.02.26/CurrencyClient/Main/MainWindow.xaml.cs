using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace Main;

public partial class MainWindow : Window
{
    private CurrencyClientService client = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void ConnectButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string host = HostBox.Text;
            int port = int.Parse(PortBox.Text);

            string answer = await client.ConnectAsync(
                host,
                port,
                LoginBox.Text,
                PasswordBox.Text
            );

            AddLog(answer);
        }
        catch
        {
            AddLog("Ошибка подключения.");
        }
    }

    private async void SendButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string answer = await client.SendAsync(CurrencyBox.Text);
            AddLog(answer);
        }
        catch
        {
            AddLog("Ошибка отправки запроса.");
        }
    }

    private async void DisconnectButton_Click(object sender, RoutedEventArgs e)
    {
        await client.DisconnectAsync();
        AddLog("Отключение от сервера.");
    }

    private void AddLog(string text)
    {
        ResultBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {text}\n");
        ResultBox.ScrollToEnd();
    }
}

public class CurrencyClientService
{
    private TcpClient? client;
    private StreamReader? reader;
    private StreamWriter? writer;

    public async Task<string> ConnectAsync(
        string host,
        int port,
        string login,
        string password)
    {
        client = new TcpClient();
        await client.ConnectAsync(host, port);

        NetworkStream stream = client.GetStream();

        reader = new StreamReader(stream);
        writer = new StreamWriter(stream) { AutoFlush = true };

        string firstMessage = await reader.ReadLineAsync() ?? "";

        await writer.WriteLineAsync($"LOGIN {login} {password}");

        string loginAnswer = await reader.ReadLineAsync() ?? "";

        return firstMessage + "\n" + loginAnswer;
    }

    public async Task<string> SendAsync(string message)
    {
        if (writer == null || reader == null)
            return "Нет подключения к серверу.";

        await writer.WriteLineAsync(message);

        string answer = await reader.ReadLineAsync() ?? "Сервер закрыл соединение.";

        return answer;
    }

    public async Task DisconnectAsync()
    {
        if (writer != null)
            await writer.WriteLineAsync("EXIT");

        client?.Close();
    }
}