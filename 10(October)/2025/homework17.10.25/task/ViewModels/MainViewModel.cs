using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using task.Models;

namespace task.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ColorModel> Colors { get; }
            = new ObservableCollection<ColorModel>();

        private byte a = 255, r, g, b;
        private bool aEnabled = true, rEnabled = true, gEnabled = true, bEnabled = true;

        public byte A
        {
            get => a;
            set { a = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentBrush)); }
        }

        public byte R
        {
            get => r;
            set { r = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentBrush)); }
        }

        public byte G
        {
            get => g;
            set { g = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentBrush)); }
        }

        public byte B
        {
            get => b;
            set { b = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentBrush)); }
        }

        public bool AEnabled { get => aEnabled; set { aEnabled = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentBrush)); } }
        public bool REnabled { get => rEnabled; set { rEnabled = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentBrush)); } }
        public bool GEnabled { get => gEnabled; set { gEnabled = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentBrush)); } }
        public bool BEnabled { get => bEnabled; set { bEnabled = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentBrush)); } }

        public SolidColorBrush CurrentBrush =>
            new SolidColorBrush(Color.FromArgb(
                AEnabled ? A : (byte)255,
                REnabled ? R : (byte)0,
                GEnabled ? G : (byte)0,
                BEnabled ? B : (byte)0));

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainViewModel()
        {
            AddCommand = new RelayCommand(_ => AddColor(), _ => CanAdd());
            DeleteCommand = new RelayCommand(c => Colors.Remove((ColorModel)c));
        }

        private void AddColor()
        {
            Colors.Add(new ColorModel
            {
                A = A,
                R = R,
                G = G,
                B = B
            });
        }

        private bool CanAdd()
        {
            var color = new ColorModel { A = A, R = R, G = G, B = B };
            return !Colors.Contains(color);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
