﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Myapp.Models;

namespace Myapp.Controllers
{
    public class TodoListController : ApiController
    {
        private TodoListContext db = new TodoListContext();

        // GET api/TodoList
        public IEnumerable<TodoList> GetTodoItems(string q = null, string sort = null, bool desc = false,
                                                        int? limit = null, int offset = 0)
        {
            var list = ((IObjectContextAdapter)db).ObjectContext.CreateObjectSet<TodoList>();

            IQueryable<TodoList> items = string.IsNullOrEmpty(sort) ? list.OrderBy(o => o.Priority)
                : list.OrderBy(String.Format("it.{0} {1}", sort, desc ? "DESC" : "ASC"));

            if (!string.IsNullOrEmpty(q) && q != "undefined") items = items.Where(t => t.Text.Contains(q));

            if (offset > 0) items = items.Skip(offset);
            //limit=10;
            if (limit.HasValue) items = items.Take(limit.Value);
            return items;
        }

        // GET api/TodoList/5
        public TodoList GetTodoList(int id)
        {
            TodoList todolist = db.TodoLists.Find(id);
            if (todolist == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return todolist;
        }

        // PUT api/TodoList/5
        public HttpResponseMessage PutTodoList(int id, TodoList todolist)
        {
            if (ModelState.IsValid && id == todolist.Id)
            {
                db.Entry(todolist).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/TodoList
        public HttpResponseMessage PostTodoList(TodoList todolist)
        {
            if (ModelState.IsValid)
            {
                db.TodoLists.Add(todolist);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, todolist);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = todolist.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/TodoList/5
        public HttpResponseMessage DeleteTodoList(int id)
        {
            TodoList todolist = db.TodoLists.Find(id);
            if (todolist == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.TodoLists.Remove(todolist);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, todolist);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}