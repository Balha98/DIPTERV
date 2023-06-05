using DIPTERV.Context;
using DIPTERV.Data;
using DIPTERV.Logic;
using DIPTERV.Pages;
using DIPTERV.Repositories;
using Humanizer;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections.Generic;

namespace DIPTERV.Services
{
    public class GeneticAlgorithmService
    {

        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        CourseRepository _courseRepo;

        public List<Teacher> Teachers { get; set; }
        public List<Room> Rooms { get; set; }
        public List<SubjectDivision> SubjectDivisions { get; set; }
        public List<SchoolClass> SchoolClasses { get; set; }

        public List<TimeBlock> TimeBlocks { get; set; }

        public GeneticAlgorithmService(IDbContextFactory<ApplicationDbContext> factory, CourseRepository courseRepository)
        {
            _factory = factory;
            _courseRepo = courseRepository;
        }


        public async Task<bool> ImportExcelFileAsync(InputFileChangeEventArgs e)
        {

            Teachers = new List<Teacher>();
            Rooms = new List<Room>();
            SubjectDivisions = new List<SubjectDivision>();
            SchoolClasses = new List<SchoolClass>();
            TimeBlocks = new List<TimeBlock>();

            Course.nextId = 0;
            Room.nextId = 0;
            SchoolClass.nextId = 0;
            SubjectDivision.nextId = 0;
            Teacher.nextId = 0;
            TimeBlock.nextId = 0;

            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // copy data from file to memory stream
                        await file.OpenReadStream().CopyToAsync(ms);
                        // positions the cursor at the beginning of the memory stream
                        ms.Position = 0;

                        // create ExcelPackage from memory stream
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (ExcelPackage package = new ExcelPackage(ms))
                        {
                            Console.WriteLine("Reading Excel file...");

                            ExcelWorksheet sd_ews = package.Workbook.Worksheets["felosztas"];
                            ExcelWorksheet t_ews = package.Workbook.Worksheets["tanarok"];
                            ExcelWorksheet r_ews = package.Workbook.Worksheets["termek"];

                            var sd_end = sd_ews.Dimension.End;
                            var t_end = t_ews.Dimension.End;
                            var r_end = r_ews.Dimension.End;




                            for (int col = 3; col <= t_end.Column; col++)
                            {
                                TimeBlocks.Add(new TimeBlock(GetDay(t_ews.Cells[1, col].Text), Int32.Parse(t_ews.Cells[2, col].Text)));
                            }

                            //Teachers and SchoolClasses
                            for (int row = 3; row <= t_end.Row; row++)
                            {
                                //Teacher's name
                                string t_name = t_ews.Cells[row, 1].Text.Trim();


                                //Free and All Timeblocks for teacher
                                var free_tb = new List<TimeBlock>();
                                for (int col = 3; col <= t_end.Column; col++)
                                {
                                    //per Teacher
                                    if (t_ews.Cells[row, col].Value == null)
                                        free_tb.Add(TimeBlocks[col - 3]);
                                    //free_tb.Add(new TimeBlock(GetDay(t_ews.Cells[1, col].Text), Int32.Parse(t_ews.Cells[2, col].Text)));
                                }

                                var act_teacher = new Teacher(t_name, free_tb);
                                Teachers.Add(act_teacher);

                                //SchoolClasses
                                if (t_ews.Cells[row, 2].Value != null)
                                    foreach (string c_name in t_ews.Cells[row, 2].Text.Split(','))
                                        SchoolClasses.Add(new SchoolClass(c_name, act_teacher));
                            }
                            Console.WriteLine("Teachers, TimeBlocks and School classes have been read.");

                            //Rooms
                            for (int row = 1; row <= r_end.Row; row++)
                            {
                                if (r_ews.Cells[row, 1].Value != null)
                                    Rooms.Add(new Room(r_ews.Cells[row, 1].Text, r_ews.Cells[row, 2].Text));
                                else
                                    Rooms.Add(new Room(r_ews.Cells[row, 1].Text));
                            }
                            Console.WriteLine("Rooms have been read.");

                            //SubjectDivisions
                            //The subjects will be numbered - ex. Maths1,Maths2,etc..
                            for (int row = 2; row <= sd_end.Row; row++)
                            {
                                for (int col = 3; col <= sd_end.Column; col++)
                                {
                                    if (sd_ews.Cells[row, col].Value != null)
                                    {
                                        var teacher = Teachers.Find(t => t.Name.Equals(sd_ews.Cells[row, 1].Text.Trim()));
                                        var subject = sd_ews.Cells[row, 2].Text.Trim();
                                        var schoolclass = SchoolClasses.Find(sc => sc.Name.Equals(sd_ews.Cells[1, col].Text.Trim('.')));

                                        for (int i = 1; i <= Int32.Parse(sd_ews.Cells[row, col].Text); i++)
                                        {
                                            SubjectDivisions.Add(new SubjectDivision(teacher, String.Concat(subject, i), schoolclass));
                                            teacher.CourseNumber++;
                                        }
                                    }
                                }
                            }

                            Console.WriteLine("SubjectDivisions have been read.");

                            using (var context = _factory.CreateDbContext())
                            {
                                //Delete existing data
                                await context.Courses.ExecuteDeleteAsync();
                                await context.Rooms.ExecuteDeleteAsync();
                                await context.SubjectDivisions.ExecuteDeleteAsync();
                                await context.SchoolClasses.ExecuteDeleteAsync();
                                await context.Teachers.ExecuteDeleteAsync();
                                await context.TimeBlocks.ExecuteDeleteAsync();
                                
                                
                                await context.Database.ExecuteSqlRawAsync("Truncate table TeacherTimeBlock");


                             

                                //add new data
                                await context.SubjectDivisions.AddRangeAsync(SubjectDivisions.ToArray());
                                await context.Rooms.AddRangeAsync(Rooms.ToArray());
                               await context.Teachers.AddRangeAsync(Teachers);
                                await context.SaveChangesAsync();
                                return true;
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return false;

        }

        public async Task<bool> RunGAAsync()
        {
            using var context = _factory.CreateDbContext();
            if (!context.Rooms.Any() || !context.SubjectDivisions.Any() || !context.TimeBlocks.Any() || !context.Teachers.Any() || !context.SchoolClasses.Any())
                return false;

            Rooms = await context.Rooms.ToListAsync();
            SubjectDivisions = await context.SubjectDivisions.Include(sd => sd.SchoolClass).Include(sd => sd.Teacher).ToListAsync();
            TimeBlocks = await context.TimeBlocks.ToListAsync();
            Teachers = await context.Teachers.Include(t => t.FreeBlocks).ToListAsync();

            var sga = new ScheduleGeneticAlgorithm(Rooms, SubjectDivisions, TimeBlocks, Teachers);
            var courses = sga.RunGA().ToArray();
            await _courseRepo.DeleteAllCoursesAsync();
            await _courseRepo.InsertAllCoursesAsync(courses);
            return true;
        }

        private static Day GetDay(string text)
        {
            switch (text.Trim())
            {
                case "Hétfő":
                    return Day.Monday;
                case "Kedd":
                    return Day.Tuesday;
                case "Szerda":
                    return Day.Wednesday;
                case "Csütörtök":
                    return Day.Thursday;
                case "Péntek":
                    return Day.Friday;
                default:
                    throw new ArgumentException("Nem megfelelő a beolvasott nap");
            }
        }
    }
}
