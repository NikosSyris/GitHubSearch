using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub_Tool.Model
{
    class SearchRepositoriesRequestParameters
    {
        public string Term { get; set; }
        public string Owner { get; set; }
        public string SortBy { get; set; }
        public string Order { get; set; }
        public string DateChoice { get; set; }
        public string Language { get; set; }
        public int Stars { get; set; }
        public int Forks { get; set; }
        public int Size { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? ReadmeIncluded { get; set; }

        public SearchRepositoriesRequestParameters(string term, string owner, int stars, int forks, int size, string sortBy, string order,
                                                   DateTime? date, string dateChoice, DateTime? updatedAt, string language, DateTime? endDate,
                                                   bool? readmeIncluded)
        {
            Term = term;
            Owner = owner;
            SortBy = sortBy;
            Order = order;
            DateChoice = dateChoice;
            Stars = stars;
            Forks = forks;
            Size = size;
            Date = date;
            EndDate = endDate;
            UpdatedAt = updatedAt;
            Language = language;
            ReadmeIncluded = readmeIncluded;
        }
    }
}
