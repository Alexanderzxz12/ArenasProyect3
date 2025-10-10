using ArenasProyect3Web.Clases;
using ArenasProyect3Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArenasProyect3Web.Controllers
{
    public class CuentasController : Controller
    {
        //INICIO DE CARGA DE MI LISTADO DE CUENTAS
        public IActionResult Index()
        {
            List<CuentasCLS> listaCuentas = new List<CuentasCLS>();

            using (BD_VENTAS_2Context db = new BD_VENTAS_2Context())
            {
                listaCuentas = (from cuentas in db.Tipomercaderias
                                select new CuentasCLS
                                {
                                    IdCuenta = cuentas.IdTipoMercaderias,
                                    Abreviatura = cuentas.Abreviatura,
                                    DescripcionCuenta = cuentas.Desciripcion,
                                    CodSunat = cuentas.CodSunet,
                                    Estado = cuentas.Estado
                                }).ToList();

            }

            return View(listaCuentas);
        }

        //FUNCION PARA AGREGAR UNA CUENTA
        public IActionResult Agregar()
        {
            return View();
        }

        //FUNCION OPST PAR ALA CARGA DE DATOS PARA GUARDARLOS
        [HttpPost]
        public IActionResult Agregar(CuentasCLS oCuentaCLS)
        {
            int repetido = 0;

            try
            {
                using (BD_VENTAS_2Context db = new BD_VENTAS_2Context())
                {
                    repetido = db.Tipomercaderias.Where(p => p.Desciripcion.ToUpper().Trim() == oCuentaCLS.DescripcionCuenta.ToUpper().Trim()).Count();


                    if (!ModelState.IsValid || repetido >= 1)
                    {
                        return View(oCuentaCLS);
                    }
                    else
                    {
                        Tipomercaderia oCuenta = new Tipomercaderia();
                        oCuenta.Desciripcion = oCuentaCLS.DescripcionCuenta;
                        oCuenta.Abreviatura = oCuentaCLS.Abreviatura;
                        oCuenta.CodSunet = oCuentaCLS.CodSunat;
                        oCuenta.Estado = 1;
                        db.Tipomercaderias.Add(oCuenta);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                return View(oCuentaCLS);
            }

            return RedirectToAction("Index");
        }

        //FUNCION PARA EDITAR UNA CUENTA
        [HttpPost]
        public IActionResult Editar(int IdCuenta)
        {
            CuentasCLS oCuentasCLS = new CuentasCLS();
            using (BD_VENTAS_2Context db = new BD_VENTAS_2Context())
            {
                oCuentasCLS = (from cuentas in db.Tipomercaderias
                                    where cuentas.IdTipoMercaderias == IdCuenta
                               select new CuentasCLS
                                    {
                                        IdCuenta = cuentas.IdTipoMercaderias,
                                        DescripcionCuenta = cuentas.Desciripcion,
                                        Abreviatura = cuentas.Abreviatura
                                    }).First();

            }
            return View(oCuentasCLS);
        }

        //FUNCION PARA ELIMINAR UNA CUENTA
        [HttpPost]
        public IActionResult Eliminar(int IdCuenta)
        {
            try
            {
                using (BD_VENTAS_2Context db = new BD_VENTAS_2Context())
                {
                    Tipomercaderia oCuentas = db.Tipomercaderias.Where(p => p.IdTipoMercaderias == IdCuenta).First();
                    oCuentas.Estado = 0;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
