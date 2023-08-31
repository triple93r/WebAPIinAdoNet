using Microsoft.AspNetCore.JsonPatch;
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
        public ActionResult Create(string StudentName, int age, string Address)
        {
            try
            {
                connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
                cmd = new SqlCommand("insert into Students(StudentName,age,[Address]) values('" + StudentName + "'," + age + ",'" + Address + "')", connString);
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
        public ActionResult Edit(int Id, string StudentName, int age, string Address)
        {
            try
            {
                connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
                cmd = new SqlCommand("update students set StudentName='" + StudentName + "', age= " + age + ", [Address]='" + Address + "' where Id=" + Id + "", connString);
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

        [Route("Patch")]
        [HttpPatch] //PATCH
        public ActionResult Patch(int id, [FromBody] JsonPatchDocument<Student> patchDoc)
        {
            try
            {
                connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
                cmd = new SqlCommand("update students set " + patchDoc.Operations[0].path +"='" + patchDoc.Operations[0].value + "' where Id=" + id + "", connString);
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

        [Route("Delete/{id}")]
        [HttpDelete] //DELETE
        public ActionResult Delete(int id)
        {
            try
            {
                connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
                cmd = new SqlCommand("delete from Students where Id=" + id + "", connString);
                connString.Open();
                int x = cmd.ExecuteNonQuery();
                if (x > 0)
                {
                    return Ok(new { Message = "Record Deleted!" });
                }
                return BadRequest(new { Message = "Record Not found!" });
            }
            catch (Exception ef)
            {
                return BadRequest(ef.Message);
            }
            finally { connString.Close(); }
        }


        [Route("CreateSemester")] //POST
        [HttpPost]
        public ActionResult CreateSemester(int Studid, int SemesterNum, string Subject1, int Mark1, string Subject2, int Mark2, int TotalMark)
        {
            try
            {
                connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
                cmd = new SqlCommand("insert into StudentSemester( Studid, SemesterNum, Subject1, Mark1, Subject2, Mark2, TotalMark) values(" + Studid + ", "+ SemesterNum + " , '" + Subject1 + "'," + Mark1 + ",'" + Subject2 + "'," + Mark2 + "," + TotalMark + ")", connString);
                connString.Open();
                cmd.ExecuteNonQuery();
                connString.Close();
                return Ok(new { Message = "Record Added!" });
            }
            catch (Exception ef)
            {
                return BadRequest(ef.Message);
            }
        }

        [Route("FindSemesterByStudId/{id}")] //GET
        [HttpGet]
        public JsonResult FindSemesterByStudId(int id)
        {
            connString = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection"));
            dtb = new DataTable();
            cmd = new SqlCommand("select * from StudentSemester where Studid=" + id, connString);
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
    }
}
