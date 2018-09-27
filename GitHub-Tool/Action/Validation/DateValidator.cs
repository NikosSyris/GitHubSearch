using System;
using System.Windows.Controls;

namespace GitHubSearch.Action.Validation
{
    class DateValidator
    {
        public DateTime? validate(DatePicker startDate, DatePicker endDate)
        {
            if (startDate.SelectedDate > endDate.SelectedDate)
            {
                return DateTime.Today;
            }
            return endDate.SelectedDate;
        }
    }
}
