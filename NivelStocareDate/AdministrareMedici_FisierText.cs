using System;
using System.Collections.Generic;
using System.IO;
using LibrarieModele;
namespace NivelStocareDate
{
    public class AdministrareMedici_FisierText
    {
        private readonly string _numeFisier;
        private List<Medic> _medici;
        public AdministrareMedici_FisierText(string numeFisier)
        {
            _numeFisier = numeFisier;
            _medici = new List<Medic>();
            string directoryPath = Path.GetDirectoryName(numeFisier);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (File.Exists(numeFisier))
            {
                IncarcaMediciDinFisier();
            }
            else
            {
                File.Create(numeFisier).Close();
            }
        }
        public void AddMedic(Medic medic)
        {
            _medici.Add(medic);
            SalveazaMediciInFisier();
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
                SalveazaMediciInFisier();
            }
        }
        private void SalveazaMediciInFisier()
        {
            using (StreamWriter writer = new StreamWriter(_numeFisier, false))
            {
                foreach (var medic in _medici)
                {
                    writer.WriteLine(medic.ConversieLaSir_PentruFisier());
                }
            }
        }
        private void IncarcaMediciDinFisier()
        {
            using (StreamReader reader = new StreamReader(_numeFisier))
            {
                string linie;
                while ((linie = reader.ReadLine()) != null)
                {
                    Medic medic = new Medic(linie);
                    _medici.Add(medic);
                }
            }
        }
    }
}