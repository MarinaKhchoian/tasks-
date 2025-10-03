using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;
using SchoolManagement.Services;

namespace SchoolManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var optionsBuilder = new DbContextOptionsBuilder<SchoolDbContext>();
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=SchoolDB;Trusted_Connection=True;");

            using (var context = new SchoolDbContext(optionsBuilder.Options))
            {
                Console.WriteLine("მონაცემთა ბაზის ინიციალიზაცია...");
                context.Database.EnsureCreated();

             
                if (!context.Teachers.Any())
                {
                    Console.WriteLine("ტესტური მონაცემების დამატება...");
                    AddTestData(context);
                    Console.WriteLine("✓ მონაცემები დაემატა!");
                }

                var teacherService = new TeacherService(context);

                Console.WriteLine("\n╔════════════════════════════════════════╗");
                Console.WriteLine("║  მოსწავლის მასწავლებლების ძიება       ║");
                Console.WriteLine("╚════════════════════════════════════════╝\n");

                string studentName = "გიორგი";
                Console.WriteLine($"მოსწავლის სახელი: {studentName}");
                Console.WriteLine("─────────────────────────────────────────\n");

                Teacher[] teachers = teacherService.GetAllTeachersByStudent(studentName);

                if (teachers.Length > 0)
                {
                    Console.WriteLine($"✓ ნაპოვნია {teachers.Length} მასწავლებელი:\n");
                    int counter = 1;
                    foreach (var teacher in teachers)
                    {
                        Console.WriteLine($"{counter}. {teacher.FirstName} {teacher.LastName}");
                        Console.WriteLine($"   საგანი: {teacher.Subject}");
                        Console.WriteLine($"   სქესი: {teacher.Gender}");
                        Console.WriteLine($"   მოსწავლეთა რაოდენობა: {teacher.Pupils.Count}");
                        Console.WriteLine();
                        counter++;
                    }
                }
                else
                {
                    Console.WriteLine($"✗ მასწავლებლები სახელით '{studentName}' არ მოიძებნა.");
                }

                Console.WriteLine("\n─────────────────────────────────────────");
                Console.WriteLine($"სულ მასწავლებლები: {context.Teachers.Count()}");
                Console.WriteLine($"სულ მოსწავლეები: {context.Pupils.Count()}");
                Console.WriteLine($"მოსწავლეები სახელით '{studentName}': {context.Pupils.Count(p => p.FirstName == studentName)}");
            }

            Console.WriteLine("\n\nდააჭირეთ ნებისმიერ ღილაკს დასასრულებლად...");
            Console.ReadKey();
        }

        
        static void AddTestData(SchoolDbContext context)
        {
          
            var teacher1 = new Teacher
            {
                FirstName = "ნინო",
                LastName = "ივანიძე",
                Gender = "Female",
                Subject = "მათემატიკა"
            };

            var teacher2 = new Teacher
            {
                FirstName = "დავითი",
                LastName = "პეტრიაშვილი",
                Gender = "Male",
                Subject = "ინგლისური"
            };

            var teacher3 = new Teacher
            {
                FirstName = "თამარ",
                LastName = "გელაშვილი",
                Gender = "Female",
                Subject = "ქართული"
            };

           
            var pupil1 = new Pupil
            {
                FirstName = "გიორგი",
                LastName = "ბერიძე",
                Gender = "Male",
                Class = "10A"
            };

            var pupil2 = new Pupil
            {
                FirstName = "მარიამ",
                LastName = "კახიძე",
                Gender = "Female",
                Class = "10A"
            };

            var pupil3 = new Pupil
            {
                FirstName = "გიორგი",
                LastName = "მამულაშვილი",
                Gender = "Male",
                Class = "10B"
            };

        
            teacher1.Pupils.Add(pupil1);
            teacher1.Pupils.Add(pupil2);
            teacher1.Pupils.Add(pupil3);

            teacher2.Pupils.Add(pupil1);
            teacher2.Pupils.Add(pupil3);

            teacher3.Pupils.Add(pupil1);
            teacher3.Pupils.Add(pupil2);

           
            context.Teachers.Add(teacher1);
            context.Teachers.Add(teacher2);
            context.Teachers.Add(teacher3);
            context.SaveChanges();
        }
    }
}