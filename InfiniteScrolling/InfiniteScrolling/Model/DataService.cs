using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniteScrolling.Model
{
    public class DataService
    {
        private readonly List<string> _dataset;

        public DataService()
        {
            _dataset = new List<string>();
            for (int i = 1; i < 1000; i++)
            {
                _dataset.Add($"Item # {i}");
            }
        }

        public async Task<(int, List<string>)> GetAll(string seachTxt, int page, int pagesize)
        {
            await Task.Delay(2000);
            int skip = (page) * pagesize;
            Debug.WriteLine($"Getting Page:{page}; Page Size: {pagesize}");

            if (string.IsNullOrWhiteSpace(seachTxt))
            {
                int totalCount = _dataset.Count;
                return (totalCount, _dataset
                .Skip(skip)
                .Take(pagesize)
                .ToList());
            }
            else
            {
                var filteredDataset = _dataset
                    .Where(x => x.Contains(seachTxt));
                int totalCount = filteredDataset.Count();
                return (totalCount, filteredDataset
                    .Skip(skip)
                    .Take(pagesize)
                    .ToList());
            }
        }
    }
}
