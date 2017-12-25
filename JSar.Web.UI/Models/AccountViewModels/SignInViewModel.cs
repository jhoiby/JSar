using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JSar.Web.UI.Models.AccountViewModels
{
    public class SignInViewModel
    {
        [Required]
        [Display(Name ="User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        [StringLength(100, ErrorMessage = "The password must be between 8 and 100 characters.", MinimumLength = 8)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        [DefaultValue(false)]
        public bool RememberMe { get; set; }
    }
}
