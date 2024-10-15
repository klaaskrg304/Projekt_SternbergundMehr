using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using Npgsql;

namespace Projekt_SternbergundMehr
{
    public class Sponsors
    {
        private DBConnection dbConnection;

        public Sponsors()
        {
            dbConnection = new DBConnection();
        }
        ~Sponsors()
        {
            
        }

        public ObservableCollection<SponsorData> LoadSponsors()
        {
            string query = "SELECT \"Firma\", \"Ansprechpartner\", \"Adresse\", \"Betrag\" FROM tbl_sponsoren";
            DataTable dataTable = dbConnection.ExecuteQuery(query);

            ObservableCollection<SponsorData> sponsors = new ObservableCollection<SponsorData>();
            foreach (DataRow row in dataTable.Rows)
            {
                sponsors.Add(new SponsorData
                {
                    Firma = row["Firma"].ToString(),
                    Ansprechpartner = row["Ansprechpartner"].ToString(),
                    Adresse = row["Adresse"].ToString(),
                    Betrag = Convert.ToDouble(row["Betrag"])
                });
            }

            return sponsors;
        }

        public void AddSponsor(SponsorData sponsor)
        {
            string query = "INSERT INTO tbl_sponsoren (\"Firma\", \"Ansprechpartner\", \"Adresse\", \"Betrag\") VALUES (@Firma, @Ansprechpartner, @Adresse, @Betrag)";
            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@Firma", sponsor.Firma),
                new NpgsqlParameter("@Ansprechpartner", sponsor.Ansprechpartner),
                new NpgsqlParameter("@Adresse", sponsor.Adresse),
                new NpgsqlParameter("@Betrag", sponsor.Betrag)
            };
            dbConnection.ExecuteNonQuery(query, parameters);
        }

        public void DeleteSponsor(string firma)
        {
            string query = "DELETE FROM tbl_sponsoren WHERE \"Firma\" = @Firma";
            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@Firma", firma)
            };
            dbConnection.ExecuteNonQuery(query, parameters);
        }

        public void UpdateSponsor(SponsorData sponsor, string oldFirma)
        {
            string query = "UPDATE tbl_sponsoren SET \"Firma\" = @NewFirma, \"Ansprechpartner\" = @Ansprechpartner, \"Adresse\" = @Adresse, \"Betrag\" = @Betrag WHERE \"Firma\" = @OldFirma";
            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@NewFirma", sponsor.Firma),
                new NpgsqlParameter("@Ansprechpartner", sponsor.Ansprechpartner),
                new NpgsqlParameter("@Adresse", sponsor.Adresse),
                new NpgsqlParameter("@Betrag", sponsor.Betrag),
                new NpgsqlParameter("@OldFirma", oldFirma)
            };
            dbConnection.ExecuteNonQuery(query, parameters);
        }

        public double GetTotalAmount()
        {
            string query = "SELECT SUM(\"Betrag\") FROM tbl_sponsoren";
            object result = dbConnection.ExecuteScalar(query);
            return result != DBNull.Value ? Convert.ToDouble(result) : 0;
        }
    }
}

