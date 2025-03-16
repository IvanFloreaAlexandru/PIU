using System;

namespace LibrarieModele
{
    public class Programare
    {
        public  int IdProgramare;
        public int IdPacient { get; set; }
        public int IdMedic { get; set; }
        public DateTime DataOra { get; set; }
        public TimeSpan Durata { get; set; }
        public string Motiv { get; set; }
        public string Status { get; set; }


        public Programare()
        {
            DataOra = DateTime.Now;
            Durata = TimeSpan.Zero;
            Motiv = string.Empty;
            Status = string.Empty;
        }

        public Programare(int idProgramare, int idPacient, int idMedic, DateTime dataOra, TimeSpan durata, string motiv, string status)
        {
            IdProgramare = idProgramare;
            IdPacient = idPacient;
            IdMedic = idMedic;
            DataOra = dataOra;
            Durata = durata;
            Motiv = motiv;
            Status = status;
        }

        public string ConversieLaSir_PentruFisier()
        {
            return $"{IdProgramare},{IdPacient},{IdMedic},{DataOra:yyyy-MM-dd HH:mm},{Durata},{Motiv},{Status}";
        }

        public string Info()
        {
            return $"ID Programare: {IdProgramare}\nPacient ID: {IdPacient}\nMedic ID: {IdMedic}\nData si Ora: {DataOra:dd.MM.yyyy HH:mm}\nDurata: {Durata}\nMotiv: {Motiv}\nStatus: {Status}";
        }
    }
}
