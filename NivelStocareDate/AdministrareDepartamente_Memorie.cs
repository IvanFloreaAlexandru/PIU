using System;
using System.Collections.Generic;
using System.IO;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareDepartamente_Memorie
    {
        private List<Departament> _departamente;
        private readonly string _numeFisier;

        public AdministrareDepartamente_Memorie(string numeFisier)
        {
            _numeFisier = numeFisier ?? throw new ArgumentException("Calea fisierului nu poate fi null sau goala.", nameof(numeFisier));
            _departamente = new List<Departament>();

            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;

            string filePath = Path.Combine(projectDirectory, "Departamente.txt");


            if (File.Exists(filePath))
            {
                IncarcaDepartamenteDinFisier(filePath);
            }
            else
            {
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                File.Create(filePath).Close();
            }
        }

        public void AddDepartament(Departament departament)
        {
            _departamente.Add(departament);
            SalveazaDepartamenteInFisier();
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
                SalveazaDepartamenteInFisier();
            }
        }

        private void SalveazaDepartamenteInFisier()
        {
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;

            string filePath = Path.Combine(projectDirectory, "Departamente.txt");


            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                foreach (var departament in _departamente)
                {
                    writer.WriteLine(departament.ConversieLaSir_PentruFisier());
                }
            }
        }

        private void IncarcaDepartamenteDinFisier(string fullPath)
        {
            _departamente.Clear();
            using (StreamReader reader = new StreamReader(fullPath))
            {
                string linie;
                while ((linie = reader.ReadLine()) != null)
                {
                    Departament departament = new Departament(linie);
                    _departamente.Add(departament);
                }
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
