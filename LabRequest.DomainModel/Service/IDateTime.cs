namespace LabRequest.DomainModel.Service
{
    public interface IDateTime
    {
        string GetDateFromYear { get; }
        string GetLocalDate { get; }
        string GetLongLocalTime { get; }
        string GetShortLocalTime { get; }
        string GetYear { get; }
    }
}
