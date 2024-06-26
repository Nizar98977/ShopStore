﻿using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string PictureUrl { get; set; }

        public string ProductType { get; set; }

        public string ProductBrand { get; set; }

    }
}
