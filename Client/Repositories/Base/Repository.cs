using Keepi.Client.Communication;
using System.ComponentModel.Design;

namespace Keepi.Client.Repositories.Base
{
    public class Repository
    {
        private readonly IHttpService commService;

        public Repository(IHttpService commService) => this.commService = commService;

        protected async Task<List<T>> Get<T>(string url)
        {
            var response = await commService.Get<List<T>>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response.Response;
        }

        protected async Task<object> Post<T>(string url, T data)
        {
            var response = await commService.Post(url, data);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response.Response;
        }
       
    }
}
