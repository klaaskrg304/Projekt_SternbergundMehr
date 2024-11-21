using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace Projekt_SternbergundMehr
{


    internal class participants
    {

        private DBConnection dbConnection;

        public participants()
        {
            dbConnection = new DBConnection();
        }

        public ObservableCollection<ParticipantsData> Loadparticipants()
        {
            string query = "SELECT * FROM tbl_umzugsteilnehmer";
            DataTable dataTable = dbConnection.ExecuteQuery(query);

            ObservableCollection<ParticipantsData> participants = new ObservableCollection<ParticipantsData>();
            foreach (DataRow row in dataTable.Rows)
            {
                participants.Add(new ParticipantsData
                {
                    Position = Convert.ToInt16(row["Position"]),
                    Firma = row["Firma"].ToString(),
                    Ansprechpartner = row["Ansprechpartner"].ToString(),
                    Adresse = row["Adresse"].ToString()


                });


            }

            return participants;
        }

        public int SearchParticipant()
        {
            string query = "SELECT MAX(\"Position\")  FROM tbl_umzugsteilnehmer";
            object result = dbConnection.ExecuteScalar(query);
            return result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public void AddParticipant(ParticipantsData participants)
        {
            string query = "INSERT INTO tbl_umzugsteilnehmer (\"Position\", \"Firma\", \"Ansprechpartner\", \"Adresse\") VALUES (@Position, @Firma, @Ansprechpartner, @Adresse)";
            NpgsqlParameter[] parameters = {
                 new NpgsqlParameter("@Position", participants.Position),
                new NpgsqlParameter("@Firma", participants.Firma),
                new NpgsqlParameter("@Ansprechpartner", participants.Ansprechpartner),
                new NpgsqlParameter("@Adresse", participants.Adresse)

            };
            dbConnection.ExecuteNonQuery(query, parameters);
        }



        public void DeleteParticipant(string firma)
        {
            string query = "DELETE FROM tbl_umzugsteilnehmer WHERE \"Firma\" = @Firma";
            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@Firma", firma)
            };
            dbConnection.ExecuteNonQuery(query, parameters);
        }

        public void UpdateParticipant(ParticipantsData participants, string oldFirma)
        {
            string query = "UPDATE tbl_umzugsteilnehmer SET  \"Position\" = @Position, \"Firma\" = @NewFirma, \"Ansprechpartner\" = @Ansprechpartner, \"Adresse\" = @Adresse WHERE \"Firma\" = @OldFirma";
            NpgsqlParameter[] parameters = {

                new NpgsqlParameter("@Position", participants.Position),
                new NpgsqlParameter("@NewFirma", participants.Firma),
                new NpgsqlParameter("@Ansprechpartner", participants.Ansprechpartner),
                new NpgsqlParameter("@Adresse", participants.Adresse),
                new NpgsqlParameter("@OldFirma", oldFirma)
            };
            dbConnection.ExecuteNonQuery(query, parameters);
        }

        /* public double GetTotalAmount()
         {
             string query = "SELECT SUM(\"Betrag\") FROM tbl_sponsoren";
             object result = dbConnection.ExecuteScalar(query);
             return result != DBNull.Value ? Convert.ToDouble(result) : 0;
         }*/
    }
}




