namespace StudentGrade.Application.DTOs.GradeDtos
{
    public class ExportFgResultDto
    {
        public string FileName { get; set; } = null!;
        public byte[] FileContent { get; set; } = null!;
        public string ContentType { get; set; } = "application/octet-stream";
    }
}
