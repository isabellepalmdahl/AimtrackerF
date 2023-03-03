using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Models.Dtos
{
    public class SeriesDto
    {
        public string Stance { get; set; }
        public DateTime DateTime { get; set; }
        public ICollection<ShotsDto> Shots { get; set; }
    }
}
  