using LearnToDo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;
namespace LearnToDo.Controllers
{
    public class TasksAnonController : ApiController
    {        
            private ApplicationDbContext db = new ApplicationDbContext();

            /// <summary>
            /// Get all tasks.
            /// </summary>
            /// <returns>All tasks.</returns>
            public async Task<IEnumerable<Todo>> GetTasks()
            {
                var userId = GetUserId();
                var model = await db.Todos.Where(item => item.UserId == userId).ToListAsync();
                return model;
            }

            /// <summary>
            /// Create a new task.
            /// </summary>
            /// <returns>Added task and status code.</returns>
            public async Task<IHttpActionResult> PostAddTask(Todo task)
            {
                var userId = GetUserId();
                task.UserId = userId;

                db.Todos.Add(task);
                await db.SaveChangesAsync();
                return Ok(task);
            }

            /// <summary>
            /// Update task(update property IsDone).
            /// </summary>
            /// <returns>Updated task and status code.</returns>
            public async Task<IHttpActionResult> PutUpdateTask(Todo task)
            {
                var userId = GetUserId();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Todo old = await db.Todos.FindAsync(task.Id);
                if (old == null)
                {
                    return NotFound();
                }
                if (old.UserId != userId)
                {
                    var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Oops!!! You are not authorized to do this action!" };
                    throw new HttpResponseException(msg);
                }

                old.Title = task.Title;
                old.IsDone = task.IsDone;
                await db.SaveChangesAsync();

                var model = db.Todos.FindAsync(task.Id);
                return Ok(model);
            }

            /// <summary>
            /// Delete task.
            /// </summary>
            /// <returns>All tasks and status code.</returns>
            public async Task<IHttpActionResult> DeleteTask(int id)
            {
                if (id <= 0)
                {
                    return BadRequest("The incorrect id of removing item");
                }

                var userId = GetUserId();
                var task = await db.Todos.FindAsync(id);
                if (task != null && task.UserId != userId)
                {
                    var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Oops!!! You are not authorized to do this action!" };
                    throw new HttpResponseException(msg);
                }
                if (task == null)
                {
                    return NotFound();
                }

                db.Todos.Remove(task);
                await db.SaveChangesAsync();

                var tasks = await db.Todos.ToListAsync();
                return Ok(tasks);
            }


            /// <summary>
            /// Delete completed tasks.
            /// </summary>
            /// <returns>All tasks and status code.</returns>
            [System.Web.Http.Route("api/tasks/deletedone")]
            public async Task<IHttpActionResult> DeleteDoneTasks()
            {
                var userId = GetUserId();
                var doneTasks = db.Todos.Where(item => item.IsDone == true).ToList();

                if (doneTasks.First().UserId != userId)
                {
                    var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Oops!!! You are not authorized to do this action!" };
                    throw new HttpResponseException(msg);
                }

                if (doneTasks.FirstOrDefault() == null)
                {
                    return NotFound();
                }

                foreach (var task in doneTasks)
                {
                    var item = await db.Todos.FindAsync(task.Id);
                    if (item != null)
                    {
                        db.Todos.Remove(item);
                        await db.SaveChangesAsync();
                    }
                }

                var tasks = await db.Todos.ToListAsync();
                return Ok(tasks);
            }

            /// <summary>
            /// Delete all (completed and non-completed) tasks.
            /// </summary>
            /// <returns>Status Code</returns>
            [System.Web.Http.Route("api/tasks/deleteall")]
            public async Task<IHttpActionResult> DeleteAllTasks()
            {
                var userId = GetUserId();

                var tasks = await db.Todos.ToListAsync();

                if (tasks.First().UserId != userId)
                {
                    var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Oops!!! You are not authorized to do this action!" };
                    throw new HttpResponseException(msg);
                }

                if (tasks.FirstOrDefault() == null)
                {
                    return NotFound();
                }

                foreach (var task in tasks)
                {
                    db.Todos.Remove(task);
                    await db.SaveChangesAsync();
                }

                return Ok();
            }


            #region private

            private string GetUserId()
            {
                //var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                //var test = RequestContext.Principal.Identity.GetUserId();
                return "2fe826a0-7667-466a-ba89-042b251b8ca4";
            }
            #endregion private
        }
    
}
