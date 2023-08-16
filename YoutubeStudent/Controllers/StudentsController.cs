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
        public JsonResult Index() //GET
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

        [Route("FindById/{id}")] //GET
        [HttpGet]
        public JsonResult FindById(int id)
        {
            List<Student> studlist = new List<Student>();
            connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
            DataTable dt = new DataTable();
            cmd = new SqlCommand("select * from Students where id="+id, connString);
            try
            {
                connString.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
                if(dt.Rows.Count>0)
                {
                    Student obj = new Student();
                    obj.Id = Convert.ToInt32(dt.Rows[0]["id"]);
                    obj.StudentName = dt.Rows[0]["StudentName"].ToString();
                    obj.age = Convert.ToInt32(dt.Rows[0]["Age"]);
                    obj.Address = dt.Rows[0]["Address"].ToString();
                    studlist.Add(obj);
                    connString.Close();
                    return Json(dt);
                }
                else
                    return Json("Not Found!");
            }
            catch (Exception ef)
            {
                return Json(StatusCodes.Status500InternalServerError, ef.Message);
            }
        }

        [Route("Create")] //POST
        [HttpPost]
        public ActionResult Create(Student st)
        {
            try
            {
                connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
                cmd = new SqlCommand("insert into Students(StudentName,age,[Address]) values('" + st.StudentName + "'," + st.age + ",'" + st.Address + "')", connString);
                connString.Open();
                cmd.ExecuteNonQuery();
                connString.Close();
                return Ok(new { Message = "Record Added" });
            }
            catch (Exception ef)
            {
                return BadRequest(ef.Message);
            }
        }

        [Route("Edit")]
        [HttpPost] //PUT
        public ActionResult Edit(Student st)
        {
            try
            {
                connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
                cmd = new SqlCommand("update students set StudentName='" + st.StudentName + "', age= " + st.age + ", [Address]='" + st.Address + "' where Id=" + st.Id + "", connString);
                connString.Open();
                int x = cmd.ExecuteNonQuery();
                if (x > 0)
                {
                    return Ok(new { Message = "Record Updated" });
                }
                return BadRequest(new { Message = "Record Not found!" });
            }
            catch (Exception ef)
            {
                return BadRequest(ef.Message);
            }
            finally { connString.Close(); }
        }

    }
}
