using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using Newtonsoft.Json;

namespace FynProjekt.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            string[] temp =  DatabaseConnect($"select * from pos;",5);

            return temp;
        }

        // GET api/values/5
        public string[] Get(int id)
        {
            return DatabaseConnect($"select * from pos where ID={id};",5);
        }


        // POST api/values
        public string Post([FromBody]string value)
        {
            try
            {
                if (value.Length == 0)
                {
                    DatabaseConnect($"INSERT INTO pos (lok,lat,latlong) VALUES ('error',200,200);", 0);
                }
                else
                {
                    DatabaseConnect($"INSERT INTO pos (lok,lat,latlong) VALUES ({value});", 0);
                }
            }
            catch {
                return "error: " + value.Length + value;
            }
            return value + $"  INSERT INTO pos (lok,lat,latlong) VALUES ({value});";
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values
        public void Delete()
        {
            string url = "drop table pos; create table pos(ID int primary key NOT NULL AUTO_INCREMENT, tid timestamp DEFAULT CURRENT_TIMESTAMP, lok varchar(200), lat int, latlong int);";

            DatabaseConnect(url, 0);
        }



        static public string[] DatabaseConnect(string query, int coll)
        {

            string[] result = new string[0];
            string[] tempresult = new string[0];
            string connString = "Server=remotemysql.com;Port=3306; Database=vXlIrkfeN7; User Id=vXlIrkfeN7;Password=nOAFV2uzcf;";

            Console.WriteLine(query);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    //retrieve the SQL Server instance version


                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    //open connection
                    conn.Open();
                    //execute the SQLCommand
                    MySqlDataReader dr = cmd.ExecuteReader();


                    //check if there are records
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            tempresult = new string[result.Length + 1];

                            for (int i = 0; i < result.Length; i++)
                            {
                                tempresult[i] = result[i];
                            }

                            tempresult[tempresult.Length - 1] = "";
                            for (int i = 0; i < coll; i++)
                            {
                                tempresult[tempresult.Length - 1] += dr.GetString(i);
                                if (i < coll - 1) 
                                {
                                    tempresult[tempresult.Length - 1] += "|";
                                }
                            }

                            result = tempresult;
                        }
                    }
                    else
                    {
                        result = new string[] { "no data" };
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                //display error message
                result = new string[] { "Exception: " + ex.Message };
            }
            return result;
        }
    }
}
