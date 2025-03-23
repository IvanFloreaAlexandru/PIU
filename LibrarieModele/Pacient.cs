using System;

namespace LibrarieModele
{
    public class Pacient
    {
        private static readonly int indexIdPacient = 0;
        private static readonly int indexNume = 1;
        private static readonly int indexPrenume = 2;
        private static readonly int indexCNP = 3;
        private static readonly int indexDataNasterii = 4;
        private static readonly int indexGen = 5;
        private static readonly int indexAdresa = 6;
        private static readonly int indexTelefon = 7;
        private static readonly int indexEmail = 8;
        private static readonly int indexGrupaSanguina = 9;
        private static readonly int indexAlergii = 10;

        public int IdPacient { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string CNP { get; set; }
        public DateTime DataNasterii { get; set; }
        public string Gen { get; set; }
        public string Adresa { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string GrupaSanguina { get; set; }
        public string[] Alergii { get; set; }

        public Pacient()
        {
            Nume = string.Empty;
            Prenume = string.Empty;
            CNP = string.Empty;
            Gen = string.Empty;
            Adresa = string.Empty;
            Telefon = string.Empty;
            Email = string.Empty;
            GrupaSanguina = string.Empty;
            Alergii = new string[0];
        }

        public Pacient(int idPacient, string nume, string prenume, string cnp, DateTime dataNasterii,
                      string gen, string adresa, string telefon, string email, string grupaSanguina)
        {
            IdPacient = idPacient;
            Nume = nume;
            Prenume = prenume;
            CNP = cnp;
            DataNasterii = dataNasterii;
            Gen = gen;
            Adresa = adresa;
            Telefon = telefon;
            Email = email;
            GrupaSanguina = grupaSanguina;
            Alergii = new string[0];
        }

        public Pacient(string linieFisier)
        {
            string[] datePacient = linieFisier.Split(',');

            IdPacient = Convert.ToInt32(datePacient[indexIdPacient]);
            Nume = datePacient[indexNume];
            Prenume = datePacient[indexPrenume];
            CNP = datePacient[indexCNP];
            DataNasterii = Convert.ToDateTime(datePacient[indexDataNasterii]);
            Gen = datePacient[indexGen];
            Adresa = datePacient[indexAdresa];
            Telefon = datePacient[indexTelefon];
            Email = datePacient[indexEmail];
            GrupaSanguina = datePacient[indexGrupaSanguina];

            Alergii = datePacient.Length > indexAlergii ? datePacient[indexAlergii].Split(';') : new string[0];
        }


        public void SetAlergii(string[] alergii)
        {
            Alergii = alergii;
        }

        public string ConversieLaSir_PentruFisier()
        {
            string alergiiString = Alergii != null && Alergii.Length > 0 ? string.Join(";", Alergii) : "";
            return $"{IdPacient},{Nume},{Prenume},{CNP},{DataNasterii:yyyy-MM-dd},{Gen},{Adresa},{Telefon},{Email},{GrupaSanguina},{alergiiString}";
        }

        public string InfoScurt()
        {
            return $"ID: {IdPacient}, {Nume} {Prenume}, CNP: {CNP}, Tel: {Telefon}";
        }

    }
}