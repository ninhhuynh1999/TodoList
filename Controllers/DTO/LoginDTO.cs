using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoList.Controllers.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Khong dc de trong email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Khong dc de trong password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string LoginErrorMessage { get; set; }

    }
}