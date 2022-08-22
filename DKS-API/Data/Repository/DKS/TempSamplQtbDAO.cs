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

namespace DFPS.API.Data.Repository
{
    public class TempSamplQtbDAO: DKSCommonDAO<TempSamplQtb>, ITempSamplQtbDAO
    {
        public TempSamplQtbDAO(DKSContext context) : base(context)
        {
        }

    }
}