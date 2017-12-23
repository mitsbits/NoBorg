using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Model
{
    public interface ITopic
    {
        string HashTag { get; set; }
        Guid CreateCommandId { get; set; }
        string Description { get; set; }
        DateTimeOffset CreatedOn { get; set; }
        string UserName { get; set; }
    }

    public  class Topic : ITopic
    {
        [Required]
        public string HashTag { get; set; }
        [Required]
        public Guid CreateCommandId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        [Required]
        [MaxLength(128)]
        public string UserName { get; set; }
    }
}
