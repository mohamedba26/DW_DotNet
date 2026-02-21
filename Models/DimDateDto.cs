namespace SalesDW.API.Models;

public class DimDateDto
{
    public int DateKey { get; set; }
    public DateOnly? FullDate { get; set; }
    public int? DayNumber { get; set; }
    public int? MonthNumber { get; set; }
    public string? MonthName { get; set; }
    public int? YearNumber { get; set; }
    public int? QuarterNumber { get; set; }
}
