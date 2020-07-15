﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Data.DAL;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class MateriaController : Controller
    {
        private readonly BlogContext _context;
        private readonly MateriaDAL materiaDAL;
        

        public MateriaController(BlogContext context)
        {
            this._context = context;
            materiaDAL = new MateriaDAL(context);
          
        }
              

        public async Task<IActionResult> Index()
        {
            
            return View(await materiaDAL.ObterMateriaClassificadaPorId().ToListAsync());
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Titulo, Texto")] Materia materia)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await materiaDAL.GravarMateria(materia);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(materia);
        }


        public async Task<IActionResult> MateriaCompleta(long? id)
        {
            return await ObterMateriaVisaoPorId(id);
        }

        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            return await ObterMateriaVisaoPorId(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(long? id, [Bind("MateriaID, Titulo, Texto")]Materia materia)
        {
            if(id != materia.MateriaID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await materiaDAL.GravarMateria(materia);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await MateriaExists(materia.MateriaID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(materia);
        }

        [Authorize]
        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterMateriaVisaoPorId(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var materia = await materiaDAL.EliminarMateriaPorId((long)id);
            TempData["Message"] = "Materia " + materia.Titulo.ToUpper() + " Foi Removida";
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MateriaExists(long? id)
        {
            return await materiaDAL.ObterMateriaPorId((long)id) != null;

        }

        public async Task<IActionResult> ObterMateriaVisaoPorId(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await materiaDAL.ObterMateriaPorId((long) id);

            if (materia == null)
            {
                return NotFound();
            }
            return View(materia);
        }
    }
}