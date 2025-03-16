using System;

namespace LibrarieModele
{
    public class Medic
    {
        public int IdMedic;

        private static readonly int indexIdMedic = 0;
        private static readonly int indexNume = 1;
        private static readonly int indexPrenume = 2;
        private static readonly int indexCNP = 3;
        private static readonly int indexSpecialitate = 4;
        private static readonly int indexIdDepartament = 5;
        private static readonly int indexTelefon = 6;
        private static readonly int indexEmail = 7;
        private static readonly int indexProgram = 8;

        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string CNP { get; set; }
        public string Specialitate { get; set; }
        public int IdDepartament { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Program { get; set; }

        public Medic()
        {
            Nume = string.Empty;
            Prenume = string.Empty;
            CNP = string.Empty;
            Specialitate = string.Empty;
            Telefon = string.Empty;
            Email = string.Empty;
            Program = string.Empty;
        }

        public Medic(int idMedic, string nume, string prenume, string cnp, string specialitate, int idDepartament, string telefon, string email, string program)
        {
            IdMedic = idMedic;
            Nume = nume;
            Prenume = prenume;
            CNP = cnp;
            Specialitate = specialitate;
            IdDepartament = idDepartament;
            Telefon = telefon;
            Email = email;
            Program = program;
        }


        public Medic(string linieFisier)
        {
            string[] dateMedic = linieFisier.Split(',');

            IdMedic = Convert.ToInt32(dateMedic[indexIdMedic]);
            Nume = dateMedic[indexNume];
            Prenume = dateMedic[indexPrenume];
            CNP = dateMedic[indexCNP];
            Specialitate = dateMedic[indexSpecialitate];
            IdDepartament = Convert.ToInt32(dateMedic[indexIdDepartament]);
            Telefon = dateMedic[indexTelefon];
            Email = dateMedic[indexEmail];
            Program = dateMedic[indexProgram];
        }

        public string ConversieLaSir_PentruFisier()
        {
            return $"{IdMedic},{Nume},{Prenume},{CNP},{Specialitate},{IdDepartament},{Telefon},{Email},{Program}";
        }

        public string InfoScurt()
        {
            return $"ID: {IdMedic}, Dr. {Nume} {Prenume}, Specialitate: {Specialitate}";
        }

        public string Info()
        {
            return $"ID: {IdMedic}\nNume: {Nume} {Prenume}\nSpecialitate: {Specialitate}\nDepartament: {IdDepartament}\nContact: {Telefon}, {Email}\nProgram: {Program}";
        }
    }
}