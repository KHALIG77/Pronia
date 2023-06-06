﻿using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100)]
        public string Email { get; set; }   
    }
}
