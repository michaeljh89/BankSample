using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttributeRoutingSample.Models
{
    public class Country
    {
        //public Country()
        //{
        //    this.Students = new List<Student>();
        //}

        public int Id { get; set; }

        public string Name { get; set; }

        public string iso2Code { get; set; }

        public string capitalCity { get; set; }

        public string latitude { get; set; }

        public string longitude { get; set; }
    }
}