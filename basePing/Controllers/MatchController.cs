﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using basePing.Models;
using basePing.DataContext;

namespace basePing.Controllers
{
    public class MatchController : Controller
    {
        // GET: Match
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InfoMatch(int id)
        {
            Match m = new DCMatch().find(id);
            m.Joueur1.RecupererJoueur();
            m.Joueur2.RecupererJoueur();
            m.LSet = new Set().GetList(id);
            return View(m);
        }

        [Authorize]
        public ActionResult AjoutSet(int idM, int cpt1, int cpt2, int score1,int  score2,string j1,string j2)
        {
            Session["cpt1"] = cpt1;
            Session["cpt2"] = cpt2;
            Session["score1"] = score1;
            Session["score2"] = score2;
            Session["j1"] = j1;
            Session["j2"] = j2;
            ViewBag.j1 = j1;
            ViewBag.j2 = j2;
            Session["idM"] = idM;
            return View();
        }


        [Authorize]
        public ActionResult SupprimerSet(int idS,int idM)
        {
            DCSet dc = new DCSet();
            dc.Delete(idS);
            return Redirect("~/Match/InfoMatch/" + idM);
        }


        [Authorize]
        [HttpPost]
        public ActionResult AjoutSet(int position,int point1, int point2)
        {
            DCSet dc = new DCSet();

            if (dc.findSamePos(position, (int)Session["idM"]))
            {
                return Redirect("/Match/AjoutSet?idM="+ (int)Session["idM"] + "8&cpt1="+ (int)Session["cpt1"] + "&cpt2="+ (int)Session["cpt2"] + "&score1="+(int)Session["score1"]+"&score2="+ (int)Session["score1"] + "&j1="+HttpUtility.HtmlEncode(Session["j1"]) + "&j2="+ HttpUtility.HtmlEncode(Session["j2"])+"&error=Il existe déja un set sur cette position");
            }

            if (point1> point2 && (int)Session["cpt1"]< (int)Session["score1"])
            {
                dc.Insert((int)Session["idM"], point1, point2,position);
                return Redirect("/Match/InfoMatch/" + Session["idM"]);
            }else if (point2 > point1 && (int)Session["cpt2"] < (int)Session["score2"])
            {
                dc.Insert((int)Session["idM"], point1, point2,position);
                return Redirect("/Match/InfoMatch/" + Session["idM"]);
            }
            else
                return Redirect("~/Match/InfoMatch/"+Session["idM"]+"?error=Erreur le gagnant du match ne correspond pas au nombre de set gagné");


        }


        [Authorize]
        public ActionResult AjoutMatch(int id)
        {
            ViewBag.idComp = id;
            List<Joueur> j = new Joueur().GetListJoueurComp(id);
            Session["listJ"] = new SelectList(j, "Id", "Identite");
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AjoutMatch(int id,int? joueur1,int score1,int? joueur2,int score2)
        {
            DCMatch dc = new DCMatch();
            if (joueur1 == joueur2)
                return Redirect("/Match/AjoutMatch?id=" + id +"&error=Les 2 joueurs choisis sont le même.");
            else
            {

                dc.Create(id, joueur1, score1, joueur2, score2);

                return Redirect("~/Competition/InfoComp/" + id);
            }
        }


        [Authorize]
        public ActionResult SuppMatch(int id)
        {
            DCMatch dc = new DCMatch();
            dc.Delete(id);
            return Redirect("~/Competition/InfoComp/"+ Session["idComp"]);
        }


        [Authorize]
        public ActionResult LieMatch(int pos,int idC,int idS)
        {
            Session["pos"] = pos;
            Session["idC"]= idC;
            Session["idS"] = idS;
            List<Joueur> listJ = new Joueur().GetListJoueurComp(idC);
           
            List<Match> listM = new Match().GetMatchComp(idC);
            foreach (Match m in listM)
            {
                m.Joueur1.RecupererJoueur();
                m.Joueur2.RecupererJoueur();
            }
            Session["listJ"] = new SelectList(listJ, "Id", "Identite");
            Session["listM"] = new SelectList(listM, "Id", "Info");
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult LieMatch(int match)
        {
            DCMatch dc = new DCMatch();
            dc.LinkMatch((int)Session["pos"],(int) Session["idS"],(int)Session["idC"]);
            return Redirect("~/Competition/InfoComp/" + Session["idC"]);
        }

        [Authorize]
        public ActionResult LieMatchPoule(int idJ, int idC, int idS)
        {
            Session["idC"] = idC;
            Session["idS"] = idS;
            Session["pos"] = 0;
            Joueur jToRemove = null;
            List<Joueur> listJ = new Joueur().GetListJoueurComp(idC);
            foreach (Joueur j in listJ)
            {
                if (j.Id == idJ)
                    jToRemove = j;
            }
            listJ.Remove(jToRemove);
            List<Match> listM = new Match().GetMatchComp(idC,idJ);
            foreach (Match m in listM)
            {
                m.Joueur1.RecupererJoueur();
                m.Joueur2.RecupererJoueur();
            }
            Session["listJ"] = new SelectList(listJ, "Id", "Identite");
            Session["listM"] = new SelectList(listM, "Id", "Info");
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult LieMatchPoule(int match)
        {
            DCMatch dc = new DCMatch();
            dc.LinkMatch((int)Session["pos"], (int)Session["idS"], (int)Session["idC"]);
            return Redirect("~/Competition/InfoComp/" + Session["idC"]);
        }


        [Authorize]
        [HttpPost]
        public ActionResult AjoutEtLieMatch(int? joueur1, int score1, int? joueur2, int score2)
        {
            DCMatch dc = new DCMatch();
           
            if (joueur1 == joueur2)
                return Redirect("/Match/LieMatch?pos="+ (int)Session["pos"] + "&idC="+ (int)Session["idC"] + "&idS="+ (int)Session["idS"] + "&error=Les 2 joueurs choisis sont le même.");
            else { 
                dc.Create(joueur1, score1, joueur2, score2, (int)Session["pos"], (int)Session["idS"], (int)Session["idC"]);
                return Redirect("~/Competition/InfoComp/" + Session["idC"]);
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult AjoutEtLieMatchPoule(int score1,int? joueur, int score2)
        {
            DCMatch dc = new DCMatch();
            dc.Create((int)Session["idJ"], score1, joueur, score2, 0, (int)Session["idS"], (int)Session["idC"]);
            return Redirect("~/Competition/InfoComp/" + Session["idC"]);
        }
    }
}