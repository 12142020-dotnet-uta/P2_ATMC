using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SpaceBook.Models.Models
{
        public class RegisterModel
        {

            [Required(ErrorMessage = "First Name is required")]
            [StringLength(20, ErrorMessage = "The first name must be from 3 to 20 characters.", MinimumLength = 3)]
            [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last Name is required")]
            [StringLength(20, ErrorMessage = "The last name must be from 3 to 20 characters.", MinimumLength = 3)]
            [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }


            [Required(ErrorMessage = "User Name is required")]
            public string Username { get; set; }

            [EmailAddress]
            [Required(ErrorMessage = "Email is required")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; }

        }
}

