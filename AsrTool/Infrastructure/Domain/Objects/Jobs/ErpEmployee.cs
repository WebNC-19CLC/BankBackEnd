namespace AsrTool.Infrastructure.Domain.Objects.Jobs
{
  public class ErpEmployee
  {
    public int Emp_Unique_Id { get; set; } // We don't mark this field as nullable
                                           // because we only import records having this field not-null from ERP
    public int Emp_Id { get; set; }

    public int? Emp_Nb { get; set; }

    public string? Emp_Visa { get; set; }

    public string? Emp_FirstName { get; set; }

    public string? Emp_LastName { get; set; }

    public string Emp_Gender { get; set; } = default!;

    public string? Emp_ADDomain { get; set; }

    public int? Emp_Level { get; set; }

    public int? Emp_Manager_Id { get; set; }

    public int? Emp_Manager_Nb { get; set; }

    public string? Emp_Manager_Visa { get; set; }

    public string? Emp_Legal_Unit { get; set; }

    public string? Emp_Org_Unit { get; set; }

    public string? Emp_Head_Unit_Visa { get; set; }

    public string? Emp_Head_Operation_Visa { get; set; }

    public string? Emp_Role { get; set; }

    public DateTime? Emp_Arrival_Date { get; set; }

    public DateTime? Emp_Departure_Date { get; set; }

    public bool Emp_Is_Active { get; set; } // We don't mark this field as nullable,
                                            // we will treat `null` (value from ERP) as inactive

    public DateTime? Emp_Birthday_Date { get; set; }

    public string? Emp_Department { get; set; }

    public string? Acc_Email { get; set; }

    public DateTime? Acc_Created_On { get; set; }

    public string? Acc_Mobile_Phone { get; set; }

    public string? Emp_Site { get; set; }

    public int? Tenure_Month { get; set; }
  }
}