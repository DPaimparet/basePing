﻿using MySql.Data.MySqlClient;
using System;
using System.Web.Mvc;
using basePing.DataContext;
using basePing.Models;
using System.Collections.Generic;

namespace basePing.Controllers
{
    public class CompetitionController : Controller
    {
        // GET: Competition
        public ActionResult Index()
        {
           
            
            return View();
        }

        public ActionResult GetComp(int id)
        {
            Competition comp = new Competition();
            List<Competition> triedList = new List<Competition>();
            foreach (Competition c in comp.GetList())
            {
                if (c.Cat.Id == id) {
                    c.GetTournoi();
                    triedList.Add(c);
                }
            }
            foreach(Categorie c in Categorie.GetList())
            {
                if (c.Id == id)
                    ViewBag.cat = c;
            }
            ViewBag.listComp = triedList;
            return View();
        }

        public ActionResult InfoComp(int id)
        {
            Competition comp = new Competition(id);
            comp = comp.GetInformation();
            
            //foreach(Match m in comp.Tournoi.LMatch)
            //{
            //    m.Joueur1.RecupererJoueur();
            //    m.Joueur2.RecupererJoueur();
            //}
            
            return View(comp);
        }

        public ActionResult AjouterTypeCompForm()
        {
            return View();
        }

        public ActionResult AjouterTypeComp(string nom,string desc)
        {
            DCCategorie dc = new DCCategorie();
            dc.Insert(nom, desc);
            return Redirect("~/Home/Connect");
        }

        public ActionResult ModifCat(int id)
        {
            ViewBag.idCat = id;
            return View();
        }

        [HttpPost]
        public ActionResult ModifCat(int id, string nom , string desc)
        {
            DCCategorie dc = new DCCategorie();
            dc.Update(nom, desc,id);
            return Redirect("~/Home/Connect");
        }


        public ActionResult AjoutComp(int id)
        {
            List<CPays> listePays = new CPays().GetListPays();
            List<String> listeType = new List<String>();
            listeType.Add("Masculin");
            listeType.Add("Féminin");
            listeType.Add("Mixte");
            List<String> listeNbrJ= new List<String>();
            listeNbrJ.Add("Individuel");
            listeNbrJ.Add("Equipe");
            ViewBag.listePays = new SelectList(listePays, "Id", "Pays");
            ViewBag.listeType = new SelectList(listeType);
            ViewBag.listeNbrJ = new SelectList(listeNbrJ);
            ViewBag.idCat = id;
            return View();
        }

        [HttpPost]
        public ActionResult AjoutComp(String nom,DateTime dateD,DateTime dateF, string pays,string type,string nbrJ,int idCat)
        {
            DCCompetition dc = new DCCompetition();
            dc.Insert(nom,dateD,dateF,pays,type,nbrJ,idCat);
            return Redirect("~/Competition/GetComp/"+idCat);
        }


        public ActionResult ModifierComp(int id, string nom, DateTime dateD, DateTime dateF, string type, string nbrJ)
        {
            List<CPays> listePays = new CPays().GetListPays();
            List<String> listeType = new List<String>();
            listeType.Add(type);
            listeType.Add("Masculin");
            listeType.Add("Féminin");
            listeType.Add("Mixte");
            List<String> listeNbrJ = new List<String>();
            listeNbrJ.Add(nbrJ);
            listeNbrJ.Add("Individuel");
            listeNbrJ.Add("Equipe");
            ViewBag.listePays = new SelectList(listePays, "Id", "Pays");
            ViewBag.listeType = new SelectList(listeType);
            ViewBag.listeNbrJ = new SelectList(listeNbrJ);
            ViewBag.Nom = nom;
            ViewBag.dateD = dateD.ToString();
            ViewBag.dateF = dateF.ToString();
            ViewBag.idComp = id;
            return View();
        }

        [HttpPost]
        public ActionResult ModifierComp(String nom, DateTime dateD, DateTime dateF, string pays, string type, string nbrJ,int idComp)
        {
            DCCompetition dc = new DCCompetition();
            dc.Update(idComp,nom, dateD, dateF, pays, type, nbrJ);
            return Redirect("~/Competition/InfoComp/" + idComp);
        }


        public ActionResult SuppressionComp(int id)
        {
            DCCompetition dc = new DCCompetition();
            dc.Delete(id);
            return Redirect("~/Home/Connect");
        }
    }
}