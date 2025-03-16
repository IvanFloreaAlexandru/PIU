using System;
using System.Collections.Generic;
using System.IO;
using LibrarieModele;
namespace NivelStocareDate
{
    public class AdministrareDepartamente_FisierText
    {
        private readonly string _numeFisier;
        private List<Departament> _departamente;
        public AdministrareDepartamente_FisierText(string numeFisier)
        {
            _numeFisier = numeFisier;
            _departamente = new List<Departament>();
            if (File.Exists(numeFisier))
            {
                IncarcaDepartamenteDinFisier();
            }
            else
            {
                File.Create(numeFisier).Close();
            }
        }
        public void AddDepartament(Departament departament)
        {
            _departamente.Add(departament);
            SalveazaDepartamenteInFisier();
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
                SalveazaDepartamenteInFisier();
            }
        }
        private void SalveazaDepartamenteInFisier()
        {
            using (StreamWriter writer = new StreamWriter(_numeFisier, false))
            {
                foreach (var departament in _departamente)
                {
                    writer.WriteLine(departament.ConversieLaSir_PentruFisier());
                }
            }
        }
        private void IncarcaDepartamenteDinFisier()
        {
            using (StreamReader reader = new StreamReader(_numeFisier))
            {
                string linie;
                while ((linie = reader.ReadLine()) != null)
                {
                    Departament departament = new Departament(linie);
                    _departamente.Add(departament);
                }
            }
        }
    }
}