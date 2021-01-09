using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AdoNetWebApi.Models;
using System.Configuration;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AdoNetWebApi.Controllers
{
    public class PersonController : ApiController
    {
        string strConn = ConfigurationManager.ConnectionStrings["testdbConnectionString"].ConnectionString;
        StringBuilder stringBuilder = new StringBuilder();

        public DataSet QuerySelect(string strSQLCommand)
        {
            using (SqlConnection thisConnection = new SqlConnection(strConn))
            using (SqlCommand thisCommand = new SqlCommand(strSQLCommand, thisConnection))
            using (SqlDataAdapter thisAdapter = new SqlDataAdapter(thisCommand))
            {
                DataSet ds = new DataSet();
                thisAdapter.Fill(ds);
                return ds;
            }
        }

        public void QueryModify(string strSQLCommand)
        {
            using (SqlConnection thisConnection = new SqlConnection(strConn))
            {
                thisConnection.Open();
                using (SqlCommand thisCommand = thisConnection.CreateCommand())
                {
                    try
                    {
                        thisCommand.CommandType = CommandType.Text;
                        thisCommand.CommandText = strSQLCommand;
                        thisCommand.ExecuteNonQuery();
                    }
                    finally
                    {
                        thisConnection.Close();
                    }
                }
            }
        }

        // GET: api/Person
        public DataSet Get()
        {
            stringBuilder.Clear();
            stringBuilder.Append("SELECT * FROM Person");
            return QuerySelect(stringBuilder.ToString());
        }

        // GET: api/Person/5
        public DataSet Get(int id)
        {
            stringBuilder.Clear();
            stringBuilder.Append("SELECT * FROM Person WHERE Id = '" + id.ToString() + "'");
            return QuerySelect(stringBuilder.ToString());
        }

        // POST: api/Person
        public void Post([FromBody] Person newPerson)
        {
            stringBuilder.Clear();
            stringBuilder.Append("INSERT INTO Person (Id, Name, Age) VALUES ('" + newPerson.Id + "','" + newPerson.Name + "','" + newPerson.Age + "')");
            QueryModify(stringBuilder.ToString());
        }

        // PUT: api/Person/5
        public void Put(int id, [FromBody] Person updatePerson)
        {
            stringBuilder.Clear();
            stringBuilder.Append("UPDATE Person SET Name = '" + updatePerson.Name + "', Age = '" + updatePerson.Age + "' WHERE Id = '" + id.ToString() + "'");
            QueryModify(stringBuilder.ToString());
        }

        // DELETE: api/Person/5
        public void Delete(int id)
        {
            stringBuilder.Clear();
            stringBuilder.Append("DELETE FROM Person WHERE Id = '" + id.ToString() + "'");
            QueryModify(stringBuilder.ToString());
        }
    }
}
