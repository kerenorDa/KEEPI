namespace Keepi.Client.Communication
{
    public interface IHttpService
    {
        Task<HttpResponseContainer<T>> Get<T>(string url);

        Task<HttpResponseContainer<object>> Post<T>(string url, T data);

        Task<HttpResponseContainer<TResponse>> Post<T, TResponse>(string url, T data);
    }
}
