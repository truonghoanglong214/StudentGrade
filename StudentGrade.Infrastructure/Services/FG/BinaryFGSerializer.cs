#pragma warning disable SYSLIB0011

using FuGradeLib;
using StudentGrade.Application.DTOs.GradeDtos;
using StudentGrade.Application.Interfaces.IServices;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace StudentGrade.Infrastructure.Services.FG
{
    public class BinaryFGSerializer : IFGSerializer
    {
        
        public byte[] Serialize(FgGradeDataDto data)
        {
            var teacherGrade = new TeacherGrade
            {
                Version = data.Version,
                Semester = data.Semester,
                Login = data.Login,
                Password = data.Password,
                SubjectClassGrades = data.SubjectClasses.Select(sc => new SubjectClassGrade
                {
                    Subject = sc.Subject,
                    Class = sc.Class,
                    Components = sc.Components,
                    Students = sc.Students.Select(s => new Student
                    {
                        Roll = s.Roll,
                        Name = s.Name,
                        Comment = s.Comment,
                        Grades = s.Grades.Select(g => new GradeComponent
                        {
                            Component = g.Component,
                            Grade = g.Grade
                        }).ToList()
                    }).ToList()
                }).ToList()
            };

            using var ms = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, teacherGrade);
            return ms.ToArray();
        }
    }
}

#pragma warning restore SYSLIB0011
