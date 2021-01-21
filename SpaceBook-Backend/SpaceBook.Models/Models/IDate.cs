using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SpaceBook.Models
{
    internal interface IDate
    {
        [Required]
        [DataType(DataType.Date)]
        DateTime Date { get; set; }

    }
}
