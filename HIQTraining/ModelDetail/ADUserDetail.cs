using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HIQTraining.ModelDetail
{
    public class ADUserDetail
    {
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string CompanyName { get; set; }
    }
}
