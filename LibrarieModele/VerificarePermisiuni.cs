using System;
using LibrarieModele;

namespace GestionareSpital
{
    public static class VerificarePermisiuni
    {
        public static bool ArePermisiune(User utilizator, Permisiuni permisiuneNecesara)
        {
            Permisiuni permisiuniUtilizator = ObtinePermisiuniPentruRang(utilizator.Rang);

            return (permisiuniUtilizator & permisiuneNecesara) == permisiuneNecesara;
        }

        private static Permisiuni ObtinePermisiuniPentruRang(RangUtilizator rang)
        {
            switch (rang)
            {
                case RangUtilizator.Director:
                    return Permisiuni.PermisiuniDirector;

                case RangUtilizator.SefDepartament:
                    return Permisiuni.PermisiuniSefDepartament;

                case RangUtilizator.Medic:
                    return Permisiuni.PermisiuniMedic;

                default:
                    return Permisiuni.None;
            }
        }
    }
}