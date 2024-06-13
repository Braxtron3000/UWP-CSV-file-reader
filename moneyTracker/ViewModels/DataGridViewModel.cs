using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Microsoft.Toolkit.Mvvm.ComponentModel;

using moneyTracker.Core.Models;
using moneyTracker.Core.Services;
using moneyTracker.Services;

namespace moneyTracker.ViewModels
{
    public class DataGridViewModel : ObservableObject
    {
        public ObservableCollection<FinancialTransaction> Source { get; } = new ObservableCollection<FinancialTransaction>();

        public DataGridViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // Replace this with your actual data
            //var data = await SampleDataService.GetGridDataAsync();
            IEnumerable<FinancialTransaction> data= await DataAccess.GetData();


            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }
}
