using Electro.Corporation.Platform.Analytics.Domain.Model.Commands;

namespace Electro.Corporation.Platform.Analytics.Domain.Model.Entities;

public class Alert
{
    public Alert()
    {
        Title = string.Empty;
        Message = string.Empty;
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    public int PropertyId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}
