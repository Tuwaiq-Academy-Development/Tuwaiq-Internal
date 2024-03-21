using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.XSSF.UserModel;

namespace AdminUi.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class ExportController(ApplicationDbContext context) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [HttpGet("[action]")]
    public async Task<IActionResult> ExportCheckedUsers(int id)
    {
        try
        {
            // var history = await context.CheckRequests.FindAsync(id);
            var result = await context
                .CheckLogs.Where(i => i.RequestId==id)
                .Select(x => new
                {
                    x.NationalId, x.Status, x.CheckedOn, x.Response
                }).ToListAsync();
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("بيانات الطلاب");
            var index = 0;
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(index).SetCellValue("رقم الهوية");
            index++;
            headerRow.CreateCell(index).SetCellValue("الحالة");
            index++;
            headerRow.CreateCell(index).SetCellValue("اخر تحديث");
            var rowIndex = 1;

                foreach (var item in result)
                {
                    var dataRow = sheet.CreateRow(rowIndex);
                    dataRow.CreateCell(0).SetCellValue(item.NationalId);
                    dataRow.CreateCell(1).SetCellValue(item?.Status);
                    dataRow.CreateCell(2).SetCellValue(item?.CheckedOn?.ToString("dd-MM-yyyy hh:mm:ss tt"));
                    rowIndex++;
                }

            var stream = new MemoryStream();
            workbook.Write(stream, true);
            stream.Seek(0, SeekOrigin.Begin);

            var memoryStream = stream;
            var fileName = " قائمة السجلات " + DateTime.Now.ToString("dd-MM-yyyy hh:mm");

            fileName += ".xlsx";


            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null!;
        }
    }
}