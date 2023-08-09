using Microsoft.AspNetCore.Mvc;
using prjDemoAjax.Models;

namespace prjDemoAjax.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;
        //取得伺服器上實際路徑
        private readonly IWebHostEnvironment _host; 

        public ApiController(DemoContext context, IWebHostEnvironment host)
        {
            _context = context; 
            _host = host;
        }

        public IActionResult Index()
        {
            //被呼叫執行時會先停6000毫秒
            //System.Threading.Thread.Sleep(6000);
            //return Content("Hello Ajax!! First Ajax Program");
            return Content("Hello Fetch");
        }

        public IActionResult GetDemo(CUser user)/*public IActionResult GetDemo(string name, int age = 30)*/
        {
            if(string.IsNullOrEmpty(user.name))
            {
                user.name = "guest" ;
            }
            return Content($"Hello {user.name}, you are {user.age} years old");
        }

        public IActionResult Register(Members member, IFormFile file)
        {
            //return Content($"{_host.ContentRootPath}");  //C:\shared\Ajax\MSIT150Site\
            //return Content($"{_host.WebRootPath}");       //C:\shared\Ajax\MSIT150Site\wwwroot
            string filePath = Path.Combine(_host.WebRootPath, "uploads", file.FileName); //C:\shared\Ajax\MSIT150Site\wwwroot\uploads\walk.gif

            //上傳檔案到uploads資料夾中
            using (var fileStream = new FileStream(filePath,FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            //將圖轉成二進位
            byte[]? imgbyte = null;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                imgbyte = memoryStream.ToArray();
            }

            //return Content($"上傳檔案到{filePath}");

            member.FileName = file.FileName;
            member.FileData = imgbyte;
            _context.Members.Add(member);
            _context.SaveChanges();
            return Content("註冊成功");
        }

        public IActionResult GetImageByte(int id=1)
        {
            Members? member = _context.Members.Find(id);
            byte[]? img = member.FileData;
            return File(img, "image/jpeg");
        }

        public IActionResult City()
        {
            var cities = _context.Address.Select(c=>c.City).Distinct();
            return Json(cities);
        }

        public IActionResult Districts(string city)
        {
            var districts = _context.Address.Where(a => a.City == city).Select(c => c.SiteId).Distinct();
            return Json(districts);
        }

        public IActionResult Roads(string siteId)
        {
            var roads = _context.Address.Where(a => a.SiteId == siteId).Select(c => c.Road).Distinct();
            return Json(roads);
        }

        //下面是作業
        public IActionResult CheckName(Members member)
        {
            if(string.IsNullOrEmpty(member.Name))
            {
                return Content("姓名欄位不可為空白");
            }

            Members member2 = _context.Members.FirstOrDefault(p => p.Name == member.Name);
            if(member2 != null)
            {
                return Content("已註冊過，請用另一名稱");
            }

            return Content("此名稱可以註冊");

        }


    }
}
