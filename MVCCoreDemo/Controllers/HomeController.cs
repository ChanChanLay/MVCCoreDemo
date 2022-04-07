using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MVCCoreDemo.Models;
using MySql.Data.MySqlClient;

namespace MVCCoreDemo.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration Configuration;

        public HomeController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public IActionResult Index()
        {
            List<Room> rooms = new List<Room>();
            //connect to Mysql
            string connString = this.Configuration.GetConnectionString("Default");
            using (MySqlConnection con = new MySqlConnection(connString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from room;", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Room room = new Room();
                    room.id = Convert.ToInt32(reader["roomId"]);
                    room.no = Convert.ToInt32(reader["roomNo"]);
                    room.type = reader["roomType"].ToString();
                    room.adult = Convert.ToInt32(reader["adult"]);
                    room.child = Convert.ToInt32(reader["child"]);
                    room.price = Convert.ToInt32(reader["price"]);

                    rooms.Add(room);
                }
                reader.Close();
                con.Close();    
            }
                return View(rooms);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
