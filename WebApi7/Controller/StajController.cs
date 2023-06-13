using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace WebApi7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StajController : ControllerBase
    {
        private readonly string _connectionString;

        public StajController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MyDatabase");
        }

        [HttpGet]
        public string Get()
        {
            using var con = new SqlConnection(_connectionString);
            using var da = new SqlDataAdapter("Select * FROM staj1", con);
            var dt = new DataTable();
            da.Fill(dt);
            return dt.Rows.Count > 0 ? JsonConvert.SerializeObject(dt) : "Veritabanında hicbir kayit bulunamadi";
        }

        [HttpGet("{id}")]
        [Route("api/staj/{id}")]
        public string Get(int id)
        {
            using var con = new SqlConnection(_connectionString);
            using var da = new SqlDataAdapter("Select * FROM staj1 WHERE id = @id", con);
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            var dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return JsonConvert.SerializeObject(dt);
            }
            else
            {
                return "Veritabanında hiçbir kayıt bulunamadi";
            }

        }

        public class Person
        {
            public string Name { get; set; }
            public string LastName { get; set; }
        }

        [HttpPost]
        [Route("api/staj")]
        public string Post([FromBody] Person person)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Insert into staj1(Name, LastName) VALUES(@Name, @LastName)", con);
            cmd.Parameters.AddWithValue("@Name", person.Name);
            cmd.Parameters.AddWithValue("@LastName", person.LastName);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if (i == 1)
            {
                return $"{person.Name} {person.LastName} Kayıtlara Eklendi";
            }
            else
            {
                return "Kayıt Eklenemedi Tekrar Dene";
            }

        }

        [HttpPut("{id}")]
        [Route("api/staj/{id}")]
        public string Put(int id, [FromBody] Person person)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("UPDATE staj1 SET Name = @Name, LastName = @LastName WHERE id = @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@Name", person.Name);
            cmd.Parameters.AddWithValue("@LastName", person.LastName);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if (i == 1)
            {
                return $"{person.Name} {person.LastName} Adlı kişinin Kaydı Güncellendi";
            }
            else
            {
                return "Kayıt Güncellenemedi Tekrar Dene";
            }

        }

        [HttpDelete("{id}")]
        [Route("api/staj/{id}")]
        public string Delete(int id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("DELETE FROM staj1 WHERE id = @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if (i == 1)
            {
                return "Kayıt Başarıyla Silindi";
            }
            else
            {
                return "Kayıt Silinemedi Tekrar Dene";
            }

        }
    }
}
