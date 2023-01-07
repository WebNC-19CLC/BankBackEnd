using AsrTool.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace AsrTool.Hubs
{
  public class MessageHub : Hub<IMessageHub>
  {
    public Task JoinGroupNotification()
    {
      throw new NotImplementedException();
    }
    public async Task SendNotificationToUser(NotifationDto notifation){
    
    }

    public async Task SendTransactionToUser(TransactionDto transaction) {
    
    }
  }
}
