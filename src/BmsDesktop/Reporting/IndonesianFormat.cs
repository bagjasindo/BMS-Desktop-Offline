using System.Globalization;
namespace BmsDesktop.Reporting;
public static class IndonesianFormat { private static readonly CultureInfo Id=CultureInfo.GetCultureInfo("id-ID"); public static string Money(decimal value)=>value.ToString("N2",Id); public static string Rupiah(decimal value)=>"Rp "+value.ToString("N2",Id); public static string Date(DateOnly value)=>value.ToString("dd/MM/yyyy",Id); public static string Number(decimal value,int decimals=2)=>value.ToString("N"+decimals,Id); }
