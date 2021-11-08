using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DKS.API.Models.DKS;
using DKS_API.Data.Repository;
using DKS_API.Data.Interface;
using DKS_API.DTOs;
using Microsoft.Data.SqlClient;
using System;
using DKS_API.Data;
using DKS_API.Helpers;

namespace DFPS.API.Data.Repository
{
    public class DtrLoginHistoryDAO : DKSCommonDAO<DtrLoginHistory>, IDtrLoginHistoryDAO
    {
        public DtrLoginHistoryDAO(DKSContext context) : base(context)
        {

        }

        public IQueryable<DtrLoginUserHistoryDto> GetDtrLoginUserHistoryDto(SDtrLoginHistory sDtrLoginHistory)
        {
            var data = from d in _context.DTR_LOGIN_HISTORY
                       join s in _context.STACCRTH on d.Account equals s.LOGIN
                       join e in _context.EMPDATAH on s.WORKPNO equals e.WORKPNO
                       select new DtrLoginUserHistoryDto
                       {
                           SystemName = d.SystemName,
                           FactoryId = s.FACTORYID,
                           Account = d.Account,
                           WorkNo = s.WORKPNO,
                           Name = e.NAME,
                           IP = d.IP,
                           LoginTime = d.LoginTime
                       };
            if (!(String.IsNullOrEmpty(sDtrLoginHistory.systemName)))
            {
                data = data.Where(x => x.SystemName == sDtrLoginHistory.systemName);
            }
            if (!(String.IsNullOrEmpty(sDtrLoginHistory.factoryId)))
            {
                data = data.Where(x => x.FactoryId == sDtrLoginHistory.factoryId);
            }
            if (!(String.IsNullOrEmpty(sDtrLoginHistory.loginTimeS)))
            {
                data = data.Where(x => x.LoginTime >= sDtrLoginHistory.loginTimeS.ToDateTime());
            }
            if (!(String.IsNullOrEmpty(sDtrLoginHistory.loginTimeE)))
            {
                data = data.Where(x => x.LoginTime <= sDtrLoginHistory.loginTimeE.ToDateTime());
            }
            return data;
        }
    }
}