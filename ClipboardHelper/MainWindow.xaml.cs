using ClipboardHelper.Pages;
using System.Windows;

namespace ClipboardHelper;

public partial class MainWindow : Window
{
    public MainWindow(MainPage mainPage)
    {
        InitializeComponent();

        MainFrame.Navigate(mainPage);
    }
}