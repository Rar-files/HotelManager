//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelManager
{
    using System;
    using System.Collections.ObjectModel;
    
    public partial class RoomsClass
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoomsClass()
        {
            this.Rooms = new ObservableCollection<Rooms>();
        }
    
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public int StarsStandard { get; set; }
        public decimal Price { get; set; }
        public decimal FlatArea { get; set; }
        public int BedCount { get; set; }
        public string AdditionalInfo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Rooms> Rooms { get; set; }
    }
}