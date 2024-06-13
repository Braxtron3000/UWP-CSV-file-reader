using System;
using System.Collections.Generic;
using System.Text;

namespace moneyTracker.Core.Models
{
    public class FinancialTransaction
    {
            public FinancialTransaction(string date, string description, string category, string amount)
            {
                Date = date;
                Description = description;
                Category = category;
                Amount = amount;
            }

            public string Date { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public string Amount { get; set; }
    }
}
