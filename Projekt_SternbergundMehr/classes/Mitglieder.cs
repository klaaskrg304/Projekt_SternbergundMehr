using Npgsql;
using System.Collections.ObjectModel;
using System.Data;
using System;



namespace Projekt_SternbergundMehr
{
    public class Mitglieder
    {
        private DBConnection dbConnection;

        public Mitglieder()
        {
            dbConnection = new DBConnection();
        }
        ~Mitglieder()
        {

        }

        public ObservableCollection<MitgliedData> LoadMitglieder()
        {
            string query = "SELECT \"Name\", \"Adresse\", \"Telefonnummer\", \"Beitrag\" FROM mitglieder";
            DataTable dataTable = dbConnection.ExecuteQuery(query);

            ObservableCollection<MitgliedData> mitglieder = new ObservableCollection<MitgliedData>();
            foreach (DataRow row in dataTable.Rows)
            {
                mitglieder.Add(new MitgliedData
                {
                    Name = row["Name"].ToString(),
                    Adresse = row["Adresse"].ToString(),
                    Telefonnummer = row["Telefonnummer"].ToString(),
                    Beitrag = Convert.ToDouble(row["Beitrag"])
                });
            }

            return mitglieder;
        }

        public void AddMitglied(MitgliedData mitglied)
        {
            string query = "INSERT INTO tbl_mitglieder (\"Name\", \"Adresse\", \"Telefonnummer\", \"Beitrag\") VALUES (@Name, @Adresse, @Telefonnummer, @Beitrag)";
            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@Name", mitglied.Name),
                new NpgsqlParameter("@Adresse", mitglied.Adresse),
                new NpgsqlParameter("@Telefonnummer", mitglied.Telefonnummer),
                new NpgsqlParameter("@Beitrag", mitglied.Beitrag)
            };
            dbConnection.ExecuteNonQuery(query, parameters);
        }

        public void DeleteMitglied(string name)
        {
            string query = "DELETE FROM tbl_mitglieder WHERE \"Name\" = @Name";
            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@Name", name)
            };
            dbConnection.ExecuteNonQuery(query, parameters);
        }

        public void UpdateMitglied(MitgliedData mitglied, string oldName)
        {
            string query = "UPDATE tbl_mitglieder SET \"Name\" = @NewName, \"Adresse\" = @Adresse, \"Telefonnummer\" = @Telefonnummer, \"Beitrag\" = @Beitrag WHERE \"Name\" = @OldName";
            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@NewName", mitglied.Name),
                new NpgsqlParameter("@Adresse", mitglied.Adresse),
                new NpgsqlParameter("@Telefonnummer", mitglied.Telefonnummer),
                new NpgsqlParameter("@Beitrag", mitglied.Beitrag),
                new NpgsqlParameter("@OldName", oldName)
            };
            dbConnection.ExecuteNonQuery(query, parameters);
        }

        public double GetTotalAmount()
        {
            string query = "SELECT SUM(\"Beitrag\") FROM tbl_mitglieder";
            object result = dbConnection.ExecuteScalar(query);
            return result != DBNull.Value ? Convert.ToDouble(result) : 0;
        }
    }

    public class MitgliedData
    {
        public string Name { get; set; }
        public string Adresse { get; set; }
        public string Telefonnummer { get; set; }
        public double Beitrag { get; set; }
    }
}
