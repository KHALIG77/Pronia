﻿using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class ProfileEditViewModel
    {
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(20)]
        public string FullName { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [MaxLength(20)]
        [DataType(DataType.Password)]


        public string NewPassword { get; set; }
        [DataType(DataType.Password)]

        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}
