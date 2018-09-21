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
        public string Term { get; }
        public string Owner { get; }
        public string SortBy { get; }
        public string Order { get; }
        public string DateChoice { get; }
        public string Language { get; }
        public string StarsChoice { get; }
        public string ForksChoice { get; }
        public string SizeChoice { get; }
        public int Stars { get; }
        public int Forks { get; }
        public int Size { get; }
        public DateTime? Date { get; }
        public DateTime? EndDate { get; }
        public DateTime? UpdatedAt { get; }
        public bool? ReadmeIncluded { get; }

        public SearchRepositoriesRequestParameters(string term, string owner, int stars, int forks, int size, string sortBy, string order,
                                                   DateTime? date, string dateChoice, DateTime? updatedAt, string language, DateTime? endDate,
                                                   bool? readmeIncluded, string starsChoice, string forksChoice, string sizeChoice)
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
            StarsChoice = starsChoice;
            ForksChoice = forksChoice;
            SizeChoice = sizeChoice;
        }
    }
}
