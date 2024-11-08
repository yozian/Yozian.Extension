using System.ComponentModel.DataAnnotations;

namespace Yozian.Extension.Test.Data.Entities;

public class Book
{
    [Key]
    public int Id { get; set; }


    public string Name { get; set; }
}
