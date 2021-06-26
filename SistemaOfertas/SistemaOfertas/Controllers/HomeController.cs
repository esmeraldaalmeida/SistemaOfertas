using SistemaOfertas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaOfertas.Controllers
{
    public class HomeController : Controller
    {

        private Context.Context db = new Context.Context();

        public ActionResult Index()
        {
            if (db.Produto.ToList().Count() == 0)
            {
                db.Produto.Add(new Produto()
                {
                    DescricaoProduto = "Gabinete Gamer, Aerocool",
                    Preco = 350,
                    Tipo = "HARDWARE"
                });
                db.Produto.Add(new Produto()
                {
                    DescricaoProduto = "Cooler",
                    Preco = 26,
                    Tipo = "HARDWARE"
                });
                db.Produto.Add(new Produto()
                {
                    DescricaoProduto = "Memória Ram 8gb",
                    Preco = 285,
                    Tipo = "HARDWARE"
                }); db.Produto.Add(new Produto()
                {
                    DescricaoProduto = "Office Home & Student 2019",
                    Preco = 499,
                    Tipo = "SOFTWARE"
                });
                db.SaveChanges();
            }
            if (db.Status.ToList().Count() == 0)
            {
                db.Status.Add(new Status()
                {
                    DescricaoStatus = "Nome Disponível",
                    FinalizaCliente = "Não",
                    ContabCliente = "Não"
                });
                db.Status.Add(new Status()
                {
                    DescricaoStatus = "Cliente Aceitou Oferta",
                    FinalizaCliente = "Sim",
                    ContabCliente = "Sim"
                });
                db.SaveChanges();
            }
            if (db.Cliente.ToList().Count() == 0)
            {
                db.Cliente.Add(new Cliente()
                {
                    Nome = "Gabriel",
                    Cpf = "45284668057",
                    Telefone = "996335052",
                    Credito = "2000",
                    Cep = "49400000",
                    Rua = "2",
                    Numero = "55",
                    Complemento = "Casa",
                    Bairro = "Centro",
                    Cidade = "Itabaiana",
                    Estado = "SE",
                    StatusId = 0
                });
                db.Cliente.Add(new Cliente()
                {
                    Nome = "Ravi",
                    Cpf = "90194388085",
                    Telefone = "999995052",
                    Credito = "2500",
                    Cep = "49400000",
                    Rua = "3",
                    Numero = "56",
                    Complemento = "Casa",
                    Bairro = "Centro",
                    Cidade = "São Cristóvão",
                    Estado = "SE",
                    StatusId = 1
                });
                db.SaveChanges();
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}