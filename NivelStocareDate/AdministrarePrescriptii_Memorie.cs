using System;
using System.Collections.Generic;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrarePrescriptii_Memorie
    {
        private List<Prescriptie> _prescriptii;

        public AdministrarePrescriptii_Memorie()
        {
            _prescriptii = new List<Prescriptie>();
        }

        public void AddPrescriptie(Prescriptie prescriptie)
        {
            _prescriptii.Add(prescriptie);
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
            }
        }
    }
}
