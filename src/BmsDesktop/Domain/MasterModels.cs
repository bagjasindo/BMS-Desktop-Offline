namespace BmsDesktop.Domain;

public sealed record CompanyProfile(string LegalName,string TradeName,string Address,string Phone,string Email,string TaxId);
public sealed record Barn(Guid Id,string Code,string Name,int Capacity,bool IsActive);
public sealed record BusinessPartner(Guid Id,string PartnerType,string Code,string Name,string Phone,string Address,bool IsActive);
