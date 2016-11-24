﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wodsoft.ComBoost.Mvc;
using Wodsoft.ComBoost.Forum.Domain;
using Wodsoft.ComBoost.Security;
using Wodsoft.ComBoost.Data.Entity;
using Wodsoft.ComBoost.Forum.Core;
using Wodsoft.ComBoost.Data;
using System.ComponentModel;
using Wodsoft.ComBoost.Forum.Entity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Wodsoft.ComBoost.Forum.Controllers
{
    public class ThreadController : DomainController
    {
        public async Task<IActionResult> Index()
        {
            var context = CreateDomainContext();
            var threadDomain = DomainProvider.GetService<EntityDomainService<Thread>>();
            IEntityEditModel<Thread> threadResult;
            try
            {
                threadResult = await threadDomain.ExecuteAsync<IEntityEditModel<Thread>>(context, "Detail");
                ViewBag.Thread = threadResult.Item;
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound();
            }
            var postDomain = DomainProvider.GetService<EntityDomainService<Post>>();
            IEntityViewModel<Post> postResult = await postDomain.ExecuteAsync<IEntityViewModel<Post>>(context, "List");
            return View(postResult);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var context = CreateDomainContext();
            var domain = DomainProvider.GetService<ThreadDomainService<Thread>>();
            try
            {
                var thread = await domain.ExecuteAsync<Thread>(context, "Create");
                return RedirectToAction("Index", new { id = thread.Index });
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
