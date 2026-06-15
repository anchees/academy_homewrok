using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace Main;

public partial class MainWindow : Window
{
    private CurrencyServerService? server;

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        int port = int.Parse(PortBox.Text);
        int maxRequests = int.Parse(MaxRequestsBox.Text);
        int maxClients = int.Parse(MaxClientsBox.Text);

        server = new CurrencyServerService(port, maxRequests, maxClients, AddLog);
        server.AddUser("user", "123");

        AddLog("Пользователь по умолчанию: user / 123");

        await server.StartAsync();
    }

    private void AddUserButton_Click(object sender, RoutedEventArgs e)
    {
        if (server == null)
        {
            AddLog("Сначала запустите сервер.");
            return;
        }

        server.AddUser(LoginBox.Text, PasswordBox.Text);
        AddLog($"Добавлен пользователь: {LoginBox.Text}");
    }

    private void AddLog(string text)
    {
        Dispatcher.Invoke(() =>
        {
            LogBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {text}\n");
            LogBox.ScrollToEnd();
        });
    }
}

public class CurrencyServerService
{
    private int port;
    private int maxRequests;
    private int maxClients;
    private Action<string> log;

    private TcpListener? listener;
    private int currentClients = 0;

    private Dictionary<string, string> users = new();
    private Dictionary<string, DateTime> blockedUsers = new();

    private Dictionary<string, double> rates = new()
    {
        { "USD_EUR", 0.92 },
        { "EUR_USD", 1.09 },
        { "USD_RUB", 90.0 },
        { "RUB_USD", 0.011 },
        { "EUR_RUB", 98.0 },
        { "RUB_EUR", 0.010 },
        { "USD_GBP", 0.79 },
        { "GBP_USD", 1.27 }
    };

    public CurrencyServerService(
        int port,
        int maxRequests,
        int maxClients,
        Action<string> log)
    {
        this.port = port;
        this.maxRequests = maxRequests;
        this.maxClients = maxClients;
        this.log = log;
    }

    public void AddUser(string login, string password)
    {
        users[login] = password;
    }

    public async Task StartAsync()
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();

        log($"Сервер запущен на порту {port}");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();

            if (currentClients >= maxClients)
            {
                using NetworkStream stream = client.GetStream();
                using StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

                await writer.WriteLineAsync("Сервер перегружен. Попробуйте позже.");
                client.Close();

                log("Отклонено подключение: максимум клиентов.");
                continue;
            }

            currentClients++;
            _ = HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        string clientEndPoint = client.Client.RemoteEndPoint!.ToString()!;
        string? login = null;
        int requestCount = 0;

        log($"Подключился клиент: {clientEndPoint}");

        try
        {
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream);
            using StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            await writer.WriteLineAsync("Введите: LOGIN логин пароль");

            string? loginLine = await reader.ReadLineAsync();

            if (loginLine == null || !loginLine.StartsWith("LOGIN "))
            {
                await writer.WriteLineAsync("Ошибка авторизации.");
                return;
            }

            string[] loginParts = loginLine.Split(' ');

            if (loginParts.Length != 3)
            {
                await writer.WriteLineAsync("Ошибка авторизации.");
                return;
            }

            login = loginParts[1];
            string password = loginParts[2];

            if (blockedUsers.ContainsKey(login))
            {
                DateTime unblockTime = blockedUsers[login];

                if (DateTime.Now < unblockTime)
                {
                    await writer.WriteLineAsync("Вы временно заблокированы. Попробуйте через минуту.");
                    return;
                }
                else
                {
                    blockedUsers.Remove(login);
                }
            }

            if (!users.ContainsKey(login) || users[login] != password)
            {
                await writer.WriteLineAsync("Неверный логин или пароль.");
                log($"Ошибка входа: {login}");
                return;
            }

            await writer.WriteLineAsync("Авторизация успешна.");
            log($"Пользователь вошёл: {login}");

            while (true)
            {
                string? message = await reader.ReadLineAsync();

                if (message == null || message.ToUpper() == "EXIT")
                    break;

                requestCount++;

                if (requestCount > maxRequests)
                {
                    blockedUsers[login] = DateTime.Now.AddMinutes(1);
                    await writer.WriteLineAsync("Превышен лимит запросов. Соединение закрыто на 1 минуту.");
                    log($"Пользователь {login} превысил лимит запросов.");
                    break;
                }

                string answer = GetRate(message);
                await writer.WriteLineAsync(answer);

                log($"{login}: запрос {message}, ответ: {answer}");
            }
        }
        catch
        {
            log($"Ошибка клиента: {clientEndPoint}");
        }
        finally
        {
            currentClients--;
            client.Close();
            log($"Отключился клиент: {clientEndPoint}");
        }
    }

    private string GetRate(string message)
    {
        string[] parts = message.ToUpper().Split(' ');

        if (parts.Length != 2)
            return "Нужно ввести две валюты. Например: USD EUR";

        string key = parts[0] + "_" + parts[1];

        if (rates.ContainsKey(key))
            return $"1 {parts[0]} = {rates[key]} {parts[1]}";

        return "Такого курса нет на сервере.";
    }
}