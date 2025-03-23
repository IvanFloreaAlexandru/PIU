using System;

namespace LibrarieModele
{
    public class Medic : User
    {

        private static readonly int indexId = 0;
        private static readonly int indexNume = 1;
        private static readonly int indexPrenume = 2;
        private static readonly int indexEmail = 3;
        private static readonly int indexTelefon = 4;
        private static readonly int indexCNP = 5;
        private static readonly int indexSpecialitate = 6;
        private static readonly int indexIdDepartament = 7;
        private static readonly int indexProgram = 8;

        public string CNP { get; set; }
        public string Specialitate { get; set; }
        public int IdDepartament { get; set; }
        public string Program { get; set; }
        public int IdMedic { get; set; }

        public Medic() : base()
        {
            CNP = string.Empty;
            Specialitate = string.Empty;
            Program = string.Empty;
        }

        public Medic(int idMedic, string nume, string prenume, string cnp, string specialitate, int idDepartament, string telefon, string email, string program,RangUtilizator rang)
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
            Rang = rang;
        }

        public Medic(string linieFisier)
    : base(
        Convert.ToInt32(linieFisier.Split(',')[indexId]),
        linieFisier.Split(',')[indexNume],
        linieFisier.Split(',')[indexPrenume],
        linieFisier.Split(',')[indexEmail],
        linieFisier.Split(',')[indexTelefon],
        RangUtilizator.Medic)
        {
            string[] dateMedic = linieFisier.Split(',');

            if (dateMedic.Length > indexCNP && dateMedic.Length > indexSpecialitate && dateMedic.Length > indexIdDepartament && dateMedic.Length > indexProgram)
            {
                CNP = dateMedic[indexCNP];
                Specialitate = dateMedic[indexSpecialitate];

                if (int.TryParse(dateMedic[indexIdDepartament], out int idDepartament))
                {
                    IdDepartament = idDepartament;
                }
                else
                {
                    Console.WriteLine("ID-ul departamentului nu este valid.");
                }

                Program = dateMedic[indexProgram];
            }
            else
            {
                Console.WriteLine("Linia de fisier nu contine suficiente date pentru a crea un medic.");
            }
        }




        public override string ConversieLaSir_PentruFisier()
        {
            return $"{IdUser},{Nume},{Prenume},{Email},{Telefon},{CNP},{Specialitate},{IdDepartament},{Program}";
        }

        public override string Info()
        {
            return base.Info() + $"\nCNP: {CNP}\nSpecialitate: {Specialitate}\nDepartament: {IdDepartament}\nProgram: {Program}";
        }

    }
}
