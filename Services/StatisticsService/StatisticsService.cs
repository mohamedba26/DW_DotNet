using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.StatisticsService
{
    public class StatisticsService : IStatisticsService
    {
        private readonly DwSalesPurchasingContext _context;

        public StatisticsService(DwSalesPurchasingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PurchasingByVendorDto>> GetPurchasingByVendorAsync(string sort = "desc", string? category = null)
        {
            var query = _context.FactPurchasings
                .AsNoTracking()
                .Include(f => f.ProductKeyNavigation)
                .Include(f => f.VendorKeyNavigation)
                .AsQueryable();

            var isAll = string.IsNullOrEmpty(category) || category.Equals("all", StringComparison.OrdinalIgnoreCase);

            if (!isAll)
            {
                query = query.Where(f => f.ProductKeyNavigation != null && f.ProductKeyNavigation.Category == category);
            }

            if (isAll)
            {
                // Aggregate across all categories: group by vendor only
                var q = query
                    .GroupBy(f => new { f.VendorKey, f.VendorKeyNavigation!.VendorName })
                    .Select(g => new PurchasingByVendorDto
                    {
                        VendorKey = g.Key.VendorKey,
                        VendorName = g.Key.VendorName,
                        Category = null,
                        TotalAmount = g.Sum(x => x.LineTotal ?? 0),
                        TotalQty = g.Sum(x => x.OrderQty ?? 0)
                    });

                if (sort?.ToLower() == "asc")
                    q = q.OrderBy(x => x.TotalAmount);
                else
                    q = q.OrderByDescending(x => x.TotalAmount);

                return await q.ToListAsync();
            }
            else
            {
                // Group by vendor and category
                var q = query
                    .GroupBy(f => new { f.VendorKey, f.VendorKeyNavigation!.VendorName, Category = f.ProductKeyNavigation!.Category })
                    .Select(g => new PurchasingByVendorDto
                    {
                        VendorKey = g.Key.VendorKey,
                        VendorName = g.Key.VendorName,
                        Category = g.Key.Category,
                        TotalAmount = g.Sum(x => x.LineTotal ?? 0),
                        TotalQty = g.Sum(x => x.OrderQty ?? 0)
                    });

                if (sort?.ToLower() == "asc")
                    q = q.OrderBy(x => x.TotalAmount);
                else
                    q = q.OrderByDescending(x => x.TotalAmount);

                return await q.ToListAsync();
            }
        }

        public async Task<IEnumerable<TopProductDto>> GetTopProductsAsync(int top = 10, string sort = "desc")
        {
            var q = _context.FactSales
                .AsNoTracking()
                .GroupBy(f => new { f.ProductKey, f.ProductKeyNavigation!.ProductName, f.ProductKeyNavigation!.Category })
                .Select(g => new TopProductDto
                {
                    ProductKey = g.Key.ProductKey,
                    ProductName = g.Key.ProductName,
                    Category = g.Key.Category,
                    TotalSalesAmount = g.Sum(x => x.LineTotal ?? 0),
                    TotalQty = g.Sum(x => x.OrderQty ?? 0)
                });

            if (sort?.ToLower() == "asc")
                q = q.OrderBy(x => x.TotalSalesAmount);
            else
                q = q.OrderByDescending(x => x.TotalSalesAmount);

            if (top > 0)
                q = q.Take(top);

            return await q.ToListAsync();
        }

        public async Task<IEnumerable<SalesByTerritoryDto>> GetSalesByTerritoryAsync(string sort = "desc", string? category = null)
        {
            var query = _context.FactSales
                .AsNoTracking()
                .Include(f => f.ProductKeyNavigation)
                .Include(f => f.TerritoryKeyNavigation)
                .AsQueryable();

            var isAll = string.IsNullOrEmpty(category) || category.Equals("all", StringComparison.OrdinalIgnoreCase);

            if (!isAll)
            {
                query = query.Where(f => f.ProductKeyNavigation != null && f.ProductKeyNavigation.Category == category);
            }

            var q = query
                .GroupBy(f => new { f.TerritoryKey, f.TerritoryKeyNavigation!.TerritoryName, Category = f.ProductKeyNavigation!.Category })
                .Select(g => new SalesByTerritoryDto
                {
                    TerritoryKey = g.Key.TerritoryKey,
                    TerritoryName = g.Key.TerritoryName,
                    Category = isAll ? null : g.Key.Category,
                    TotalSalesAmount = g.Sum(x => x.LineTotal ?? 0),
                    TotalQty = g.Sum(x => x.OrderQty ?? 0)
                });

            if (sort?.ToLower() == "asc")
                q = q.OrderBy(x => x.TotalSalesAmount);
            else
                q = q.OrderByDescending(x => x.TotalSalesAmount);

            return await q.ToListAsync();
        }

        public async Task<IEnumerable<SalesByYearDto>> GetSalesByYearAsync(string sort = "desc", string? category = null)
        {
            var query = _context.FactSales
                .AsNoTracking()
                .Include(f => f.ProductKeyNavigation)
                .Include(f => f.OrderDateKeyNavigation)
                .AsQueryable();

            var isAll = string.IsNullOrEmpty(category) || category.Equals("all", StringComparison.OrdinalIgnoreCase);

            if (!isAll)
            {
                query = query.Where(f => f.ProductKeyNavigation != null && f.ProductKeyNavigation.Category == category);
            }

            var q = query
                .Where(f => f.OrderDateKey.HasValue)
                .Join(_context.DimDates, f => f.OrderDateKey, d => d.DateKey, (f, d) => new { f, d })
                .GroupBy(x => new { Year = x.d.YearNumber, Category = x.f.ProductKeyNavigation!.Category })
                .Select(g => new SalesByYearDto
                {
                    Year = g.Key.Year,
                    TotalSalesAmount = g.Sum(x => x.f.LineTotal ?? 0),
                    TotalQty = g.Sum(x => x.f.OrderQty ?? 0)
                });

            if (sort?.ToLower() == "asc")
                q = q.OrderBy(x => x.Year);
            else
                q = q.OrderByDescending(x => x.Year);

            return await q.ToListAsync();
        }

        public async Task<IEnumerable<SalesByVendorDto>> GetSalesByVendorAsync(string sort = "desc", int top = 0, string? category = null)
        {
            var query = _context.FactPurchasings
                .AsNoTracking()
                .Include(f => f.ProductKeyNavigation)
                .Include(f => f.VendorKeyNavigation)
                .AsQueryable();

            var isAll = string.IsNullOrEmpty(category) || category.Equals("all", StringComparison.OrdinalIgnoreCase);

            if (!isAll)
            {
                query = query.Where(f => f.ProductKeyNavigation != null && f.ProductKeyNavigation.Category == category);
            }

            var q = query
                .GroupBy(f => new { f.VendorKey, f.VendorKeyNavigation!.VendorName, Category = f.ProductKeyNavigation!.Category })
                .Select(g => new SalesByVendorDto
                {
                    VendorKey = g.Key.VendorKey,
                    VendorName = g.Key.VendorName,
                    Category = isAll ? null : g.Key.Category,
                    TotalSalesAmount = g.Sum(x => x.LineTotal ?? 0),
                    TotalQty = g.Sum(x => x.OrderQty ?? 0)
                });

            if (sort?.ToLower() == "asc")
                q = q.OrderBy(x => x.TotalSalesAmount);
            else
                q = q.OrderByDescending(x => x.TotalSalesAmount);

            if (top > 0)
                q = q.Take(top);

            return await q.ToListAsync();
        }

        public async Task<IEnumerable<ProductProfitDto>> GetProductsByProfitAsync(int top = 10, string? category = null, string sort = "desc")
        {
            var isAll = string.IsNullOrEmpty(category) || category.Equals("all", StringComparison.OrdinalIgnoreCase);

            // Load product sales aggregates and purchase aggregates separately, then join in memory
            var salesAgg = await _context.FactSales
                .AsNoTracking()
                .GroupBy(s => s.ProductKey)
                .Select(g => new
                {
                    ProductKey = g.Key,
                    TotalSalesAmount = g.Sum(x => x.LineTotal ?? 0M),
                    TotalQtySold = g.Sum(x => x.OrderQty ?? 0)
                })
                .ToListAsync();

            var purchaseAgg = await _context.FactPurchasings
                .AsNoTracking()
                .GroupBy(p => p.ProductKey)
                .Select(g => new
                {
                    ProductKey = g.Key,
                    TotalPurchaseAmount = g.Sum(x => x.LineTotal ?? 0M),
                    TotalQtyPurchased = g.Sum(x => x.OrderQty ?? 0)
                })
                .ToListAsync();

            var products = await _context.DimProducts.AsNoTracking().ToListAsync();

            var query = from prod in products
                        join s in salesAgg on prod.ProductKey equals s.ProductKey into sg
                        from s in sg.DefaultIfEmpty()
                        join p in purchaseAgg on prod.ProductKey equals p.ProductKey into pg
                        from p in pg.DefaultIfEmpty()
                        select new ProductProfitDto
                        {
                            ProductKey = prod.ProductKey,
                            ProductName = prod.ProductName,
                            TotalSalesAmount = s?.TotalSalesAmount ?? 0M,
                            TotalPurchaseAmount = p?.TotalPurchaseAmount ?? 0M,
                            TotalQtySold = s?.TotalQtySold ?? 0,
                            TotalQtyPurchased = p?.TotalQtyPurchased ?? 0,
                            Profit = (s?.TotalSalesAmount ?? 0M) - (p?.TotalPurchaseAmount ?? 0M)
                        };

            if (!isAll)
            {
                query = query.Where(x => products.FirstOrDefault(p => p.ProductKey == x.ProductKey)!.Category == category);
            }

            if (sort?.ToLower() == "asc")
                query = query.OrderBy(x => x.Profit);
            else
                query = query.OrderByDescending(x => x.Profit);

            if (top > 0)
                query = query.Take(top);

            return query.ToList();
        }

        public async Task<IEnumerable<TimeSeriesPointDto>> GetTimeSeriesAsync(string metric = "sales", string period = "month", int months = 12, string? category = null)
        {
            metric = (metric ?? "sales").ToLowerInvariant();
            period = (period ?? "month").ToLowerInvariant();
            var isSales = metric == "sales";

            DateTime today = DateTime.UtcNow.Date;

            // determine date range
            DateOnly endDateDO = DateOnly.FromDateTime(today);
            DateTime startDate;
            if (period == "year")
            {
                int years = Math.Max(1, months);
                startDate = new DateTime(today.Year - years + 1, 1, 1);
            }
            else if (period == "month")
            {
                startDate = new DateTime(today.Year, today.Month, 1).AddMonths(-months + 1);
            }
            else if (period == "week")
            {
                startDate = today.AddDays(-7 * (months - 1));
            }
            else // day
            {
                startDate = today.AddDays(-months + 1);
            }

            DateOnly startDateDO = DateOnly.FromDateTime(startDate);

            // get matching dates from DimDates within range
            var dateDict = await _context.DimDates
                .AsNoTracking()
                .Where(d => d.FullDate >= startDateDO && d.FullDate <= endDateDO)
                .ToDictionaryAsync(d => d.DateKey);

            if (dateDict.Count == 0)
                return new List<TimeSeriesPointDto>();

            var dateKeys = dateDict.Keys.ToList();

            // load facts for selected metric and date keys, include product for category filtering
            if (isSales)
            {
                var facts = await _context.FactSales
                    .AsNoTracking()
                    .Where(f => f.OrderDateKey != null && dateKeys.Contains(f.OrderDateKey.Value))
                    .Include(f => f.ProductKeyNavigation)
                    .ToListAsync();

                if (!string.IsNullOrEmpty(category) && !category.Equals("all", StringComparison.OrdinalIgnoreCase))
                {
                    facts = facts.Where(f => f.ProductKeyNavigation != null && f.ProductKeyNavigation.Category == category).ToList();
                }

                // group in-memory
                if (period == "year")
                {
                    var grouped = facts
                        .GroupBy(f => dateDict[f.OrderDateKey!.Value].YearNumber)
                        .OrderBy(g => g.Key)
                        .Select(g => new TimeSeriesPointDto
                        {
                            Period = g.Key.ToString(),
                            TotalAmount = g.Sum(x => x.LineTotal ?? 0),
                            TotalQty = g.Sum(x => x.OrderQty ?? 0)
                        })
                        .ToList();

                    return grouped;
                }
                else if (period == "month")
                {
                    var grouped = facts
                        .GroupBy(f => new { dateDict[f.OrderDateKey!.Value].YearNumber, dateDict[f.OrderDateKey!.Value].MonthNumber })
                        .OrderBy(g => g.Key.YearNumber).ThenBy(g => g.Key.MonthNumber)
                        .Select(g => new TimeSeriesPointDto
                        {
                            Period = g.Key.YearNumber + "-" + (g.Key.MonthNumber?.ToString("D2")),
                            TotalAmount = g.Sum(x => x.LineTotal ?? 0),
                            TotalQty = g.Sum(x => x.OrderQty ?? 0)
                        })
                        .ToList();

                    return grouped;
                }
                else if (period == "week")
                {
                    var grouped = facts
                        .Select(f => new { Fact = f, Date = dateDict[f.OrderDateKey!.Value] })
                        .GroupBy(x => new { x.Date.YearNumber, Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(x.Date.FullDate.Value.ToDateTime(new TimeOnly(0,0)), CalendarWeekRule.FirstDay, DayOfWeek.Monday) })
                        .OrderBy(g => g.Key.YearNumber).ThenBy(g => g.Key.Week)
                        .Select(g => new TimeSeriesPointDto
                        {
                            Period = g.Key.YearNumber + "-W" + g.Key.Week,
                            TotalAmount = g.Sum(x => x.Fact.LineTotal ?? 0),
                            TotalQty = g.Sum(x => x.Fact.OrderQty ?? 0)
                        })
                        .ToList();

                    return grouped;
                }
                else // day
                {
                    var grouped = facts
                        .GroupBy(f => new { dateDict[f.OrderDateKey!.Value].YearNumber, dateDict[f.OrderDateKey!.Value].MonthNumber, dateDict[f.OrderDateKey!.Value].DayNumber })
                        .OrderBy(g => g.Key.YearNumber).ThenBy(g => g.Key.MonthNumber).ThenBy(g => g.Key.DayNumber)
                        .Select(g => new TimeSeriesPointDto
                        {
                            Period = g.Key.YearNumber + "-" + (g.Key.MonthNumber?.ToString("D2")) + "-" + (g.Key.DayNumber?.ToString("D2")),
                            TotalAmount = g.Sum(x => x.LineTotal ?? 0),
                            TotalQty = g.Sum(x => x.OrderQty ?? 0)
                        })
                        .ToList();

                    return grouped;
                }
            }
            else // purchasing
            {
                var facts = await _context.FactPurchasings
                    .AsNoTracking()
                    .Where(f => f.OrderDateKey != null && dateKeys.Contains(f.OrderDateKey.Value))
                    .Include(f => f.ProductKeyNavigation)
                    .ToListAsync();

                if (!string.IsNullOrEmpty(category) && !category.Equals("all", StringComparison.OrdinalIgnoreCase))
                {
                    facts = facts.Where(f => f.ProductKeyNavigation != null && f.ProductKeyNavigation.Category == category).ToList();
                }

                if (period == "year")
                {
                    var grouped = facts
                        .GroupBy(f => dateDict[f.OrderDateKey!.Value].YearNumber)
                        .OrderBy(g => g.Key)
                        .Select(g => new TimeSeriesPointDto
                        {
                            Period = g.Key.ToString(),
                            TotalAmount = g.Sum(x => x.LineTotal ?? 0),
                            TotalQty = g.Sum(x => x.OrderQty ?? 0)
                        })
                        .ToList();

                    return grouped;
                }
                else if (period == "month")
                {
                    var grouped = facts
                        .GroupBy(f => new { dateDict[f.OrderDateKey!.Value].YearNumber, dateDict[f.OrderDateKey!.Value].MonthNumber })
                        .OrderBy(g => g.Key.YearNumber).ThenBy(g => g.Key.MonthNumber)
                        .Select(g => new TimeSeriesPointDto
                        {
                            Period = g.Key.YearNumber + "-" + (g.Key.MonthNumber?.ToString("D2")),
                            TotalAmount = g.Sum(x => x.LineTotal ?? 0),
                            TotalQty = g.Sum(x => x.OrderQty ?? 0)
                        })
                        .ToList();

                    return grouped;
                }
                else if (period == "week")
                {
                    var grouped = facts
                        .Select(f => new { Fact = f, Date = dateDict[f.OrderDateKey!.Value] })
                        .GroupBy(x => new { x.Date.YearNumber, Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(x.Date.FullDate.Value.ToDateTime(new TimeOnly(0,0)), CalendarWeekRule.FirstDay, DayOfWeek.Monday) })
                        .OrderBy(g => g.Key.YearNumber).ThenBy(g => g.Key.Week)
                        .Select(g => new TimeSeriesPointDto
                        {
                            Period = g.Key.YearNumber + "-W" + g.Key.Week,
                            TotalAmount = g.Sum(x => x.Fact.LineTotal ?? 0),
                            TotalQty = g.Sum(x => x.Fact.OrderQty ?? 0)
                        })
                        .ToList();

                    return grouped;
                }
                else // day
                {
                    var grouped = facts
                        .GroupBy(f => new { dateDict[f.OrderDateKey!.Value].YearNumber, dateDict[f.OrderDateKey!.Value].MonthNumber, dateDict[f.OrderDateKey!.Value].DayNumber })
                        .OrderBy(g => g.Key.YearNumber).ThenBy(g => g.Key.MonthNumber).ThenBy(g => g.Key.DayNumber)
                        .Select(g => new TimeSeriesPointDto
                        {
                            Period = g.Key.YearNumber + "-" + (g.Key.MonthNumber?.ToString("D2")) + "-" + (g.Key.DayNumber?.ToString("D2")),
                            TotalAmount = g.Sum(x => x.LineTotal ?? 0),
                            TotalQty = g.Sum(x => x.OrderQty ?? 0)
                        })
                        .ToList();

                    return grouped;
                }
            }
        }
    }
}
