using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaOfertas.Context;
using SistemaOfertas.Models;

namespace SistemaOfertas.Controllers
{
    public class OfertaController : Controller
    {
        private Context.Context db = new Context.Context();

        public static List<int> listaIDsProdutosSelecionados { get; set; }
        public static Cliente clienteOfertado { get; set; }
        // GET: Oferta
        public ActionResult OfertarCliente(int idCliente)
        {
            Oferta oferta = new Oferta();
            //inicializando a lista completa de prod na oferta
            oferta.listaProdutos = db.Produto.ToList();

            if (idCliente == -1) //Controle interno para exibição dos botões de adicionar ou remover na lista
            {
                //Fazendo um foreach para controlar os botões que serão exibidos na view, dependendo dos produtos que já foram slecionados
                foreach (int idProd in listaIDsProdutosSelecionados)
                {
                    Produto produto = oferta.listaProdutos.Where(x => x.IdProduto == idProd).FirstOrDefault();
                    produto.AddListaControle = true;
                    oferta.ValorOfertaFinal += produto.Preco;//Adicionando o valor do profuto ao Valor Final da oferta para cada prod selecionado
                }
            }
            else
            {
                oferta.IdCliente = idCliente;
                clienteOfertado = db.Cliente.Find(idCliente);

                //Inicializando ListaIDs
                listaIDsProdutosSelecionados = new List<int>();
            }

            //Preenchendo Viewbags para exibir informação na tela
            ViewBag.idC = clienteOfertado.IdCliente;
            ViewBag.NomeC = clienteOfertado.Nome;
            ViewBag.CPFC = clienteOfertado.Cpf;
            ViewBag.CreditoC = clienteOfertado.Credito;
            ViewBag.TelefoneC = clienteOfertado.Telefone;
            ViewBag.StatusC = clienteOfertado.StatusId;

            return View(oferta);
        }

        //Aqui é o metodo que cria a oferta
        [HttpPost]//Metodo quando clica no botao Create na tela de OfertarCliente
        public ActionResult OfertarCliente([Bind(Include = "IdOferta,IdCliente,ValorOfertaFinal")] Oferta oferta)
        {
            //Validação para se algum produto tipo Hardware, então endereço é obrigatorio
            bool tipoHardware = false;
            foreach (int idProd in listaIDsProdutosSelecionados)
            {
                Produto produto = db.Produto.Where(x => x.IdProduto == idProd).FirstOrDefault();
                if (produto.Tipo == "HARDWARE")
                {
                    tipoHardware = true;
                    break;
                }
            }
            if (tipoHardware && String.IsNullOrEmpty(clienteOfertado.Rua) && String.IsNullOrEmpty(clienteOfertado.Numero) && String.IsNullOrEmpty(clienteOfertado.Bairro) && String.IsNullOrEmpty(clienteOfertado.Complemento) && String.IsNullOrEmpty(clienteOfertado.Cidade) && String.IsNullOrEmpty(clienteOfertado.Estado) && String.IsNullOrEmpty(clienteOfertado.Cep))
            {
                ViewBag.Msg = "Produto tipo hardware, endereço obrigado!";
                List<Produto> produtolist = db.Produto.ToList();
                oferta.listaProdutos = produtolist;
                return View(oferta);
            }
            //Validação para soma dos produtos nao ser maior que o valor total dos prod selecionados
            if (oferta.ValorOfertaFinal > Convert.ToDecimal(clienteOfertado.Credito))
            {
                ViewBag.Msg = "O Cliente não tem a quantidade de créditos suficiente.";
                oferta.listaProdutos = db.Produto.ToList();
                return View(oferta);
            }
            if (ModelState.IsValid)
            {
                //Adicionando oferta no db
                db.Oferta.Add(oferta);
                db.SaveChanges();
                //Removendo do banco todos os produtos que foram ofertados para o Cliente
                foreach (int idProd in listaIDsProdutosSelecionados)
                {
                    Produto produto = oferta.listaProdutos.FirstOrDefault(x => x.IdProduto == idProd); 
                    db.Produto.Remove(produto);
                    db.SaveChanges();
                }
                //Alterar Status para "Cliente aceitou oferta" / ID = 2
                Cliente clienteAceitouOferta = db.Cliente.Find(clienteOfertado.IdCliente);
                clienteAceitouOferta.StatusId = 2;
                db.Entry(clienteAceitouOferta).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(oferta);
        }

        public ActionResult AddProdutoLista(int idProduto)
        {
            listaIDsProdutosSelecionados.Add(idProduto);
            return RedirectToAction("OfertarCliente", new { idCliente = -1 });
        }

        public ActionResult RemProdutoLista(int idProduto)
        {
            listaIDsProdutosSelecionados.Remove(idProduto);
            return RedirectToAction("OfertarCliente", new { idCliente = -1 });

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
