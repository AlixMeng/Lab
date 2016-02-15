namespace LabRequest.DomainModel.Entities
{
    public class User
    {
        public int PersonId { get; set; }
        public string UserName { get; set; }
        public string PersonName { get; set; }
        public string PersonFamily { get; set; }
        public string PersonPass { get; set; }
        public int UnitId { get; set; }
        public int UserType { get; set; }
        public int Com_Id { get; set; }
    }
}