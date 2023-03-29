using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VinylRecordsApp.Data;
using VinylRecordsApp.Models;
using VinylRecordsApp.Models.Domain;

namespace VinylRecordsApp.Controllers
{
    public class VinylController : Controller
    {
        private readonly VinylContext vinylContext;
        public VinylController(VinylContext vinylContext)
        {
            this.vinylContext = vinylContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string SearchString,string sortOrder)
        {
            ViewData["CurrentFilter"] = SearchString;
            ViewData["DateSort"] = sortOrder == "Date" ? "date_desc" : "Date";
            var records = from b in vinylContext.VinylRecords select b;

            switch (sortOrder)
            {
                case "Date":
                    records = records.OrderBy(b => b.ReleaseDate);
                    break;
                case "date_desc":
                    records = records.OrderByDescending(b => b.ReleaseDate);
                    break;

            }
            if (!String.IsNullOrEmpty(SearchString))
            {
               records =  records.Where(b => b.Description.Contains(SearchString));
            }
            return View(await records.ToListAsync()); 
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRecordViewModel addRecordView)
        {
            var vinylRecord = new Vinyl()
            {
                Id = Guid.NewGuid(),
                Name = addRecordView.Name,
                Label = addRecordView.Label,
                Price = addRecordView.Price,
                ReleaseDate = addRecordView.ReleaseDate,
                Description = addRecordView.Description
            };

            await vinylContext.VinylRecords.AddAsync(vinylRecord);
            await vinylContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var record = await vinylContext.VinylRecords.FirstOrDefaultAsync(x => x.Id == id);

            if (record != null)
            {
                var viewModel = new UpdateRecordViewModel()
                {
                    Id = record.Id,
                    Name = record.Name,
                    Label = record.Label,
                    Price = record.Price,
                    ReleaseDate = record.ReleaseDate,
                    Description = record.Description
                };
                return await Task.Run(() => View("View",viewModel));
            }

           

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateRecordViewModel model)
        {
            var record = await vinylContext.VinylRecords.FindAsync(model.Id);

            if(record != null)
            {
                record.Name = model.Name;
                record.Label = model.Label;
                record.Price =  model.Price;
                record.ReleaseDate = model.ReleaseDate;
                record.Description = model.Description;

                await vinylContext.SaveChangesAsync();

                return RedirectToAction("Index");
              
            }
            return RedirectToAction("Index"); 
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateRecordViewModel model)
        {
            var record = await vinylContext.VinylRecords.FindAsync(model.Id);
            if(record != null)
            {
                vinylContext.VinylRecords.Remove(record);
                await vinylContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

    }
}
