using System;
using System.Collections.Generic;
using System.Text;

namespace VanEscolar.Domain
{
    public class TravelStudent
    {
        public Guid Id { get; set; }
        public TravelStatus Status { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime FinishAt { get; set; }
        public Student Student { get; set; }
    }

    public enum TravelStatus
    {
        //Aluno na escola
        AtScholl = 70,
        //Aluno em casa
        AtHome = 75,
        //Aluno em transporte
        Trasnporting = 80,
        //Van a caminho
        IsComing = 85
    }
}
