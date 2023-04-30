using ConsoleApp1.Models;
using ConsoleApp1.Utils;
using System.Runtime.ConstrainedExecution;

namespace ConsoleApp1
{
    internal class Program
    {
        private static InitFromFile init = new InitFromFile();
        private static LinqRequests requests = new LinqRequests();
        static void Main(string[] args)
        {
            ClearDb();
            InitDb();
            PrintTasks();
            var choice = -1;
            while (choice != 0)
            {
                Console.WriteLine("Ваш выбор: ");
                choice = int.Parse(Console.ReadLine());
                Console.WriteLine("\n");
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        Console.WriteLine("Начальная стоимость:");
                        var start = double.Parse(Console.ReadLine());
                        Console.WriteLine("Конечная стоимость:");
                        var end = double.Parse(Console.ReadLine());
                        Console.WriteLine("Название района:");
                        var name = Console.ReadLine();
                        var output = requests.GetApartInDistBetween(start, end, name);
                        output.ForEach(Console.WriteLine);
                        break;
                    case 2:
                        Console.WriteLine("Количество комнат:");
                        var count = int.Parse(Console.ReadLine());
                        output = requests.GetFio2Rooms(count);
                        output.ForEach(Console.WriteLine);
                        break;
                    case 3:
                        Console.WriteLine("Этаж: ");
                        int floor = int.Parse(Console.ReadLine());
                        output = requests.GetDiffInPrices(floor);
                        output.ForEach(Console.WriteLine);
                        break;
                    case 4:
                        Console.WriteLine("Количество комнат:");
                        count = int.Parse(Console.ReadLine());
                        Console.WriteLine("Название района:");
                        name = Console.ReadLine();
                        Console.WriteLine(requests.GetSumPriceInDist(count, name));
                        break;
                    case 5:
                        Console.WriteLine("Фамилия риэлтора:");
                        name = Console.ReadLine();
                        Console.WriteLine(requests.GetMunMaxByRealtor(name));
                        break;
                    case 6:
                        Console.WriteLine("Название района:");
                        name = Console.ReadLine();
                        Console.WriteLine(requests.GetAvgScore(name));
                        break;
                    case 7:
                        Console.WriteLine("Этаж:");
                        floor = int.Parse(Console.ReadLine());
                        requests.GetInfoByDistrictAndFloor(floor).ForEach(Console.WriteLine);
                        break;
                    case 8:
                        Console.WriteLine("Тип объекта (Квартира, Дом, Апартаменты):");
                        var type = Console.ReadLine();
                        Console.WriteLine("Риэлтор:");
                        name = Console.ReadLine();
                        Console.WriteLine("Критерий (Близость к метро, Ремонт, Обстановка в районе, Транспортная доступность):");
                        var name2 = Console.ReadLine();
                        Console.WriteLine(requests.GetAvgScoreByCriteriaAndRealtor(type, name2, name));
                        break;
                    case 9:
                        Console.WriteLine("Тип объекта (Квартира, Дом, Апартаменты):");
                        type = Console.ReadLine();
                        Console.WriteLine("Начальная дата:");
                        var startDate = DateOnly.Parse(Console.ReadLine());
                        Console.WriteLine("Конечная дата:");
                        var endDate = DateOnly.Parse(Console.ReadLine());
                        Console.WriteLine(requests.GetAvgSalePricePerMeter(type, startDate, endDate));
                        break;
                    case 10:
                        requests.GetBonuses().ForEach(Console.WriteLine);
                        break;
                    case 11:
                        Console.WriteLine("Тип объекта (Квартира, Дом, Апартаменты):");
                        type = Console.ReadLine();
                        requests.GetCounts(type).ForEach(Console.WriteLine);
                        break;
                    case 12:
                        Console.WriteLine("Этаж:");
                        floor = int.Parse(Console.ReadLine());
                        requests.GetAvgPriceByTypeAndMaterial(floor).ForEach(Console.WriteLine);
                        break;

                    case 14:
                        Console.WriteLine("Район:");
                        name = Console.ReadLine();
                        requests.GetApartmentsInDistNotSold(name).ForEach(Console.WriteLine);
                        break;
                    case 15:
                        Console.WriteLine("Район:");
                        name = Console.ReadLine();
                        requests.GetApartmentsLessThan20InDisrtict(name).ForEach(Console.WriteLine);
                        break;
                    case 16:
                        Console.WriteLine("Риэлтор:");
                        name = Console.ReadLine();
                        requests.GetApartmentsWithDiffMoreThan(name).ForEach(Console.WriteLine);
                        break;
                    case 17:
                        Console.WriteLine("Риэлтор:");
                        name = Console.ReadLine();
                        Console.WriteLine("Год:");
                        var year = int.Parse(Console.ReadLine());
                        requests.GetDiffInSaleByRealtorAndYear(name, year).ForEach(Console.WriteLine);
                        break;
                    case 18:
                        requests.GetApartmentsWithPriceLessThanAgerageBuDist().ForEach(Console.WriteLine);
                        break;
                    case 19:
                        requests.GetRealtorsWithoutSalesThisYear().ForEach(Console.WriteLine);
                        break;
                    case 20:
                        requests.GetApartmentsWithPriceLessThanAgerageByDistAndEarlierThan4Month().ForEach(Console.WriteLine);
                        break;
                }
                Console.WriteLine("\n");
            }
        }

        private static void InitDb()
        {
            using (ApplicationContext db = new ApplicationContext())
            {

                db.District.AddRange(init.GetDistricts());
                db.Type.AddRange(init.GetTypes());
                db.Criteria.AddRange(init.GetCriteria());
                db.Realtor.AddRange(init.GetRealtor());
                db.Material.AddRange(init.GetMaterial());
                db.Apartment.AddRange(init.GetApartment());
                db.Score.AddRange(init.GetScore());
                db.Sale.AddRange(init.GetSale());
                db.SaveChanges();
            }
        }

        private static void ClearDb()
        {
            using (ApplicationContext db = new())
            {
                db.Sale.RemoveRange(db.Sale);
                db.Score.RemoveRange(db.Score);
                db.Criteria.RemoveRange(db.Criteria);
                db.Realtor.RemoveRange(db.Realtor);
                db.Apartment.RemoveRange(db.Apartment);
                db.Material.RemoveRange(db.Material);
                db.District.RemoveRange(db.District);
                db.Type.RemoveRange(db.Type);
                db.SaveChanges();
            }
        }


        private static void readAll(string Type)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var districts = db.District.ToList();
                Console.WriteLine("District list:");
                foreach (District d in districts)
                {
                    Console.WriteLine($"{d.Id}.{d.Name}");
                }
            }
        }

        private static void PrintTasks()
        {
            Console.WriteLine("1. Вывести объекты недвижимости, расположенные в указанном районе стоимостью «ОТ» и «ДО»");
            Console.WriteLine("2. Вывести фамилии риэлтор, которые продали двухкомнатные объекты недвижимости");
            Console.WriteLine("3. Вывести разницу между заявленной и продажной стоимостью объектов недвижимости, расположенных на 2 этаже");
            Console.WriteLine("4. Определить общую стоимость всех двухкомнатных объектов недвижимости, расположенных в указанном районе");
            Console.WriteLine("5. Определить максимальную и минимальную стоимости объекта недвижимости, проданного указанным риэлтором");
            Console.WriteLine("6. Определить среднюю оценку объектов недвижимости, расположенных в указанном районе");
            Console.WriteLine("7. Вывести информацию о количестве объектов недвижимости, расположенных на 2 этаже по каждому району");
            Console.WriteLine("8. Определить среднюю оценку апартаментов по критерию «Безопасность», проданных указанным риэлтором");
            Console.WriteLine("9. Определить среднюю продажную стоимость 1м2 для квартир, которые были проданы в указанную дату «ОТ» и «ДО»");
            Console.WriteLine("10. Вывести информацию о премии риэлтора, которая рассчитывается по формуле:\r\nКоличество проданных квартир*Стоимость*5%-НДФЛ (13%)");
            Console.WriteLine("11. Вывести информацию о количестве квартир, проданных каждым риэлтором");
            Console.WriteLine("12. Вывести информацию о средней стоимости объектов недвижимости, расположенных на 2 этаже по каждому материалу здания");
            Console.WriteLine("13. Вывести информацию о трех самых дорогих объектах недвижимости, расположенных в каждом районе");
            Console.WriteLine("14. Определить адреса квартир, расположенных в указанном районе, которые еще не проданы");
            Console.WriteLine("15. Вывести информацию об объектах недвижимости, у которых разница между заявленной и продажной стоимостью составляет не более 20 % и расположенных в указанном районе");
            Console.WriteLine("16. Вывести информацию об объектах недвижимости, у которых разница между заявленной и продажной стоимостью составляет больше 100000 рублей и проданную указанным риэлтором");
            Console.WriteLine("17. Вывести разницу в % между заявленной и продажной стоимостью для объектов недвижимости, проданных указанным риэлтором в текущем году");
            Console.WriteLine("18. Определить адреса квартир, стоимость 1м2 которых меньше средней по району");
            Console.WriteLine("19. Определить ФИО риэлторов, которые ничего не продали в текущем году");
            Console.WriteLine("20. Вывести адреса объектов недвижимости, стоимость 1м2 которых меньше средней всех объектов недвижимости по району, объявления о которых были размещены не более 4 месяцев назад");
            Console.WriteLine("0 - Выход");
        }
    }
}