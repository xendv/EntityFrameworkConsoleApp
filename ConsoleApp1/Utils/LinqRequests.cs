using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Timers;

namespace ConsoleApp1.Utils
{
    internal class LinqRequests
    {
        public LinqRequests() { }

        /*
         * 1. Вывести объекты недвижимости, расположенные в указанном районе
стоимостью «ОТ» и «ДО»
         */
        public List<string> GetApartInDistBetween(double start, double end, string district)
        {
            var list = new List<string>();

            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Apartment.Join(db.District,
                    a => a.District,
                    b => b.Id,
                    (a, b) => new
                    {
                        Address = a.Address,
                        Area = a.Area,
                        Floor = a.Floor,
                        District = b.Name,
                        Price = a.Price
                    })
                    .Where(el => el.District.Equals(district) && (el.Price > start && el.Price < end))
                    .ToList();

                result.ForEach(data => list.Add(data.Address + ", Площадь: " + data.Area + ", Этаж: " + data.Floor));
            }
            return list;
        }

        /*
         * 2. Вывести фамилии риэлтор, которые продали двухкомнатные объекты
недвижимости
         */
        public List<string> GetFio2Rooms(int count)
        {
            var list = new List<string>();

            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Realtor.Join(db.Sale,
                    a => a.Id,
                    b => b.Realtor,
                    (a, b) => new
                    {
                        L = a.LastName,
                        M = a.MiddleName,
                        F = a.FirstName,
                        Apartment = b.Apartment
                    }).Join(db.Apartment,
                    a => a.Apartment,
                    b => b.Id,
                    (a, b) => new
                    {
                        L = a.L,
                        M = a.M,
                        F = a.F,
                        Rooms = b.Rooms
                    })
                    .Where(el => el.Rooms.Equals(count))
                    .Distinct()
                    .ToList();

                result.ForEach(data => list.Add(data.L + ' ' + data.F + ' ' + data.M));
            }
            return list;
        }

        /*
         * Вывести разницу между заявленной и продажной стоимостью объектов
недвижимости, расположенных на 2 этаже
         */
        public List<string> GetDiffInPrices(int floor)
        {
            using ApplicationContext db = new ApplicationContext();
            var list = new List<string>();
            var result = db.Apartment.Join(db.Sale,
                a => a.Id,
                b => b.Apartment,
                (a, b) => new
                {
                    Address = a.Address,
                    Floor = a.Floor,
                    Price = a.Price,
                    PriceSale = b.Price,
                    Realtor = b.Realtor
                })
                .Join(db.Realtor,
                 a => a.Realtor,
                 b => b.Id,
                (a, b) => new
                {
                    Floor = a.Floor,
                    Address = a.Address,
                    PriceDiff = a.PriceSale - a.Price,
                    L = b.LastName,
                    M = b.MiddleName,
                    F = b.FirstName
                })
                .Where(el => el.Floor.Equals(floor))
                .ToList();
            result.ForEach(data =>
            list.Add(data.Address + ", Разница стоимости: " + data.PriceDiff + ", Риелтор: " + data.L + ' ' + data.F + ' ' + data.M)
            );
            return list;
        }

        /*
         * 4. Определить общую стоимость всех двухкомнатных объектов
недвижимости, расположенных в указанном районе
         */
        public double GetSumPriceInDist(int count, string district)
        {
            using ApplicationContext db = new ApplicationContext();
            var result = db.Apartment.Join(db.District,
                a => a.District,
                b => b.Id,
                (a, b) => new
                {
                    Rooms = a.Rooms,
                    District = b.Name,
                    Price = a.Price
                })
                .Where(el => el.District.Equals(district) && (el.Rooms == count))
                .Sum(el => el.Price);
            return result;
        }

        /*
         * 5. Определить максимальную и минимальную стоимости объекта
недвижимости, проданного указанным риэлтором
         */
        public string GetMunMaxByRealtor(string realtor)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Sale.Join(db.Realtor,
                    a => a.Realtor,
                    b => b.Id,
                    (a, b) => new
                    {
                        L = b.LastName,
                        M = b.MiddleName,
                        F = b.FirstName,
                        Price = a.Price,
                        Apartment = a.Apartment
                    })
                    .Where(el => el.L.Equals(realtor));
                if (result.ToList().Count == 0) return "У риэлтора нет продаж";
                var min = result.Min(res => res.Price);
                var max = result.Max(res => res.Price);
                return "Макс стоимость продажи: " + max + ", Мин стоимость продажи: " + min;
            }
        }

        /*
         * 6. Определить среднюю оценку объектов недвижимости, расположенных в
указанном районе
         */
        public string GetAvgScore(string district)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Score.Join(db.Apartment,
                    a => a.Apartment,
                    b => b.Id,
                    (a, b) => new
                    {
                        District = b.District,
                        Value = a.Value
                    })
                    .Join(db.District,
                    a => a.District,
                    b => b.Id,
                    (a, b) => new
                    {
                        District = b.Name,
                        Value = a.Value
                    })
                    .Where(el => el.District.Equals(district));
                if (result.ToList().Count == 0) return "Нет оценок для района";
                return "Средняя оценка по району: " + result.Average(r => r.Value).ToString();
            }
        }

        /*
         * 7. Вывести информацию о количестве объектов недвижимости,
расположенных на 2 этаже по каждому району
         */
        public List<string> GetInfoByDistrictAndFloor(int floor)
        {
            using ApplicationContext db = new ApplicationContext();
            var list = new List<string>();
            var result = db.Apartment.Join(db.District,
                a => a.District,
                b => b.Id,
                (a, b) => new
                {
                    Floor = a.Floor,
                    District = b.Name
                })
                .Where(el => el.Floor.Equals(floor))
                .GroupBy(el => el.District)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .ToList();
            result.ForEach(data =>
            list.Add("Район: " + data.Name + ", Количество: " + data.Count)
            );
            return list;
        }

        /*
         * 8. Определить среднюю оценку апартаментов по критерию «Безопасность»,
проданных указанным риэлтором
         */
        public string GetAvgScoreByCriteriaAndRealtor(string type, string criteria, string realtor)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Sale.Join(db.Realtor,
                    a => a.Realtor,
                    b => b.Id,
                    (a, b) => new
                    {
                        Apartment = a.Apartment,
                        Realtor = b.LastName
                    })
                    .Join(db.Apartment,
                    a => a.Apartment,
                    b => b.Id,
                    (a, b) => new
                    {
                        Apartment = a.Apartment,
                        Realtor = a.Realtor,
                        Type = b.Type
                    })
                    .Join(db.Type,
                    a => a.Type,
                    b => b.Id,
                    (a, b) => new
                    {
                        Apartment = a.Apartment,
                        Realtor = a.Realtor,
                        Type = b.Name
                    })
                    .Join(db.Score,
                    a => a.Apartment,
                    b => b.Apartment,
                    (a, b) => new
                    {
                        Apartment = a.Apartment,
                        Realtor = a.Realtor,
                        Type = a.Type,
                        Criteria = b.Criteria,
                        Score = b.Value
                    })
                    .Join(db.Criteria,
                    a => a.Criteria,
                    b => b.Id,
                    (a, b) => new
                    {
                        Apartment = a.Apartment,
                        Realtor = a.Realtor,
                        Type = a.Type,
                        Criteria = b.Name,
                        Score = a.Score
                    })
                    .Where(el => el.Realtor.Equals(realtor) && el.Type.Equals(type) && el.Criteria.Equals(criteria));
                if (result.ToList().Count == 0) return "Нет оценок";
                return "Средняя типа по риэлтору и критерию: " + result.Average(r => r.Score).ToString();
            }
        }

        /*
         * 9. Определить среднюю продажную стоимость 1м2 для квартир, которые были проданы в указанную дату «ОТ» и «ДО»
         */
        public string GetAvgSalePricePerMeter(string type, DateOnly start, DateOnly end)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Sale.Join(db.Apartment,
                    a => a.Apartment,
                    b => b.Id,
                    (a, b) => new
                    {
                        Date = a.Date,
                        SalePrice = a.Price,
                        Type = b.Type,
                        Area = b.Area
                    })
                    .Join(db.Type,
                    a => a.Type,
                    b => b.Id,
                    (a, b) => new
                    {
                        Date = a.Date,
                        SalePricePerMeter = a.SalePrice / a.Area,
                        Type = b.Name
                    })
                    .Where(el => el.Type.Equals(type) && (el.Date > start && el.Date < end))
                    .ToList();
                if (result.ToList().Count == 0) return "Нет продаж";
                return "Средняя продажная 1м2: " + Math.Round(result.Average(r => r.SalePricePerMeter), 2).ToString();
            }
        }

        /*
        * 10. Вывести информацию о премии риэлтора, которая рассчитывается по формуле
        */
        public List<string> GetBonuses()
        {
            var list = new List<string>();
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Sale.Join(db.Realtor,
                    a => a.Realtor,
                    b => b.Id,
                    (a, b) => new
                    {
                        L = b.LastName,
                        M = b.MiddleName,
                        F = b.FirstName,
                        Apartment = a.Apartment,
                        Price = a.Price
                    })
                    .GroupBy(el => el.L)
                    .Select(g => new { FIO = g.Select(s => s.L + ' ' + s.F + ' ' + s.M), Bonuses = g.Count() * g.Sum(c => c.Price) * 0.05 * 0.87 })
                    .ToList();
                if (result.Count == 0) return list;
                result.ForEach(r => list.Add("ФИО: " + r.FIO.First() + ", Премия: " + r.Bonuses));
                return list;
            }
        }

        /*
         * 11. Вывести информацию о количестве квартир, проданных каждым риэлтором
         */
        public List<string> GetCounts(string type)
        {
            var list = new List<string>();
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Sale.Join(db.Realtor,
                    a => a.Realtor,
                    b => b.Id,
                    (a, b) => new
                    {
                        L = b.LastName,
                        M = b.MiddleName,
                        F = b.FirstName,
                        Apartment = a.Apartment
                    })
                    .Join(db.Apartment,
                    a => a.Apartment,
                    b => b.Id,
                    (a, b) => new
                    {
                        L = a.L,
                        M = a.M,
                        F = a.F,
                        Apartment = a.Apartment,
                        Type = b.Type
                    })
                    .Join(db.Type,
                    a => a.Type,
                    b => b.Id,
                    (a, b) => new
                    {
                        L = a.L,
                        M = a.M,
                        F = a.F,
                        Apartment = a.Apartment,
                        Type = b.Name
                    })
                    .Where(el => el.Type.Equals(type))
                    .GroupBy(el => el.L)
                    .Select(g => new { FIO = g.Select(s => s.L + ' ' + s.F + ' ' + s.M), SalesOfType = g.Count() })
                    .ToList();
                if (result.Count == 0) return list;
                result.ForEach(r => list.Add("ФИО: " + r.FIO.First() + ", Продажи типа: " + r.SalesOfType));
                return list;
            }
        }

        /*
       * 12. Вывести информацию о средней стоимости объектов недвижимости, расположенных на 2 этаже по каждому материалу здания
       */
        public List<string> GetAvgPriceByTypeAndMaterial(int floor)
        {
            var list = new List<string>();
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Apartment.Join(db.Type,
                    a => a.Type,
                    b => b.Id,
                    (a, b) => new
                    {
                        Apartment = a.Id,
                        Floor = a.Floor,
                        Type = b.Name,
                        Material = a.Material,
                        Price = a.Price
                    })
                    .Join(db.Material,
                    a => a.Material,
                    b => b.Id,
                    (a, b) => new
                    {
                        Apartment = a.Apartment,
                        Floor = a.Floor,
                        Type = b.Name,
                        Material = b.Name,
                        Price = a.Price
                    })
                    .Where(el => el.Floor.Equals(floor))
                    .GroupBy(el => el.Material)
                    .Select(g => new { Material = g.Key, AvgPrice = g.Average(c => c.Price) })
                    .ToList();
                if (result.Count == 0) return list;
                result.ForEach(r => list.Add("Материал: " + r.Material + ", Средняя стоимость: " + r.AvgPrice));
                return list;
            }
        }

        /*
      * 13. Вывести информацию о трех самых дорогих объектах недвижимости, расположенных в каждом районе
      */
        public List<string> GetTop5ByPriceInDists()
        {
            var list = new List<string>();
            using (ApplicationContext db = new ApplicationContext())
            {
               /* var result = db.Apartment.Join(db.District,
                    a => a.District,
                    b => b.Id,
                    (a, b) => new
                    {
                        Apartment = a.Id,
                        Floor = a.Floor,
                        Address = a.Address,
                        District = b.Name,
                        Price = a.Price
                    })
                    .GroupBy(el => el.District)
                    .Select(g => new { District = g.Key, })
                    .OrderBy(el => new { el.Price, el.Floor })
                    .ToList();
                if (result.Count == 0) return list;
                result.ForEach(r => list.Add("Материал: " + r.Material + ", Средняя стоимость: " + r.AvgPrice));*/
                return list;
            }
        }

        /*
         * 14. Определить адреса квартир, расположенных в указанном районе, которые
         * еще не проданы.
         */
        public List<string> GetApartmentsInDistNotSold(string district)
        {
            var list = new List<string>();
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Apartment
                    .Join(db.District,
                    a => a.District,
                    b => b.Id,
                    (a, b) => new
                    {
                        Address = a.Address,
                        District = b.Name,
                        Status = a.Status
                    })
                    .Where(el => el.Status.Equals(1) && el.District.Equals(district))
                    .ToList();
                if (result.Count == 0) return list;
                result.ForEach(r => list.Add("Адрес: " + r.Address));
                return list;
            }
        }

        /*
      * 15. Вывести информацию об объектах недвижимости, у которых разница
между заявленной и продажной стоимостью составляет не более 20 % и
расположенных в указанном районе
      */
        public List<string> GetApartmentsLessThan20InDisrtict(string district)
        {
            var list = new List<string>();
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Sale
                    .Join(db.Apartment,
                    a => a.Apartment,
                    b => b.Id,
                    (a, b) => new
                    {
                        Address = b.Address,
                        District = b.District,
                        Diff = (a.Price - b.Price) * 100 / a.Price
                    })
                    .Join(db.District,
                    a => a.District,
                    b => b.Id,
                    (a, b) => new
                    {
                        Address = a.Address,
                        District = b.Name,
                        Diff = a.Diff
                    })
                    .Where(el => el.Diff <= 20 && el.District.Equals(district))
                    .ToList();
                if (result.Count == 0) return list;
                result.ForEach(r => list.Add("Адрес: " + r.Address + ", Район: " + r.District));
                return list;
            }
        }


        /*
       * 16. Вывести информацию об объектах недвижимости, у которых разница
между заявленной и продажной стоимостью составляет больше 100000 рублей и
проданную указанным риэлтором
       */
        public List<string> GetApartmentsWithDiffMoreThan(string realtor)
        {
            var list = new List<string>();
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Sale.Join(db.Realtor,
                    a => a.Realtor,
                    b => b.Id,
                    (a, b) => new
                    {
                        L = b.LastName,
                        Apartment = a.Apartment,
                        SalePrice = a.Price
                    })
                    .Join(db.Apartment,
                    a => a.Apartment,
                    b => b.Id,
                    (a, b) => new
                    {
                        L = a.L,
                        Address = b.Address,
                        District = b.District,
                        Diff = a.SalePrice - b.Price
                    })
                    .Join(db.District,
                    a => a.District,
                    b => b.Id,
                    (a, b) => new
                    {
                        L = a.L,
                        Address = a.Address,
                        District = b.Name,
                        Diff = a.Diff
                    })
                    .Where(el => el.L.Equals(realtor) && Math.Abs(el.Diff) > 100000)
                    .ToList();
                if (result.Count == 0) return list;
                result.ForEach(r => list.Add("Адрес: " + r.Address + ", Район: " + r.District));
                return list;
            }
        }


        /*
        * 17. Вывести разницу в % между заявленной и продажной стоимостью для
объектов недвижимости, проданных указанным риэлтором в текущем году
        */
        public List<string> GetDiffInSaleByRealtorAndYear(string realtor, int year)
        {
            var list = new List<string>();
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Sale.Join(db.Realtor,
                    a => a.Realtor,
                    b => b.Id,
                    (a, b) => new
                    {
                        L = b.LastName,
                        Year = a.Date.Year,
                        Apartment = a.Apartment,
                        SalePrice = a.Price
                    })
                    .Join(db.Apartment,
                    a => a.Apartment,
                    b => b.Id,
                    (a, b) => new
                    {
                        L = a.L,
                        Year = a.Year,
                        Address = b.Address,
                        Diff = Math.Round((a.SalePrice - b.Price) * 100 / a.SalePrice, 2)
                    })
                    .Where(el => el.L.Equals(realtor) && el.Year.Equals(year))
                    .ToList();
                if (result.Count == 0) return list;
                result.ForEach(r => list.Add("Адрес: " + r.Address + ", Разница: " + r.Diff + '%'));
                return list;
            }
        }


        /*
         * 18. Определить адреса квартир, стоимость 1м2 которых меньше средней по району.
         */
        public List<string> GetApartmentsWithPriceLessThanAgerageBuDist()
        {
            var list = new List<string>();
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Apartment.Join(db.Type,
                a => a.Type,
                b => b.Id,
                (a, b) => new
                {
                    Address = a.Address,
                    District = a.District,
                    Type = b.Name,
                    PriceM2 = a.Price / a.Area
                })
                .Where(row => row.Type.Equals("Квартира") && row.PriceM2 <
                (db.Apartment
                    .Join(db.Type,
                    a => a.Type,
                    b => b.Id,
                    (a, b) => new
                    {
                        Address = a.Address,
                        District = a.District,
                        Type = b.Name,
                        PriceM2 = a.Price / a.Area
                    })
                    .Where(el => el.Type.Equals("Квартира") && el.District.Equals(row.District))
                    .Average(r => r.PriceM2)
                )
                )
                .ToList();
                result.ForEach(r => list.Add(r.Address));
                return list;
            }
        }

        /*
         * 19. Определить ФИО риэлторов, которые ничего не продали в текущем году.
         */
        public List<string> GetRealtorsWithoutSalesThisYear()
        {
            var list = new List<string>();
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Realtor
                    .Where(r => !db.Sale
                    .Select(s => new { Realtor = s.Realtor, Year = s.Date.Year })
                    .Where(c => c.Year.Equals(2023))
                    .Any(el => el.Realtor.Equals(r.Id))
                    )
                    .Select(el => el.LastName + ' ' + el.FirstName + ' ' + el.MiddleName)
                    .ToList();
                if (result.Count == 0) return list;
                result.ForEach(r => list.Add("ФИО: " + r));

                return list;
            }
        }

        /*
         * 20. Вывести адреса объектов недвижимости, стоимость 1м2 которых меньше
средней всех объектов недвижимости по району, объявления о которых были
размещены не более 4 месяцев назад.
         */
        public List<string> GetApartmentsWithPriceLessThanAgerageByDistAndEarlierThan4Month()
        {
            var list = new List<string>();
            using (ApplicationContext db = new ApplicationContext())
            {
                var now = DateOnly.FromDateTime(DateTime.Now);
                var result = db.Apartment
                .Where(row => ((row.Price / row.Area) <
                (db.Apartment
                    .Select(
                    a => new
                    {
                        Address = a.Address,
                        District = a.District,
                        PriceM2 = a.Price / a.Area,
                        Status = a.Status.ToString()
                    })
                    .Where(el => el.District.Equals(row.District))
                    .Average(r => r.PriceM2)
                )))
                .ToList();
                var res = result.Where(row => ((now.Year - row.Date.Year) * 12) + now.Month - row.Date.Month <= 4).ToList();
                var maxLength = 0;
                res.ForEach(r => maxLength = r.Address.Length > maxLength ? r.Address.Length : maxLength);
                list.Add(Format("Адрес", "Статус", maxLength));
                res.ForEach(r => list.Add(Format(r.Address, r.Status.Equals(1) ? "в продаже" : "продано", maxLength)));
                return list;
            }
        }

        private string Format(string a, int maxLength)
        {
            var len = (maxLength - a.Length + 6) / 2;
            return new string(' ', len) + a + new string(' ', a.Length % 2 == 0 ? len + 1 : len);
        }

        private string Format(string a, string b, int maxLength)
        {
            return Format(a, maxLength) + '|' + Format(b, 9);
        }
    }
}
