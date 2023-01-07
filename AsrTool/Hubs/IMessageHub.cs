using AsrTool.Dtos;

namespace AsrTool.Hubs
{
  public interface IMessageHub
  {
    Task SendNotificationToUser(string username, NotifationDto notifation);

    Task SendNotificationToUser(NotifationDto notifation);

    Task SendTransactionToUser(TransactionDto transaction);

    Task JoinGroupNotification();
  }
}
