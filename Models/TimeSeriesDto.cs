namespace SalesDW.API.Models;

public class TimeSeriesPointDto
{
    public string Period { get; set; } = string.Empty; // label e.g., 2023-05 or 2023-05-21 or 2023
    public decimal TotalAmount { get; set; }
    public int TotalQty { get; set; }
}
