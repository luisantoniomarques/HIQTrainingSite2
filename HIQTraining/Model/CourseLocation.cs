//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HIQTraining.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CourseLocation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CourseLocation()
        {
            this.Courses = new HashSet<Course>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayColor { get; set; }
        public string UserCreated { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UserUpdated { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }
    }
}
