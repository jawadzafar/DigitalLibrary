﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DigitalLibrary.Models
{
    public class Publisher
    {
        [DontInsert]
        [DontUpdate]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        internal static List<Publisher> GetAll()
        {
            Publisher publisher = new Publisher();
            List<Publisher> ListOfPublisher = new List<Publisher>();
            string query = "select * from publishers";
            Database_Helpers db = new Database_Helpers();
            try
            {
                db.Connection.Open();
                SqlCommand cmd = new SqlCommand(query, db.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        publisher = new Publisher();
                        publisher.Id = (int)reader["Id"];
                        publisher.Name = reader["Name"].ToString();
                        publisher.Address = reader["Address"].ToString();
                        ListOfPublisher.Add(publisher);

                    }

                }
                return ListOfPublisher;
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return ListOfPublisher;
                throw ex;
            }
        }

        internal static bool Save(Publisher publisher)
        {
            Database_Helpers db = new Database_Helpers();
            if (db.Insert("Publishers", publisher))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}