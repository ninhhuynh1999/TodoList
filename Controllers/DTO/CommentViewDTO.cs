using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ToDoList.Models;

namespace ToDoList.Controllers.DTO
{
    public class CommentViewDTO
    {
        public int Id { get; set; }
        public int? IdEmployee { get; set; }
        public string NameEmployee { get; set; }
        public int? IdWork { get; set; }
        [Required]
        public string CommentContent { get; set; }
        public DateTime? DateCreated { get; set; }

        public CommentViewDTO(Comment comment,string tenNhanvien)
        {
            Id = comment.Id;
            IdEmployee = comment.IdEmployee;
            IdWork = comment.IdWork;
            CommentContent = comment.CommentContent;
            DateCreated = comment.DateCreated;
            NameEmployee = tenNhanvien;
        }
        public CommentViewDTO() { }
    }
}