﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using basePing.DataContext;

namespace basePing.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DBConnection con = DBConnection.Instance();
            if (con.IsConnect())
                return View();
            else
                return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Pour toutes questions ou demandes d'informations.";

            return View();
        }
    }
}