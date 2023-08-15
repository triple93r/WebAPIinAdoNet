using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using YoutubeStudent.Models;

namespace YoutubeStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : Controller
    {
        private IConfiguration Configuration;
        public StudentsController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        SqlConnection connString;
        SqlCommand cmd;

        [HttpGet]
        public JsonResult Index()
        {
            List<Student> studlist = new List<Student>();
            connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
            DataTable dt = new DataTable();
            cmd = new SqlCommand("select * from Students", connString);
            connString.Open();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Student obj = new Student();
                obj.Id = Convert.ToInt32(dt.Rows[i]["id"]);
                obj.StudentName = dt.Rows[i]["StudentName"].ToString();
                obj.age = Convert.ToInt32(dt.Rows[i]["Age"]);
                obj.Address = dt.Rows[i]["Address"].ToString();
                studlist.Add(obj);
            }
            connString.Close();
            return Json(dt);
        }

    }
}
