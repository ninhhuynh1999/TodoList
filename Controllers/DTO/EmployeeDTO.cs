using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Controllers.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Khong dc de trong email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Khong dc de trong password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public string Password2 { get; set; }
        
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int? IdRole { get; set; }
        public DateTime? DateCreated { get; set; }

        public string LoginErrorMessage { get; set; }
        public string RegisterErrorMessage { get; set; }
    }
}
