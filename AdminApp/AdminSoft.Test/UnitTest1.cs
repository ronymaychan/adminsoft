using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plenumsoft.Data;
using Plenumsoft.Domain.Entities.Employees;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Plenumsoft.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Insert()
        {
            using (var context = new PlenumsoftContext())
            {
                Funcion funcion1 = new Funcion() { FuncionId = 1 };
                Funcion funcion2 = new Funcion() { FuncionId = 2 };
                Funcion funcion3 = new Funcion() { FuncionId = 3 };

                //context.Funciones.Attach(funcion1);
                //context.Funciones.Attach(funcion2);
                //context.Funciones.Attach(funcion3);


                Institucion institucion = new Institucion() { Nombre = "Insttitucion 1" };
                institucion.Funciones = new List<InstituccionFuncion>();
                institucion.Funciones.Add(new InstituccionFuncion() { Funcion =  funcion1 });
                institucion.Funciones.Add(new InstituccionFuncion() { Funcion = funcion2 });
                institucion.Funciones.Add(new InstituccionFuncion() { Funcion = funcion3 });

                context.Instituciones.Add(institucion);
                context.SaveChanges();

                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void Select()
        {
            using (var context = new PlenumsoftContext())
            {
                Institucion institucion = context.Instituciones.Include(x => x.Funciones).ToList().First();

                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void Delete()
        {
            using (var context = new PlenumsoftContext())
            {

                Institucion institucion = context.Instituciones.Include(x => x.Funciones).ToList().First();
                
                context.Instituciones.Remove(institucion);
                context.SaveChanges();

                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void Update()
        {
            using (var context = new PlenumsoftContext())
            {

                Institucion institucion = context.Instituciones.Include(x => x.Funciones).ToList().First();

                Funcion funcion1 = new Funcion() { FuncionId = 5 };

                context.Funciones.Attach(funcion1);
                institucion.Funciones = new List<InstituccionFuncion>();
                institucion.Funciones.Add(new InstituccionFuncion() { Funcion = funcion1 });

                context.SaveChanges();

                Assert.IsTrue(true);
            }
        }
    }
}