using StudentGrade.Application.DTOs.GradeDtos;

namespace StudentGrade.Application.Interfaces.IServices
{
    /// <summary>
    /// Serializes grade data into the binary .fg file format used by FUGE.
    /// </summary>
    public interface IFGSerializer
    {
        byte[] Serialize(FgGradeDataDto data);
    }
}