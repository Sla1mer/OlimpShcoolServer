using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using FoodServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodServer.Controllers
{
    public class FoodController : Controller
    {
        ApplicationContext dbContext = new ApplicationContext();
        private readonly IWebHostEnvironment _host;
        public FoodController(IWebHostEnvironment host)
        {
            this._host = host;
        }

        [HttpGet]
        public async Task<IEnumerable<FoodEntries>> getAllFood()
        {
            List<FoodEntries> data = await dbContext.FoodEntries.ToListAsync();
            return data;
        }

        [HttpGet]
        public async Task<IEnumerable<FoodEntries>> getFoodByDate(int year, int month, int day)
        {
            DateTime date = new DateTime(year, month, day);
            List<FoodEntries> data = await dbContext.FoodEntries.Where(u => u.date == date && 
                dbContext.Users.Where(j => j.id == u.student).First().is_admin == false).ToListAsync();
            return data;

            // Пример api/food?year=2022&month=10&day=24
        }

        [HttpGet]
        public async Task<IEnumerable<FoodEntries>> getFoodByStudentId(int student_id)
        {
            List<FoodEntries> data = await dbContext.FoodEntries.Where(u => u.student == student_id 
                && dbContext.Users.Where(j => j.id == u.student).First().is_admin == false).ToListAsync();
            return data;
            // Пример api/food?student_id=1
        }


        // Получение подробной таблицы по каждому студенту об его питании на определенный день
        [HttpGet]
        [Route("food/document/getDocumentFoodNow")]
        public async Task<IActionResult> getDocumentFoodNow(int year, int month, int day)
        {

            //Создание таблицы и подготовка нужных данных
            DateTime date = new DateTime(year, month, day);
            List<FoodEntries> data = 
                await dbContext.FoodEntries.Where(u => u.date == date &
                    dbContext.Users.Where(j => j.id == u.student).First().is_admin == false).ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var workSheet = workbook.Worksheets.Add(String.Format("Отчет за {0}.{1}.{2}", day, month, year));
                List<string> header = new List<string>() { "ФИО", "Завтрак", "Обед", "Полдник", "Ужин", "Подпись" };

                for (int i = 1; i <= 6; i++)
                {
                    workSheet.Cell(1, i).Value = header[i - 1].ToString();
                }

                for (int i = 2; i < data.Count + 2; i++)
                {
                    var student_id = data[i - 2].student;
                    var user = await dbContext.Users.Where(u => u.id == student_id).FirstAsync();
                    List<Object> values = new List<Object>();
                    values.Add(String.Format("{0} {1} {2}", user.last_name, user.name, user.middle_name));
                    if ((bool)data[i - 2].is_breakfast)
                    {
                        if ((bool)data[i - 2].is_breakfast_competition)
                        {
                            values.Add("C");
                        }
                        else
                        {
                            values.Add("+");
                        }
                    }
                    else
                    {
                        values.Add("-");
                    }

                    if ((bool)data[i - 2].is_lunch)
                    {
                        if ((bool)data[i - 2].is_lunch_competition)
                        {
                            values.Add("C");
                        }
                        else
                        {
                            values.Add("+");
                        }
                    }
                    else
                    {
                        values.Add("-");
                    }

                    if ((bool)data[i - 2].is_after_lunch)
                    {
                        if ((bool)data[i - 2].is_after_lunch_competition)
                        {
                            values.Add("C");
                        }
                        else
                        {
                            values.Add("+");
                        }
                    }
                    else
                    {
                        values.Add("-");
                    }

                    if ((bool)data[i - 2].is_dinner)
                    {
                        if ((bool)data[i - 2].is_dinner_competition)
                        {
                            values.Add("C");
                        }
                        else
                        {
                            values.Add("+");
                        }
                    }
                    else
                    {
                        values.Add("-");
                    }



                    for (int j = 1; j <= 5; j++)
                    {
                        workSheet.Cell(i, j).Value = values[j - 1];
                    }

                    workSheet.Columns().AdjustToContents();
                    workSheet.Columns().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        fileContents: content,
                        contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileDownloadName: "Otchet_za_day.xlsx"
                );
                }

            }

            // Пример /food/document/getDocumentFoodNow?year=2022&month=10&day=24
        }

        // Получение таблицы по кол-ву порций на сегодняшний день
        [HttpGet]
        [Route("food/document/getDocumentCountPersonFood")]
        public async Task<IActionResult> getDocumentCountPersonFood(int year, int month, int day)
        {
            //Создание таблицы и подготовка нужных данных
            DateTime date = new DateTime(year, month, day);
            List<FoodEntries> data = await dbContext.FoodEntries.Where(u => u.date == date 
                && dbContext.Users.Where(j => j.id == u.student).First().is_admin == false).ToListAsync();

            List<string> header = new List<string>() { "Завтрак\n(кол-во)", "Обед\n(кол-во)",
                "Полдник\n(кол-во)", "Ужин\n(кол-во)", "Сухие пайки\n(кол-во)" };

            List<Object> values = new List<Object>();
            values.Add(data.Where(u => u.is_breakfast == true).ToList().Count());
            values.Add(data.Where(u => u.is_lunch == true).ToList().Count());
            values.Add(data.Where(u => u.is_after_lunch == true).ToList().Count());
            values.Add(data.Where(u => u.is_dinner == true).ToList().Count());

            var count_dryRation = data.Where(u => u.is_breakfast_competition == true).Count() +
                data.Where(u => u.is_lunch_competition == true).Count() +
                data.Where(u => u.is_after_lunch_competition == true).Count() +
                data.Where(u => u.is_dinner_competition == true).Count();

            values.Add(count_dryRation);

            using (var workbook = new XLWorkbook())
            {
                var workSheet = workbook.Worksheets.Add(String.Format("Количество порций за {0}.{1}.{2}", day, month, year));

                for (int i = 1; i <= 5; i++)
                {
                    workSheet.Cell(1, i).Value = header[i - 1].ToString();
                }

                for (int j = 1; j <= 5; j++)
                {
                    workSheet.Cell(2, j).Value = values[j - 1];
                }

                workSheet.Columns().AdjustToContents();
                workSheet.Columns().Style.Alignment.WrapText = true;
                workSheet.Rows().AdjustToContents();
                workSheet.Columns().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        fileContents: content,
                        contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileDownloadName: "Count_porcii.xlsx"
                );
                }
            }
               

            // /food/document/getDocumentCountPersonFood?year=2022&month=10&day=24
        }

        // Получение таблицы по кол-ву порций на сегодняшний день
        [HttpGet]
        [Route("food/document/getDocumentFoodFoolMonth")]
        public async Task<IActionResult> getDocumentFoodFoolMonth(int month, int  year)
        {
            //Создание таблицы и подготовка нужных данных
            List<FoodEntries> data = await dbContext.FoodEntries.Where(u => u.date.Month == month &
                u.date.Year == year && 
                dbContext.Users.Where(j => j.id == u.student).First().is_admin == false).OrderBy(n => n.date).ToListAsync();
            var count = data.Count;

            using (var workbook = new XLWorkbook())
            {
                var workSheet = workbook.Worksheets.Add(String.Format("Отчет {0} месяц", month));

                List<string> header = new List<string>() { "№", "Фамилия, имя",
                "", "дата"};



                var minDate = data.First().date;
                while (minDate <= data.Last().date)
                {
                    header.Add(minDate.Day.ToString());
                    minDate = minDate.AddDays(1);
                }

                data = data.DistinctBy(u => u.student).ToList();

                int position = 9;
                for (int i = 1; i <= header.Count; i++)
                {
                    if (i >= 5)
                    {
                        if (i == 5)
                        {
                            workSheet.Range(workSheet.Cell(1, i), workSheet.Cell(1, i + 3)).Merge();
                        }
                        else
                        {
                            workSheet.Range(workSheet.Cell(1, position), workSheet.Cell(1, position + 3)).Merge();
                            position += 4;
                        }

                        workSheet.Cell(1, position - 4).Value = header[i - 1].ToString();


                    }
                    else
                    {
                        workSheet.Cell(1, i).Value = header[i - 1].ToString();
                    }

                }

                workSheet.Cell(2, 3).Value = "Вид спорта";
                workSheet.Cell(2, 4).Value = "Курс/класс";

                List<string> header2 = new List<string>() { "завтрак", "обед", "полдник", "ужин" };

                int indexCollumn = 5;
                for (int j = 0; j < header.Count - 4; j++)
                {
                    workSheet.Cell(2, indexCollumn).Value = header2[0].ToString();
                    workSheet.Cell(2, indexCollumn + 1).Value = header2[1].ToString();
                    workSheet.Cell(2, indexCollumn + 2).Value = header2[2].ToString();
                    workSheet.Cell(2, indexCollumn + 3).Value = header2[3].ToString();
                    indexCollumn += 4;
                }

                var dateListMonth = header.Skip(4).ToList();


                for (int i = 3; i - 3 < data.Count; i++)
                {
                    int positionCells = 5;
                    for (int j = 0; j < dateListMonth.Count; j++)
                    {
                        var id = data[i - 3].student;
                        DateTime date = new DateTime(year, month, int.Parse(dateListMonth[j]));
                        var food = dbContext.FoodEntries.Where(k => k.date == date && k.student == id).FirstOrDefault();
                        if (food == null)
                        {
                            workSheet.Cell(i, positionCells).Value = "-";

                            positionCells += 1;

                            workSheet.Cell(i, positionCells).Value = "-";

                            positionCells += 1;

                            workSheet.Cell(i, positionCells).Value = "-";

                            positionCells += 1;

                            workSheet.Cell(i, positionCells).Value = "-";

                            positionCells += 1;
                            continue;
                        }
                        workSheet.Cell(i, 1).Value = i - 2;
                        var user = dbContext.Users.Where(u => u.id == food.student).First();
                        workSheet.Cell(i, 2).Value = String.Format("{0} {1}", user.last_name, user.name);
                        workSheet.Cell(i, 3).Value = user.sport;
                        workSheet.Cell(i, 4).Value = user.class_group;
                        if (food.is_breakfast)
                        {
                            if (food.is_breakfast_competition)
                            {
                                workSheet.Cell(i, positionCells).Value = "C";
                            }
                            else
                            {
                                workSheet.Cell(i, positionCells).Value = "+";
                            }
                        }
                        else
                        {
                            workSheet.Cell(i, positionCells).Value = "-";
                        }

                        positionCells += 1;

                        if (food.is_lunch)
                        {
                            if (food.is_lunch_competition)
                            {
                                workSheet.Cell(i, positionCells).Value = "C";
                            }
                            else
                            {
                                workSheet.Cell(i, positionCells).Value = "+";
                            }
                        }
                        else
                        {
                            workSheet.Cell(i, positionCells).Value = "-";
                        }

                        positionCells += 1;

                        if (food.is_after_lunch)
                        {
                            if (food.is_after_lunch_competition)
                            {
                                workSheet.Cell(i, positionCells).Value = "C";
                            }
                            else
                            {
                                workSheet.Cell(i, positionCells).Value = "+";
                            }
                        }
                        else
                        {
                            workSheet.Cell(i, positionCells).Value = "-";
                        }

                        positionCells += 1;

                        if (food.is_dinner)
                        {
                            if (food.is_dinner_competition)
                            {
                                workSheet.Cell(i, positionCells).Value = "C";
                            }
                            else
                            {
                                workSheet.Cell(i, positionCells).Value = "+";
                            }
                        }
                        else
                        {
                            workSheet.Cell(i, positionCells).Value = "-";
                        }

                        positionCells += 1;
                    }
                }

                workSheet.Columns().AdjustToContents();
                workSheet.Columns().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        fileContents: content,
                        contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileDownloadName: "Otchet_za_month.xlsx"
                );
                }
            }

            // food/document/getDocumentFoodFoolMonth?year=2022&month=10
        }

        [HttpPost]
        public async Task<string> createFood([FromBody] FoodEntries foodEntries)
        {
            try
            {
                await dbContext.FoodEntries.AddAsync(foodEntries);
                await dbContext.SaveChangesAsync();
                return "FoodEntries created";
            }
            catch (DbUpdateException e)
            {
                return e.Message.ToString();
            }

            // Пример запроса


            /*
             * /api/food
                {
                    "student": 1,
                    "date": "2022-10-24",
                    "is_breakfast": true,
                    "is_lunch": true,
                    "is_after_lunch": false,
                    "is_dinner": true,
                    "is_breakfast_competition": false,
                    "is_lunch_competition": true,
                    "is_after_lunch_competition": false,
                    "is_dinner_competition": false


                }
             */
        }

        [HttpDelete]
        public async Task<string> deleteFoodByDate([FromBody] DeletedFood deletedFood)
        {

            var date = new DateTime(deletedFood.year, deletedFood.month, deletedFood.day);
            FoodEntries data = await dbContext.FoodEntries.Where(u => u.date == date && u.student == deletedFood.student_id).FirstAsync();
            dbContext.FoodEntries.Remove(data);
            await dbContext.SaveChangesAsync();
            return "Deleted food";

            /* 
             * /api/food
                {
                    "year": 2022,
                    "month": 10,
                    "day": 24,
                    "student_id": 1
                }
            */
        }
    }
}
