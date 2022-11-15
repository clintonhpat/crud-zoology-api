using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AnimalCrudProject.Models;

namespace AnimalCrudProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalClassController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AnimalClassController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {

            string query = @"
                        select AnimalClassId, AnimalClassName from dbo.AnimalClass";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AnimalAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(AnimalClass dep)
        {
            string query = @"
                        insert into dbo.AnimalClass values
                        ('" + dep.AnimalClassName + @"')
                        ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AnimalAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(AnimalClass dep)
        {
            string query = @"
                    update dbo.AnimalClass set 
                    AnimalClassName = '" + dep.AnimalClassName + @"'
                    where AnimalClassId = " + dep.AnimalClassId + @" 
                    ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AnimalAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        //Since we are sending the ID from the URL we added ID to root parameter
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                    delete from dbo.AnimalClass
                    where AnimalClassId = " + id + @" 
                    ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AnimalAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}