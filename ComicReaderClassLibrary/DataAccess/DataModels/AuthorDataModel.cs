using System;
using System.Collections.Generic;
using System.Text;

namespace ComicReaderClassLibrary.DataAccess.DataModels
{
    public class AuthorDataModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime DateDeceased { get; set; }
        public bool Active { get; set; }
        public string FullName { get {
                if (!String.IsNullOrEmpty(MiddleName))
                {
                    return $"{FirstName} {MiddleName} {LastName}";
                }
                else
                {
                    return $"{FirstName} {LastName}";
                }
            } 
        }
    }
}
