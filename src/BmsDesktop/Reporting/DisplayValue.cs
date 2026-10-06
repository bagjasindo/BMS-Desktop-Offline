namespace BmsDesktop.Reporting;
public static class DisplayValue { public static string Money(decimal? value)=>value.HasValue?IndonesianFormat.Rupiah(value.Value):"-";public static string Number(decimal? value,int decimals=2)=>value.HasValue?IndonesianFormat.Number(value.Value,decimals):"-";public static string Text(string? value)=>string.IsNullOrWhiteSpace(value)?"-":value; }
