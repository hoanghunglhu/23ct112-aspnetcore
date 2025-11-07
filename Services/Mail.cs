using System.Collections.Concurrent;
using LearnApiNetCore.Services; 

// Định nghĩa đối tượng chứa thông tin email
public class EmailMessage
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}

// Lớp hàng đợi bất đồng bộ
public class EmailQueue : IEmailQueue
{
    private readonly ConcurrentQueue<EmailMessage> _messages = new();

    public void Enqueue(EmailMessage message)
    {
        _messages.Enqueue(message);
    }

    public bool TryDequeue(out EmailMessage message)
    {
        return _messages.TryDequeue(out message);
    }
}

// Tạo Interface
public interface IEmailQueue
{
    void Enqueue(EmailMessage message);
    bool TryDequeue(out EmailMessage message);
}