using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;
using ConsoleApp1.Models;

namespace ConsoleApp1.Utils
{
    internal class InitFromFile
    {
        readonly string startupPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public List<District> GetDistricts()
        {
            var list = new List<District>();
            using var sr = new StreamReader(@startupPath + "\\resources\\districts_init.json");
            var reader = new JsonTextReader(sr);
            var jArray = JArray.Load(reader);
            foreach (var content in jArray)
            {
                list.Add(new District { Id = (int)content["Id"], Name = content["Name"].ToString() });
            }
            return list;
        }

        public List<Models.Type> GetTypes()
        {
            var list = new List<Models.Type>();
            using var sr = new StreamReader(@startupPath + "\\resources\\types_init.json");
            var reader = new JsonTextReader(sr);
            var jArray = JArray.Load(reader);
            foreach (var content in jArray)
            {
                list.Add(new Models.Type { Id = (int)content["Id"], Name = content["Name"].ToString() });
            }
            return list;
        }


        public List<Criteria> GetCriteria()
        {
            var list = new List<Criteria>();
            using var sr = new StreamReader(@startupPath + "\\resources\\criteria_init.json");
            var reader = new JsonTextReader(sr);
            var jArray = JArray.Load(reader);
            foreach (var content in jArray)
            {
                list.Add(new Criteria { Id = (int)content["Id"], Name = content["Name"].ToString() });
            }
            return list;
        }


        public List<Realtor> GetRealtor()
        {
            var list = new List<Realtor>();
            using var sr = new StreamReader(@startupPath + "\\resources\\realtors_init.json");
            var reader = new JsonTextReader(sr);
            var jArray = JArray.Load(reader);
            foreach (var content in jArray)
            {
                list.Add(new Realtor { Id = (int)content["Id"], LastName = content["LastName"].ToString(),
                    MiddleName = content["MiddleName"].ToString(), FirstName = content["FirstName"].ToString(),
                    Phone = content["Phone"].ToString()
                });
            }
            return list;
        }


        public List<Material> GetMaterial()
        {
            var list = new List<Material>();
            using var sr = new StreamReader(@startupPath + "\\resources\\materials_init.json");
            var reader = new JsonTextReader(sr);
            var jArray = JArray.Load(reader);
            foreach (var content in jArray)
            {
                list.Add(new Material { Id = (int)content["Id"], Name = content["Name"].ToString() });
            }
            return list;
        }


        public List<Apartment> GetApartment()
        {
            var list = new List<Apartment>();
            using var sr = new StreamReader(@startupPath + "\\resources\\apartments_init.json");
            var reader = new JsonTextReader(sr);
            var jArray = JArray.Load(reader);
            foreach (var content in jArray)
            {
                list.Add(new Apartment { Id = (int)content["Id"], District = (int)content["District"],
                    Address = content["Address"].ToString(),
                    Floor = (int)content["Floor"],
                    Rooms = (int)content["Rooms"],
                    Type = (int)content["Type"],
                    Status = (int)content["Status"],
                    Price = (double)content["Price"],
                    Description = content["District"].ToString(),
                    Material = (int)content["Material"],
                    Area = (double)content["Area"],
                    Date = DateOnly.Parse(content["Date"].ToString())
                });
            }
            return list;
        }

        public List<Sale> GetSale()
        {
            var list = new List<Sale>();
            using var sr = new StreamReader(@startupPath + "\\resources\\sales_init.json");
            var reader = new JsonTextReader(sr);
            var jArray = JArray.Load(reader);
            foreach (var content in jArray)
            {
                list.Add(new Sale { Id = (int)content["Id"],
                    Apartment = (int)content["Apartment"],
                    Date = DateOnly.Parse(content["Date"].ToString()),
                    Realtor = (int)content["Realtor"],
                    Price = (double)content["Price"]
                });
            }
            return list;
        }


        public List<Score> GetScore()
        {
            var list = new List<Score>();
            using var sr = new StreamReader(@startupPath + "\\resources\\scores_init.json");
            var reader = new JsonTextReader(sr);
            var jArray = JArray.Load(reader);
            foreach (var content in jArray)
            {
                list.Add(new Score { Id = (int)content["Id"], Apartment = (int)content["Apartment"],
                    Date = DateOnly.Parse(content["Date"].ToString()),
                    Criteria = (int)content["Criteria"],
                    Value = (double)content["Value"]
                });
            }
            return list;
        }
    }
}
