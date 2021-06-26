using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaOfertas.Models
{
    public class Oferta
    {
        [Key]
        public int IdOferta { get; set; }
        public int IdCliente { get; set; }

        [Display(Name = "Valor Final")]
        public decimal ValorOfertaFinal { get; set; }

        public virtual List<Produto> listaProdutos { get; set; }
    }
}