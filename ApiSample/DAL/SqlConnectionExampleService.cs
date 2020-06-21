using ApiSample.DTOs.Guests;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace ApiSample.DAL
{
    public class SqlConnectionExampleService : IExampleService
    {
        //Proszę tutaj wpisać swój connection string
        private readonly string ConnStr = @"Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=PD3246;Integrated Security=True;";

        public ICollection<GuestResponseDto> GetGuestsCollection(string lastName)
        {
            var listToReturn = new List<GuestResponseDto>();

            using var sqlConn = new SqlConnection(ConnStr);
            using var sqlCmd = new SqlCommand
            {
                Connection = sqlConn
            };
            if (string.IsNullOrEmpty(lastName))
                sqlCmd.CommandText = @"SELECT g.IdGosc, g.Imie, g.Nazwisko, g.Procent_rabatu
                                    FROM Gosc g;";
            else
            {
                sqlCmd.CommandText = @"SELECT g.IdGosc, g.Imie, g.Nazwisko, g.Procent_rabatu
                                       FROM Gosc g
                                       WHERE g.Nazwisko = @LastName;";
                sqlCmd.Parameters.AddWithValue("LastName", lastName);
            }
            sqlConn.Open();
            using var reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                var item = new GuestResponseDto
                {
                    IdGuest = int.Parse(reader["IdGosc"].ToString()),
                    FirstName = reader["Imie"].ToString(),
                    LastName = reader["Nazwisko"].ToString(),
                    DiscountPercent = !string.IsNullOrEmpty(reader["Procent_rabatu"]?.ToString())
                                        ? int.Parse(reader["Procent_rabatu"].ToString()) : (int?)null

                };
                listToReturn.Add(item);
            }

            return listToReturn;
        }


        public bool AddGuest(GuestRequestDto newGuest)
        {
            try
            {
                using var sqlConn = new SqlConnection(ConnStr);
                using var sqlCmd = new SqlCommand
                {
                    Connection = sqlConn
                };
                sqlCmd.CommandText = @"INSERT INTO Gosc(IdGosc, Imie, Nazwisko, Procent_rabatu)
                                   VALUES ((SELECT ISNULL(MAX(IdGosc),0) + 1 FROM Gosc), @FirstName, @LastName, @DiscountPercent);";
                sqlCmd.Parameters.AddWithValue("FirstName", newGuest.FirstName);
                sqlCmd.Parameters.AddWithValue("LastName", newGuest.LastName);
                sqlCmd.Parameters.AddWithValue("DiscountPercent", newGuest.DiscountPercent ?? SqlInt32.Null);
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteGuest(int id)
        {
            throw new NotImplementedException();
        }

        public GuestResponseDto GetGuestById(int idGuest)
        {
            var listToReturn = new List<GuestResponseDto>();

            using var sqlConn = new SqlConnection(ConnStr);
            using var sqlCmd = new SqlCommand
            {
                Connection = sqlConn
            };
            {
                sqlCmd.CommandText = @"SELECT g.IdGosc, g.Imie, g.Nazwisko, g.Procent_rabatu
                                       FROM Gosc g
                                       WHERE g.IdGosc = @IdGosc;";
                sqlCmd.Parameters.AddWithValue("IdGosc", idGuest);
            }
            sqlConn.Open();
            using var reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                var item = new GuestResponseDto
                {
                    IdGuest = int.Parse(reader["IdGosc"].ToString()),
                    FirstName = reader["Imie"].ToString(),
                    LastName = reader["Nazwisko"].ToString(),
                    DiscountPercent = !string.IsNullOrEmpty(reader["Procent_rabatu"]?.ToString())
                                        ? int.Parse(reader["Procent_rabatu"].ToString()) : (int?)null

                };
                return item;


            }

            return null;
        }
        public string Test()
        {
            throw new NotImplementedException();
        }

        public bool UpdateGuest(int id, GuestRequestDto updateGuest)
        {
            try
            {
                using var sqlConn = new SqlConnection(ConnStr);
                using var sqlCmd = new SqlCommand
                {
                    Connection = sqlConn
                };
                sqlCmd.CommandText = @"UPDATE Gosc
                                        SET
                                            Imie = @FirstName,
                                            Nazwisko = @LastName,
                                            Procent_rabatu = @DiscountPercent   
                                        WHERE IdGosc = @IdGuest;";
                sqlCmd.Parameters.AddWithValue("IdGuest", id);
                sqlCmd.Parameters.AddWithValue("FirstName", updateGuest.FirstName);
                sqlCmd.Parameters.AddWithValue("LastName", updateGuest.LastName);
                sqlCmd.Parameters.AddWithValue("DiscountPercent", updateGuest.DiscountPercent ?? SqlInt32.Null);
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }
    }
}
