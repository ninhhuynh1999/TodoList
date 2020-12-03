using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Controllers.DTO
{
    public partial class WorkDTO
    {
        public int Id { get; set; }
        public int? IdWorkList { get; set; }
        public int? IdWorkStatus { get; set; }

        [Required(ErrorMessage = "Không được để trống Content")]
        public string WorkContent { get; set; }

        [Required(ErrorMessage = "Không được để trống Name")]
        public string WorkName { get; set; }
        public DateTime? DateCreated { get; set; }

        [Required(ErrorMessage = "Không được để trống StartDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Không được để trống Enddate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        public int? IdEmployee { get; set; }
        public string AddWorkErrorMessage { get; set; }
    }
}
