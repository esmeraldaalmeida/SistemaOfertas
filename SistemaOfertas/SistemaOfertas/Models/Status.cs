using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaOfertas.Models
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }

        [Display(Name = "Descrição")]
        public string DescricaoStatus { get; set; }

        [Display(Name = "Finaliza Cliente")]
        public string FinalizaCliente { get; set; }

        [Display(Name = "Contabiliza Venda")]
        public string ContabCliente { get; set; }
    }
}