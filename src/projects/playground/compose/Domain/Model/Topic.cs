using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Model
{
  public  class Topic
    {
        [Key][MaxLength(128)]
        public string Id { get; set; }
    }
}
