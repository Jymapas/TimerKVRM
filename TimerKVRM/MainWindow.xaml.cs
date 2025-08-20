using System.Windows;
using TimerKVRM.Services;
using TimerKVRM.ViewModels;

namespace TimerKVRM;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new TimerViewModel(new SoundService());
    }
}