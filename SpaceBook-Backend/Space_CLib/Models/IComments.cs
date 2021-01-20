using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Space_CLib.Models
{
    internal interface IComments
    {
        [StringLength(300, ErrorMessage = "The comment must not exceed 300 characters.")]
        [DataType(DataType.Text)]
        [Required]
        string CommentText { get; set; }

    }
}
