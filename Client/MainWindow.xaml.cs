using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System;
using System.Text;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public MainWindow()
        {
            ListBoxItems = new ObservableCollection<XmlManager.LocationSheet>();
            
            DataContext = this;

            InitializeComponent();
        }
        
        private void NotifyPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        /// <summary>
        /// Contains LocationSheet items that are shown in the ListBox. 
        /// </summary>
        private ObservableCollection<XmlManager.LocationSheet> _ListBoxItems;
        public ObservableCollection<XmlManager.LocationSheet> ListBoxItems
        {
            get { return _ListBoxItems; }
            set
            {
                _ListBoxItems = value;
                NotifyPropertyChanged("ListBoxItems");
            }
        }

        /// <summary>
        /// Information about the trip from origin to destination.
        /// </summary>
        private XmlManager.TripSheet _SelectedTrip;
        public XmlManager.TripSheet SelectedTrip
        {
            get { return _SelectedTrip; }
            set
            {
                _SelectedTrip = value;
                NotifyPropertyChanged("SelectedTrip");

            }
        }

        /// <summary>
        /// Information about the weather at the destination.
        /// </summary>
        private XmlManager.WeatherSheet _WeatherSheet;
        public XmlManager.WeatherSheet WeatherSheet
        {
            get { return _WeatherSheet; }
            set
            {
                _WeatherSheet = value;
                NotifyPropertyChanged("WeatherSheet");                
            }
        }
        
        /// <summary>
        /// The selected LocationSheet value in the ListBox.
        /// </summary>
        private XmlManager.LocationSheet _SelectedListBoxItem;
        public XmlManager.LocationSheet SelectedListBoxItem
        {
            get { return _SelectedListBoxItem; }
            set
            {
                if (value != null)
                {
                    _SelectedListBoxItem = value;

                    //Make sure to not block UI
                    RecieveWeatherAndTripToDestination();

                    NotifyPropertyChanged("SelectedListBoxItem");
                }
            }
        }

        /// <summary>
        /// Seperate function to get trip and weather information to make sure UI thread isn't blocked.
        /// </summary>
        private async void RecieveWeatherAndTripToDestination()
        {
            await Task.Run(() =>
            {
                try
                {
                    SelectedTrip = WebServiceAPIContext.ReceiveTripToDestination(int.Parse(SelectedListBoxItem.ID), _ConnectionString);
                    WeatherSheet = WebServiceAPIContext.ReceiveWeather(SelectedTrip.DestinationLat,
                    SelectedTrip.DestinationLon, DateTime.Parse(SelectedTrip.DestinationTime), _ConnectionString);                    
                }
                catch (Exception ex)
                {                    
                    
                    ShowErrorMessageBox(ex.Message);
                }

                if (SelectedTrip.Error)
                    ShowErrorMessageBox(SelectedTrip.ExMsg);
                if (WeatherSheet.Error)
                    ShowErrorMessageBox(WeatherSheet.ExMsg);
            });
        }
       
        /// <summary>
        /// Location to search for.
        /// </summary>
        private string _SearchLocation;
        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            _SearchLocation = textBox.Text;
        }

        /// <summary>
        /// Connection URL to WebService API.
        /// </summary>
        private string _ConnectionString;
        private void ConnectionTextBox(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            _ConnectionString = textBox.Text;
        }

        /// <summary>
        /// Button click that initiates a search for diffrent locations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItems.Clear();
            XmlManager.LocationSheetList locationSheetList = new XmlManager.LocationSheetList();

            //Make sure to not block UI
            await Task.Run(() =>
             {
                 try
                 {
                     locationSheetList = WebServiceAPIContext.ReceiveLocationList(_SearchLocation, _ConnectionString);
                 }
                 catch (Exception ex)
                 {
                     ShowErrorMessageBox(ex.Message);
                 }
                 if (locationSheetList.Error && locationSheetList.ExMsg != String.Empty)
                     ShowErrorMessageBox(locationSheetList.ExMsg);
             });

            if (locationSheetList.LocationSheets != null)
            {
                foreach (var x in locationSheetList.LocationSheets)
                {
                    Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(x.Name));

                    ListBoxItems.Add(x);
                }
            }
        }

        /// <summary>
        /// Shows error message in a MessageBox.
        /// </summary>
        /// <param name="ErrorMessage">Error message to display.</param>
        private void ShowErrorMessageBox(string ErrorMessage)
        {
            if(ErrorMessage != null || ErrorMessage != String.Empty)
                MessageBox.Show("Error in recieveing data from API. " + ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Error in recieveing data from API. " + "No error message.", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
