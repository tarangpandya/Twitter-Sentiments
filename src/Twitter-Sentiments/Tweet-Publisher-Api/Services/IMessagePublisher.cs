namespace Tweet_Publisher_Api.Services
{
    public interface IMessagePublisher
    {
        public Task SendMessageAsync(string message);
    }
}
