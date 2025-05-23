﻿using AutoNest.Data.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace AutoNest.Data.Entities
{
    public class Contact : BaseDeletableModel<string>
    {
        public Contact()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
