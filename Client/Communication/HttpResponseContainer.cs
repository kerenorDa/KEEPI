namespace Keepi.Client.Communication
{
    public class HttpResponseContainer<T>
    {
        public HttpResponseContainer(T response, bool success, HttpResponseMessage httpResponseMessage)
        {
            Response = response;
            Success = success;
            HttpResponseMessage = httpResponseMessage;
        }

        public T Response { get; }

        public bool Success { get; }

        public HttpResponseMessage HttpResponseMessage { get; }

        public async Task<string> GetBody() => await HttpResponseMessage.Content.ReadAsStringAsync();
    }

}
