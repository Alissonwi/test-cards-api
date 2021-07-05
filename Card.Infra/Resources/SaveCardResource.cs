﻿using System.ComponentModel.DataAnnotations;

namespace Cards.Infra.Resources
{
    public class SaveCardResource
    {
        [Required]
        public int costumerId { get; set; }

        [Required]
        [MaxLength(16)]
        [MinLength(4)]
        public string cardNumber { get; set; }

        [Required]
        [MaxLength(5)]
        public string CVV { get; set; }
    }
}