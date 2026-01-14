using System;
using System.Windows;
using System.Windows.Controls;

namespace task2
{
    public partial class MainWindow : Window
    {
        double result = 0;
        string operation = "";
        bool newNumber = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        void Digit(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            if (newNumber)
            {
                DisplayBox.Text = b.Content.ToString();
                newNumber = false;
            }
            else
            {
                if (DisplayBox.Text == "0")
                    DisplayBox.Text = b.Content.ToString();
                else
                    DisplayBox.Text += b.Content.ToString();
            }
        }

        void Dot(object sender, RoutedEventArgs e)
        {
            if (!DisplayBox.Text.Contains("."))
                DisplayBox.Text += ".";
        }

        void Operation(object sender, RoutedEventArgs e)
        {
            Calculate();
            Button b = (Button)sender;
            operation = b.Content.ToString();
            HistoryBox.Text = $"{result} {operation}";
            newNumber = true;
        }

        void Equals(object sender, RoutedEventArgs e)
        {
            Calculate();
            HistoryBox.Text = "";
            DisplayBox.Text = result.ToString();
            newNumber = true;
        }

        void Calculate()
        {
            double current = double.Parse(DisplayBox.Text);

            switch (operation)
            {
                case "+": result += current; break;
                case "-": result -= current; break;
                case "*": result *= current; break;
                case "/": result /= current; break;
                default: result = current; break;
            }
        }

        void ClearEntry(object sender, RoutedEventArgs e)
        {
            DisplayBox.Text = "0";
            newNumber = true;
        }

        void ClearAll(object sender, RoutedEventArgs e)
        {
            result = 0;
            operation = "";
            DisplayBox.Text = "0";
            HistoryBox.Text = "";
            newNumber = true;
        }

        void Backspace(object sender, RoutedEventArgs e)
        {
            if (DisplayBox.Text.Length > 1)
                DisplayBox.Text = DisplayBox.Text[..^1];
            else
                DisplayBox.Text = "0";
        }
    }
}
