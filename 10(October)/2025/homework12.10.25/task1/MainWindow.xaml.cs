using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace task1
{
    public partial class MainWindow : Window
    {
        private bool _running = false;
        private string _target = "";
        private int _pos = 0;
        private int _errors = 0;

        private readonly Stopwatch _sw = new();
        private bool _shiftDown = false;
        private bool _capsOn = false;
        private readonly Dictionary<Key, Border> _keyToVisual = new();

        private readonly Dictionary<Key, (string normal, string shifted)> _labels = new();

        private readonly string _letters = "abcdefghijklmnopqrstuvwxyz";
        private readonly string _digits = "0123456789";
        private readonly string _symbolsNormal = "`-=[]\\;',./";
        private readonly string _symbolsShifted = "~_+{}|:\"<>?";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BuildKeyLabels();
            BuildKeyboardUI();
            UpdateControls();
            UpdateKeyboardLegends();
        }

        private void LenSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LenValueText != null)
                LenValueText.Text = ((int)LenSlider.Value).ToString();
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            _running = true;
            _pos = 0;
            _errors = 0;
            _sw.Reset();
            _sw.Start();

            _target = GenerateTarget((int)LenSlider.Value, UseCaseCheck.IsChecked == true);
            TargetText.Text = _target;
            TypedText.Text = "";

            StateText.Text = "Type the text…";
            UpdateStats();
            UpdateControls();

            Focus();
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            StopSession("Stopped.");
        }

        private void StopSession(string reason)
        {
            _running = false;
            _sw.Stop();
            StateText.Text = reason;
            UpdateControls();
            ClearHighlights();
        }

        private void UpdateControls()
        {
            StartBtn.IsEnabled = !_running;
            StopBtn.IsEnabled = _running;

            LenSlider.IsEnabled = !_running;
            UseCaseCheck.IsEnabled = !_running;
        }

        private string GenerateTarget(int length, bool useCase)
        {
            var rng = new Random();

            var pool = new List<char>();
            pool.AddRange(_letters);
            pool.AddRange(_digits);
            pool.Add(' ');

            pool.AddRange(_symbolsNormal);

            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                char c = pool[rng.Next(pool.Count)];

                if (char.IsLetter(c) && useCase)
                {
                    c = rng.Next(2) == 0 ? char.ToLower(c) : char.ToUpper(c);
                }
                else if (IsSymbol(c) && useCase)
                {
                    int idx = _symbolsNormal.IndexOf(c);
                    if (idx >= 0 && rng.Next(2) == 1)
                        c = _symbolsShifted[idx];
                }

                sb.Append(c);
            }

            return sb.ToString();
        }

        private bool IsSymbol(char c) => _symbolsNormal.Contains(c) || _symbolsShifted.Contains(c);

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                _shiftDown = true;
                HighlightKey(e.Key, true);
                UpdateKeyboardLegends();
                return;
            }

            if (e.Key == Key.CapsLock)
            {
                _capsOn = !_capsOn;
                HighlightKey(Key.CapsLock, true);
                UpdateKeyboardLegends();
                return;
            }

            HighlightKey(e.Key, true);

            if (!_running)
                return;
            char? typedChar = KeyToChar(e.Key);
            if (typedChar == null)
            {
                return;
            }
            if (_pos >= _target.Length)
                return;

            char expected = _target[_pos];
            char actual = typedChar.Value;

            AppendTyped(actual, actual == expected);

            if (actual == expected)
            {
                _pos++;
                if (_pos >= _target.Length)
                {
                    _sw.Stop();
                    UpdateStats();
                    StopSession("Done!");
                }
            }
            else
            {
                _errors++;
            }

            UpdateStats();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                _shiftDown = false;
                HighlightKey(e.Key, false);
                UpdateKeyboardLegends();
                return;
            }

            HighlightKey(e.Key, false);
        }

        private void AppendTyped(char c, bool ok)
        {
            string token = ok ? c.ToString() : $"[{c}]";
            TypedText.Text += token;
        }

        private void UpdateStats()
        {
            ErrorsText.Text = _errors.ToString();
            double minutes = Math.Max(_sw.Elapsed.TotalMinutes, 1.0 / 60.0); 
            int correct = Math.Min(_pos, _target.Length);
            int cpm = (int)Math.Round(correct / minutes);

            SpeedText.Text = $"{cpm} cpm";
        }

        private char? KeyToChar(Key key)
        {
            if (key == Key.Space) return ' ';
            if (key >= Key.A && key <= Key.Z)
            {
                char baseChar = (char)('a' + (key - Key.A));
                bool upper = _capsOn ^ _shiftDown;
                return upper ? char.ToUpper(baseChar) : baseChar;
            }

            if (key >= Key.D0 && key <= Key.D9)
            {
                int d = key - Key.D0;
                if (_shiftDown)
                {
                    string shifted = ")!@#$%^&*(";
                    return shifted[d];
                }
                return (char)('0' + d);
            }

            if (key >= Key.NumPad0 && key <= Key.NumPad9)
            {
                int d = key - Key.NumPad0;
                return (char)('0' + d);
            }
            if (_labels.TryGetValue(key, out var pair))
            {
                string s = _shiftDown ? pair.shifted : pair.normal;
                if (!string.IsNullOrEmpty(s) && s.Length == 1)
                    return s[0];
            }
            return null;
        }

        private void BuildKeyLabels()
        {
            for (Key k = Key.A; k <= Key.Z; k++)
            {
                char ch = (char)('A' + (k - Key.A));
                _labels[k] = (ch.ToString(), ch.ToString());
            }
            _labels[Key.D1] = ("1", "!");
            _labels[Key.D2] = ("2", "@");
            _labels[Key.D3] = ("3", "#");
            _labels[Key.D4] = ("4", "$");
            _labels[Key.D5] = ("5", "%");
            _labels[Key.D6] = ("6", "^");
            _labels[Key.D7] = ("7", "&");
            _labels[Key.D8] = ("8", "*");
            _labels[Key.D9] = ("9", "(");
            _labels[Key.D0] = ("0", ")");

            _labels[Key.Oem3] = ("`", "~");
            _labels[Key.OemMinus] = ("-", "_");
            _labels[Key.OemPlus] = ("=", "+");
            _labels[Key.OemOpenBrackets] = ("[", "{");
            _labels[Key.Oem6] = ("]", "}");
            _labels[Key.Oem5] = ("\\", "|");
            _labels[Key.Oem1] = (";", ":");
            _labels[Key.OemQuotes] = ("'", "\"");
            _labels[Key.OemComma] = (",", "<");
            _labels[Key.OemPeriod] = (".", ">");
            _labels[Key.OemQuestion] = ("/", "?");

            _labels[Key.Tab] = ("Tab", "Tab");
            _labels[Key.CapsLock] = ("Caps", "Caps");
            _labels[Key.LeftShift] = ("Shift", "Shift");
            _labels[Key.RightShift] = ("Shift", "Shift");
            _labels[Key.LeftCtrl] = ("Ctrl", "Ctrl");
            _labels[Key.LeftAlt] = ("Alt", "Alt");
            _labels[Key.Space] = ("Space", "Space");
            _labels[Key.Back] = ("Back", "Back");
            _labels[Key.Enter] = ("Enter", "Enter");
        }

        private void BuildKeyboardUI()
        {
            KeyboardPanel.Children.Clear();
            _keyToVisual.Clear();

            AddRow(new (Key key, double w)[] {
                (Key.Oem3, 1), (Key.D1,1), (Key.D2,1), (Key.D3,1), (Key.D4,1), (Key.D5,1),
                (Key.D6,1), (Key.D7,1), (Key.D8,1), (Key.D9,1), (Key.D0,1),
                (Key.OemMinus,1), (Key.OemPlus,1), (Key.Back, 2.2)
            });

            AddRow(new (Key key, double w)[] {
                (Key.Tab, 1.6),
                (Key.Q,1),(Key.W,1),(Key.E,1),(Key.R,1),(Key.T,1),(Key.Y,1),(Key.U,1),(Key.I,1),(Key.O,1),(Key.P,1),
                (Key.OemOpenBrackets,1),(Key.Oem6,1),(Key.Oem5,1.6)
            });

            AddRow(new (Key key, double w)[] {
                (Key.CapsLock, 1.9),
                (Key.A,1),(Key.S,1),(Key.D,1),(Key.F,1),(Key.G,1),(Key.H,1),(Key.J,1),(Key.K,1),(Key.L,1),
                (Key.Oem1,1),(Key.OemQuotes,1),
                (Key.Enter, 2.2)
            });

            AddRow(new (Key key, double w)[] {
                (Key.LeftShift, 2.4),
                (Key.Z,1),(Key.X,1),(Key.C,1),(Key.V,1),(Key.B,1),(Key.N,1),(Key.M,1),
                (Key.OemComma,1),(Key.OemPeriod,1),(Key.OemQuestion,1),
                (Key.RightShift, 2.6)
            });

            AddRow(new (Key key, double w)[] {
                (Key.LeftCtrl, 1.5),
                (Key.LeftAlt, 1.5),
                (Key.Space, 6.5)
            });
        }

        private void AddRow(IEnumerable<(Key key, double w)> keys)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 8) };

            foreach (var (key, w) in keys)
            {
                var border = new Border
                {
                    CornerRadius = new CornerRadius(8),
                    BorderBrush = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
                    BorderThickness = new Thickness(1),
                    Background = Brushes.White,
                    Margin = new Thickness(4, 0, 4, 0),
                    Width = 60 * w,
                    Height = 52
                };

                var label = new TextBlock
                {
                    Text = GetLegendForKey(key),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 14,
                    FontWeight = FontWeights.SemiBold
                };

                border.Child = label;
                row.Children.Add(border);
                _keyToVisual[key] = border;
            }

            KeyboardPanel.Children.Add(row);
        }

        private string GetLegendForKey(Key key)
        {
            if (!_labels.TryGetValue(key, out var pair))
                return key.ToString();

            if (key >= Key.A && key <= Key.Z)
            {
                char ch = pair.normal[0]; // 'A'
                bool upper = _capsOn ^ _shiftDown;
                return upper ? ch.ToString().ToUpper() : ch.ToString().ToLower();
            }
            return _shiftDown ? pair.shifted : pair.normal;
        }

        private void UpdateKeyboardLegends()
        {
            foreach (var kv in _keyToVisual)
            {
                if (kv.Value.Child is TextBlock tb)
                    tb.Text = GetLegendForKey(kv.Key);

                if (kv.Key == Key.CapsLock)
                {
                    kv.Value.Background = _capsOn ? new SolidColorBrush(Color.FromArgb(255, 230, 245, 255)) : Brushes.White;
                }
            }
        }

        private void HighlightKey(Key key, bool on)
        {
            if (!_keyToVisual.TryGetValue(key, out var border))
                return;

            if (on)
                border.Background = new SolidColorBrush(Color.FromArgb(255, 255, 245, 200)); // мягкая подсветка
            else
            {
                if (key == Key.CapsLock && _capsOn)
                    border.Background = new SolidColorBrush(Color.FromArgb(255, 230, 245, 255));
                else
                    border.Background = Brushes.White;
            }
        }

        private void ClearHighlights()
        {
            foreach (var kv in _keyToVisual)
            {
                if (kv.Key == Key.CapsLock && _capsOn)
                    kv.Value.Background = new SolidColorBrush(Color.FromArgb(255, 230, 245, 255));
                else
                    kv.Value.Background = Brushes.White;
            }
        }
    }
}
