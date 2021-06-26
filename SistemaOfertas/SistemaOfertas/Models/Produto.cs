using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaOfertas.Models
{
    public class Produto
    {
        [Key]
        public int IdProduto { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
        [Display(Name = "Descrição")]
        public string DescricaoProduto { get; set; }

        [Required(ErrorMessage = "O campo Preço é obrigatório.")]
        [Display(Name = "Preço")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O campo Tipo é obrigatório.")]
        public string Tipo { get; set; }

        public virtual bool AddListaControle { get; set; }//Controle para selecionar/deselecionar os prod
    }
}