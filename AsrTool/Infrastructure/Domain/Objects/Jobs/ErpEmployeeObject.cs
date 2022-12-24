using AsrTool.Infrastructure.Jobs;

namespace AsrTool.Infrastructure.Domain.Objects.Jobs
{
  public class ErpEmployeeObject
  {
    public string? Emp_Unique_Id { get; set; }

    public string Emp_Id { get; set; } = default!;

    public string? Emp_Nb { get; set; }

    public string? Emp_Visa { get; set; }

    public string? Emp_FirstName { get; set; }

    public string? Emp_LastName { get; set; }

    public string Emp_Gender { get; set; } = default!;

    public string? Emp_ADDomain { get; set; }

    public string? Emp_Level { get; set; }

    public string? Emp_Manager_Id { get; set; }

    public string? Emp_Manager_Nb { get; set; }

    public string? Emp_Manager_Visa { get; set; }

    public string? Emp_Legal_Unit { get; set; }

    public string? Emp_Org_Unit { get; set; }

    public string? Emp_Head_Unit_Visa { get; set; }

    public string? Emp_Head_Operation_Visa { get; set; }

    public string? Emp_Role { get; set; }

    [DateTimeData]
    public string? Emp_Arrival_Date { get; set; }

    [DateTimeData]
    public string? Emp_Departure_Date { get; set; }

    public string? Emp_Is_Active { get; set; }

    [DateTimeData]
    public string? Emp_Birthday_Date { get; set; }

    public string? Emp_Department { get; set; }

    public string? Acc_Email { get; set; }

    [DateTimeData]
    public string? Acc_Created_On { get; set; }

    public string? Emp_Site { get; set; }

    public string? Tenure_Month { get; set; }
  }
}