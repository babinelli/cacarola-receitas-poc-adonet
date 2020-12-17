using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Department
    {

        #region Scalar Properties
        [Key]
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Limite de 50 caracteres")]
        [MaxLength(50)]
        public string DepartmentName { get; set; }
        #endregion

        #region Navigation Properties
        // 1 departamento tem mais de um usuário
        public virtual List<User> User { get; set; }
        #endregion


    }
}
