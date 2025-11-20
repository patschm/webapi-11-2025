using System.ComponentModel.DataAnnotations;

public class Person
{
    public int ID { get; set; }
    [Required]
    public string?  Name { get; set; }
}