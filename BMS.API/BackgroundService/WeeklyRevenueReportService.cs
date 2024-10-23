using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.DAL.DataContext;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class WeeklyRevenueReportService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public WeeklyRevenueReportService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Chạy vào mỗi tối chủ nhật lúc 23:59
            var nextRunTime = GetNextSundayAtTime(23, 59);
            var delay = nextRunTime - DateTime.UtcNow;
            await Task.Delay(delay, stoppingToken);

            // Thực hiện báo cáo
            await GenerateAndSendWeeklyReports(stoppingToken);
        }
    }

    private DateTime GetNextSundayAtTime(int hour, int minute)
    {
        var now = DateTime.UtcNow;
        var sunday = now.AddDays((7 - (int)now.DayOfWeek) % 7);
        return new DateTime(sunday.Year, sunday.Month, sunday.Day, hour, minute, 0, DateTimeKind.Utc);
    }

    private async Task GenerateAndSendWeeklyReports(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var shopService = scope.ServiceProvider.GetRequiredService<IShopService>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var shops = await shopService.GetAllShopToRevenue(LastWeekStartDate(), LastWeekEndDate());
            var shopWeeklyReportService = scope.ServiceProvider.GetRequiredService<IShopWeeklyReportService>();
            foreach (var shop in shops)
            {
                var revenue = shop.Orders
                    .Where(o => o.Transactions.Any(t => t.Status == TransactionStatus.PAID && (t.Method == TransactionMethod.VnPay.ToString() || t.Method == TransactionMethod.PayOs.ToString())))
                    .Sum(o => o.TotalPrice);
                if (revenue > 0) {
                    var report = GenerateReportForShop(shop, revenue);

                    await emailService.SendEmailWithAttachmentAsync(shop.Email, "Weekly Revenue Report", "Your revenue report for the past week.", report);

                    await shopWeeklyReportService.CreateShopWeeklyReport(shop.Id, report);
                    await shopWeeklyReportService.SaveChange();
                }
            }

            
        }
    }

    private DateTime LastWeekStartDate()
    {
        var now = DateTime.UtcNow;
        return now.AddDays(-(int)now.DayOfWeek - 6).Date;
    }

    private DateTime LastWeekEndDate()
    {
        var now = DateTime.UtcNow;
        return now.AddDays(-(int)now.DayOfWeek).Date.AddDays(1).AddSeconds(-1);
    }

    private byte[] GenerateReportForShop(Shop shop, double revenue)
    {
        using (var ms = new MemoryStream())
        {
            var document = new Document();
            PdfWriter.GetInstance(document, ms);
            document.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            document.Add(new Paragraph($"Weekly Revenue Report for {shop.Name}", titleFont));
            document.Add(new Paragraph($"Address: {shop.Address}", regularFont));
            document.Add(new Paragraph($"Email: {shop.Email}", regularFont));
            document.Add(new Paragraph($"Phone: {shop.PhoneNumber}", regularFont));
            document.Add(new Paragraph(" "));

            var table = new PdfPTable(5);
            table.AddCell("Order ID");
            table.AddCell("Customer ID");
            table.AddCell("Order Date");
            table.AddCell("Total Price");
            table.AddCell("Status");

            foreach (var order in shop.Orders)
            {
                table.AddCell(order.Id.ToString());
                table.AddCell(order.CustomerId.ToString());
                table.AddCell(order.CreateDate.ToString("dd/MM/yyyy"));
                table.AddCell(order.TotalPrice.ToString("C"));
                table.AddCell(order.Status);
            }

            document.Add(table);

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph($"Total Revenue for the Week: {revenue.ToString("C")}", titleFont));

            document.Close();

            return ms.ToArray();
        }
    }

    private async Task CreateShopWeeklyReport(Guid shopId, byte[] report)
    {
        
    }
}
