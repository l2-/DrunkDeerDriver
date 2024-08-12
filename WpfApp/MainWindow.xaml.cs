using Driver;
using IWshRuntimeLibrary;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using WpfApp.Components;
using WpfApp.GlobalKeyHook;
using WpfApp.Profile;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool ShouldStartMinimized { get; set; } = false;
        private readonly Dictionary<int, KeyHandler> handlers = [];
        private readonly ProfileManager ProfileManager;

        public MainWindow(ProfileManager profileManager, TrayIcon icon)
        {
            ProfileManager = profileManager;
            InitializeComponent();
            icon.DoubleClick = () => Restore();
        }

        private void RefreshDataGrid()
        {
            CollectionViewSource.GetDefaultView(dataGrid.ItemsSource).Refresh();
            dataGrid.Columns.First().Width = 0;
            var col = dataGrid.Columns.First(c => c.Header.Equals("Process triggers"));
            col.Width = DataGridLength.SizeToCells;
            dataGrid.UpdateLayout();
            dataGrid.Columns.First().Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void ProfileChanged(int index, ProfileItem item)
        {
            CurrentProfileLabel.Content = string.Format("Current Profile: {0}", item.Name);
            ProfileManager.PushCurrentProfile();
        }

        private void ProfileCollectionChanged(int index, ProfileItem item)
        {
            RefreshDataGrid();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            ProfileManager.CurrentProfileChanged += ProfileChanged;
            ProfileManager.ProfileCollectionChanged += ProfileCollectionChanged;
            dataGrid.ItemsSource = ProfileManager.Profiles;
            ProfileManager.DiscoverProfiles();

            var windowHandle = new WindowInteropHelper(this).Handle;
            var source = HwndSource.FromHwnd(windowHandle);

            var enterHandler = new KeyHandler(KeyHandler.ToKeycode(Key.Enter), windowHandle, source, KeyHandler.MOD_CONTROL | KeyHandler.MOD_ALT | KeyHandler.MOD_NOREPEAT)
            {
                Callback = ProfileManager.QuickSwitchProfile,
            };
            handlers[enterHandler.GetHashCode()] = enterHandler;
            foreach (var handler in handlers.Values)
            {
                handler.Register();
            }

            StartOnWindowsStartupToggle.IsChecked = StartupShortcutHelper.StartupFileExists();
            StartOnWindowsStartupToggle.Click += OnCheckChanged;
        }

        protected override void OnClosed(EventArgs e)
        {
            foreach (var handler in handlers.Values)
            {
                handler.Unregiser();
            }
            base.OnClosed(e);
        }

        protected void OnImportButtonCLicked(object sender, EventArgs e)
        {
            // Configure open file dialog box
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".json", // Default file extension
                Filter = "Text documents (.json)|*.json" // Filter files by extension
            };

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string path = dialog.FileName;
                ProfileManager.ImportProfile(path);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ShouldStartMinimized)
            {
                WindowState = WindowState.Minimized;
            }
        }

        public void Restore()
        {
            if (WindowState is WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState is WindowState.Minimized)
            {
                WindowStyle = WindowStyle.ToolWindow;
                ShowInTaskbar = false;
            }
            else
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                ShowInTaskbar = true;
            }
        }

        private void OnCheckChanged(object? sender, EventArgs e)
        {
            StartupShortcutHelper.OnCheckChanged(StartOnWindowsStartupToggle.IsChecked ?? false);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.DataContext is ProfileItem profileItem)
            {
                var window = new ProcessSelector();
                window.Show();
            }
        }
    }
}