using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Manage.ViewModels;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="SuperAdmin,Admin")]

    public class CommentController : Controller
    {
        private readonly ProniaContext _context;

        public CommentController(ProniaContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page=1,string search=null)
        {
            var query = _context.PlantComments.Include(x => x.Plant).Include(x => x.AppUser).AsQueryable();

            if (search != null)
            {
                query = query.Where(x => x.Plant.Name.Contains(search)).AsQueryable();
            }

            return View(PaginatedList<PlantComment>.Create(query, page,2));

           
        }
        public IActionResult Detail(int id)
        {
            PlantComment comment = _context.PlantComments.FirstOrDefault(x => x.Id == id);
            if (comment == null)
            {
                return View("Error");
            }
            ViewBag.Comment = comment.Comment;
           
            ViewBag.CommentId=comment.Id;
            CommentFormViewModel vm = new CommentFormViewModel()
            {
                ReplyComment=comment.ReplyComment,
                Show=comment.ShowComment
                
            };
          
                
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Detail(CommentFormViewModel plantComment)
        {
            PlantComment plantCom= _context.PlantComments.FirstOrDefault(x => x.Id == plantComment.CommentId);
            if (plantCom == null)
            {
                return View();
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("ReplyComment", "Message length must be max 100");
                ViewBag.Comment = plantCom.Comment;
                ViewBag.CommentId = plantCom.Id;
                ViewBag.Reply = plantCom.ReplyComment;
                CommentFormViewModel vm = new CommentFormViewModel();
                return View(vm);
            }
            plantCom.ShowComment = plantComment.Show;
            plantCom.ReplyComment = plantComment.ReplyComment;
            plantCom.ReplyTime = DateTime.UtcNow.AddHours(4);
         
            _context.SaveChanges();
            return RedirectToAction("index");
        }
       
        public IActionResult Delete(int id)
        {
            PlantComment comment = _context.PlantComments.FirstOrDefault(x => x.Id == id);
            if (comment == null)
            {
                return BadRequest();

            }
            _context.PlantComments.Remove(comment);
            _context.SaveChanges();
            return Ok();
        }
    }
}
