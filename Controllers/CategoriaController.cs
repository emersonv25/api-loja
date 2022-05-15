﻿using api_produtos.Data;
using api_produtos.Models;
using api_produtos.Models.Object;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_produtos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CategoriaController(AppDbContext context)
        {
            _db = context;
        }

        // GET api/<ProdutoController>/
        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {
            return _db.Categoria.Where(x => x.idPai == null).Include(x => x.SubCategorias).ToList();
        }

        // GET api/<ProdutoController>/5
        [HttpGet("{id:int}")]
        public ActionResult<dynamic> GetById(int id)
        {
            var categoria = _db.Categoria.Where(x => x.idCategoria == id).Include(x => x.SubCategorias).ToList();
            if (categoria == null)
            {
                return NotFound(new { error = "Categoria não encontrada" });
            }
            return categoria;
        }
        // GET api/<ProdutoController>/Hardware
        [HttpGet("{nome}")]
        public ActionResult<IEnumerable<Categoria>> GetByName(string nome)
        {
            return _db.Categoria.Where(i => i.Nome.Contains(nome)).Include(x => x.SubCategorias).ToList();
        }
        // POST api/<CategoriaController>
        [HttpPost]
        public async Task<ActionResult<dynamic>> Post([FromBody] ParamCategoria param)
        {
            try
            {
                Categoria categoria = new Categoria { Nome = param.Nome, idPai = param.idPai != 0 ? param.idPai : null };
                _db.Categoria.Add(categoria);
                await _db.SaveChangesAsync();
                return Ok(categoria);
            }
            catch(Exception ex)
            {
                return BadRequest(new { error = "Não foi possivel cadastrar a categoria: " + ex.Message });
            }

        }

        // PUT api/<CategoriaController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<dynamic>> Put(int id, [FromBody] ParamCategoria param)
        {
            try
            {
                Categoria categoria = await _db.Categoria.FindAsync(id);
                if (categoria == null)
                {
                    return BadRequest(new { error = "Categoria não encontrada" });
                }
                categoria.Nome = param.Nome;
                if(param.idPai != 0)
                    categoria.idPai = param.idPai;
                await _db.SaveChangesAsync();
                return Ok(categoria);
            }
            catch(Exception ex)
            {
                return BadRequest(new { error = "Não foi possivel editar a categoria: " + ex.Message });
            }
        }

        // DELETE api/<CategoriaController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<dynamic>> Delete(int id)
        {
            try
            {
                Categoria categoria = await _db.Categoria.FindAsync(id);
                if(categoria == null)
                {
                    return NotFound(new { error = "Categoria não encontrada" });
                }
                _db.Categoria.Remove(categoria);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(new { error = "Não foi possivel deletar a categoria: " + ex.Message });
            }

        }
    }
}
