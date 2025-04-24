using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models.DTOs
{
    public class TodoDto
    {

        public DateTime ExpiryDateTime { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Range(0, 100)]
        public int PercentComplete { get; set; }
    }
}