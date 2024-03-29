﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using YAZHXQ.IService;
using YAZHXQ.Models.Models;
using System;
using System.Linq;

namespace YAZHXQ.Web.Controllers
{
    public class StaffInfoesController : BaseController
    {
        private readonly IStaffInfoService service;

        public StaffInfoesController(IStaffInfoService _service)
        {
            service = _service;
        }

        // GET: StaffInfoes
        public async Task<IActionResult> Index(string sortOrder,
                                                string currentFilter,
                                                string searchString,
                                                int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var staffs = service.GetALL();
            if (!String.IsNullOrEmpty(searchString))
            {
                staffs = staffs.Where(s => s.Name.Contains(searchString)
                                       || s.AccountNo.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    staffs = staffs.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    staffs = staffs.OrderBy(s => s.CreateTime);
                    break;
                case "date_desc":
                    staffs = staffs.OrderByDescending(s => s.CreateTime);
                    break;
                default:
                    staffs = staffs.OrderBy(s => s.Name);
                    break;
            }
            return View(await PaginatedList<StaffInfo>.CreateAsync(staffs, pageNumber ?? 1, PageSize));
        }

        // 用户详情
        public async Task<IActionResult> Details(int? staffId)
        {
            if (staffId == null)
            {
                return NotFound();
            }

            var staffInfo = await service.FirstOrDefaultAsync(m => m.Id == staffId);
            if (staffInfo == null)
            {
                return NotFound();
            }

            return View(staffInfo);
        }

        // GET: StaffInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StaffInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountNo,Kqjno,Name,IsDel,Tel,OrderId,AreasCode,AccountPwd,SexName,SexCode,Birthday,ColledgeName,ProfessionalName,StaffImg,MarriageName,MarriageCode,NationalCode,NationalName,PoliticalStatusName,PoliticalStatusCode,Idnumber,Address,Email,OtherInfo,PostId,DeptId,StaffIp,Grade,CreateStaffId,CreateTime,UpdateStaffId,UpdateTime")] StaffInfo staffInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await service.AddAsync(staffInfo);
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                           "Try again, and if the problem persists " +
                           "see your system administrator.");
            }
            return View(staffInfo);
        }

        // GET: StaffInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffInfo = await service.FindAsync(id);
            if (staffInfo == null)
            {
                return NotFound();
            }
            return View(staffInfo);
        }

        // POST: StaffInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountNo,Kqjno,Name,IsDel,Tel,OrderId,AreasCode,AccountPwd,SexName,SexCode,Birthday,ColledgeName,ProfessionalName,StaffImg,MarriageName,MarriageCode,NationalCode,NationalName,PoliticalStatusName,PoliticalStatusCode,Idnumber,Address,Email,OtherInfo,PostId,DeptId,StaffIp,Grade,CreateStaffId,CreateTime,UpdateStaffId,UpdateTime")] StaffInfo staffInfo)
        {
            if (id != staffInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await service.UpdateAsync(staffInfo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffInfoExists(staffInfo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(staffInfo);
        }

        // GET: StaffInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffInfo = await service.FirstOrDefaultAsync(m => m.Id == id);
            if (staffInfo == null)
            {
                return NotFound();
            }

            return View(staffInfo);
        }

        // POST: StaffInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staffInfo = await service.FindAsync(id);
            await service.DeleteAsync(staffInfo);
            return RedirectToAction(nameof(Index));
        }

        private bool StaffInfoExists(int id)
        {
            return service.Any(e => e.Id == id);
        }
    }
}
