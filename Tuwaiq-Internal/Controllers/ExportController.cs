using System.Runtime.InteropServices.JavaScript;
using IdentityService.SDK.Models;
using TuwaiqInternal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using NPOI.XSSF.UserModel;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;
using System.Net.Http;

namespace IdentityService.Controllers;

using Microsoft.AspNetCore.Mvc;

//[Authorize]

[Route("[controller]")]
[ApiController]
public class ExportController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [HttpGet("[action]")]
    public async Task<IActionResult> ExportCheckedUsers()
    {
        try
        {
            var result = context.ToBeCheckeds.Where(i => i.IsChecked == true).Select(x => new
            {
                x.NationalId, x.IsRegistered, x.FirstName, x.SecondName,
                x.ThirdName, x.FourthName
            });
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("بيانات الطلاب");
            var index = 0;
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(index).SetCellValue("رقم الهوية");
            index++;
            headerRow.CreateCell(index).SetCellValue("الحالة");
            index++;
            headerRow.CreateCell(index).SetCellValue("الاسم الأول");
            index++;
            headerRow.CreateCell(index).SetCellValue("الاسم الثاني");
            index++;
            headerRow.CreateCell(index).SetCellValue("الاسم الثالث");
            index++;
            headerRow.CreateCell(index).SetCellValue("الاسم الرابع");
            var rowIndex = 1;

            foreach (var item in result)
            {
                var dataRow = sheet.CreateRow(rowIndex);
                dataRow.CreateCell(0).SetCellValue(item.NationalId);
                dataRow.CreateCell(1).SetCellValue(item.IsRegistered==0 ? "غير مشترك" : item.IsRegistered==1 ? "غير فعال | غير محدث" :item.IsRegistered==2 ?"مشترك بالفعل":"");
                dataRow.CreateCell(2).SetCellValue(item.FirstName);
                dataRow.CreateCell(3).SetCellValue(item.SecondName);
                dataRow.CreateCell(4).SetCellValue(item.ThirdName);
                dataRow.CreateCell(5).SetCellValue(item.FourthName);
                rowIndex++;
            }

            var stream = new MemoryStream();
            workbook.Write(stream, true);
            stream.Seek(0, SeekOrigin.Begin);

            var memoryStream = stream;
            var fileName = "قائمة السجلات";

            fileName += ".xlsx";

    
            return    File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);;
        }

        catch (Exception ex)
        {
            return null;
        }
    }
}