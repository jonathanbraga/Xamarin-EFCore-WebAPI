using System;
using System.Collections.Generic;
using System.Text;

namespace VanEscolar.Domain
{
    public class Travel
    {
        public Guid Id { get; set; }
        public bool NeedTravel { get; set; }
        public TravelStatus Status { get; set; }
        public Student Student { get; set; }
        public TravelStudent TravelStudent { get; set; }
    }

    public enum TravelStatus
    {
        //Aluno na escola
        AtScholl = 70,
        //Aluno em casa
        AtHome  = 75,
        //Aluno em transporte
        Trasnporting = 80,
        //Van a caminho
        IsComing = 85
    }
}
