using System;
using System.Collections.Generic;
using LibrarieModele;
namespace NivelStocareDate
{
    public class AdministrareMedici_Memorie
    {
        private List<Medic> _medici;

        public AdministrareMedici_Memorie()
        {
            _medici = new List<Medic>();
        }

        public void AddMedic(Medic medic)
        {
            _medici.Add(medic);
        }

        public Medic[] GetMedici(out int nrMedici)
        {
            nrMedici = _medici.Count;
            return _medici.ToArray();
        }

        public Medic GetMedicDupaId(int idMedic)
        {
            return _medici.Find(m => m.IdMedic == idMedic);
        }

        public Medic[] GetMediciDupaSpecialitate(string specialitate)
        {
            return _medici.FindAll(m => m.Specialitate.Equals(specialitate, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public Medic[] GetMediciDupaDepartament(int idDepartament)
        {
            return _medici.FindAll(m => m.IdDepartament == idDepartament).ToArray();
        }

        public void UpdateMedic(Medic medicActualizat)
        {
            var medicIndex = _medici.FindIndex(m => m.IdMedic == medicActualizat.IdMedic);
            if (medicIndex >= 0)
            {
                _medici[medicIndex] = medicActualizat;
            }
        }

        public static void AfisareMedici(Medic[] medici, int nrMedici)
        {
            if (nrMedici == 0)
            {
                Console.WriteLine("Nu exista medici in sistem.");
                return;
            }
            Console.WriteLine("--- Lista Medicilor ---");
            foreach (var medic in medici)
            {
                Console.WriteLine($"ID: {medic.IdMedic} - Dr. {medic.Nume} {medic.Prenume} - Specialitate: {medic.Specialitate} - Departament ID: {medic.IdDepartament}");
            }
        }
        public static void AfisareMedic(Medic medic)
        {
            Console.WriteLine($"ID: {medic.IdMedic} - Dr. {medic.Nume} {medic.Prenume} - Specialitate: {medic.Specialitate} - Departament ID: {medic.IdDepartament}");
        }



    }
}