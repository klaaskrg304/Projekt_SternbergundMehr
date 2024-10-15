using System;
using System.Data;
using Npgsql;

namespace Projekt_SternbergundMehr
{
    public class DBConnection
    {
        private string connString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=db_sponsoren";

        public DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connString))
                {
                    connection.Open();
                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, connection);
                    dataAdapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Ausführen der Abfrage: {ex.Message}");
            }

            return dataTable;
        }


        public string get_Hash()
        {
            
           

            string hashValue = string.Empty;

            try
            {
                using (var connection = new NpgsqlConnection(connString))
                {
                    connection.Open();

                    using (var cmd = new NpgsqlCommand("SELECT hash FROM public.tbl_hash", connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Liest die erste Zeile
                        {
                            hashValue = reader.GetString(0); // Holt den ersten Spaltenwert der aktuellen Zeile
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                throw new Exception($"Fehler beim Ausführen der Abfrage: {ex.Message}");
            }

            return hashValue;
        }



        public void ExecuteNonQuery(string query, NpgsqlParameter[] parameters)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connString))
                {
                    connection.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Ausführen der Nicht-Abfrage: {ex.Message}");
            }
        }

        public object ExecuteScalar(string query)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connString))
                {
                    connection.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                    {
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Ausführen der Skalaren Abfrage: {ex.Message}");
            }
        }
    }
}

