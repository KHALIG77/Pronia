using System.Diagnostics.Contracts;

namespace Pronia.Areas.Manage.ViewModels
{
    public class ChartsDataViewModel
    {
        public decimal MonthlyEarnings { get; set; }
        public decimal YearlyEarnings { get; set;}
        public int PendingOrderCount {get; set;}
        public MonthViewModel Months {get; set;}
        public PieChartViewModel PieChart { get; set;}
       
     
    }
}
