using DFPS.API.Data.Repository;
using DKS_API.Controllers;
using DKS_API.Data;
using DKS_API.Data.Interface;
using DKS_API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFPS.API.Controllers
{
    [Authorize]
    public class TestController : ApiController
    {
        private readonly DataContext _context;
        //private readonly IArticledLdtmDAO _dao;
        public TestController(IConfiguration config, IWebHostEnvironment webHostEnvironment,DataContext context,IArticledLdtmDAO dao)
        : base(config, webHostEnvironment)
        {
            _context = context;
            //_dao = dao;
        }
        //http://localhost:5000/api/test/getTest
        [AllowAnonymous]
        [HttpGet("getTest")]
        public async Task<IActionResult> GetTestJustName()
        {
            //var test = _dao.FindAll();
            var deptList = from t1 in _context.SampleWorkBase
                           join t2 in _context.SampleWorkProcess on t1.Fid equals t2.Hid
                           join t3 in _context.SampleWorkWorker on t2.Fid equals t3.Hid
                           where t1.SampleNr != null && t1.Maker != null
                           select new PlanProcessWorkDto
                           {
                               Maker = t1.Maker,
                               SampleNr = t1.SampleNr,
                               SampleDesc = t1.SampleDesc,
                               OrdinalNum = t2.OrdinalNum,
                               ProcessNum = t2.ProcessNum,
                               Description = t2.Description,
                               WorkerNum = t3.WorkerNum,
                               WorkerName = t3.WorkerName
                           };
            var list = await deptList.ToListAsync().ConfigureAwait(false); // <-- notice the `await`

            return Ok(list);
        }
        //http://localhost:5000/api/test/0000
        [AllowAnonymous]
        [HttpGet("{customernrs}")]
        public async Task<IActionResult> GetTestById(string customernr)
        {
            var dept = await _context.Jang1Base.FirstOrDefaultAsync(x => x.Customernr == customernr);
            return Ok(dept);
        }
        //http://localhost:5000/api/test/
        [AllowAnonymous]
        [HttpGet("getPlanProcessWork")]
        public async Task<IActionResult> GetPlanProcessWorkByLeanNModelNo(string sampleNr, string maker)
        {
            var deptList = from t1 in _context.SampleWorkBase
                           join t2 in _context.SampleWorkProcess on t1.Fid equals t2.Hid
                           join t3 in _context.SampleWorkWorker on t2.Fid equals t3.Hid
                           where t1.SampleNr == sampleNr && t1.Maker == maker
                           select new PlanProcessWorkDto
                           {
                               Maker = t1.Maker,
                               SampleNr = t1.SampleNr,
                               SampleDesc = t1.SampleDesc,
                               OrdinalNum = t2.OrdinalNum,
                               ProcessNum = t2.ProcessNum,
                               Description = t2.Description,
                               WorkerNum = t3.WorkerNum,
                               WorkerName = t3.WorkerName
                           };
            var list = await deptList.ToListAsync().ConfigureAwait(false); // <-- notice the `await`

            return Ok(list);
        }


    }
}