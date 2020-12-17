using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class User
    {

        #region Scalar properties
        // Chave Primária
        [Key]
        public int UserID { get; set; }

        // Foreign key
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Limite de 40 caracteres")]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Limite de 40 caracteres")]
        [MaxLength(40)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Limite de 100 caracteres")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Limite de 50 caracteres")]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Limite de 50 caracteres")]
        [MaxLength(50)]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        public bool Admin { get; set; }

        #endregion

        #region Navigation properties
        // 1 usuário pertence a 1 departamento
        public virtual Department Department { get; set; }
        #endregion

    }
}
