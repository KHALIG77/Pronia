using System.ComponentModel.DataAnnotations;
using Pronia.Models;

namespace Pronia.Areas.Manage.ViewModels
{
    public class CommentFormViewModel
    {
        [MaxLength(100)]
        public string ReplyComment {get; set;}
         public int CommentId {get; set;}
         public bool Show { get; set;}
    }
}
