using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using moneyTracker.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace moneyTracker.Views
{
    public sealed partial class DataGridPage : Page
    {
        public DataGridViewModel ViewModel { get; } = new DataGridViewModel();

        // TODO: Change the grid as appropriate to your app, adjust the column definitions on DataGridPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public DataGridPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"WHATS UP I AM A BUTTON AHHHHHH");
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Downloads;
            picker.FileTypeFilter.Add(".csv");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                //this.textBlock.Text = "Picked photo: " + file.Name;
                System.Diagnostics.Debug.WriteLine("BB FILE NAME =" + file.Path);

                //convert to streamreader
                var randomAccessStream = await file.OpenReadAsync();
                Stream stream = randomAccessStream.AsStreamForRead();
                


                //using (var reader = new StreamReader("C:\\Users\\Brax\\Downloads\\accountActivityExport (16).csv"))
                using (StreamReader reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<StreetClassMap>();
                    var records = csv.GetRecords<Transaction>().ToList();
                    var streets = (from s in records select s);
                    System.Diagnostics.Debug.WriteLine("yo mama " + streets.First().Description);

                    records.ForEach(m => System.Diagnostics.Debug.WriteLine("and me: " + m.Description));
                }

            }
            else
            {
                //this.textBlock.Text = "Operation cancelled.";
                System.Diagnostics.Debug.WriteLine("OPERATION CANCELLED HOMIE");


            }
        }
    }

    public class Transaction
    {
        public string Date { get; set; }
        public string Description { get; set; }
        public string Withdrawals { get; set; }
        public string Deposits { get; set; }
        public string Category { get; set; }
        public string Balance { get; set; }
    }

    public class StreetClassMap : ClassMap<Transaction>
    {
        public StreetClassMap()
        {
            Map(m => m.Date);
            Map(m => m.Description);
            Map(m => m.Withdrawals);
            Map(m => m.Deposits);
            Map(m => m.Category);
            Map(m => m.Balance);
        }
    }



}
