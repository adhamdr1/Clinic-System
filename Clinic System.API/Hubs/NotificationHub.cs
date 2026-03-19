namespace Clinic_System.API.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        // الدالة دي بتشتغل أوتوماتيك أول ما أي حد (دكتور أو سكرتير) يفتح الموقع والخط يوصل
        public override async Task OnConnectedAsync()
        {
            // 1. بنسأل التوكن: هل الراجل اللي لسه فاتح الموقع ده شغال "Admin"؟
            var isUserAdmin = Context.User?.IsInRole("Admin") ?? false;

            if (isUserAdmin)
            {
                // 2. لو هو أدمن، دخله فوراً في جروب اسمه "Admins"
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }
            // ممكن قدام نطبع هنا في الـ Log إن في يوزر اتصل
            await base.OnConnectedAsync();
        }

        // الدالة دي بتشتغل لما اليوزر يقفل الموقع
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // لما يقفل الموقع، بنخرجه من الجروب أوتوماتيك
            var isUserAdmin = Context.User?.IsInRole("Admin") ?? false;
            if (isUserAdmin)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admins");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinWaitingRoom()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "WaitingRoomScreens");
        }
    }
}
