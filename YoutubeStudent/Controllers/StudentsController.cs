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
        SqlDataAdapter adap;
        DataTable dtb;

        [HttpGet]
        public JsonResult Index() //GET
        {
            connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
            DataTable dt = new DataTable();
            cmd = new SqlCommand("select * from Students", connString);
            try
            {
                connString.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
                return Json(dt);
            }
            catch (Exception ef)
            {
                return Json(StatusCodes.Status500InternalServerError, ef.Message);
            }
            finally { connString.Close(); }
        }

        [Route("FindById/{id}")] //GET
        [HttpGet]
        public JsonResult FindById(int id)
        {
            connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
            dtb = new DataTable();
            cmd = new SqlCommand("select * from Students where id="+id, connString);
            try
            {
                connString.Open();
                adap = new SqlDataAdapter(cmd);
                adap.Fill(dtb);
                if(dtb.Rows.Count>0)
                {
                    return Json(dtb);
                }
                else
                    return Json("Not Found!");
            }
            catch (Exception ef)
            {
                return Json(StatusCodes.Status500InternalServerError, ef.Message);
            }
            finally { connString.Close(); }
        }


        [Route("FindByParams/{col}/{val}")] //GET
        [HttpGet]
        public JsonResult FindByParams(string col, string val)
        {
            connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
            dtb = new DataTable();
            cmd = new SqlCommand("select * from Students where "+col+"='"+val+"'", connString);
            try
            {
                connString.Open();
                adap = new SqlDataAdapter(cmd);
                adap.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                   return Json(dtb);
                }
                else
                   return Json("Not Found!");
            }
            catch (Exception ef)
            {
                return Json(StatusCodes.Status500InternalServerError, ef.Message);
            }
            finally { connString.Close(); }
        }


        [Route("FindByColumnVal/{val}")] //GET
        [HttpGet]
        public JsonResult FindByColumnVal(string val)
        {
            connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
            dtb = new DataTable();
            cmd = new SqlCommand("select * from Students where Address='"+ val +"'", connString);
            try
            {
                connString.Open();
                adap = new SqlDataAdapter(cmd);
                adap.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    return Json(dtb);
                }
                else
                    return Json("Not Found!");
            }
            catch (Exception ef)
            {
                return Json(StatusCodes.Status500InternalServerError, ef.Message);
            }
            finally { connString.Close(); }
        }

        [Route("FindByMultiColumnVal/{val1}/{val2}")] //GET
        [HttpGet]
        public JsonResult FindByMultiColumnVal(string val1, int val2)
        {
            connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
            dtb = new DataTable();
            cmd = new SqlCommand("select * from Students where Address='" + val1 + "' and age>"+val2, connString);
            try
            {
                connString.Open();
                adap = new SqlDataAdapter(cmd);
                adap.Fill(dtb);
                if (dtb.Rows.Count > 0)
                {
                    return Json(dtb);
                }
                else
                    return Json("Not Found!");
            }
            catch (Exception ef)
            {
                return Json(StatusCodes.Status500InternalServerError, ef.Message);
            }
            finally { connString.Close(); }
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
        [HttpPut] //PUT
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
