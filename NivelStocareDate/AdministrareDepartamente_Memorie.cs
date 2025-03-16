using System;
using System.Collections.Generic;
using LibrarieModele;
namespace NivelStocareDate
{
    public class AdministrareDepartamente_Memorie
    {
        private List<Departament> _departamente;

        public AdministrareDepartamente_Memorie()
        {
            _departamente = new List<Departament>();
        }

        public void AddDepartament(Departament departament)
        {
            _departamente.Add(departament);
        }

        public Departament[] GetDepartamente(out int nrDepartamente)
        {
            nrDepartamente = _departamente.Count;
            return _departamente.ToArray();
        }

        public Departament GetDepartamentDupaId(int idDepartament)
        {
            return _departamente.Find(d => d.IdDepartament == idDepartament);
        }

        public void UpdateDepartament(Departament departamentActualizat)
        {
            var departamentIndex = _departamente.FindIndex(d => d.IdDepartament == departamentActualizat.IdDepartament);
            if (departamentIndex >= 0)
            {
                _departamente[departamentIndex] = departamentActualizat;
            }
        }

        public static void AfisareDepartamente(Departament[] departamente, int nrDepartamente)
        {
            if (nrDepartamente == 0)
            {
                Console.WriteLine("Nu exista departamente in sistem.");
                return;
            }
            Console.WriteLine("--- Lista Departamentelor ---");
            foreach (var departament in departamente)
            {
                Console.WriteLine($"ID: {departament.IdDepartament} - {departament.Nume} - Locatie: {departament.Locatie}");
            }
        }
    }
}