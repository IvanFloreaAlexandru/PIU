using System;
using System.Collections.Generic;
using System.IO;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareDepartamente_FisierText
    {
        private readonly string _numeFisier;

        public AdministrareDepartamente_FisierText(string numeFisier)
        {
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            _numeFisier = Path.Combine(locatieFisierSolutie, numeFisier);

            string directoryPath = Path.GetDirectoryName(_numeFisier);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(_numeFisier))
            {
                File.Create(_numeFisier).Close();
            }
        }

        public void AddDepartament(Departament departament)
        {
            using (StreamWriter writer = new StreamWriter(_numeFisier, true))
            {
                writer.WriteLine(departament.ConversieLaSir_PentruFisier());
            }
        }

        public Departament[] GetDepartamente(out int nrDepartamente)
        {
            List<Departament> departamente = new List<Departament>();
            using (StreamReader reader = new StreamReader(_numeFisier))
            {
                string linie;
                while ((linie = reader.ReadLine()) != null)
                {
                    departamente.Add(new Departament(linie));
                }
            }
            nrDepartamente = departamente.Count;
            return departamente.ToArray();
        }

        public Departament GetDepartamentDupaId(int idDepartament)
        {
            using (StreamReader reader = new StreamReader(_numeFisier))
            {
                string linie;
                while ((linie = reader.ReadLine()) != null)
                {
                    Departament departament = new Departament(linie);
                    if (departament.IdDepartament == idDepartament)
                    {
                        return departament;
                    }
                }
            }
            return null;
        }

        public void UpdateDepartament(Departament departamentActualizat)
        {
            List<Departament> departamente = new List<Departament>();
            using (StreamReader reader = new StreamReader(_numeFisier))
            {
                string linie;
                while ((linie = reader.ReadLine()) != null)
                {
                    Departament departament = new Departament(linie);
                    if (departament.IdDepartament == departamentActualizat.IdDepartament)
                    {
                        departament = departamentActualizat;
                    }
                    departamente.Add(departament);
                }
            }
            using (StreamWriter writer = new StreamWriter(_numeFisier, false))
            {
                foreach (var departament in departamente)
                {
                    writer.WriteLine(departament.ConversieLaSir_PentruFisier());
                }
            }
        }

    }
}
