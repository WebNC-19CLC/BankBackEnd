using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.Extensions;

namespace AsrTool.Infrastructure.Context.Seeders.Imp
{
  public class EmployeeSeeder : BaseSeeder<Employee>
  {
    private readonly IAsrContext _context;

    protected override IEnumerable<Employee> SeedItems()
    {
      var adminRole = Store.GetFromStore<Role>().RunAwait().Single(x => x.Name == Constants.Roles.Admin);

      yield return new Employee
      {
        Username = $"{Constants.Seeder.Employee.AKO.VISA}",
        FirstName = Constants.Seeder.Employee.AKO.FIRSTNAME,
        LastName = Constants.Seeder.Employee.AKO.LASTNAME,
        Role = adminRole,
        Gender = Gender.Male,
        Visa = Constants.Seeder.Employee.AKO.VISA,
        AdDomain = Constants.Seeder.Employee.AD_DOMAIN,
        OrganizationUnit = Constants.Seeder.Employee.AKO.ORGANIZATION_UNIT,
        HeadUnitVisa = Constants.Seeder.Employee.AKO.HEAD_UNIT_VISA,
        HeadOperationVisa = Constants.Seeder.Employee.AKO.HEAD_OPERATION_VISA,
        Email = Constants.Seeder.Employee.AKO.EMAIL,
        Site = Constants.Seeder.Employee.AKO.SITE,
        BirthDate = Constants.Seeder.Employee.DEFAULT_BIRTH_DATE,
        UniqueId = Constants.Seeder.Employee.AKO.EMPID,
        Active = true,
      };

      yield return new Employee
      {
        Username = $"{Constants.Seeder.Employee.MPT.VISA}",
        FirstName = Constants.Seeder.Employee.MPT.FIRSTNAME,
        LastName = Constants.Seeder.Employee.MPT.LASTNAME,
        Role = adminRole,
        Gender = Gender.Male,
        Visa = Constants.Seeder.Employee.MPT.VISA,
        AdDomain = Constants.Seeder.Employee.AD_DOMAIN,
        OrganizationUnit = Constants.Seeder.Employee.MPT.ORGANIZATION_UNIT,
        HeadUnitVisa = Constants.Seeder.Employee.MPT.HEAD_UNIT_VISA,
        HeadOperationVisa = Constants.Seeder.Employee.MPT.HEAD_OPERATION_VISA,
        Email = Constants.Seeder.Employee.MPT.EMAIL,
        Site = Constants.Seeder.Employee.MPT.SITE,
        BirthDate = Constants.Seeder.Employee.DEFAULT_BIRTH_DATE,
        UniqueId = Constants.Seeder.Employee.MPT.EMPID,
        Active = true,
      };

      yield return new Employee
      {
        Username = $"{Constants.Seeder.Employee.TYI.VISA}",
        FirstName = Constants.Seeder.Employee.TYI.FIRSTNAME,
        LastName = Constants.Seeder.Employee.TYI.LASTNAME,
        Role = adminRole,
        Gender = Gender.Male,
        Visa = Constants.Seeder.Employee.TYI.VISA,
        AdDomain = Constants.Seeder.Employee.AD_DOMAIN,
        OrganizationUnit = Constants.Seeder.Employee.TYI.ORGANIZATION_UNIT,
        HeadUnitVisa = Constants.Seeder.Employee.TYI.HEAD_UNIT_VISA,
        HeadOperationVisa = Constants.Seeder.Employee.TYI.HEAD_OPERATION_VISA,
        Email = Constants.Seeder.Employee.TYI.EMAIL,
        Site = Constants.Seeder.Employee.TYI.SITE,
        BirthDate = Constants.Seeder.Employee.DEFAULT_BIRTH_DATE,
        UniqueId = Constants.Seeder.Employee.TYI.EMPID,
        Active = true,
      };

      yield return new Employee
      {
        Username = $"{Constants.Seeder.Employee.KAL.VISA}",
        FirstName = Constants.Seeder.Employee.KAL.FIRSTNAME,
        LastName = Constants.Seeder.Employee.KAL.LASTNAME,
        Role = adminRole,
        Gender = Gender.Female,
        Visa = Constants.Seeder.Employee.KAL.VISA,
        AdDomain = Constants.Seeder.Employee.AD_DOMAIN,
        OrganizationUnit = Constants.Seeder.Employee.KAL.ORGANIZATION_UNIT,
        HeadUnitVisa = Constants.Seeder.Employee.KAL.HEAD_UNIT_VISA,
        HeadOperationVisa = Constants.Seeder.Employee.KAL.HEAD_OPERATION_VISA,
        Email = Constants.Seeder.Employee.KAL.EMAIL,
        Site = Constants.Seeder.Employee.KAL.SITE,
        BirthDate = Constants.Seeder.Employee.DEFAULT_BIRTH_DATE,
        UniqueId = Constants.Seeder.Employee.KAL.EMPID,
        Active = true,
      };

      yield return new Employee
      {
        Username = $"{Constants.Seeder.Employee.ADT.VISA}",
        FirstName = Constants.Seeder.Employee.ADT.FIRSTNAME,
        LastName = Constants.Seeder.Employee.ADT.LASTNAME,
        Role = adminRole,
        Gender = Gender.Female,
        Visa = Constants.Seeder.Employee.ADT.VISA,
        AdDomain = Constants.Seeder.Employee.AD_DOMAIN,
        OrganizationUnit = Constants.Seeder.Employee.ADT.ORGANIZATION_UNIT,
        HeadUnitVisa = Constants.Seeder.Employee.ADT.HEAD_UNIT_VISA,
        HeadOperationVisa = Constants.Seeder.Employee.ADT.HEAD_OPERATION_VISA,
        Email = Constants.Seeder.Employee.ADT.EMAIL,
        Site = Constants.Seeder.Employee.ADT.SITE,
        BirthDate = Constants.Seeder.Employee.DEFAULT_BIRTH_DATE,
        UniqueId = Constants.Seeder.Employee.ADT.EMPID,
        Active = true,
      };

      yield return new Employee
      {
        Username = $"{Constants.Seeder.Employee.VNH.VISA}",
        FirstName = Constants.Seeder.Employee.VNH.FIRSTNAME,
        LastName = Constants.Seeder.Employee.VNH.LASTNAME,
        Role = adminRole,
        Gender = Gender.Male,
        Visa = Constants.Seeder.Employee.VNH.VISA,
        AdDomain = Constants.Seeder.Employee.AD_DOMAIN,
        OrganizationUnit = Constants.Seeder.Employee.VNH.ORGANIZATION_UNIT,
        HeadUnitVisa = Constants.Seeder.Employee.VNH.HEAD_UNIT_VISA,
        HeadOperationVisa = Constants.Seeder.Employee.VNH.HEAD_OPERATION_VISA,
        Email = Constants.Seeder.Employee.VNH.EMAIL,
        Site = Constants.Seeder.Employee.VNH.SITE,
        BirthDate = Constants.Seeder.Employee.DEFAULT_BIRTH_DATE,
        UniqueId = Constants.Seeder.Employee.VNH.EMPID,
        Active = true
      };
    }

    protected override async Task AddToContext(ICollection<Employee> items)
    {
      await _context.AddRangeAsync(items);
    }

    public EmployeeSeeder(IStore store, IAsrContext context) : base(store)
    {
      _context = context;
    }
  }
}