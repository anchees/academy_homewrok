using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Main;

public partial class MainWindow : Window
{
    private string openWeatherKey = "API_KEY";
    private string omdbKey = "API_KEY"; 
// проверял с API ключ из материала но git не дает запушить файлы с API ключами поэтому заменил
    private string lastMovieResult = "";

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void WeatherButton_Click(object sender, RoutedEventArgs e)
    {
        string city = CityBox.Text;

        if (string.IsNullOrWhiteSpace(city))
        {
            MessageBox.Show("Введите город");
            return;
        }

        try
        {
            string result = await GetWeather(city);
            ResultBox.Text = result;
        }
        catch
        {
            MessageBox.Show("Ошибка получения погоды");
        }
    }

    private async Task<string> GetWeather(string city)
    {
        HttpClient client = new HttpClient();

        string geoUrl =
            $"https://api.openweathermap.org/geo/1.0/direct?q={WebUtility.UrlEncode(city)}&limit=1&appid={openWeatherKey}";

        string geoJson = await client.GetStringAsync(geoUrl);

        JsonDocument geoDoc = JsonDocument.Parse(geoJson);

        if (geoDoc.RootElement.GetArrayLength() == 0)
            return "Город не найден.";

        double lat = geoDoc.RootElement[0].GetProperty("lat").GetDouble();
        double lon = geoDoc.RootElement[0].GetProperty("lon").GetDouble();

        string weatherUrl =
            $"https://api.openweathermap.org/data/3.0/onecall?lat={lat}&lon={lon}&exclude=minutely,hourly,alerts&units=metric&lang=ru&appid={openWeatherKey}";

        string weatherJson = await client.GetStringAsync(weatherUrl);

        JsonDocument weatherDoc = JsonDocument.Parse(weatherJson);

        var current = weatherDoc.RootElement.GetProperty("current");

        string text = $"Погода в городе: {city}\n\n";
        text += "Сегодня:\n";
        text += $"Температура: {current.GetProperty("temp").GetDouble()} °C\n";
        text += $"Влажность: {current.GetProperty("humidity").GetInt32()}%\n";
        text += $"Ветер: {current.GetProperty("wind_speed").GetDouble()} м/с\n";
        text += $"Описание: {current.GetProperty("weather")[0].GetProperty("description").GetString()}\n\n";

        text += "Прогноз на 7 дней:\n\n";

        var daily = weatherDoc.RootElement.GetProperty("daily");

        for (int i = 0; i < 7; i++)
        {
            var day = daily[i];

            long unixTime = day.GetProperty("dt").GetInt64();
            DateTime date = DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;

            double dayTemp = day.GetProperty("temp").GetProperty("day").GetDouble();
            double nightTemp = day.GetProperty("temp").GetProperty("night").GetDouble();

            string description =
                day.GetProperty("weather")[0].GetProperty("description").GetString();

            text += $"{date:dd.MM.yyyy}\n";
            text += $"Днём: {dayTemp} °C\n";
            text += $"Ночью: {nightTemp} °C\n";
            text += $"Описание: {description}\n\n";
        }

        return text;
    }

    private async void MovieButton_Click(object sender, RoutedEventArgs e)
    {
        string movie = MovieBox.Text;

        if (string.IsNullOrWhiteSpace(movie))
        {
            MessageBox.Show("Введите название фильма");
            return;
        }

        try
        {
            string result = await GetMovie(movie);
            lastMovieResult = result;
            ResultBox.Text = result;
        }
        catch
        {
            MessageBox.Show("Ошибка получения информации о фильме");
        }
    }

    private async Task<string> GetMovie(string movie)
    {
        HttpClient client = new HttpClient();

        string url =
            $"https://www.omdbapi.com/?t={WebUtility.UrlEncode(movie)}&apikey={omdbKey}&plot=full";

        string json = await client.GetStringAsync(url);

        JsonDocument doc = JsonDocument.Parse(json);

        string response = doc.RootElement.GetProperty("Response").GetString();

        if (response == "False")
            return "Фильм не найден.";

        string title = doc.RootElement.GetProperty("Title").GetString();
        string year = doc.RootElement.GetProperty("Year").GetString();
        string genre = doc.RootElement.GetProperty("Genre").GetString();
        string director = doc.RootElement.GetProperty("Director").GetString();
        string actors = doc.RootElement.GetProperty("Actors").GetString();
        string plot = doc.RootElement.GetProperty("Plot").GetString();
        string rating = doc.RootElement.GetProperty("imdbRating").GetString();

        string result = "";

        result += $"Название: {title}\n";
        result += $"Год: {year}\n";
        result += $"Жанр: {genre}\n";
        result += $"Режиссёр: {director}\n";
        result += $"Актёры: {actors}\n";
        result += $"Рейтинг IMDb: {rating}\n\n";
        result += $"Описание:\n{plot}\n";

        return result;
    }

    private void SendEmailButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(lastMovieResult))
        {
            MessageBox.Show("Сначала найдите фильм");
            return;
        }

        string toEmail = EmailBox.Text;

        if (string.IsNullOrWhiteSpace(toEmail))
        {
            MessageBox.Show("Введите email получателя");
            return;
        }

        try
        {
            string fileName = "movie_result.txt";
            File.WriteAllText(fileName, lastMovieResult);

            MailMessage message = new MailMessage();
            message.From = new MailAddress("YOUR_EMAIL@gmail.com");
            message.To.Add(toEmail);
            message.Subject = "Информация о фильме";
            message.Body = "Здравствуйте! Во вложении находится информация о фильме.";
            message.Attachments.Add(new Attachment(fileName));

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(
                "YOUR_EMAIL@gmail.com",
                "YOUR_APP_PASSWORD"
            );

            smtp.Send(message);

            MessageBox.Show("Письмо отправлено");
        }
        catch
        {
            MessageBox.Show("Ошибка отправки письма");
        }
    }
}