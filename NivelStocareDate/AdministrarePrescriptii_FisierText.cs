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
            _numeFisier = numeFisier;
            _prescriptii = new List<Prescriptie>();

            if (File.Exists(numeFisier))
            {
                IncarcaPrescriptiiDinFisier();
            }
            else
            {
                File.Create(numeFisier).Close();
            }
        }

        public void AddPrescriptie(Prescriptie prescriptie)
        {
            _prescriptii.Add(prescriptie);
            SalveazaPrescriptiiInFisier();
        }

        public Prescriptie[] GetPrescriptii(out int nrPrescriptii)
        {
            nrPrescriptii = _prescriptii.Count;
            return _prescriptii.ToArray();
        }

        public Prescriptie GetPrescriptieDupaId(int idPrescriptie)
        {
            return _prescriptii.Find(p => p.IdPrescriptie == idPrescriptie);
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

        public void UpdatePrescriptie(Prescriptie prescriptieActualizata)
        {
            var prescriptieIndex = _prescriptii.FindIndex(p => p.IdPrescriptie == prescriptieActualizata.IdPrescriptie);
            if (prescriptieIndex >= 0)
            {
                _prescriptii[prescriptieIndex] = prescriptieActualizata;
                SalveazaPrescriptiiInFisier();
            }
        }

        private void SalveazaPrescriptiiInFisier()
        {
            using (StreamWriter writer = new StreamWriter(_numeFisier, false))
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
