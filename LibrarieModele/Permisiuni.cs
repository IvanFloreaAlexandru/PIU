using System;

namespace LibrarieModele
{
    [Flags]
    public enum Permisiuni
    {
        None = 0,

        VizualizareMedici = 1 << 0,
        VizualizarePacienti = 1 << 1,
        VizualizareProgramari = 1 << 2,
        VizualizarePrescriptii = 1 << 3,
        VizualizareDepartamente = 1 << 4,
        VizualizareUtilizatori = 1 << 5,

        AdaugarePacienti = 1 << 6,
        ModificarePacienti = 1 << 7,
        StergerePacienti = 1 << 8,
        AdaugareProgramari = 1 << 9,
        ModificareProgramari = 1 << 10,
        StergereProgramari = 1 << 11,
        AdaugarePrescriptii = 1 << 12,
        ModificarePrescriptii = 1 << 13,
        StergerePrescriptii = 1 << 14,

        AdaugareMedici = 1 << 15,
        ModificareMedici = 1 << 16,
        StergereMedici = 1 << 17,
        GestionareDepartamentPropriu = 1 << 18,

        AdaugareDepartamente = 1 << 19,
        ModificareDepartamente = 1 << 20,
        StergereDepartamente = 1 << 21,
        AdaugareUtilizatori = 1 << 22,
        ModificareUtilizatori = 1 << 23,
        StergereUtilizatori = 1 << 24,

        PermisiuniMedic = VizualizareMedici | VizualizarePacienti | VizualizareProgramari | VizualizarePrescriptii |
                          AdaugarePacienti | ModificarePacienti | AdaugareProgramari | ModificareProgramari |
                          AdaugarePrescriptii | ModificarePrescriptii,

        PermisiuniSefDepartament = PermisiuniMedic | AdaugareMedici | ModificareMedici | StergerePacienti |
                                   StergereProgramari | StergerePrescriptii | GestionareDepartamentPropriu |
                                   VizualizareDepartamente,

        PermisiuniDirector = PermisiuniSefDepartament | StergereMedici | AdaugareDepartamente | ModificareDepartamente |
                             StergereDepartamente | VizualizareUtilizatori | AdaugareUtilizatori |
                             ModificareUtilizatori | StergereUtilizatori,

        ToatePermisiunile = ~None 
    }
}