using DemoAsyncProgramming.Data.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace DemoAsyncProgramming.Data
{
    public class DataStore
    {
        public List<Company> Companies = new List<Company>();
        public IList<string> names = new List<string>();
        public IList<string> description = new List<string>();
        public IList<string> sector = new List<string>();

        private string basePath { get; }

        public DataStore(string basePath)
        {
            this.basePath = basePath;
        }
        public Task<IList<string>> LoadCompanyNames(CancellationToken token)
        {
            if (names.Count > 0) return Task.FromResult(names);

            LoadCompanies(token);

            names = Companies.Select(s => s.CompanyName).ToList();
            
            return Task.FromResult(names);
        }

        public Task<IList<string>> LoadCompanyDescription(CancellationToken token)
        {
            if (description.Count > 0) return Task.FromResult(description); 

            LoadCompanies(token);

            description = Companies.Select(s => s.Description).ToList();
           
            return Task.FromResult(description);
        }
        public Task<IList<string>> LoadCompanySector(CancellationToken token)
        {
            if (sector.Count > 0) return Task.FromResult(sector); //return sector;

            LoadCompanies(token);

            sector = Companies.Select(s => s.Sector).ToList();

            return Task.FromResult(sector);
        }

        private void LoadCompanies(CancellationToken token)
        {
            using (var stream = new StreamReader(File.OpenRead(Path.Combine(basePath, @"CompanyData.csv"))))
            {
                stream.ReadLine();

                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    #region Loading and Adding Company to In-Memory Dictionary

                    var segments = line.Split(',');

                    for (var i = 0; i < segments.Length; i++) segments[i] = segments[i].Trim('\'', '"');

                    var company = new Company
                    {
                        Symbol = segments[0],
                        CompanyName = segments[1],
                        Exchange = segments[2],
                        Industry = segments[3],
                        Website = segments[4],
                        Description = segments[5],
                        CEO = segments[6],
                        IssueType = segments[7],
                        Sector = segments[8]
                    };
                    
                    Companies.Add(company);

                    #endregion
                }
            }
        }
    }
}
