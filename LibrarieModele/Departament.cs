using System;

namespace LibrarieModele
{
    public class Departament
    {
        public int IdDepartament { get; set; }
        public string Nume { get; set; }
        public string Descriere { get; set; }
        public string Locatie { get; set; }

        public Departament()
        {
            Nume = string.Empty;
            Descriere = string.Empty;
            Locatie = string.Empty;
        }

        public Departament(int idDepartament, string nume, string descriere, string locatie)
        {
            IdDepartament = idDepartament;
            Nume = nume;
            Descriere = descriere;
            Locatie = locatie;
        }

        public Departament(string linieFisier)
        {
            string[] dateDepartament = linieFisier.Split(',');

            IdDepartament = Convert.ToInt32(dateDepartament[0]);
            Nume = dateDepartament[1];
            Descriere = dateDepartament.Length > 2 ? dateDepartament[2] : string.Empty;
            Locatie = dateDepartament.Length > 3 ? dateDepartament[3] : string.Empty;
        }

        public string ConversieLaSir_PentruFisier()
        {
            return $"{IdDepartament},{Nume},{Descriere},{Locatie}";
        }

        public string Info()
        {
            return $"ID: {IdDepartament}\nNume: {Nume}\nDescriptie: {Descriere}\nLocatie: {Locatie}";
        }
        public string InfoScurt()
        {
            return $"{IdDepartament}: {Nume}";
        }
    }
}