using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AnimalCrudProject.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AnimalCrudProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public AnimalController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {

            string query = @"
                        select AnimalId, AnimalName, AnimalClass,
                        convert(varchar(10),DateOfListing,120) as DateOfListing,
                        PhotoFileName
                        from dbo.Animal
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

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Animal dep)
        {
            string query = @"
                        insert into dbo.Animal
                        (AnimalName, AnimalClass, DateOfListing, PhotoFileName)
                        values
                        (
                        '" + dep.AnimalName + @"'
                        ,'" + dep.AnimalClass + @"'
                        ,'" + dep.DateOfListing + @"'
                        ,'" + dep.PhotoFileName + @"'
                        )
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
        public JsonResult Put(Animal dep)
        {
            string query = @"
                    update dbo.Animal set
                    AnimalName = '" + dep.AnimalName + @"'
                    ,AnimalClass = '" + dep.AnimalClass + @"'
                    ,DateOfListing = '" + dep.DateOfListing + @"'
                    where AnimalId = " + dep.AnimalId + @" 
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

            return new JsonResult("Updated Successfully");
        }

        //Since we are sending the ID from the URL we added ID to root parameter
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                    delete from dbo.Animal
                    where AnimalId = " + id + @" 
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


        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch
            {
                return new JsonResult("anonymous.jpeg");
            }
        }


        [Route("GetAllAnimalNames")]
        [HttpGet]
        public JsonResult GetAllAnimalNames()
        {
            string query = @"
                    select AnimalName from dbo.Animal
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

            return new JsonResult(table);
        }

    }
}
