namespace Airlines.Api.Services
{
    public interface IMessageProducer
    {
        Task<bool> SendMessage<T>(T message);
    }
}
