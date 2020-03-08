using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using DemoAsyncProgramming.Data;

namespace DemoAsyncProgramming.Controllers
{
    public class CompanyController : ApiController
    {
        //we can use containers for instantiating this object
        public DataStore companies = new DataStore(HostingEnvironment.MapPath("~/bin"));

        [Route("company/GetCompanyName")]
        public async Task<IList<string>> GetCompanyName(CancellationToken cancellationToken)
        {
            return await companies.LoadCompanyNames(cancellationToken).ConfigureAwait(false);
        }

        [Route("company/GetCompanyDescription")]
        public async Task<IList<string>> GetCompanyDescription(CancellationToken cancellationToken)
        {
            return await companies.LoadCompanyDescription(cancellationToken).ConfigureAwait(false);
        }

        [Route("company/GetCompanySector")]
        public async Task<IList<string>> GetCompanySector(CancellationToken cancellationToken)
        {
            return await companies.LoadCompanySector(cancellationToken).ConfigureAwait(false);
        }
    }
}
