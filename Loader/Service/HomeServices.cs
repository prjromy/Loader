using ChannakyaCustomeDatePicker.Repository;
using Loader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.Service
{
    public class HomeServices
    {
        private GenericUnitOfWork uow = null;
        public HomeServices()
        {
            uow = new GenericUnitOfWork();
        }

        //public string GetTranscationDate(int FYID)
        //{
        //    int BrnhID = Loader.Models.Global.BranchId;
        //    var Currentdate1 = uow.Repository<FiscalYear>().GetAll();
        //    string query = "select * from fin.LicenseBranch where BrnhID=" + BrnhID + "";
        //    LicenseBranch GetSingle = uow.Repository<LicenseBranch>().GetAll().Where(x=>x.BrnhID==BrnhID).SingleOrDefault();
        //    int BrnhFYID = Convert.ToInt32(GetSingle.FYID);
        //    DateTime TDate = (DateTime)GetSingle.TDate;
        //    DateTime MigDate = (DateTime)GetSingle.MigDate;
        //    var Currentdate = uow.Repository<FiscalYear>().GetAll().Where(x => x.StartDt <= TDate && x.EndDt >= TDate).SingleOrDefault();
        //    var MigrationDate = uow.Repository<FiscalYear>().GetAll().Where(x => x.StartDt <= MigDate && x.EndDt >= MigDate).SingleOrDefault();
        //    //var allFyearLists = uow.Repository<FiscalYear>().SqlQuery("select * from lg.FiscalYears where FYID between '" + MigrationDate.FYID + "' and '" + Currentdate.FYID + "' order by FYID desc").ToList();
        //    var allFyearLists = uow.Repository<FiscalYear>().GetAll().Where(x => x.FYID >= MigrationDate.FYID && x.FYID <= Currentdate.FYID).OrderByDescending(x => x.FYID).ToList();

        //    var Tlist = "";
        //    if (FYID == allFyearLists.FirstOrDefault().FYID)
        //    {
        //        Tlist = Convert.ToDateTime(Loader.Models.Global.TransactionDate).ToShortDateString();
        //    }
        //    else
        //    {
        //        Tlist = allFyearLists.Find(x => x.FYID == FYID).EndDt.ToShortDateString().ToString();
        //    }

        //    return Tlist;
        //}

    }
}