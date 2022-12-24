namespace AsrTool.Infrastructure.Domain
{
  [AttributeUsage(AttributeTargets.Property)]
  public class PlaceHolderAttribute : Attribute
  {
    public PlaceHolderAttribute(string name, string description, string replacementTemplate = null)
    {
      Description = description;
      ReplacementTemplate = replacementTemplate;
      Name = name;
    }

    public string Description { get; set; }

    public string ReplacementTemplate { get; set; }

    public string Name { get; set; }
  }
}
