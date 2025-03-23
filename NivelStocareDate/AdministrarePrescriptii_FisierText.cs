using System;
using System.Collections.Generic;
using System.IO;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrarePrescriptii_FisierText
    {
        private readonly string _numeFisier;
        private List<Prescriptie> _prescriptii;

        public AdministrarePrescriptii_FisierText(string numeFisier)
        {
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            _numeFisier = Path.Combine(locatieFisierSolutie, numeFisier);


            _prescriptii = new List<Prescriptie>();

            string directoryPath = Path.GetDirectoryName(_numeFisier);

            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (File.Exists(_numeFisier))
            {
                IncarcaPrescriptiiDinFisier();
            }
            else
            {
                using (FileStream fs = File.Create(_numeFisier)) { }
            }
        }


        public void AddPrescriptie(Prescriptie prescriptie,string caleFisierCompleta)
        {
            _prescriptii.Add(prescriptie);
            SalveazaPrescriptiiInFisier(caleFisierCompleta);
        }

        public Prescriptie[] GetPrescriptii(out int nrPrescriptii)
        {
            nrPrescriptii = _prescriptii.Count;
            return _prescriptii.ToArray();
        }

        public Prescriptie[] GetPrescriptiiDupaPacient(int idPacient)
        {
            return _prescriptii.FindAll(p => p.IdPacient == idPacient).ToArray();
        }

        public Prescriptie[] GetPrescriptiiDupaMedic(int idMedic)
        {
            return _prescriptii.FindAll(p => p.IdMedic == idMedic).ToArray();
        }

        public Prescriptie[] GetPrescriptiiDupaData(DateTime data)
        {
            return _prescriptii.FindAll(p => p.DataEmitere.Date == data.Date).ToArray();
        }


        private void SalveazaPrescriptiiInFisier(string caleFisierCompleta)
        {
            using (StreamWriter writer = new StreamWriter(caleFisierCompleta, false))
            {
                foreach (var prescriptie in _prescriptii)
                {
                    writer.WriteLine(prescriptie.ConversieLaSir_PentruFisier());
                }
            }
        }

        private void IncarcaPrescriptiiDinFisier()
        {
            using (StreamReader reader = new StreamReader(_numeFisier))
            {
                string linie;
                while ((linie = reader.ReadLine()) != null)
                {
                    Prescriptie prescriptie = new Prescriptie(linie);
                    _prescriptii.Add(prescriptie);
                }
            }
        }

    }
}
