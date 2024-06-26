﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Enums;
using Pronia.Models;
using Pronia.Services;
using Pronia.ViewModels;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="SuperAdmin,Admin")]
    public class OrderController : Controller
    {
        private readonly ProniaContext _context;
        private readonly IEmailSender _email;

        public OrderController(ProniaContext context,IEmailSender email)
        {
            _context = context;
            _email = email;
        }
        public IActionResult Index(int page=1,int orderstatus=0,string search=null)
        {
            var query = _context.Orders.Include(x => x.OrderItems).AsQueryable();
            if (orderstatus != 0)
            {
                query = query.Where(x => (int)x.OrderStatus == orderstatus);

            }
            if(search != null)
            {
                query = query.Where(x => x.AppUser.FullName.Contains(search));
            }
            ViewBag.Search = search;    
            ViewBag.OrderStatus=orderstatus;
            return View(PaginatedList<Order>.Create(query,page,3));
        }
        public IActionResult Detail(int id)
        {
            Order order = _context.Orders.Include(x=>x.OrderItems).ThenInclude(x=>x.Plant).FirstOrDefault(x => x.Id == id);
            if (order == null) return View("Error");
            return View(order);
        }
        public IActionResult Accept(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);
            
            if (order == null) return View("Error");
            order.OrderStatus= OrderStatus.Access;
            _context.SaveChanges();
            _email.Send(order.Email, "Accept", "Yoldadi gelir");
            
            
            return RedirectToAction("index");
        }
        public IActionResult Reject(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);
            if (order == null) return View("Error");
            order.OrderStatus = OrderStatus.Rejected;
            _email.Send(order.Email, "Reject", "Ureyim bele isdedi");
            _context.SaveChanges();
            return RedirectToAction("index");
        }

    }
}
