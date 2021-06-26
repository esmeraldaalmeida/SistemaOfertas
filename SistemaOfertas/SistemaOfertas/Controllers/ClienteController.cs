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
    public class ClienteController : Controller
    {
        private Context.Context db = new Context.Context();

        // GET: Cliente
        public ActionResult Index()
        {

            return View(db.Cliente.ToList());
        }

        public ActionResult Pesquisar(string searchString)
        {
            List<Cliente> clientes = new List<Cliente>();
            List<Status> status = new List<Status>();
            List<int> idsClientes = new List<int>();

            if (!String.IsNullOrEmpty(searchString))
            {
                status = db.Status.Where(x => x.FinalizaCliente == "não").ToList();
                foreach (var item in status)
                {
                    idsClientes.Add(item.StatusId);
                    //clientes2 = clientes.Where(x => x.StatusId == item.StatusId).ToList();
                }
                clientes = db.Cliente.Where(x => x.Cpf == searchString && idsClientes.Contains(x.StatusId)).ToList();
                if (clientes.Count == 0)
                {
                    clientes = db.Cliente.Where(x => x.Nome.Contains(searchString)).ToList();
                }
            }
            return View(clientes);
        }

        public JsonResult Valida_CPF(string cpf)
        {
            // Obs.: para que funcione é preciso que no seu localhost não tenha 2 ou mais users
            // com um mesmo cpf cadastrados previamente
            Cliente c = db.Cliente.SingleOrDefault(s => s.Cpf == cpf);
            bool retorno = false;

            try
            {
                if (ValidacaoInternaCPF(cpf))
                {
                    /*if (c == default)
                    {
                        retorno = true;
                        return Json(retorno, JsonRequestBehavior.AllowGet);
                    }*/
                    return Json(true, JsonRequestBehavior.AllowGet);

                }
                else
                    return Json(retorno, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Remove caracteres não numéricos
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemovePontos(string text)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[^0-9]");
            string ret = reg.Replace(text, string.Empty);
            return ret;
        }

        /// <summary>
        /// Valida se um cpf é válido
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>

        public static bool ValidacaoInternaCPF(string cpf)
        {
            //Remove formatação do número, ex: "123.456.789-01" vira: "12345678901"
            cpf = RemovePontos(cpf);

            if (cpf.Length > 11)
                return false;

            while (cpf.Length != 11)
                cpf = '0' + cpf;

            bool igual = true;
            for (int i = 1; i < 11 && igual; i++)
                if (cpf[i] != cpf[0])
                    igual = false;

            if (igual || cpf == "12345678909")
                return false;

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(cpf[i].ToString());

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else
                if (numeros[10] != 11 - resultado)
                return false;

            return true;
        }

        // GET: Cliente/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCliente,Nome,Cpf,Telefone,Credito,Cep,Rua,Numero,Complemento,Bairro,Cidade,Estado,StatusId")] Cliente cliente)
        {
            cliente.StatusId = 1;//Setando validação para que o Status seja Nome disponivel (IdStatus = 1) ao ser cadastrado um novo cliente
            Cliente userCpf = db.Cliente.FirstOrDefault(x => x.Cpf == cliente.Cpf);
            if (userCpf != default)
            {
                cliente = userCpf;
                ViewBag.UserCPF = userCpf;
                return View("Create", userCpf);//RedirectToAction("Details", "Cliente", new { id = userCpf.IdUser});
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Cliente.Add(cliente);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(userCpf);
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCliente,Nome,Cpf,Telefone,Credito,Cep,Rua,Numero,Complemento,Bairro,Cidade,Estado,StatusId")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
            db.SaveChanges();
            return RedirectToAction("Index");
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
